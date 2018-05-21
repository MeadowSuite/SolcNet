using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SolcNet.DataDescription.Output;
using SolCodeGen.AbiEncoding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;


namespace SolCodeGen
{
    public class ContractGenerator
    {
        string _name;
        Contract _contract;

        public ContractGenerator(string name, Contract contract)
        {
            _name = name;
            _contract = contract;
        }

        public string GenerateCodeFile()
        {
            var code = GenerateNamespaceContainer();
            var formatted = FormatCode(code);
            return formatted;
        }

        string FormatCode(string csCode)
        {
            var tree = CSharpSyntaxTree.ParseText(csCode);
            // var tree = SyntaxFactory.ParseSyntaxTree(csCode);
            var result = tree.GetRoot().NormalizeWhitespace().ToString();
            // var result = tree.GetCompilationUnitRoot().NormalizeWhitespace().ToFullString();
            return result;
        }

        string GenerateNamespaceContainer()
        {
            return $@"
                using SolcNet.DataDescription.Output;
                using SolCodeGen;
                using SolCodeGen.AbiEncoding;
                using SolCodeGen.AbiEncoding.Encoders;
                using SolCodeGen.JsonRpc;
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Threading.Tasks;

                namespace GeneratedContracts
                {{
                    {GenerateClassDef()}
                }}
            ";
        }

        string GenerateClassDef()
        {
            return $@"
                public class {_name} : BaseContract
                {{

                    public override Lazy<ReadOnlyMemory<byte>> Bytecode {{ get; }} = new Lazy<ReadOnlyMemory<byte>>(() => BYTECODE_HEX.HexToReadOnlyMemory());
                    public override Lazy<Abi> Abi {{ get; }} = new Lazy<Abi>(() => ABI_JSON);
                    public override Lazy<Doc> DevDoc {{ get; }} = new Lazy<Doc>(() => DEV_DOC_JSON);
                    public override Lazy<Doc> UserDoc {{ get; }} = new Lazy<Doc>(() => USER_DOC_JSON);

                    public const string BYTECODE_HEX = ""{_contract.Evm.Bytecode.Object}"";
                    public const string ABI_JSON = ""{_contract.AbiJsonString.Replace("\"", "\\\"")}"";
                    public const string DEV_DOC_JSON = """";
                    public const string USER_DOC_JSON = """";

                    private {_name}(JsonRpcClient rpcClient, Address address, Address defaultFromAccount)
                        : base(rpcClient, address, defaultFromAccount)
                    {{  }}

                    public static {_name} At(JsonRpcClient rpcClient, Address address, Address defaultFromAccount)
                    {{
                        return new {_name}(rpcClient, address, defaultFromAccount);
                    }}

                    {GenerateClassMembers()}
                }}
            ";
        }

        string GenerateClassMembers()
        {
            var template = new StringBuilder();
            foreach (var item in _contract.Abi)
            {
                if (item.Type == AbiType.Constructor)
                {
                    template.AppendLine(GenerateConstructor(item));
                }
                else if (item.Type == AbiType.Event)
                {
                    template.AppendLine(GenerateEvent(item));
                }
                else if (item.Type == AbiType.Function)
                {
                    template.AppendLine(GenerateFunction(item));
                }
            }

            return template.ToString();
        }

        string GenerateConstructor(Abi constructorAbi)
        {
            string inputConstructorArg = string.Empty;
            string inputEncoders = string.Empty;
            bool hasInputs = constructorAbi.Inputs.Length > 0;
            if (hasInputs)
            {
                var inputs = GenerateInputs(constructorAbi.Inputs);
                if (inputs.Length == 1)
                {
                    inputConstructorArg = GenerateInputString(inputs) + ", ";
                }
                else if (inputs.Length > 1)
                {
                    inputConstructorArg = "(" + GenerateInputString(inputs) + ") args, ";
                }

                var encoderLines = new string[inputs.Length];
                if (inputs.Length > 1)
                {
                    for (var i = 0; i < inputs.Length; i++)
                    {
                        encoderLines[i] = $"EncoderFactory.LoadEncoder(\"{constructorAbi.Inputs[i].Type}\", args.{inputs[i].Name})";
                    }
                    inputEncoders = string.Join(", ", encoderLines);
                }
                else
                {
                    inputEncoders = $"EncoderFactory.LoadEncoder(\"{constructorAbi.Inputs[0].Type}\", {inputs[0].Name})";
                }
            }

            return $@"
                public static async Task<{_name}> New(
                    JsonRpcClient rpcClient, 
                    {inputConstructorArg}
                    SendParams sendParams,
                    Address defaultFromAccount)
                {{
                    var encodedParams = EncoderUtil.GetBytes(
                        {inputEncoders}
                    );

                    var contractAddr = await ContractFactory.Deploy(rpcClient, BYTECODE_HEX.HexToReadOnlyMemory(), encodedParams, sendParams);
                    return new {_name}(rpcClient, contractAddr, defaultFromAccount);
                }}
            ";
        }

        string GenerateFunction(Abi methodAbi)
        {
            var callDataParams = new List<string>();
         
            string functionSig = GetFunctionSignature(methodAbi);
            callDataParams.Add($"\"{functionSig}\"");

            string inputConstructorArg = string.Empty;
            bool hasInputs = methodAbi.Inputs.Length > 0;
            if (hasInputs)
            {
                var inputs = GenerateInputs(methodAbi.Inputs);
                inputConstructorArg = GenerateInputString(inputs);

                for (var i = 0; i < inputs.Length; i++)
                {
                    var encoderLine = $"EncoderFactory.LoadEncoder(\"{methodAbi.Inputs[i].Type}\", {inputs[i].Name})";
                    callDataParams.Add(encoderLine);
                }
            }

            string callDataString = string.Join(", ", callDataParams);

            var outputs = GenerateOutputs(methodAbi.Outputs);
            string outputParams = GenerateOutputString(outputs);
            string outputClrTypes = string.Empty;
            if (outputs.Length > 0)
            {
                outputClrTypes = "<" + string.Join(", ", outputs.Select(s => s.Type)) + ">";
            }
            string returnType;
            if (outputs.Length == 0)
            {
                returnType = string.Empty;
            }
            else if (outputs.Length == 1)
            {
                returnType = $"<{outputs[0].Type}>";
            }
            else
            {
                returnType = $"<({outputParams})>";
            }

            string[] decoderParams = new string[outputs.Length];
            for(var i = 0; i < outputs.Length; i++)
            {
                string decoder;
                if (outputs[i].AbiType.IsArrayType)
                {
                    decoder = $"DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder(\"{outputs[i].AbiType.ArrayItemInfo.SolidityName}\", default({outputs[i].AbiType.ArrayItemInfo.ClrTypeName})))";
                }
                else
                {
                    decoder = "DecoderFactory.Decode";
                }
                decoderParams[i] = $"\"{methodAbi.Outputs[i].Type}\", {decoder}";
            }

            string decoderStr;
            if (outputs.Length > 0)
            {
                decoderStr = "this, callData, " + string.Join(", ", decoderParams);
            }
            else
            {
                decoderStr = "this, callData";
            }

            return $@"
                public EthFunc{returnType} {methodAbi.Name}({inputConstructorArg})
                {{
                    var callData = GetCallData({callDataString});

                    return EthFunc.Create{outputClrTypes}({decoderStr});
                }}
            ";
        }
        
        string GenerateFallbackFunction(Abi methodAbi)
        {
            return null;
        }

        string GenerateEvent(Abi eventAbi)
        {
            var inputs = GenerateInputs(eventAbi.Inputs);
            string[] propertyLines = new string[eventAbi.Inputs.Length];
            string[] paramArgs = new string[eventAbi.Inputs.Length];
            string[] propAssignmentLines = new string[eventAbi.Inputs.Length];
            string[] logBoxArgs = new string[eventAbi.Inputs.Length];

            for (var i = 0; i < eventAbi.Inputs.Length; i++)
            {
                string clrType;
                if (inputs[i].AbiType.IsDynamicType)
                {
                    clrType = typeof(Hash).FullName;
                }
                else
                {
                    clrType = inputs[i].Type;
                }
                propertyLines[i] = $"public readonly (string Name, string Type, bool Indexed, {clrType} Value) {eventAbi.Inputs[i].Name};";
                paramArgs[i] = $"{clrType} _{eventAbi.Inputs[i].Name}";
                propAssignmentLines[i] = $"{eventAbi.Inputs[i].Name} = (\"{eventAbi.Inputs[i].Name}\", \"{eventAbi.Inputs[i].Type}\", {eventAbi.Inputs[i].Indexed.Value.ToString().ToLowerInvariant()}, _{eventAbi.Inputs[i].Name});";
                logBoxArgs[i] = $"Box({eventAbi.Inputs[i].Name})";
            }

            string propertyLinesString = string.Join(Environment.NewLine, propertyLines);
            string paramArgsString = string.Empty;
            if (paramArgs.Length > 0)
            {
                paramArgsString = ", " + string.Join(", ", paramArgs);
            }
            string propAssignmentString = string.Join(Environment.NewLine, propAssignmentLines);
            string logBoxArgString = string.Join(", ", logBoxArgs);

            return $@"
                public class Event_{eventAbi.Name} : EventLog
                {{
                    public override string EventName {{ get; }} = ""{eventAbi.Name}"";

                    {propertyLinesString}

                    public Event_{eventAbi.Name}(
                        Address address, Hash? blockHash, long? blockNumber, long? logIndex, Hash? transactionHash
                        {paramArgsString})
                        : base(address, blockHash, blockNumber, logIndex, transactionHash)
                    {{
                        {propAssignmentString}

                        LogArgs = new (string, string, bool, object)[] {{ {logBoxArgString} }};
                    }}
                }}
            ";
        }

        string GetFunctionSignature(Abi methodAbi)
        {
            return $"{methodAbi.Name}({string.Join(",", methodAbi.Inputs.Select(i => i.Type))})";
        }

        (string Name, string Type, AbiTypeInfo AbiType)[] GenerateInputs(Input[] inputs)
        {
            (string, string, AbiTypeInfo)[] items = new (string, string, AbiTypeInfo)[inputs.Length];
            int unnamed = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var input = inputs[i];
                var abiType = AbiTypeMap.GetSolidityTypeInfo(input.Type);
                var type = abiType.ClrTypeName;
                var name = string.IsNullOrEmpty(input.Name) ? $"unamed{unnamed++}" : input.Name;
                items[i] = (name, type, abiType);
            }
            return items;
        }

        string GenerateInputString((string Name, string Type, AbiTypeInfo AbiType)[] inputs)
        {
            string[] items = new string[inputs.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = inputs[i].Type + " " + inputs[i].Name;
            }
            return string.Join(", ", items);
        }

        string GenerateInputString(Input[] inputs)
        {
            var p = GenerateInputs(inputs);
            return GenerateInputString(p);
        }

        (string Name, string Type, AbiTypeInfo AbiType)[] GenerateOutputs(Output[] outputs)
        {
            (string, string, AbiTypeInfo)[] items = new(string, string, AbiTypeInfo)[outputs.Length];
            int unnamed = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var output = outputs[i];
                var abiType = AbiTypeMap.GetSolidityTypeInfo(output.Type);
                var type = abiType.ClrTypeName;
                var name = string.IsNullOrEmpty(output.Name) ? $"unamed{unnamed++}" : output.Name;
                items[i] = (name, type, abiType);
            }
            return items;
        }


        string GenerateOutputString((string Name, string Type, AbiTypeInfo abiType)[] outputs)
        {
            string[] items = new string[outputs.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = outputs[i].Type + " " + outputs[i].Name;
            }
            return string.Join(", ", items);
        }

        string GenerateOutputString(Output[] outputs)
        {
            var p = GenerateOutputs(outputs);
            return GenerateOutputString(p);
        }

        string GenerateFunctionSignature(Abi abiItem)
        {
            var types = new string[abiItem.Inputs.Length];
            for (var i = 0; i < types.Length; i++)
            {
                types[i] = abiItem.Inputs[i].Type;
            }
            return $"{abiItem.Name}({string.Join(",", types)})";
        }


    }
}

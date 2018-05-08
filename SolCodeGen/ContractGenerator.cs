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
                using System;
                using System.Collections.Generic;
                using System.Globalization;
                using System.Linq;
                using System.Numerics;
                using System.Text;
                using System.Threading.Tasks;
                using SolCodeGen;

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
            return $@"
                public {_name}({GenerateParameters(constructorAbi.Inputs)})
                {{
                    

                }}
            ";
        }

        string GenerateFunction(Abi methodAbi)
        {
            var returnType = methodAbi.Outputs.Any() ? $"Task<({GenerateOutputType(methodAbi.Outputs)})>" : "Task";

            return $@"
                public {returnType} {methodAbi.Name}({GenerateParameters(methodAbi.Inputs)})
                {{
                    

                }}
            ";
        }
        
        string GenerateFallbackFunction(Abi methodAbi)
        {
            return null;
        }

        string GenerateEvent(Abi eventAbi)
        {
            var parameters = GenerateParameters(eventAbi.Inputs);

            return $@"
                public class Event_{eventAbi.Name} : EventLog<({parameters})>
                {{
                    public Event_{eventAbi.Name}(({parameters}) logData, EventLog eventLog) : base(logData, eventLog)
                    {{
                    }}
                }}
            ";
        }

        string GenerateParameters(Input[] inputs)
        {
            string[] items = new string[inputs.Length];
            int unnamed = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var input = inputs[i];
                var type = SolidityTypeMap.SolidityTypeToClrTypeString(input.Type);
                var name = string.IsNullOrEmpty(input.Name) ? $"unamed{unnamed++}" : input.Name;
                items[i] = type + " " + name;
            }
            return string.Join(", ", items);
        }

        string GenerateOutputType(Output[] outputs)
        {
            string[] items = new string[outputs.Length];
            int unnamed = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var output = outputs[i];
                var type = SolidityTypeMap.SolidityTypeToClrTypeString(output.Type);
                var name = string.IsNullOrEmpty(output.Name) ? $"unamed{unnamed++}" : output.Name;
                items[i] = type + " " + name;
            }
            return string.Join(", ", items);
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

﻿using SolCodeGen.AbiEncoding;
using SolCodeGen.AbiEncoding.Encoders;
using SolCodeGen.Contract;
using SolCodeGen.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests.Abi
{
    public class AbiDecoding
    {
        [Fact]
        public void Address()
        {
            var encodedAddr = "00000000000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae";
            var buff = new AbiDecodeBuffer(encodedAddr, "address");
            DecoderFactory.Decode("address", ref buff, out Address address);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.Equal("0x11f4d0A3c12e86B4b5F39B213F7E19D048276DAe".ToLowerInvariant(), address.ToString());
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Address_BadInput()
        {
            
            var encodedAddr = "00000020000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae";
            try
            {
                DecoderFactory.Decode("address", ref encodedAddr, out Address addr);
                throw null;
            }
            catch (ArgumentException) { }
            

            var encodedAddr2 = "00000000000000000000000811f4d0a3c12e86b4b5f39b213f7e19d048276dae";
            try
            {
                DecoderFactory.Decode("address", ref encodedAddr2, out Address addr);
                throw null;
            }
            catch (ArgumentException)
            {

            }
        }
        */

        [Fact]
        public void Boolean_True()
        {
            var encodedTrue = "0000000000000000000000000000000000000000000000000000000000000001";
            var buff = new AbiDecodeBuffer(encodedTrue, "bool");
            DecoderFactory.Decode("bool", ref buff, out bool decodedTrue);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.True(decodedTrue);

        }

        [Fact]
        public void Boolean_False()
        {
            var encodedFalse = "0000000000000000000000000000000000000000000000000000000000000000";
            var buff = new AbiDecodeBuffer(encodedFalse, "bool");
            DecoderFactory.Decode("bool", ref buff, out bool decodedFalse);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.False(decodedFalse);
        }

        [Fact]
        public void Boolean_BadInput_1()
        {
            var encodedTrue = "0000000000000000000000000000000000000000000000000000000000000021";
            var buff = new AbiDecodeBuffer(encodedTrue, "bool");
            try
            {
                DecoderFactory.Decode("bool", ref buff, out bool val);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void Boolean_BadInput_2()
        {
            var encodedTrue = "0000000000000000000000000000000000000000000000000000000000000002";
            var buff = new AbiDecodeBuffer(encodedTrue, "bool");
            try
            {
                DecoderFactory.Decode("bool", ref buff, out bool val);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void Bytes_M()
        {
            var encodedBytes22 = "072696e74657220746f6f6b20612067616c6c657920600000000000000000000";
            var buff = new AbiDecodeBuffer(encodedBytes22, "bytes22");
            DecoderFactory.Decode("bytes22", ref buff, out byte[] result);
            Assert.Equal(0, buff.HeadCursor.Length);

            byte[] expected = HexConverter.HexToBytes("072696e74657220746f6f6b20612067616c6c6579206");
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Bytes_M_BadInput()
        {
            var encodedBytes22 = "072696e74657220746f6f6b20612067616c6c657920600000000040000000000";
            try
            {
                DecoderFactory.Decode("bytes22", ref encodedBytes22, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }
        */

        [Fact]
        public void Bytes()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000";
            var buff = new AbiDecodeBuffer(encodedBytes, "bytes");
            DecoderFactory.Decode("bytes", ref buff, out byte[] result);
            Assert.Equal(0, buff.HeadCursor.Length);

            byte[] expected = "207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970".HexToBytes();
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Bytes_BadInput_BadPadding()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970100000";
            try
            {
                DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }
        */

        [Fact]
        public void Bytes_BadInput_BadFixedPrefix ()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000";
            var buff = new AbiDecodeBuffer(encodedBytes, "bytes");
            try
            {
                DecoderFactory.Decode("bytes", ref buff, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
        }

        [Fact]
        public void Bytes_BadInput_BadLengthPrefix()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000010000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000";
            var buff = new AbiDecodeBuffer(encodedBytes, "bytes");
            try
            {
                DecoderFactory.Decode("bytes", ref buff, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
        }

        [Fact]
        public void String()
        {
            var encodedStr = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000";
            var buff = new AbiDecodeBuffer(encodedStr, "bool");
            DecoderFactory.Decode("string", ref buff, out string result);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.Equal("Hello, world!", result);
        }

        [Fact]
        public void StringUnicode()
        {
            var encodedStr = "00000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000028757466383b20342062797465733a20f0a0beb43b20332062797465733a20e28fb020776f726b7321000000000000000000000000000000000000000000000000";
            var buff = new AbiDecodeBuffer(encodedStr, "string");
            DecoderFactory.Decode("string", ref buff, out string result);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.Equal("utf8; 4 bytes: 𠾴; 3 bytes: ⏰ works!", result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void String_BadInput_BadPadding()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642101000000000000000000000000000000000000";
            try
            {
                DecoderFactory.Decode("string", ref encodedBytes, out string result);
                throw null;
            }
            catch (ArgumentException) { }
        }
        */

        [Fact]
        public void String_BadInput_BadFixedPrefix()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000";
            var buff = new AbiDecodeBuffer(encodedBytes, "bool");
            try
            {
                DecoderFactory.Decode("bytes", ref buff, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
        }

        [Fact]
        public void String_BadInput_BadLengthPrefix()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000060000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000";
            var buff = new AbiDecodeBuffer(encodedBytes, "bytes");
            try
            {
                DecoderFactory.Decode("bytes", ref buff, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
        }

        [Fact]
        public void Int24_1()
        {
            var encodedNum = "0000000000000000000000000000000000000000000000000000000000012da0";
            var buff = new AbiDecodeBuffer(encodedNum, "int24");
            DecoderFactory.Decode("int24", ref buff, out int result);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.Equal(77216, result);
        }

        [Fact]
        public void Int24_2()
        {
            var encodedNum = "0000000000000000000000000000000000000000000000000000000000fed260";
            var buff = new AbiDecodeBuffer(encodedNum, "int24");
            DecoderFactory.Decode("int24", ref buff, out int result);
            Assert.Equal(0, buff.HeadCursor.Length);
            Assert.Equal(-77216, result);
        }

        [Fact]
        public void Int56()
        {
            var encodedNum = "00000000000000000000000000000000000000000000000000ffffffffffd492";
            var buff = new AbiDecodeBuffer(encodedNum, "int56");
            DecoderFactory.Decode("int56", ref buff, out long result);
            Assert.Equal(0, buff.HeadCursor.Length);
            var expected = (long)-11118;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void UInt24()
        {
            var encodedNum = "0000000000000000000000000000000000000000000000000000000000005ba0";
            var buff = new AbiDecodeBuffer(encodedNum, "uint24");
            DecoderFactory.Decode("uint24", ref buff, out uint result);
            Assert.Equal(0, buff.HeadCursor.Length);
            uint expected = 23456;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void UInt32()
        {
            var encodedNum = "00000000000000000000000000000000000000000000000000000000ffff5544";
            var buff = new AbiDecodeBuffer(encodedNum, "uint32");
            DecoderFactory.Decode("uint32", ref buff, out uint result);
            Assert.Equal(0, buff.HeadCursor.Length);
            uint expected = 4294923588;
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void UInt32_BadInput()
        {
            var encodedNum = "00000000030000000000000000000000000000000000000000000000ffff5544";
            try
            {
                DecoderFactory.Decode("uint32", ref encodedNum, out uint result);
                throw null;
            }
            catch (ArgumentException) { }
        }
        */

        [Fact]
        public void Int64DynamicArray()
        {
            var encodedArr = "00000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000005000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff";
            var buff = new AbiDecodeBuffer(encodedArr, "int64[]");
            DecoderFactory.Decode("int64[]", ref buff, out long[] result, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Equal(0, buff.HeadCursor.Length);
            long[] expected = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            Assert.Equal(expected, result);
        }


        [Fact]
        public void Int64FixedArray()
        {
            var encodedArr = "000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff";
            var buff = new AbiDecodeBuffer(encodedArr, "int64[5]");
            DecoderFactory.Decode("int64[5]", ref buff, out long[] result, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Equal(0, buff.HeadCursor.Length);
            long[] expected = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            Assert.Equal(expected, result);
        }


        [Fact]
        public void UInt8FixedArray()
        {
            var encodedArr = "00000000000000000000000000000000000000000000000000000000000000070000000000000000000000000000000000000000000000000000000000000026000000000000000000000000000000000000000000000000000000000000009600000000000000000000000000000000000000000000000000000000000000e70000000000000000000000000000000000000000000000000000000000000046";
            var buff = new AbiDecodeBuffer(encodedArr, "uint8[5]");
            DecoderFactory.Decode("uint8[5]", ref buff, out byte[] result, EncoderFactory.LoadEncoder("uint8", default(byte)));
            Assert.Equal(0, buff.HeadCursor.Length);
            byte[] expected = HexConverter.HexToBytes("072696e746");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FunctionData_MultipleStringParams()
        {
            var strP1 = "first string";
            var strP2 = "asdf";
            var strP3 = "utf8; 4 bytes: 𠾴; 3 bytes: ⏰ works!";

            var encoded = "000000000000000000000000000000000000000000000000000000000000006000000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000c666972737420737472696e670000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000461736466000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000028757466383b20342062797465733a20f0a0beb43b20332062797465733a20e28fb020776f726b7321000000000000000000000000000000000000000000000000";

            AbiDecodeBuffer buff = new AbiDecodeBuffer(encoded, "string", "string", "string");

            DecoderFactory.Decode("string", ref buff, out string ru1);
            Assert.Equal(strP1, ru1);

            DecoderFactory.Decode("string", ref buff, out string ru2);
            Assert.Equal(strP2, ru2);

            DecoderFactory.Decode("string", ref buff, out string ru3);
            Assert.Equal(strP3, ru3);
        }

        [Fact]
        public void FunctionData_MixedParamTypes()
        {
            var encoded = "0x00000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000100ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffd4920000000000000000000000000000000000000000000000000000000000000140000000000000000000000000000000000000000000000000000000000000006300000000000000000000000000000000000000000000000000000000000000090000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffffffffffff00000000000000000000000000000000000000000000000000000000000000096d7920737472696e670000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000300000000000000000000000098e4625b2d7424c403b46366150ab28df406340800000000000000000000000040515114eea1497d753659dff85910f838c6b234000000000000000000000000df0270a6bff43e7a3fd92372dfb549292d683d22";
            var ethFunc = EthFunc.Create<bool, string, long, Address[], byte, ulong[]>(
                null, null,
                "bool", DecoderFactory.Decode,
                "string", DecoderFactory.Decode,
                "int56", DecoderFactory.Decode,
                "address[]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("address", default(Address))),
                "uint8", DecoderFactory.Decode,
                "uint64[3]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("uint64", default(ulong))));

            var p1 = true;
            var p2 = "my string";
            var p3 = (long)-11118;
            var p4 = new Address[] { "0x98E4625b2d7424C403B46366150AB28Df4063408", "0x40515114eEa1497D753659DFF85910F838c6B234", "0xDf0270A6BFf43e7A3Fd92372DfB549292D683D22" };
            var p5 = (byte)99;
            var p6 = new ulong[] { 9, 0, ulong.MaxValue };

            var (r1, r2, r3, r4, r5, r6) = ethFunc.ParseReturnData(encoded);

            Assert.Equal(p1, r1);
            Assert.Equal(p2, r2);
            Assert.Equal(p3, r3);
            Assert.Equal(p4, r4);
            Assert.Equal(p5, r5);
            Assert.Equal(p6, r6);
        }


    }
}

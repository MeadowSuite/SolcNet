using SolCodeGen.AbiEncoding;
using SolCodeGen.AbiEncoding.Encoders;
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
            var encodedAddr = "00000000000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae".HexToReadOnlySpan();
            DecoderFactory.Decode("address", ref encodedAddr, out Address address);
            Assert.Equal(0, encodedAddr.Length);
            Assert.Equal("0x11f4d0A3c12e86B4b5F39B213F7E19D048276DAe".ToLowerInvariant(), address.ToString());
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Address_BadInput()
        {
            
            var encodedAddr = "00000020000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("address", ref encodedAddr, out Address addr);
                throw null;
            }
            catch (ArgumentException) { }
            

            var encodedAddr2 = "00000000000000000000000811f4d0a3c12e86b4b5f39b213f7e19d048276dae".HexToReadOnlySpan();
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
        public void Boolean()
        {
            var encodedTrue = "0000000000000000000000000000000000000000000000000000000000000001".HexToReadOnlySpan();
            DecoderFactory.Decode("bool", ref encodedTrue, out bool decodedTrue);
            Assert.Equal(0, encodedTrue.Length);
            Assert.True(decodedTrue);

            var encodedFalse = "0000000000000000000000000000000000000000000000000000000000000000".HexToReadOnlySpan();
            DecoderFactory.Decode("bool", ref encodedFalse, out bool decodedFalse);
            Assert.Equal(0, encodedFalse.Length);
            Assert.False(decodedFalse);
        }

        [Fact]
        public void Boolean_BadInput()
        {
            var encodedTrue = "0000000000000000000000000000000000000000000000000000000000000021".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bool", ref encodedTrue, out bool val);
                throw null;
            }
            catch (ArgumentException) { }

            var encodedTrue2 = "0000000000000000000000000000000000000000000000000000000000000002".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bool", ref encodedTrue2, out bool val);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void Bytes_M()
        {
            var encodedBytes22 = "072696e74657220746f6f6b20612067616c6c657920600000000000000000000".HexToReadOnlySpan();
            DecoderFactory.Decode("bytes22", ref encodedBytes22, out byte[] result);
            Assert.Equal(0, encodedBytes22.Length);

            byte[] expected = HexConverter.HexToBytes("072696e74657220746f6f6b20612067616c6c6579206");
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Bytes_M_BadInput()
        {
            var encodedBytes22 = "072696e74657220746f6f6b20612067616c6c657920600000000040000000000".HexToReadOnlySpan();
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
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000".HexToReadOnlySpan();
            DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
            Assert.Equal(0, encodedBytes.Length);

            byte[] expected = HexConverter.HexToBytes("207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970");
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void Bytes_BadInput_BadPadding()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970100000".HexToReadOnlySpan();
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
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000030000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void Bytes_BadInput_BadLengthPrefix()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000010000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void String()
        {
            var encodedStr = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000".HexToReadOnlySpan();
            DecoderFactory.Decode("string", ref encodedStr, out string result);
            Assert.Equal(0, encodedStr.Length);
            Assert.Equal("Hello, world!", result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void String_BadInput_BadPadding()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642101000000000000000000000000000000000000".HexToReadOnlySpan();
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
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void String_BadInput_BadLengthPrefix()
        {
            var encodedBytes = "0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000060000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000".HexToReadOnlySpan();
            try
            {
                DecoderFactory.Decode("bytes", ref encodedBytes, out byte[] result);
                throw null;
            }
            catch (ArgumentException) { }
        }

        [Fact]
        public void Int24()
        {
            var encodedNum = "0000000000000000000000000000000000000000000000000000000000012da0".HexToReadOnlySpan();
            DecoderFactory.Decode("int24", ref encodedNum, out int result);
            Assert.Equal(0, encodedNum.Length);
            Assert.Equal(77216, result);
        }

        [Fact]
        public void UInt24()
        {
            var encodedNum = "0000000000000000000000000000000000000000000000000000000000005ba0".HexToReadOnlySpan();
            DecoderFactory.Decode("uint24", ref encodedNum, out uint result);
            Assert.Equal(0, encodedNum.Length);
            uint expected = 23456;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void UInt32()
        {
            var encodedNum = "00000000000000000000000000000000000000000000000000000000ffff5544".HexToReadOnlySpan();
            DecoderFactory.Decode("uint32", ref encodedNum, out uint result);
            Assert.Equal(0, encodedNum.Length);
            uint expected = 4294923588;
            Assert.Equal(expected, result);
        }

        // Zero-bytes padding verification disabled; ganache liters padding with garbage bytes
        /*
        [Fact]
        public void UInt32_BadInput()
        {
            var encodedNum = "00000000030000000000000000000000000000000000000000000000ffff5544".HexToReadOnlySpan();
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
            var encodedArr = "00000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000005000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff".HexToReadOnlySpan();
            DecoderFactory.Decode("int64[]", ref encodedArr, out long[] result, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Equal(0, encodedArr.Length);
            long[] expected = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            Assert.Equal(expected, result);
        }


        [Fact]
        public void Int64FixedArray()
        {
            var encodedArr = "000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff".HexToReadOnlySpan();
            DecoderFactory.Decode("int64[5]", ref encodedArr, out long[] result, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Equal(0, encodedArr.Length);
            long[] expected = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            Assert.Equal(expected, result);
        }


        [Fact]
        public void UInt8FixedArray()
        {
            var encodedArr = "00000000000000000000000000000000000000000000000000000000000000070000000000000000000000000000000000000000000000000000000000000026000000000000000000000000000000000000000000000000000000000000009600000000000000000000000000000000000000000000000000000000000000e70000000000000000000000000000000000000000000000000000000000000046".HexToReadOnlySpan();
            DecoderFactory.Decode("uint8[5]", ref encodedArr, out byte[] result, EncoderFactory.LoadEncoder("uint8", default(byte)));
            Assert.Equal(0, encodedArr.Length);
            byte[] expected = HexConverter.HexToBytes("072696e746");
            Assert.Equal(expected, result);
        }


    }
}

﻿using SolCodeGen.AbiEncoding;
using SolCodeGen.AbiEncoding.Encoders;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests.Abi
{
    public class AbiEncoding
    {
        [Fact]
        public void FunctionSelector()
        {
            var result = MethodID.GetMethodIDHex("baz(uint32,bool)", hexPrefix: true);
            Assert.Equal("0xcdcd77c0", result);
        }

        [Fact]
        public void FunctionData_MultipleStringParams()
        {
            var funcSig = "echoMultipleDynamic(string,string,string)";
            var strP1 = "first string";
            var strP2 = "asdf";
            var strP3 = "utf8; 4 bytes: 𠾴; 3 bytes: ⏰ works!";
            var callData = BaseContract.GetCallData(funcSig,
                EncoderFactory.LoadEncoder("string", strP1),
                EncoderFactory.LoadEncoder("string", strP2),
                EncoderFactory.LoadEncoder("string", strP3));
            var expectedEncode = "0x14d6b8fa000000000000000000000000000000000000000000000000000000000000006000000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000c666972737420737472696e670000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000461736466000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000028757466383b20342062797465733a20f0a0beb43b20332062797465733a20e28fb020776f726b7321000000000000000000000000000000000000000000000000";
            Assert.Equal(expectedEncode, callData);
        }

        [Fact]
        public void FunctionData_MixedParamTypes()
        {
            var p1 = true;
            var p2 = "my string";
            var p3 = (long)-11118;
            var p4 = new Address[] { "0x98E4625b2d7424C403B46366150AB28Df4063408", "0x40515114eEa1497D753659DFF85910F838c6B234", "0xDf0270A6BFf43e7A3Fd92372DfB549292D683D22" };
            var p5 = (byte)99;
            var p6 = new ulong[] { 9, 0, ulong.MaxValue };
            var callData = BaseContract.GetCallData(
                "boat(bool,string,int56,address[],uint8,uint64[3])",
                EncoderFactory.LoadEncoder("bool", p1),
                EncoderFactory.LoadEncoder("string", p2),
                EncoderFactory.LoadEncoder("int56", p3),
                EncoderFactory.LoadEncoder("address[]", p4, EncoderFactory.LoadEncoder("address", default(Address))),
                EncoderFactory.LoadEncoder("uint8", p5),
                EncoderFactory.LoadEncoder("uint64[3]", p6, EncoderFactory.LoadEncoder("uint64", default(ulong))));

            var expected = "0x7a4a328f0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000ffffffffffd4920000000000000000000000000000000000000000000000000000000000000140000000000000000000000000000000000000000000000000000000000000006300000000000000000000000000000000000000000000000000000000000000090000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ffffffffffffffff00000000000000000000000000000000000000000000000000000000000000096d7920737472696e670000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000300000000000000000000000098e4625b2d7424c403b46366150ab28df406340800000000000000000000000040515114eea1497d753659dff85910f838c6b234000000000000000000000000df0270a6bff43e7a3fd92372dfb549292d683d22";
            Assert.Equal(expected, callData);
        }

        [Fact]
        public void Address()
        {
            Address myAddr = "0x11f4d0A3c12e86B4b5F39B213F7E19D048276DAe";
            var encoder = EncoderFactory.LoadEncoder("address", myAddr);
            Assert.IsType<AddressEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buffer = new AbiEncodeBuffer(data, "address");
            encoder.Encode(ref buffer);
            Assert.Equal(0, buffer.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("00000000000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae", result);
        }

        [Fact]
        public void UInt24_1()
        {
            uint num = 16777216;
            var encoder = EncoderFactory.LoadEncoder("uint24", num);
            Assert.IsType<UInt32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "uint24");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", result);
        }

        [Fact]
        public void UInt24_2()
        {
            uint num = 16777217;
            Assert.Throws<OverflowException>(() => EncoderFactory.LoadEncoder("uint24", num));
        }


        [Fact]
        public void UInt24_3()
        {
            uint num = 23456;
            var encoder = EncoderFactory.LoadEncoder("uint24", num);
            Assert.IsType<UInt32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "uint24");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000005ba0", result);
        }

        [Fact]
        public void Int24_1()
        {
            int num = 77216;
            var encoder = EncoderFactory.LoadEncoder("int24", num);
            Assert.IsType<Int32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int24");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000012da0", result);
        }

        [Fact]
        public void Int24_2()
        {
            int num = -77216;
            var encoder = EncoderFactory.LoadEncoder("int24", num);
            Assert.IsType<Int32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int24");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000fed260", result);
        }

        [Fact]
        public void Int24_3()
        {
            int num = 8388610;
            Assert.Throws<OverflowException>(() => EncoderFactory.LoadEncoder("int24", num));
        }

        [Fact]
        public void Int24_4()
        {
            int num = -8388609;
            Assert.Throws<OverflowException>(() => EncoderFactory.LoadEncoder("int24", num));
        }

        [Fact]
        public void Int56()
        {
            var num = (long)-11118;
            var encoder = EncoderFactory.LoadEncoder("int56", num);
            Assert.IsType<Int64Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int56");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("00000000000000000000000000000000000000000000000000ffffffffffd492", result);

        }

        [Fact]
        public void UInt32()
        {
            uint num = 4294923588;
            var encoder = EncoderFactory.LoadEncoder("uint32", num);
            Assert.IsType<UInt32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "uint32");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000ffff5544", result);
        }

        [Fact]
        public void UInt256()
        {
            UInt256 num = 4294923588;
            var encoder = EncoderFactory.LoadEncoder("uint256", num);
            Assert.IsType<UInt256Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "uint256");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000ffff5544", result);
        }

        [Fact]
        public void Int256_1()
        {
            BigInteger num = -4294923588;
            var encoder = EncoderFactory.LoadEncoder("int256", num);
            Assert.IsType<Int256Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int256");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("000000000000000000000000000000000000000000000000000000bcaa0000ff", result);
        }

        [Fact]
        public void Int256_2()
        {
            BigInteger num = 4294923588;
            var encoder = EncoderFactory.LoadEncoder("int256", num);
            Assert.IsType<Int256Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int256");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data, hexPrefix: false);
            Assert.Equal("0000000000000000000000000000000000000000000000000000004455ffff00", result);
        }

        [Fact]
        public void Boolean_True()
        {
            bool boolean = true;
            var encoder = EncoderFactory.LoadEncoder("bool", boolean);
            Assert.IsType<BoolEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bool");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001", result);
        }

        [Fact]
        public void Boolean_False()
        {
            bool boolean = false;
            var encoder = EncoderFactory.LoadEncoder("bool", boolean);
            Assert.IsType<BoolEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bool");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", result);
        }

        [Fact]
        public void StaticBoolArray()
        {
            IEnumerable<bool> arr = new[] { true, true, false, true, };
            var encoder = EncoderFactory.LoadEncoder("bool[4]", arr, EncoderFactory.LoadEncoder("bool", default(bool)));
            Assert.IsType<FixedArrayEncoder<bool>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(128, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bool[4]");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001", result);
        }

        [Fact]
        public void DynamicBoolArray()
        {
            IEnumerable<bool> arr = new[] { true, true, false, true, };
            var encoder = EncoderFactory.LoadEncoder("bool[]", arr, EncoderFactory.LoadEncoder("bool", default(bool)));
            Assert.IsType<DynamicArrayEncoder<bool>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(192, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bool[]");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001", result);
        }

        [Fact]
        public void String()
        {
            var str = "Hello, world!";
            var encoder = EncoderFactory.LoadEncoder("string", str);
            Assert.IsType<StringEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(96, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "string");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000d48656c6c6f2c20776f726c642100000000000000000000000000000000000000", result);
        }

        [Fact]
        public void StringUnicode()
        {
            var str = "utf8; 4 bytes: 𠾴; 3 bytes: ⏰ works!";
            var encoder = EncoderFactory.LoadEncoder("string", str);
            Assert.IsType<StringEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(128, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "string");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000028757466383b20342062797465733a20f0a0beb43b20332062797465733a20e28fb020776f726b7321000000000000000000000000000000000000000000000000", result);

        }

        [Fact]
        public void LargeString()
        {
            var str = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.";
            var encoder = EncoderFactory.LoadEncoder("string", str);
            Assert.IsType<StringEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(320, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "string");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000f54c6f72656d20497073756d2069732073696d706c792064756d6d792074657874206f6620746865207072696e74696e6720616e64207479706573657474696e6720696e6475737472792e204c6f72656d20497073756d20686173206265656e2074686520696e6475737472792773207374616e646172642064756d6d79207465787420657665722073696e6365207468652031353030732c207768656e20616e20756e6b6e6f776e207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970652073706563696d656e20626f6f6b2e0000000000000000000000", result);
        }

        [Fact]
        public void Bytes()
        {
            byte[] bytes = HexConverter.HexToBytes("207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970");
            var encoder = EncoderFactory.LoadEncoder("bytes", bytes);
            Assert.IsType<BytesEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(128, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bytes");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003d207072696e74657220746f6f6b20612067616c6c6579206f66207479706520616e6420736372616d626c656420697420746f206d616b65206120747970000000", result);
        }

        [Fact]
        public void Bytes_M()
        {
            byte[] bytes = HexConverter.HexToBytes("072696e74657220746f6f6b20612067616c6c6579206");
            var encoder = EncoderFactory.LoadEncoder("bytes22", bytes);
            Assert.IsType<BytesMEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(32, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "bytes22");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("072696e74657220746f6f6b20612067616c6c657920600000000000000000000", result);
        }

        [Fact]
        public void UInt8FixedArray()
        {
            byte[] bytes = HexConverter.HexToBytes("072696e746");
            var encoder = EncoderFactory.LoadEncoder("uint8[5]", bytes, EncoderFactory.LoadEncoder("uint8", default(byte)));
            Assert.IsType<FixedArrayEncoder<byte>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(160, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "uint8[5]");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000070000000000000000000000000000000000000000000000000000000000000026000000000000000000000000000000000000000000000000000000000000009600000000000000000000000000000000000000000000000000000000000000e70000000000000000000000000000000000000000000000000000000000000046", result);
        }

        [Fact]
        public void Int64DynamicArray()
        {
            long[] bytes = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            var encoder = EncoderFactory.LoadEncoder("int64[]", bytes, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.IsType<DynamicArrayEncoder<long>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(224, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int64[]");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000005000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff", result);
        }

        [Fact]
        public void Int64FixedArray()
        {
            long[] bytes = new long[] { 1, 4546, long.MaxValue, 0, long.MaxValue };
            var encoder = EncoderFactory.LoadEncoder("int64[5]", bytes, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.IsType<FixedArrayEncoder<long>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.True(encodedSize % 32 == 0);
            Assert.Equal(160, encodedSize);
            Span<byte> data = new byte[encodedSize];
            var buff = new AbiEncodeBuffer(data, "int64[5]");
            encoder.Encode(ref buff);
            Assert.Equal(0, buff.HeadCursor.Length);
            var result = HexConverter.GetHexFromBytes(data);
            Assert.Equal("000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000011c20000000000000000000000000000000000000000000000007fffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007fffffffffffffff", result);
        }


        [Fact]
        public void FixedArrayUndersizedException()
        {
            long[] bytes = new long[] { 1, 4546, long.MaxValue, 0 };
            var encoder = EncoderFactory.LoadEncoder("int64[5]", bytes, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Throws<ArgumentOutOfRangeException>(() => encoder.ToEncodedHex());
        }

        [Fact]
        public void FixedArrayOversizedException()
        {
            long[] bytes = new long[] { 1, 4546, long.MaxValue, 0, 1, 2 };
            var encoder = EncoderFactory.LoadEncoder("int64[5]", bytes, EncoderFactory.LoadEncoder("int64", default(long)));
            Assert.Throws<ArgumentOutOfRangeException>(() => encoder.ToEncodedHex());
        }

    }
}

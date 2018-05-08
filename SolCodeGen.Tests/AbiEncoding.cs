using SolCodeGen.SolidityTypeEncoding;
using SolCodeGen.SolidityTypeEncoding.Encoders;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests
{
    public class AbiEncoding
    {
        [Fact]
        public void FunctionSelector()
        {
            var result = MethodID.GetMethodID("baz(uint32,bool)", hexPrefix: true);
            Assert.Equal("0xcdcd77c0", result);
        }

        [Fact]
        public void Address()
        {
            Address myAddr = "0x11f4d0A3c12e86B4b5F39B213F7E19D048276DAe";
            var encoder = EncoderFactory.LoadEncoder("address", myAddr);
            Assert.IsType<AddressEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> buffer = new byte[encodedSize];
            var bufferCursor = encoder.Encode(buffer);
            Assert.Equal(0, bufferCursor.Length);
            var result = HexConverter.BytesToHex(buffer, checkEndian: false, hexPrefix: false);
            Assert.Equal("00000000000000000000000011f4d0a3c12e86b4b5f39b213f7e19d048276dae", result);
        }

        [Fact]
        public void UInt32()
        {
            uint num = 4294923588;
            var encoder = EncoderFactory.LoadEncoder("uint32", num);
            Assert.IsType<UInt32Encoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> buffer = new byte[encodedSize];
            var bufferCursor = encoder.Encode(buffer);
            Assert.Equal(0, bufferCursor.Length);
            var result = HexConverter.BytesToHex(buffer, checkEndian: false, hexPrefix: false);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000ffff5544", result);
        }

        [Fact]
        public void Boolean()
        {
            bool boolean = true;
            var encoder = EncoderFactory.LoadEncoder("bool", boolean);
            Assert.IsType<BoolEncoder>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(32, encodedSize);
            Span<byte> buffer = new byte[encodedSize];
            var bufferCursor = encoder.Encode(buffer);
            Assert.Equal(0, bufferCursor.Length);
            var result = HexConverter.BytesToHex(buffer);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001", result);

            encoder.SetValue(false);
            bufferCursor = encoder.Encode(buffer);
            result = HexConverter.BytesToHex(buffer);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", result);
        }

        [Fact]
        public void StaticBoolArray()
        {
            IEnumerable<bool> arr = new[] { true, true, false, true, };
            var encoder = EncoderFactory.LoadEncoder("bool[4]", arr, EncoderFactory.LoadEncoder("bool", default(bool)));
            Assert.IsType<ArrayEncoder<bool>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(128, encodedSize);
            Span<byte> buffer = new byte[encodedSize];
            var bufferCursor = encoder.Encode(buffer);
            Assert.Equal(0, bufferCursor.Length);
            var result = HexConverter.BytesToHex(buffer);
            Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001", result);
        }

        [Fact]
        public void DynamicBoolArray()
        {
            IEnumerable<bool> arr = new[] { true, true, false, true, };
            var encoder = EncoderFactory.LoadEncoder("bool[]", arr, EncoderFactory.LoadEncoder("bool", default(bool)));
            Assert.IsType<ArrayEncoder<bool>>(encoder);
            var encodedSize = encoder.GetEncodedSize();
            Assert.Equal(160, encodedSize);
            Span<byte> buffer = new byte[encodedSize];
            var bufferCursor = encoder.Encode(buffer);
            Assert.Equal(0, bufferCursor.Length);
            var result = HexConverter.BytesToHex(buffer);
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001", result);
        }
    }
}

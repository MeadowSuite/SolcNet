using System;
using System.Collections.Generic;
using Xunit;

namespace SolCodeGen.Tests
{
    public class HexConverterTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { (int)-16098398, },
            new object[] { (long)4611686018427387904 },
            new object[] { (ulong)1844674407370955161 },
            new object[] { int.MaxValue },
            new object[] { int.MinValue },
            new object[] { (byte)123 },
            new object[] { (sbyte)-56 }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void NumericByDynamicObject(object val)
        {
            var numHex = HexConverter.GetHexFromObject(val, hexPrefix: true);
            var numRoundTrip = HexConverter.HexToObject(val.GetType(), numHex);
            Assert.Equal(val, numRoundTrip);
        }

        [Fact]
        public void IntByValue()
        {
            int num = -16098398;
            var numHex = HexConverter.GetHexFromInteger(num);
            var numRoundTrip = HexConverter.HexToValue<int>(numHex);
            Assert.Equal(num, numRoundTrip);
        }

        [Fact]
        public void AddressGeneric()
        {
            var addrHex = "0xb60e8dd61c5d32be8058bb8eb970870f07233155";
            var addr = HexConverter.HexToValue<Address>(addrHex);
            var roundTrip = HexConverter.GetHex(addr, hexPrefix: true);
            Assert.Equal(addrHex, roundTrip);
        }

        [Fact]
        public void AddressDynamic()
        {
            var addrHex = "0xb60e8dd61c5d32be8058bb8eb970870f07233155";
            var addr = (Address)HexConverter.HexToObject(typeof(Address), addrHex);
            var roundTrip = HexConverter.GetHexFromObject(addr, hexPrefix: true);
            Assert.Equal(addrHex, roundTrip);
        }

        [Fact]
        public void UInt256()
        { 
            UInt256 num = 1234565;
            var hex = HexConverter.GetHexFromInteger(num);
            var roundTrip = HexConverter.HexToValue<UInt256>(hex);
            Assert.Equal("000000000000000000000000000000000000000000000000000000000012d685", hex);
            Assert.Equal(num, roundTrip);
        }

        [Fact]
        public void Hash()
        {
            Hash hash = "0xb903239f8543d04b5dc1ba6579132b143087c68db1b2168786408fcbce568238";
            var hex = HexConverter.GetHex<Hash>(hash, hexPrefix: true);
            var roundTrip = HexConverter.HexToValue<Hash>(hex);
            Assert.Equal(hash.ToString(), roundTrip);
        }
    }
}

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
            var numRoundTrip = HexConverter.HexToObject(val.GetType(), numHex, bigEndian: true);
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
            var addr = HexConverter.HexToValue<Address>(addrHex, checkEndian: false);
            var roundTrip = HexConverter.GetHexFromInteger(addr, hexPrefix: true, checkEndian: false);
            Assert.Equal(addrHex, roundTrip);
        }

        [Fact]
        public void AddressDynamic()
        {
            var addrHex = "0xb60e8dd61c5d32be8058bb8eb970870f07233155";
            var addr = (Address)HexConverter.HexToObject(typeof(Address), addrHex, bigEndian: false);
            var roundTrip = HexConverter.GetHexFromObject(addr, hexPrefix: true);
            Assert.Equal(addrHex, roundTrip);
        }

        [Fact]
        public void UInt256()
        { 
            UInt256 num = 1234565;
            var hex = HexConverter.GetHexFromInteger(num, checkEndian: true);
            var roundTrip = HexConverter.HexToValue<UInt256>(hex, checkEndian: true);
            Assert.Equal(num, roundTrip);
        }
    }
}

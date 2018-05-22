using SolCodeGen.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests
{
    public class UIntTests
    {
        [Fact]
        public void UInt32Cast()
        {
            uint num = 2147483640;
            UInt256 bigNum = num;
            var same = bigNum == num;
            Assert.True(same);
            Assert.Equal(num, (uint)bigNum);
        }

        [Fact]
        public void UInt64Cast()
        {
            ulong num = UInt64.MaxValue;
            UInt256 bigNum = num;
            var same = bigNum == num;
            Assert.True(same);
            Assert.Equal(num, (ulong)bigNum);
        }

        [Fact]
        public void ByteCast()
        {
            byte num = 0xF5;
            UInt256 bigNum = num;
            var same = bigNum == num;
            Assert.True(same);
            Assert.Equal(num, (byte)bigNum);
        }

        [Fact]
        public void WeiTest()
        {
            var weiInt = BigInteger.Pow(10, 18);
            var wei = EthUtil.ONE_ETHER_IN_WEI;
            Assert.Equal(weiInt.ToString(), wei.ToString());
            var exp = Math.Round(BigInteger.Log10(wei));
            Assert.Equal(18, exp);
        }
    }
}

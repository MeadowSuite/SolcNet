using System;
using System.Collections.Generic;
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
    }
}

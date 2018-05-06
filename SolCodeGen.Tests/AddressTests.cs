using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests
{
    public class AddressTests
    {
        [Fact]
        public void ZeroAddress()
        {
            Address addr1 = "0x0";
            Address addr2 = "0";

            Assert.Equal(Address.Zero, addr1);
            Assert.Equal(Address.Zero, addr2);
        }

        [Fact]
        public void AddressToString()
        {
            var str = "0xb60e8dd61c5d32be8058bb8eb970870f07233155";
            Address addr = str;
            Assert.Equal(str, addr.ToString());
        }
    }
}

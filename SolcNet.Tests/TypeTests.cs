using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SolcNet.Tests
{
    public class TypeTests
    {

        SolcLib _lib;

        public TypeTests()
        {
            _lib = new SolcLib();
        }

        [Fact]
        public void HasAllTypes()
        {
            var exampleContract = "TestContracts/ExampleContract.sol";
            var output = _lib.Compile(exampleContract);
        }

    }
}

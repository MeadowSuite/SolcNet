using SolcNet.NativeLib;
using System;
using Xunit;

namespace SolcNet.Tests
{
    public class BindingTests
    {
        SolcLib solcLib;

        public BindingTests()
        {
            solcLib = new SolcLib("OpenZeppelin");
        }

        [Fact]
        public void VersionTest()
        {
            var version = solcLib.VersionDescription;
            Assert.Equal("0.4.23-develop.2018.4.25+commit.124ca40d.mod.Windows.msvc", version);
        }

        [Fact]
        public void LicenseTest()
        {
            var license = solcLib.License;
            Assert.StartsWith("Most of the code is licensed under GPLv3", license);
        }

        [Fact]
        public void CompileOpenZeppelin()
        {
            var srcs = new[] {
                "contracts/AddressUtils.sol",
                "contracts/Bounty.sol",
                "contracts/DayLimit.sol",
                "contracts/ECRecovery.sol",
                "contracts/LimitBalance.sol",
                "contracts/MerkleProof.sol",
                "contracts/ReentrancyGuard.sol",
                "contracts/access/SignatureBouncer.sol",
                "contracts/crowdsale/Crowdsale.sol",
                "contracts/crowdsale/distribution/FinalizableCrowdsale.sol",
                "contracts/crowdsale/distribution/PostDeliveryCrowdsale.sol",
                "contracts/crowdsale/distribution/RefundableCrowdsale.sol",
                "contracts/crowdsale/distribution/utils/RefundVault.sol",
                "contracts/crowdsale/emission/AllowanceCrowdsale.sol",
                "contracts/crowdsale/emission/MintedCrowdsale.sol",
                "contracts/crowdsale/price/IncreasingPriceCrowdsale.sol",
                "contracts/crowdsale/validation/CappedCrowdsale.sol",
                "contracts/crowdsale/validation/IndividuallyCappedCrowdsale.sol",
                "contracts/crowdsale/validation/TimedCrowdsale.sol",
                "contracts/crowdsale/validation/WhitelistedCrowdsale.sol",
                "contracts/examples/SampleCrowdsale.sol",
                "contracts/examples/SimpleSavingsWallet.sol",
                "contracts/examples/SimpleToken.sol",
                "contracts/lifecycle/Destructible.sol",
                "contracts/lifecycle/Pausable.sol",
                "contracts/lifecycle/TokenDestructible.sol",
                "contracts/math/Math.sol",
                "contracts/math/SafeMath.sol",
                "contracts/ownership/CanReclaimToken.sol",
                "contracts/ownership/Claimable.sol",
                "contracts/ownership/Contactable.sol",
                "contracts/ownership/DelayedClaimable.sol",
                "contracts/ownership/HasNoContracts.sol",
                "contracts/ownership/HasNoEther.sol",
                "contracts/ownership/HasNoTokens.sol",
                "contracts/ownership/Heritable.sol",
                "contracts/ownership/NoOwner.sol",
                "contracts/ownership/Ownable.sol",
                "contracts/ownership/Whitelist.sol",
                "contracts/ownership/rbac/RBAC.sol",
                //"contracts/ownership/rbac/RBACWithAdmin.sol",
                //"contracts/ownership/rbac/Roles.sol",
                "contracts/payment/PullPayment.sol",
                //"contracts/payment/SplitPayment.sol",
                "contracts/token/ERC20/BasicToken.sol",
                "contracts/token/ERC20/BurnableToken.sol",
                "contracts/token/ERC20/CappedToken.sol",
                "contracts/token/ERC20/DetailedERC20.sol",
                "contracts/token/ERC20/ERC20.sol",
                "contracts/token/ERC20/ERC20Basic.sol",
                "contracts/token/ERC20/MintableToken.sol",
                "contracts/token/ERC20/PausableToken.sol",
                "contracts/token/ERC20/SafeERC20.sol",
                "contracts/token/ERC20/StandardBurnableToken.sol",
                "contracts/token/ERC20/StandardToken.sol",
                "contracts/token/ERC20/TokenTimelock.sol",
                //"contracts/token/ERC20/TokenVesting.sol",
                "contracts/token/ERC721/DeprecatedERC721.sol",
                "contracts/token/ERC721/ERC721.sol",
                "contracts/token/ERC721/ERC721Basic.sol",
                "contracts/token/ERC721/ERC721BasicToken.sol",
                "contracts/token/ERC721/ERC721Holder.sol",
                "contracts/token/ERC721/ERC721Receiver.sol",
                "contracts/token/ERC721/ERC721Token.sol",
                "contracts/token/ERC827/ERC827.sol",
                "contracts/token/ERC827/ERC827Token.sol"
            };

            solcLib.Compile(srcs);
        }
    }
}

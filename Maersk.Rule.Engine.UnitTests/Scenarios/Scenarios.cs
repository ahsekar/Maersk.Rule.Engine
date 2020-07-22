using Maersk.Rule.Engine.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Maersk.Rule.Engine.UnitTests.Scenarios
{
    public class Scenarios
    {
        private readonly Mock<IBusinessLogic> _manager;
        private readonly Mock<ILogger> _logger;
        public Scenarios()
        {
            _manager = new Mock<IBusinessLogic>();
            _logger = new Mock<ILogger>();
        }
        [Fact]
        public void GivenPaymentIsMadeForPhysicalProduct_ThenGeneratePackingSlipForShipping_AndGenerateCommisionPayment()
        {
            _manager.Setup(x => x.PhysicalProductPacking(It.IsAny<HttpContext>())).Returns(true);
            _manager.Setup(x => x.ProcessCommision(It.IsAny<HttpContext>())).Returns(true);

        }
        [Fact]
        public void GivenPaymentIsMadeForPhysicalProduct_WHenErrorOccured_ThenBreakthChainAndReturnError() 
        { 

        }
        [Fact]
        public void GivenPaymentIsMadeForBook_ThenCreateDuplicateSlipForRoyaltyDepartment_AndGenerateCommisionPayment()
        {

        }
        [Fact]
        public void GivenPaymentIsMadeForMembership_ThenActivateMembership_AndNotifyEmailToOwner() { }
        [Fact]
        public void GivenPaymentIsMadeForMembershipUpgrade_ThenUpgrade_AndNotifyEmailToOwner() { }
        [Fact]
        public void GivenPaymentIsMadeForVideo_ThenAddFreeFirstAidVideor() { }
        

    }   
}

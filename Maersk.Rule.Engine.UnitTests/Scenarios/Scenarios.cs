
using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Maersk.Rule.Engine.UnitTests.Scenarios
{
    public class Scenarios
    {
        private readonly Mock<IBusinessLogic> _manager;
        private readonly Mock<ILogger> _logger;
        public Mock<HandlerDelegate> handlerDelegate;
        public ProcessCommissionPaymentHandler processCommissionPaymentHandler;
        public PhysicalProductPackingSlipHandler physicalProductPackingSlipHandler;
        public ResponseHandler responseHandler;
        public Scenarios()
        {
            _manager = new Mock<IBusinessLogic>();
            _logger = new Mock<ILogger>();
            handlerDelegate = new Mock<HandlerDelegate>();
            processCommissionPaymentHandler = new ProcessCommissionPaymentHandler(logger: _logger.Object, manager: _manager.Object);
            physicalProductPackingSlipHandler = new PhysicalProductPackingSlipHandler(logger: _logger.Object, manager: _manager.Object);
            responseHandler = new ResponseHandler(logger: _logger.Object);
        }
        [Fact]
        public void GivenPaymentIsMadeForPhysicalProduct_ThenGeneratePackingSlipForShipping_AndGenerateCommisionPayment()
        {
            _manager.Setup(x => x.PhysicalProductPacking(It.IsAny<HttpContext>())).Returns(true);
            _manager.Setup(x => x.ProcessCommision(It.IsAny<HttpContext>())).Returns(true);
            physicalProductPackingSlipHandler.Next(processCommissionPaymentHandler.Invoke);
            processCommissionPaymentHandler.Next(responseHandler.Invoke);
            var result = physicalProductPackingSlipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("SUCCESS", result.Status);
            _logger.Verify(
               m => m.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Physical product packing slip generated")),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
               )
               );
            _logger.Verify(
              m => m.Log(
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Processed Commision Payment")),
                  It.IsAny<Exception>(),
                  (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
              )
              );
        }
        [Fact]
        public void GivenPaymentIsMadeForPhysicalProduct_WHenErrorOccured_ThenBreakthChainAndReturnError()
        {
            _manager.Setup(x => x.PhysicalProductPacking(It.IsAny<HttpContext>())).Returns(false);
            physicalProductPackingSlipHandler.Next(processCommissionPaymentHandler.Invoke);
            var result = physicalProductPackingSlipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("FAILED", result.Status);
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

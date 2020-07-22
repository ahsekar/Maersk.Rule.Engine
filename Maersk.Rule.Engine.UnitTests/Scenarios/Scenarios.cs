
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
        public CreateDuplicateSlipForRoyaldepartmentHandler createDuplicateSlip;
        public NotifyEmailHandler notifyEmailHandler;
        public ActivateMemeberShipHandler activateMemeberShipHandler;
        public ResponseHandler responseHandler;
        public UpgradeMembershipHandler upgradeMembershipHandler;
        public AddFirstAidVideoHandler addFirstAidVideoHandler;
        public Scenarios()
        {
            _manager = new Mock<IBusinessLogic>();
            _logger = new Mock<ILogger>();
            handlerDelegate = new Mock<HandlerDelegate>();
            responseHandler = new ResponseHandler(logger: _logger.Object);
            processCommissionPaymentHandler = new ProcessCommissionPaymentHandler(logger: _logger.Object, manager: _manager.Object);
            physicalProductPackingSlipHandler = new PhysicalProductPackingSlipHandler(logger: _logger.Object, manager: _manager.Object);
            createDuplicateSlip = new CreateDuplicateSlipForRoyaldepartmentHandler(logger: _logger.Object, manager: _manager.Object);
            notifyEmailHandler = new NotifyEmailHandler(logger: _logger.Object, manager: _manager.Object);
            activateMemeberShipHandler = new ActivateMemeberShipHandler(logger: _logger.Object, manager: _manager.Object);
            upgradeMembershipHandler = new UpgradeMembershipHandler(logger: _logger.Object, manager: _manager.Object);
            addFirstAidVideoHandler = new AddFirstAidVideoHandler(logger: _logger.Object, manager: _manager.Object);
        }

        #region PositiveScenarios

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
        public void GivenPaymentIsMadeForBook_ThenCreateDuplicateSlipForRoyaltyDepartment_AndGenerateCommisionPayment()
        {
            _manager.Setup(x => x.CreateDuplicateSlip(It.IsAny<HttpContext>())).Returns(true);
            _manager.Setup(x => x.ProcessCommision(It.IsAny<HttpContext>())).Returns(true);
            createDuplicateSlip.Next(processCommissionPaymentHandler.Invoke);
            processCommissionPaymentHandler.Next(responseHandler.Invoke);
            var result = createDuplicateSlip.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("SUCCESS", result.Status);
            _logger.Verify(
               m => m.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("duplicate slip for royalty department created")),
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
        public void GivenPaymentIsMadeForMembership_ThenActivateMembership_AndNotifyEmailToOwner() 
        {
            _manager.Setup(x => x.ActivateMembership(It.IsAny<HttpContext>())).Returns(true);
            _manager.Setup(x => x.NotifyEmail(It.IsAny<HttpContext>())).Returns(true);
            activateMemeberShipHandler.Next(notifyEmailHandler.Invoke);
            notifyEmailHandler.Next(responseHandler.Invoke);
            var result = activateMemeberShipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("SUCCESS", result.Status);
            _logger.Verify(
               m => m.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Membership has been activated")),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
               )
               );
            _logger.Verify(
              m => m.Log(
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Email notification sent")),
                  It.IsAny<Exception>(),
                  (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
              )
              );
        }
        [Fact]
        public void GivenPaymentIsMadeForMembershipUpgrade_ThenUpgrade_AndNotifyEmailToOwner() 
        {
            _manager.Setup(x => x.UpgradeMembershi(It.IsAny<HttpContext>())).Returns(true);
            _manager.Setup(x => x.NotifyEmail(It.IsAny<HttpContext>())).Returns(true);
            upgradeMembershipHandler.Next(notifyEmailHandler.Invoke);
            notifyEmailHandler.Next(responseHandler.Invoke);
            var result = upgradeMembershipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("SUCCESS", result.Status);
            _logger.Verify(
               m => m.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Membership has been upgrade")),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
               )
               );
            _logger.Verify(
              m => m.Log(
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("Email notification sent")),
                  It.IsAny<Exception>(),
                  (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
              )
              );
        }
        [Fact]
        public void GivenPaymentIsMadeForVideo_ThenAddFreeFirstAidVideo() 
        {
            _manager.Setup(x => x.AddFirstAidVideo(It.IsAny<HttpContext>())).Returns(true);
            addFirstAidVideoHandler.Next(responseHandler.Invoke);
            var result = addFirstAidVideoHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("SUCCESS", result.Status);
            _logger.Verify(
               m => m.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((object v, Type _) => v.ToString().Contains("First ad video added")),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
               )
               );
        }

        #endregion

        #region NegetiveScenarios

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
        public void GivenPaymentIsMadeForBook_WHenErrorOccured_ThenBreakthChainAndReturnError()
        {
            _manager.Setup(x => x.CreateDuplicateSlip(It.IsAny<HttpContext>())).Returns(false);
            createDuplicateSlip.Next(processCommissionPaymentHandler.Invoke);
            var result = createDuplicateSlip.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("FAILED", result.Status);
        }

        [Fact]
        public void GivenPaymentIsMadeForMembership_WHenErrorOccured_ThenBreakthChainAndReturnError()
        {
            _manager.Setup(x => x.ActivateMembership(It.IsAny<HttpContext>())).Returns(false);
            activateMemeberShipHandler.Next(notifyEmailHandler.Invoke);
            var result = activateMemeberShipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("FAILED", result.Status);
        }

        [Fact]
        public void GivenPaymentIsMadeForMembershipUpgrade_WHenErrorOccured_ThenBreakthChainAndReturnError()
        {
            _manager.Setup(x => x.UpgradeMembershi(It.IsAny<HttpContext>())).Returns(false);
            upgradeMembershipHandler.Next(notifyEmailHandler.Invoke);
            var result = upgradeMembershipHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("FAILED", result.Status);
        }

        [Fact]
        public void GivenPaymentIsMadeForVideo_WHenErrorOccured_ThenBreakthChainAndReturnError()
        {
            _manager.Setup(x => x.AddFirstAidVideo(It.IsAny<HttpContext>())).Returns(false);
            addFirstAidVideoHandler.Next(responseHandler.Invoke);
            var result = addFirstAidVideoHandler.Invoke(new DefaultHttpContext());
            Assert.NotNull(result);
            Assert.Equal("FAILED", result.Status);
        }

        #endregion

    }
}

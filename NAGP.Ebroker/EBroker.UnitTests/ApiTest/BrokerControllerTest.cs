using Api.Controllers;
using Api.Helpers;
using Api.Models;
using EBroker.Domain;
using EBroker.Interfaces.services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EBroker.UnitTests.ControllerTest
{
    public class BrokerControllerTest
    {
        private readonly BrokerController _brokerController;
        private readonly Mock<IBrokerService> _mockBrokerSevice;
        private readonly Mock<IWrapper> _mockWrapper;

        public BrokerControllerTest()
        {
            _mockBrokerSevice = new Mock<IBrokerService>();
            _mockWrapper = new Mock<IWrapper>();
            _brokerController = new BrokerController(_mockBrokerSevice.Object,_mockWrapper.Object);
        }


        [Theory]
        [InlineData (true,false, "InSufficient fund")]
        [InlineData(false, true, "Can not buy on non working day or hours")]
        public void BuyEquity_Returns_BadRequestResult(bool isValidDayTime ,bool isSufficientFund,string message)
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(true);
            _mockBrokerSevice.Setup(x => x.BuyEquity(It.IsAny<int>(),It.IsAny<Equity>() )).Returns(isSufficientFund);
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(isValidDayTime);

            //Act
            var response = _brokerController.BuyEquity(req);

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(message, result.Value);

        }

        [Fact]
        public void BuyEquity_Returns_NotFoundResult()
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(false);
            _mockBrokerSevice.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns(true);
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(true);

            //Act
            var response = _brokerController.BuyEquity(req);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Broker not exist", result.Value);

        }

        [Fact]
        public void BuyEquity_Returns_SuccessResult()
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(true);
            _mockBrokerSevice.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns(true);
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(true);

            //Act
            var response = _brokerController.BuyEquity(req);

            //Assert
            Assert.IsType<NoContentResult>(response);

        }

        [Theory]
        [InlineData(true, false, "Equity doesn't belong to the portfolio")]
        [InlineData(false, true, "Can not sell equity on non working day or hours")]
        public void SellEquity_Returns_BadRequestResult(bool isValidDayTime, bool isValidEquity, string message)
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(true);
            _mockBrokerSevice.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns((isValidEquity,100000));
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(isValidDayTime);

            //Act
            var response = _brokerController.SellEquity(req);

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(message, result.Value);

        }

        [Fact]
        public void SellEquity_Returns_NotFoundResult()
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(false);
            _mockBrokerSevice.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns((true,100000));
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(true);

            //Act
            var response = _brokerController.SellEquity(req);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Broker not exist", result.Value);

        }

        [Fact]
        public void SellEquity_Returns_SuccessResult()
        {
            //Arrange
            ApiBrokerReq req = new ApiBrokerReq { BrokerId = 1, Code = "TARP", NoOfShares = 10 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(true);
            _mockBrokerSevice.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns((true,100000));
            _mockWrapper.Setup(x => x.IsValidDayTime(It.IsAny<DateTime>())).Returns(true);

            //Act
            var response = _brokerController.SellEquity(req);

            //Assert
            Assert.IsType<NoContentResult>(response);

        }

        [Fact]
        public void AddFunds_Returns_NotFoundResult()
        {
            //Arrange
            ApiAddFundReq req = new ApiAddFundReq { BrokerId = 1 , Amount=90000 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(false);
            _mockBrokerSevice.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns((90000));

            //Act
            var response = _brokerController.AddFunds(req);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Broker not exist", result.Value);

        }

        [Fact]
        public void AddFunds_Returns_SuccessResult()
        {
            //Arrange
            ApiAddFundReq req = new ApiAddFundReq { BrokerId = 1, Amount = 90000 };
            _mockBrokerSevice.Setup(x => x.IsBrokerExist(It.IsAny<int>())).Returns(true);
            _mockBrokerSevice.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(90000);

            //Act
            var response = _brokerController.AddFunds(req);

            //Assert
            Assert.IsType<OkResult>(response);

        }
    }
}

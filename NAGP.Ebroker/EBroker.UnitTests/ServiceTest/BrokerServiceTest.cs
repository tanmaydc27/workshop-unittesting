using EBroker.Domain;
using EBroker.Interfaces.repository;
using EBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EBroker.UnitTests.ServiceTest
{
    public class BrokerServiceTest
    {
        private readonly BrokerService _brokerService;
        private readonly Mock<IBrokerRepository> _mockBrokerRepository;
        private readonly Mock<IEquityRepository> _mockEquityRepository;

        public BrokerServiceTest()
        {
            _mockBrokerRepository = new Mock<IBrokerRepository>();
            _mockEquityRepository = new Mock<IEquityRepository>();
            _brokerService = new BrokerService(_mockBrokerRepository.Object, _mockEquityRepository.Object);
        }

        [Fact]
        public void BuyEquity_InSufficientFund_Returns_False()
        {
            //Arrange
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 };

            _mockBrokerRepository.Setup(x => x.GetBrokerInfoByID(It.IsAny<int>())).Returns(new Broker {  Id=1, Name="Tanmay", AvailableFund=10000});
            _mockEquityRepository.Setup(x => x.GetEquityByCode(It.IsAny<string>())).Returns(new Equity {  Code="TARP", Price=5000});
            _mockBrokerRepository.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<Equity>()));

            //Act
            var response = _brokerService.BuyEquity(brokerID,equity);

            //Assert
            Assert.False(response);
        }

        [Fact]
        public void BuyEquity_SufficientFund_Returns_True()
        {
            //Arrange
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 };

            _mockBrokerRepository.Setup(x => x.GetBrokerInfoByID(It.IsAny<int>())).Returns(new Broker { Id = 1, Name = "Tanmay", AvailableFund = 100000 });
            _mockEquityRepository.Setup(x => x.GetEquityByCode(It.IsAny<string>())).Returns(new Equity { Code = "TARP", Price = 5000 });
            _mockBrokerRepository.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<Equity>()));

            //Act
            var response = _brokerService.BuyEquity(brokerID, equity);

            //Assert
            Assert.True(response);
        }

        [Fact]
        public void SellEquity_NotInPortFolio()
        {
            //Arrange
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 };

            _mockBrokerRepository.Setup(x => x.IsValidEquityForBroker(It.IsAny<int>(),It.IsAny<string>())).Returns(false);
            _mockBrokerRepository.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()));
            _mockBrokerRepository.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns(0);

            //Act
            var response = _brokerService.SellEquity(brokerID, equity);

            //Assert
            _mockBrokerRepository.Verify(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>()), Times.Never);
            _mockBrokerRepository.Verify(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
            Assert.False(response.isValidEquity);
            Assert.Equal(0,response.refundAmount);
        }

        [Fact]
        public void SellEquity_BrokerageAmount_Min20Rs()
        {
            //Arrange
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 };
            double totalSellPrice = 38000;
            _mockBrokerRepository.Setup(x => x.IsValidEquityForBroker(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _mockBrokerRepository.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()));
            _mockBrokerRepository.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns(totalSellPrice);

            //Act
            var response = _brokerService.SellEquity(brokerID, equity);

            //Assert
            Assert.True(response.isValidEquity);
            Assert.Equal((totalSellPrice - 20), response.refundAmount);
        }
        [Fact]
        public void SellEquity_BrokerageAmount_MoreThan20Rs()
        {
            //Arrange
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 };
            double totalSellPrice = 45000;
            _mockBrokerRepository.Setup(x => x.IsValidEquityForBroker(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _mockBrokerRepository.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()));
            _mockBrokerRepository.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<Equity>())).Returns(totalSellPrice);

            //Act
            var response = _brokerService.SellEquity(brokerID, equity);

            //Assert
            Assert.True(response.isValidEquity);
            Assert.Equal((totalSellPrice-(.0005*totalSellPrice)), response.refundAmount);
        }

        [Fact]
        public void AddFund_MoreThan100000()
        {
            //Arrange
            int brokerID = 1;
            int amount = 200000;
 
            _mockBrokerRepository.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()));

            //Act
            var response = _brokerService.AddFunds(brokerID, amount);

            //Assert
            Assert.Equal((amount - amount*.0005), response);
        }
        [Fact]
        public void AddFund_LessThan100000()
        {
            //Arrange
            int brokerID = 1;
            int amount = 90000;
            _mockBrokerRepository.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>()));

            //Act
            var response = _brokerService.AddFunds(brokerID, amount);

            //Assert
            Assert.Equal(amount, response);
        }

        [Fact]
        public void IsBrokerExist_ReturnsFalse()
        {
            //Arrange
            int brokerID = 1;
            _mockBrokerRepository.Setup(x => x.GetBrokerInfoByID(It.IsAny<int>())).Returns((Broker)null);

            //Act
            var response = _brokerService.IsBrokerExist(brokerID);

            //Assert
            Assert.False(response);
        }

        [Fact]
        public void IsBrokerExist_ReturnsTrue()
        {
            //Arrange
            int brokerID = 1;
            _mockBrokerRepository.Setup(x => x.GetBrokerInfoByID(It.IsAny<int>())).Returns(new Broker {  Id=1, Name="Tanmay"});

            //Act
            var response = _brokerService.IsBrokerExist(brokerID);

            //Assert
            Assert.True(response);
        }

    }
}

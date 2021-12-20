using Api;
using Api.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EBroker.UnitTests.ApiTest
{
    public class StartUpTest
    {
        [Fact]
        public void ConfigureServices_RegistersDependenciesCorrectly()
        {
            //  Arrange

            //  Setting up the stuff required for Configuration.GetConnectionString("DefaultConnection")
            Mock<IConfigurationSection> configurationSectionStub = new Mock<IConfigurationSection>();
            configurationSectionStub.Setup(x => x["EBrokerConnectionString"]).Returns("TestConnectionString");
            Mock<Microsoft.Extensions.Configuration.IConfiguration> configurationStub = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            configurationStub.Setup(x => x.GetSection("ConnectionStrings")).Returns(configurationSectionStub.Object);

            IServiceCollection services = new ServiceCollection();
            var target = new Startup(configurationStub.Object);

            //  Act

            target.ConfigureServices(services);
            services.AddTransient<BrokerController>();

            //  Assert

            var serviceProvider = services.BuildServiceProvider();

            var controller = serviceProvider.GetService<BrokerController>();
            Assert.NotNull(controller);
        }
    }
}

using Api.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace EBroker.UnitTests.ApiTest
{
    public class WrapperTest
    {

        [Theory]
        [InlineData("2021-12-20 12:10:15", true)]
        [InlineData("2021-12-19 12:10:15", false)]
        [InlineData("2021-12-20 08:00:00", false)]
        [InlineData("2021-12-20 18:00:00", false)]
 
        public void IsValidDayTime(string dateTime,bool expectedResult)
        {
            DateTime tempDate = DateTime.Parse(dateTime);
            Wrapper wrapper = new Wrapper();
            var response = wrapper.IsValidDayTime(tempDate);
            Assert.Equal(expectedResult, response);
        }
    }
}

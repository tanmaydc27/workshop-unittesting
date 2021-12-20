using Api.Models;
using EBroker.Interfaces.services;
using Microsoft.AspNetCore.Mvc;
using System;
using EBroker.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerController : ControllerBase
    {

        private readonly IBrokerService _brokerService;
        private readonly IWrapper _wrapper;

        public BrokerController(IBrokerService brokerService , IWrapper wrapper)
        {
            _brokerService = brokerService;
            _wrapper = wrapper;
        }

        // POST api/<BrokerController>
        [HttpPost("buyEquity")]
        public IActionResult BuyEquity([FromBody] ApiBrokerReq req)
        {
            if (!_wrapper.IsValidDayTime(DateTime.UtcNow))
                return BadRequest("Can not buy on non working day or hours");

            if (! _brokerService.IsBrokerExist(req.BrokerId))
                return NotFound("Broker not exist");
            
            bool response = _brokerService.BuyEquity(req.BrokerId, new Equity { Code = req.Code, NoOfShares = req.NoOfShares });
            
            if(!response)
            {
                return BadRequest("InSufficient fund");
            }

            return NoContent();
        }

        [HttpPost("sellEquity")]
        public IActionResult SellEquity([FromBody] ApiBrokerReq req)
        {
            if (!_wrapper.IsValidDayTime(DateTime.UtcNow))
                return BadRequest("Can not sell equity on non working day or hours");

            if (! _brokerService.IsBrokerExist(req.BrokerId))
                return NotFound("Broker not exist");

            var response = _brokerService.SellEquity(req.BrokerId, new Equity { Code = req.Code, NoOfShares = req.NoOfShares });

            if (! response.isValidEquity)
            {
                return BadRequest("Equity doesn't belong to the portfolio");
            }

            return NoContent();
        }

        [HttpPost("addFunds")]
        public IActionResult AddFunds([FromBody] ApiAddFundReq req)
        {
            if (! _brokerService.IsBrokerExist(req.BrokerId))
                return NotFound("Broker not exist");

            double amount = _brokerService.AddFunds(req.BrokerId, req.BrokerId);
            return Ok();
        }


    }
}

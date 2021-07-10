using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Homo.Core.Constants;
using Homo.Api.Models;
using Homo.Api.Filters;

namespace Homo.Api.Controllers
{
    [Route("v1/test")]
    public class TestController : ControllerBase
    {
        private readonly string _jwtKey;
        public TestController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
        }

        [HttpGet]
        public dynamic getTest()
        {
            return new { website = "test" };
        }

        [HttpPost]
        [Validate]
        public dynamic postTest([FromBody] DTOs.Test dto)
        {
            return new { status = CUSTOM_RESPONSE.OK };
        }
    }
}
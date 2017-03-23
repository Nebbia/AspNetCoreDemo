using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreDemo
{
    [Route("api/some")]
    public class SomeController : Controller
    {
        private readonly ISomeService _someService;

        public SomeController(ISomeService someService)
        {
            _someService = someService;
        }

        [Route("")]
        [HttpGet]
        // GET: api/some
        public IActionResult Some()
        {
            return Ok($"Some response.Has Service ? {(_someService == null)}");
        }
    }
}

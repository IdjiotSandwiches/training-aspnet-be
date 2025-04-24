using CalculatorApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        [HttpPost("add/:id")]
        public Numeric Add(Numeric numeric, [FromQuery] string? q, int? id)
        {
            numeric.Result = numeric.Number1 + numeric.Number2;
            return numeric;
        }

        [HttpPost("subtract")]
        public Numeric Subtract(Numeric numeric)
        {
            numeric.Result = numeric.Number1 - numeric.Number2;
            return numeric;
        }

        [HttpPost("multiply")]
        public Numeric Multiply(Numeric numeric)
        {
            numeric.Result = numeric.Number1 * numeric.Number2;
            return numeric;
        }

        [HttpPost("divide")]
        public Numeric Divide(Numeric numeric)
        {
            numeric.Result = numeric.Number1 / numeric.Number2;
            return numeric;
        }
    }
}

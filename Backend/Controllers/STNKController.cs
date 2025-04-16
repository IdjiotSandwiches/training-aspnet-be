using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;

namespace Backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]

    public class STNKController(CrudHelper crudHelper, IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

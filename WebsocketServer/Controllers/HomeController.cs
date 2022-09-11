using Microsoft.AspNetCore.Mvc;

namespace WebsocketServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Football.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncorrectAlignment : ControllerBase
    {
        //Controlador del IncorrectAlignment

        [HttpPost]
        public ActionResult Post()
        {
            return this.Ok(0);
        }
    }
}

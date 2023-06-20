using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Inherrit from controller if you want to return HMTL page also for HTTP request
    public class BaseApiController : ControllerBase
    {
        
    }
}
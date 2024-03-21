using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helper;
using WebAPI.Models;

namespace Staff.API.Controllers
{
    [Authorize]
    [Route("api/aviasearch/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected UserProfile CurUserProfile => HelperUserProfile.GetUserProfile(User);

    }
}

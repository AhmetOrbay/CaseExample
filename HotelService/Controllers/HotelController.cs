using HotelLibrary.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HotelController : Controller
    {
        private readonly JwtHandler _jwtHandler;

        public HotelController(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        [AllowAnonymous]

        [HttpGet("GetToken")]
        public async Task<bool> GetToken()
        {
            var aa = _jwtHandler.GenerateJwt("Key2");
            return true;
        }
        //    [HideInSwagger] // Metodu gizlemek için özel anotasyonu kullanın

        [HttpGet(Name ="GetPrivateMethodCheck")]
        public async Task<bool> GetPrivateMethodCheck()
        {
            string jwt = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // İlgili anahtarın kullanıldığı JWT'yi doğrulayın
            bool isValidKey1 = _jwtHandler.ValidateJwt(jwt, "key1");
            bool isValidKey2 = _jwtHandler.ValidateJwt(jwt, "key2");

            if (isValidKey1)
            {
            }
            else if (isValidKey2)
            {
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}

using HotelLibrary.Extensions;
using HotelService.Services;
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
        private readonly ILogger<ElasticsearchLogger> _logger;

        public HotelController(JwtHandler jwtHandler, ILogger<ElasticsearchLogger> logger)
        {
            _jwtHandler = jwtHandler;
            _logger = logger;
    }

        [AllowAnonymous]

        [HttpGet("GetToken")]
        public async Task<bool> GetToken()
        {

            _logger.LogError("Error");

            _logger.LogInformation("information"); 
            _logger.LogWarning("warning");
            var aa = _jwtHandler.GenerateJwt("Key2");
            return true;
        }

        /// <summary>
        /// Bu kisim Rapor kismindan kullanici bilgilerini cekmek icindir.
        /// parametre ile Gonderilen tokeni parse edip key kontrol edilir
        /// id ile kulanici bilgisi alinir
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Token"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet("GetPrivateMethodCheck/{Id}/{Token}")]
        public async Task<bool> GetPrivateMethodCheck(long Id,string Token)
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

using HotelLibrary.Dtos;
using HotelLibrary.Extensions;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace HotelService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class HotelController : Controller
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IHotelService _hotelService;

        public HotelController(JwtHandler jwtHandler
                            , IHotelService hotelService)
        {
            _jwtHandler = jwtHandler;
            _hotelService = hotelService;
    }

        [AllowAnonymous]

        [HttpGet("GetToken")]
        public async Task<bool> GetToken()
        {
            var aa = _jwtHandler.GenerateJwt("Key2");
            return true;
        }

        /// <summary>
        /// This section is for pulling user information from the Report section.
        /// Parse the sent token with the parameter and check the key
        /// Get user information with id
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

        /// <summary>
        /// Hotel creation
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost("AddedHotel")]
        public async Task<ResponseData<HotelDto>> CreateHotel(HotelDto hotel)
        {
            return await _hotelService.CreateHotel(hotel);
        }

        /// <summary>
        /// Hotel deletion. No deletion is performed, but an update is made. Updates the delete status
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteHotel")]
        public async Task<ResponseData<bool>> HotelDelete(long Id)
        {
            return await _hotelService.DeleteHotel(Id);
        }

        /// <summary>
        /// Add hotel contact
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost("AddedHotelContact")]
        public async Task<ResponseData<HotelContactDto>> CreateHotelContact(HotelContactDto hotel)
        {
            return await _hotelService.AddHotelContact(hotel);
        }

        /// <summary>
        /// Hotel message deletion. It does the update, not the deletion. Updates the isdelete status
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        [HttpPost("DeleteHotelContact")]
        public async Task<ResponseData<bool>> HotelContactDelete(long ContactId)
        {
            return await _hotelService.HotelContactDelete(ContactId);
        }

        /// <summary>
        /// Hotel manager brings information.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetHotelManagers/{Id}")]
        public async Task<ResponseData<List<HotelManagerDto>>> GetHotelManagers(long Id)
        {
            return await _hotelService.GetHotelManager(Id);
        }


        /// <summary>
        /// Returns the hotel details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetHotelDetail/{Id}")]
        public async Task<ResponseData<HotelDto>> GetHotelById(long Id)
        {
            return await _hotelService.GetHotelById(Id);
        }
    }
}

using HotelLibrary.Dtos;
using HotelLibrary.Extensions;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Reflection.Metadata;

namespace HotelService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class HotelController : Controller
    {
        //private readonly JwtHandler _jwtHandler;
        private readonly IHotelService _hotelService;

        public HotelController(
            //JwtHandler jwtHandler,
                             IHotelService hotelService)
        {
            //_jwtHandler = jwtHandler;
            _hotelService = hotelService;
        }


        /// <summary>
        /// Hotel creation
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost("AddedHotel")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<HotelDto>), (int)HttpStatusCode.OK)]

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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<bool>), (int)HttpStatusCode.OK)]
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

        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<HotelContactDto>), (int)HttpStatusCode.OK)]
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

        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<bool>), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<List<HotelManagerDto>>), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<List<HotelDto>>), (int)HttpStatusCode.OK)]
        public async Task<ResponseData<HotelDto>> GetHotelById(long Id)
        {
            return await _hotelService.GetHotelById(Id);
        }


        /// <summary>
        /// Returns the hotel List.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetHotelList")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<List<HotelDto>>), (int)HttpStatusCode.OK)]
        public async Task<ResponseData<List<HotelDto>>> GetHotelList()
        {
            return await _hotelService.GetHotelList();
        }
    }
}

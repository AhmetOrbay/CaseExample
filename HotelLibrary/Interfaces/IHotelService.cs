using HotelLibrary.Dtos;
using HotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Interfaces
{
    public interface IHotelService
    {
        Task<ResponseData<List<HotelDto>>> GetHotelList();
        Task<ResponseData<HotelDto>> GetHotelById(long Id);
        Task<ResponseData<HotelDto>> CreateHotel(HotelDto hotel);
        Task<ResponseData<HotelDto>> UpdateHotel(HotelDto hotel);
        Task<ResponseData<List<HotelManagerDto>>> GetHotelManager(long HotelId);


    }
}

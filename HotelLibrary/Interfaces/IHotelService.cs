using HotelLibrary.Dtos;
using HotelLibrary.Model;
using HotelLibrary.Models;
using HotelLibrary.Models.RabbitMq;
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
        Task<ResponseData<bool>> DeleteHotel(long Id); 
        Task<ResponseData<HotelContactDto>> AddHotelContact(HotelContactDto hotelContact);
        Task<ResponseData<bool>> HotelContactDelete(long hotelContactId);
        Task<ResponseData<List<HotelManagerDto>>> GetHotelByManagerList(long HotelId);
        Task<ReportDetail> GetReportDetails(ConsumeModel Consume);

    }
}

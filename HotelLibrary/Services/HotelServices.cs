using AutoMapper;
using HotelLibrary.Dtos;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using HotelLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelLibrary.Services
{
    public class HotelServices : IHotelService
    {
        private readonly HotelDbContext _hotelDbContext;
        private readonly IMapper _mapper;
        public HotelServices(HotelDbContext hotelDbContext, IMapper mapper)
        {
            _hotelDbContext = hotelDbContext;
            _mapper  = mapper;
        }

        public async Task<ResponseData<HotelDto>> CreateHotel(HotelDto hotel)
        {
            ResponseData<HotelDto> response = new ();
            try
            {
                var map = _mapper.Map<Hotel>(hotel);
                var dbModel =await _hotelDbContext.Hotels.AddAsync(map);
                await _hotelDbContext.SaveChangesAsync();
                hotel.Id = dbModel.Entity.Id;
                response.Data = hotel;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<HotelDto>> GetHotelById(long Id)
        {
            ResponseData<HotelDto> response = new();
            try
            {
                var DbData = await _hotelDbContext.Hotels.FirstOrDefaultAsync(x=>x.Id == Id);
                response.Data = _mapper.Map<HotelDto>(DbData);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<List<HotelDto>>> GetHotelList()
        {
            ResponseData<List<HotelDto>> response = new();
            try
            {
                var query = _hotelDbContext.Hotels.AsQueryable();
                var DbData = await query.ToListAsync();
                response.Data = _mapper.Map<List<HotelDto>>(DbData);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<List<HotelManagerDto>>> GetHotelManager(long HotelId)
        {
            ResponseData<List<HotelManagerDto>> response = new();
            try
            {
                var query = _hotelDbContext.HotelManagers.Include(x=>x.Hotels)
                    .Where(x=>x.HotelId == HotelId).AsQueryable();
                var DbData = await query.ToListAsync();
                response.Data = _mapper.Map<List<HotelManagerDto>>(DbData);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<HotelDto>> UpdateHotel(HotelDto hotel)
        {
            ResponseData<HotelDto> response = new();
            try
            {
                var map = _mapper.Map<Hotel>(hotel);
                var dbModel = await _hotelDbContext.Hotels.FirstOrDefaultAsync(x=>x.Id == hotel.Id);
                if (dbModel is not null)
                {
                    dbModel = _mapper.Map<Hotel>(hotel);
                    var entity = _hotelDbContext.Hotels.Update(dbModel);
                    await _hotelDbContext.SaveChangesAsync();
                    response.Data = _mapper.Map<HotelDto>(entity.Entity);
                }
                else
                {
                    response.ErrorMessage = $"Not found Hotels";
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }
    }
}

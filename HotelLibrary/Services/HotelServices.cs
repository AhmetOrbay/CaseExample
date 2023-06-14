using AutoMapper;
using HotelLibrary.Dtos;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using HotelLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelLibrary.Services
{
    public class HotelServices : IHotelService
    {
        private readonly HotelDbContext _hotelDbContext;
        private readonly ILogger<HotelServices> _logger;
        private readonly IMapper _mapper;

        public HotelServices(HotelDbContext hotelDbContext
                    , IMapper mapper
                    , ILogger<HotelServices> logger)
        {
            _hotelDbContext = hotelDbContext;
            _mapper  = mapper;
            _logger = logger;  
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
                _logger.LogError($"Hotel Error => {ex.Message}");

                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<HotelDto>> GetHotelById(long Id)
        {
            ResponseData<HotelDto> response = new();
            try
            {
                var DbData = await _hotelDbContext.Hotels
                    .Include(x=>x.HotelFeatures)
                    .Include(x=>x.Address)
                    .Include(x=>x.HotelContacts)
                    .FirstOrDefaultAsync(x=>x.Id == Id);
                response.Data = _mapper.Map<HotelDto>(DbData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hotel Error => {ex.Message}");
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
                _logger.LogError($"Hotel Error => {ex.Message}");
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
                _logger.LogError($"Hotel Error => {ex.Message}");
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
                _logger.LogError($"Hotel Error => {ex.Message}");
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }


        public async Task<ResponseData<bool>> DeleteHotel(long Id)
        {
            ResponseData<bool> response = new();
            try
            {
                var dBData = await _hotelDbContext.Hotels.FirstOrDefaultAsync(x => x.Id == Id);
                if(dBData is not null) {
                    dBData.IsDelete = true;
                    _hotelDbContext.Entry(dBData).Property(x => x.IsDelete).IsModified = true;
                    _hotelDbContext.SaveChanges();
                }
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hotel Error => {ex.Message}");
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<HotelContactDto>> AddHotelContact(HotelContactDto hotelContact)
        {
            ResponseData<HotelContactDto> response = new();
            try
            {
                var map = _mapper.Map<HotelContact>(hotelContact);
                var dbModel = await _hotelDbContext.HotelContacts.AddAsync(map);
                await _hotelDbContext.SaveChangesAsync();
                hotelContact.Id = dbModel.Entity.Id;
                response.Data = hotelContact;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hotel Error => {ex.Message}");
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<bool>> HotelContactDelete(long hotelContactId)
        {
            ResponseData<bool> response = new();
            try
            {
                var dBData = await _hotelDbContext.HotelContacts.FirstOrDefaultAsync(x => x.Id == hotelContactId);
                if (dBData is not null)
                {
                    dBData.IsDelete = true;
                    _hotelDbContext.Entry(dBData).Property(x => x.IsDelete).IsModified = true;
                    _hotelDbContext.SaveChanges();
                }
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hotel Error => {ex.Message}");
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }

        public async Task<ResponseData<List<HotelManagerDto>>> GetHotelByManagerList(long HotelId)
        {
            ResponseData<List<HotelManagerDto>> response = new();
            try
            {
                var query = _hotelDbContext.HotelManagers.AsQueryable();
                var DbData = await query.ToListAsync();
                response.Data = _mapper.Map<List<HotelManagerDto>>(DbData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hotel Error => {ex.Message}");
                response.ErrorMessage = $"{ex.Message}";
            }
            return response;
        }
    }
}

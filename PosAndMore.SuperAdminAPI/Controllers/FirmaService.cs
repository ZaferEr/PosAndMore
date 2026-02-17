using AutoMapper;
using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.AspNetCore.Mvc;
using PosAndMore.SuperAdmin.Models;
using PosAndMore.SuperAdmin.Models.DbModels;
using PosAndMore.SuperAdmin.Models.DtoModels;
using PosAndMore.SuperAdminUI.Services;

namespace PosAndMore.SuperAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaService : ControllerBase
    {
        private readonly string _connectionString;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public FirmaService(IConfiguration config, JwtService jwtService, IMapper mapper)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Default connection string eksik!");
            _jwtService = jwtService;
            _mapper  = mapper;
        }
        [HttpGet("GetIller")]
        public async Task<ApiResponse<List<IlDto>>>GetIller()
        {
            var db = new DataConnection(new DataOptions().UseSqlServer(_connectionString));  
            //var tempRes = await db.GetTable<Il>().LoadWith(m => m.Ilceler).ToListAsync();
            var tempRes = await db.GetTable<Il>().ToListAsync();
            try
            { 

               var dtos = _mapper.Map<List<IlDto>>(tempRes);
                return ApiResponse<List<IlDto>>.Success(dtos);
           
            }
            catch (Exception ex)
            {
                return ApiResponse<List<IlDto>>.FromException(ex);
            }
       

        }
        [HttpGet("GetIlceler")]
        public async Task<ApiResponse<List<IlceDto>>> GetIlceler(int? IlId)
        {
            try
            {
                var db = new DataConnection(new DataOptions().UseSqlServer(_connectionString));
            //var tempRes = await db.GetTable<Il>().LoadWith(m => m.Ilceler).ToListAsync();
            List<Ilce> tempRes = new List<Ilce>();
            if (IlId.HasValue)
            {
                  tempRes = await db.GetTable<Ilce>().Where(c => c.IlId ==IlId.Value).ToListAsync();

            }
            else
            {

                  tempRes = await db.GetTable<Ilce>().ToListAsync();

            }
           

                var dtos = _mapper.Map<List<IlceDto>>(tempRes);
                return ApiResponse<List<IlceDto>>.Success(dtos);

            }
            catch (Exception ex)
            {
                return ApiResponse<List<IlceDto>>.FromException(ex);
            } 
        }

        [HttpPost("FirmaEkle")]
        public async Task<ApiResponse<bool>> FirmaEkle(FirmalarDto dtoModel)
        {

            try
            {
                var db = new DataConnection(new DataOptions().UseSqlServer(_connectionString));
                var m = _mapper.Map<Firmalar>(dtoModel);
                await db.InsertAsync(m);

                return   ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.FromException(ex);
            }
           
        }
    }
}

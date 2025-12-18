namespace PosAndMore.API
{
    using Microsoft.Data.SqlClient;
 
    using Dapper.SimpleLoadCore;
    using Dapper.SimpleSaveCore;
    using PosAndMore.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Dapper;

    //public void getdata()
    //{
    //    string sqlcon = "Server=89.144.202.242; Database=EuroDigiPos; Trusted_Connection=True; TrustServerCertificate=True;";
    //    using (var c = new SqlConnection(sqlcon))
    //    {
    //        c.Open();
    //        var d = c.AutoQuery<Kdv>(null);
    //    }
    //}
    [ApiController]
        [Route("api/[controller]")]  // Route: /api/kdv
        public class KdvController : ControllerBase
        {
            private readonly string _connectionString;

            public KdvController(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("Default")
                    ?? "Server=89.144.20.242;Database=EuroDigiPos;User Id=sa;Password=mb88421;TrustServerCertificate=True;";
        }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var kdvs =   connection.AutoQuery<Kdv>(null);
                return Ok(kdvs);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                await using var connection = new SqlConnection(_connectionString);
                var kdv = await connection.QueryFirstOrDefaultAsync<Kdv>("SELECT * FROM Kdv WHERE KdvId = @Id", new { Id = id });
                return kdv == null ? NotFound() : Ok(kdv);
            }

            // İstersen Post, Put, Delete de ekle...
        }
    }
 

using PosAndMore.SuperAdmin.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models.DtoModels
{
    public class IlDto
    {
        public IlDto()
        {
            Ilceler = new List<IlceDto>();
        }
        public int Id { get; set; }
        public string? IlAdi { get; set; }

        public List<IlceDto> Ilceler { get; set; }
    }
}

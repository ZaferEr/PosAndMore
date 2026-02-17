using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models.DbModels
{
    public class Ilce
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int IlId { get; set; }
        public string? IlceAdi { get; set; }
    }
}

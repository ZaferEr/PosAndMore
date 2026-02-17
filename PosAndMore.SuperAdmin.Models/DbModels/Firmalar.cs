using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models.DbModels
{
    public class Firmalar
    {
        public int Id { get; set; }

        [Required]
        public  string? FirmaAdi { get; set; }
        public   string? FirmaSqlString { get; set; }
        public string? FirmaAdres { get; set; }
        public string? FirmaIlce { get; set; }
        public string? FirmaSehir { get; set; }
        public  string? FirmaVd { get; set; }
        public  string? FirmaVNo { get; set; }
        public string? FirmaYetkili { get; set; }
        public   string? FirmaDbName { get; set; }
        public   string? FirmaAdmin { get; set; }
        public string? AdminPass { get; set; }
        public bool IsActive { get; set; }
        public  bool TekSube { get; set; }
    }
}

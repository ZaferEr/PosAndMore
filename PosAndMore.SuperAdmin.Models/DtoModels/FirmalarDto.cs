using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;  // ← Bu önemli!
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosAndMore.SuperAdmin.Models.DtoModels
{
    public class FirmalarDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? FirmaAdi { get; set; } = string.Empty;
        [ValidateNever]
        public string? FirmaSqlString { get; set; }
        public string? FirmaAdres { get; set; }
        public string? FirmaIlce { get; set; }
        public string? FirmaSehir { get; set; }
        public string? FirmaVd { get; set; }
        public string? FirmaVNo { get; set; }
        public string? FirmaYetkili { get; set; }
        [ValidateNever]
        public string? FirmaDbName { get; set; }


        [Required]
        [StringLength(50)]
        public string? FirmaAdmin { get; set; }
        [StringLength(20)]
        public string? AdminPass { get; set; }
        public bool IsActive { get; set; }

        public int IlId { get; set; }
        public int IlceId { get; set; }
        public bool TekSube { get; set; }

    }
}

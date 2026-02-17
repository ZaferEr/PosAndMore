using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models.DbModels
{
    internal class Subeler
    {
        public int Id { get; set; }
        public int FirmaId { get; set; }
        public string? SubeAdi { get; set; }
        public string? SubeAdres { get; set; }
        public string? SubeIlce { get; set; }
        public string? SubeIl { get; set; }
        public string? SubeSchemaName { get; set; }
        public bool IsActive { get; set; }
        public byte LisansTip { get; set; }
    }
}

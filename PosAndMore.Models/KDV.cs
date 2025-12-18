using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using Dapper;
using Dapper.SimpleSaveCore;

namespace PosAndMore.Models
{
    [Table("Kdv")]
    public class Kdv
    {
       
        [PrimaryKey]
        public int? KdvId { get; set; }
        public string KdvAdi { get; set; }
        public byte KdvOran { get; set; }
        public string Notes { get; set; }
        public string UserModified { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public bool SoftDelete { get; set; }
 
    }
}

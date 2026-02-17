using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PosAndMore.Models
{
   
    public class Kdv
    {
       
       
        public int KdvId { get; set; }
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

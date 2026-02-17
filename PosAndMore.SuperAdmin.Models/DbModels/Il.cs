using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models.DbModels
{
    public class Il
    {
        public Il()
        {
            Ilceler = new List<Ilce>();
        }
        [PrimaryKey]
        public int Id { get; set; }
        public string? IlAdi { get; set; }

        [Association(ThisKey = nameof(Id), OtherKey = nameof(Ilce.IlId))]
        public List<Ilce> Ilceler { get; set; }
    }
}

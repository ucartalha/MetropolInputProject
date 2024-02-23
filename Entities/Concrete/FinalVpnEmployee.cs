using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class FinalVpnEmployee:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Department { get; set; }
        public int RemoteEmployeeId { get; set; }
        public DateTime Date{ get; set; }
        public DateTime FirstRecord { get; set; }
        public DateTime LastRecord { get; set; }
        public int? BytesIn { get; set; }
        public int? BytesOut { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

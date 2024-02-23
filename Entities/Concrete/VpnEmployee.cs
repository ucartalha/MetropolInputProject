using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class VpnEmployee:IEntity
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
        public int? Bytesout { get; set; }
        public int? Bytesin { get; set; }
        public int Duration { get; set; }
        public DateTime? FirstRecord{ get; set; }
        public DateTime? LastRecord{ get; set; }
        public int RemoteEmployeeId { get; set; }
        

    }
}

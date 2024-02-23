using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class MixLateEmployeeDto
    {
        public int ProcessTemp { get; set; }
        public List<MixEmployeeDto> Employees { get; set; }
        public string Message { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class LateEmployeeDto
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public DateTime FirstRecord { get; set; }
        public DateTime LastRecord { get; set; }
        public TimeSpan WorkingHour { get; set; }
        public DateTime LastOfDate { get; set; }
        public bool IsLate { get; set; }
        public bool IsFullWork { get; set; }
        public int ProcessTemp { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class RemoteLogDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime LogDate { get; set; }
        public  int Duration { get; set; }
    }
}

using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class InputContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=inputdb;TrustServerCertificate=True;MultipleActiveResultSets=true;", options => options.MigrationsAssembly("DataAccess"));
            optionsBuilder.UseSqlServer(@"Server=10.100.14.20,1433;Database=PersonnelTrackingSystemDb;TrustServerCertificate=True;MultipleActiveResultSets=true;user id=extadmn;password=Ea9130+!;", options => options.MigrationsAssembly("DataAccess"));
        }

        public DbSet<EmployeeRecord> EmployeeRecords { get; set; }
        public DbSet<RemoteWorkEmployee> RemoteWorkEmployees { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<OverShift> OverShifts { get; set; }
        //public DbSet<RemoteEmployeeDto> RemoteEmployeeDto { get; set; }
        //public DbSet<InfoEmployeeDto> InfoEmployeeDto { get; set; }
       
        public DbSet<RemoteEmployee> EmployeeDtos { get; set; }
        public DbSet<ReaderDataDto> ReaderDataDtos { get; set; }

        public DbSet<VpnEmployee> VpnEmployees { get; set; }
        public DbSet<FinalVpnEmployee> FinalVpnEmployees { get; set; }


    }
}

using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfVpnEmployeeDal : EfEntityRepositoryBase<VpnEmployee, InputContext>, IVpnEmployeeDal
    {
        public List<MixLateEmployeeDto> GetLatesByMonth(int month, int year,int? id, string? department)
        {
            //       using (InputContext context = new InputContext())
            //       {
            //           var lateEmployees = context.VpnEmployees.Where(x => x.LogDate.Month == month && x.LogDate.Year == year && x.RemoteEmployeeId==id).ToList();

            //           var lateOffice = context.EmployeeRecords.Where(x=>x.Date.Month==month && x.Date.Year == year && x.Department ==department && x.RemoteEmployeeId ==id).ToList();

            //           //var lateAll= from emp in context.EmployeeRecords
            //           //             join vpn in context.VpnEmployees
            //           //             on emp.RemoteEmployeeId equals vpn.RemoteEmployeeId
            //           //             select(new MixEmployeeDto
            //           //             {
            //           //                 Id = emp.RemoteEmployeeId,
            //           //                 FullName =emp.Name + " " +emp.SurName,
            //           //                 FirstRecord=emp.FirstRecord,
            //           //                 LastRecord=emp.LastRecord,
            //           //                 RemoteDate=vpn.LogDate,
            //           //                 RemoteWorkingHour=vpn.Duration,
            //           //                 Department=emp.Department,
            //           //                 WorkingHour
            //           //             })
            //           var sumDurationWithIDs = lateEmployees
            //           .Where(x => x.Id != null) // Id'si null olmayan çalışanları filtrele
            //           .GroupBy(x => x.Id) // Id'ye göre grupla
            //           .Select(g => new
            //           {
            //               Id = g.Key, // Gruplanan Id
            //               ToplamSure = g.Sum(x => x.Duration) // Aynı Id'ye sahip olan çalışanların duration'larını topla
            //           })
            //           .ToList();

            //           // Şimdi her bir çalışanın toplam süresine sahip olan grupları döngü içinde kullanabilirsiniz


            //           int daysInMonth = DateTime.DaysInMonth(year, month);
            //           DateTime currentDate = new DateTime(year, month, 1);
            //           DateTime lastDayofMonth = new DateTime(year, month, daysInMonth);
            //           // Hafta başlangıç ve bitiş tarihlerini hesapla
            //           //DateTime startOfWeek = currentDate.AddDays((week - 1) * 7 - (int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
            //           //DateTime endOfWeek = startOfWeek.AddDays(6);
            //           var StartingTime = TimeSpan.Parse("08:31:00");
            //           // Geç kalanları haftanın başlangıç ve bitiş tarihlerine göre filtrele
            //           var lateEmployeesInMonth = lateEmployees
            //               .Where(x => x.LogDate >= currentDate && x.LogDate <= lastDayofMonth)
            //               .Select(e => new MixEmployeeDto
            //               {
            //                   Id = e.RemoteEmployeeId,
            //                   FullName = e.FirstName + " " + e.LastName,
            //                   RemoteDate = e.LogDate.AddSeconds(-e.Duration),
            //                   RemoteWorkingHour = sumDurationWithIDs.FirstOrDefault(x => x.Id == e.RemoteEmployeeId)?.ToplamSure ?? 0,
            //                   IsLate = false,
            //                   IsFullWork = true,
            //               })
            //               .ToList();

            //           var lateEmployeesAfter8AM = lateEmployeesInMonth //geç kaldı fakat tam çalıştı
            //.Where(x => x.RemoteDate != null && x.RemoteDate.TimeOfDay > StartingTime).Select(e => new MixEmployeeDto
            //{
            //    Id = e.Id,
            //    FullName = e.FullName,
            //    FirstRecord = e.RemoteDate,
            //    RemoteWorkingHour = e.RemoteWorkingHour,

            //    // O günün tarihini al
            //    IsLate = true, // Geç kaldığı için IsLate'i true yap
            //    IsFullWork = true, // Tam olarak çalıştılar
            //    ProcessTemp = 1
            //})
            //.ToList();


            //           var employeesLessThan1130Mins = lateEmployeesInMonth //geç kaldı tam çalışmadı
            //               .Where(x => x.WorkingHour.TotalMinutes < 570 && x.RemoteDate != null && x.RemoteDate.TimeOfDay > StartingTime)
            //               .Select(e => new MixEmployeeDto
            //               {
            //                   Id = e.Id,
            //                   FullName = e.FullName,
            //                   RemoteDate = e.RemoteDate,


            //                   RemoteWorkingHour = e.RemoteWorkingHour,

            //                   IsLate = true, // Geç kalmadılar ama çalışma süresi yetersiz
            //                   IsFullWork = false, // 9:30'dan az çalıştılar
            //                   ProcessTemp = 2
            //               })
            //               .ToList();

            //           var employeesLessWorkMins = lateEmployeesInMonth //geç kalmadı ama tam çalıştı
            //               .Where(x => x.WorkingHour.TotalMinutes < 570 && x.FirstRecord != null && x.FirstRecord.TimeOfDay < StartingTime)
            //               .Select(e => new LateEmployeeDto
            //               {
            //                   Id = e.Id,
            //                   FullName = e.FullName,
            //                   FirstRecord = e.FirstRecord,
            //                   LastRecord = e.LastRecord,
            //                   LastOfDate = e.LastOfDate,
            //                   WorkingHour = e.WorkingHour,

            //                   IsLate = false, // Geç kalmadılar ama çalışma süresi yetersiz
            //                   IsFullWork = false, // 9:30'dan az çalıştılar
            //                   ProcessTemp = 3

            //               })
            //               .ToList();

            //           var employeeSuccess = lateEmployeesInMonth
            //               .Where(x => x.WorkingHour.TotalMinutes > 570 && x.FirstRecord != null && x.FirstRecord.TimeOfDay < StartingTime)
            //               .Select(e => new LateEmployeeDto
            //               {
            //                   Id = e.Id,
            //                   FullName = e.FullName,
            //                   FirstRecord = e.FirstRecord,
            //                   LastRecord = e.LastRecord,
            //                   LastOfDate = e.LastOfDate,
            //                   WorkingHour = e.WorkingHour,

            //                   IsLate = false,
            //                   IsFullWork = true,
            //                   ProcessTemp = 4
            //               }).ToList();

            //           // İki kategoriyi ayrı ayrı listelerde tutun
            //           var lateAndShortWorkEmployees = new List<LateEmployeeDto>();

            //           lateAndShortWorkEmployees.AddRange(employeesLessWorkMins);
            //           lateAndShortWorkEmployees.AddRange(employeeSuccess);

            //           var groupedLateEmployees = lateAndShortWorkEmployees
            //   .GroupBy(e => e.ProcessTemp)
            //   .Select(group => new MixLateEmployeeDto
            //   {
            //       ProcessTemp = group.Key,
            //       //Employees = group.ToList()
            //   })
            //   .ToList();


            //           return groupedLateEmployees;


            //}
            return null;
        }

        public void TransformToData()
        {
            using (var context = new InputContext()) 
            {
                var vpnEmployees = context.VpnEmployees.ToList(); 

                var groupedEmployees = vpnEmployees.GroupBy(e => new { e.LogDate.Date, e.RemoteEmployeeId });

                foreach (var group in groupedEmployees)
                {
                    var firstRecord = group.OrderBy(e => e.LogDate).FirstOrDefault();
                    var lastRecord = group.OrderByDescending(e => e.LogDate).FirstOrDefault();

                    var finalVpnEmployee = new FinalVpnEmployee
                    {
                        Name = firstRecord.FirstName,
                        SurName = firstRecord.LastName,
                        Department = firstRecord.Group,
                        RemoteEmployeeId = firstRecord.RemoteEmployeeId,
                        Date = group.Key.Date,
                        BytesIn=group.Sum(e=>e.Bytesin),
                        BytesOut=group.Sum(e=>e.Bytesout),
                        FirstRecord = firstRecord.FirstRecord.Value,
                        LastRecord = lastRecord?.LogDate ?? DateTime.MinValue,
                        Duration = TimeSpan.FromSeconds(group.Sum(e => e.Duration))
                    };

                    context.FinalVpnEmployees.Add(finalVpnEmployee);
                }

                context.SaveChanges();
            }
        }
        private Dictionary<int, int> CalculateTotalHoursById(List<LateEmployeeGroupDto> groupedLateEmployees)
        {
            var totalsById = new Dictionary<int, int>();

            foreach (var group in groupedLateEmployees)
            {
                foreach (var employee in group.Employees)
                {
                    if (!totalsById.ContainsKey(employee.Id.Value))
                    {
                        totalsById[employee.Id.Value] = 0;
                    }


                }
            }
            return totalsById;
        }
    }
}

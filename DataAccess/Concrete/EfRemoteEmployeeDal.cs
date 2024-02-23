﻿using Core.DataAccess.EntityFramework;
using Core.Utilites.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfRemoteEmployeeDal : EfEntityRepositoryBase<RemoteEmployee, InputContext>, IRemoteEmployee
    {
        public List<CombinedDataDto> GetAllWithLogs()
        {
            using (InputContext context = new InputContext())
            {
                var result = from emp in context.EmployeeDtos
                             join rdr in context.ReaderDataDtos
                             on emp.Id equals rdr.EmployeeDtoId
                             select new CombinedDataDto
                             {
                                 Id = rdr.Id,
                                 FirstName = emp.FirstName,
                                 LastName = emp.LastName,
                                 StartDate = rdr.StartDate,
                                 EndDate = rdr.EndDate,
                                 CalculatedDuration = rdr.Duration,
                                 RemoteEmployeeDtoId = rdr.EmployeeDtoId
                             };
                return result.ToList();
            }
        }

        public List<int> GetDurationByName(int Id, int month, int year, List<int> result)
        {
            
            int day = 1;
            int duration2 = 0;
            using (InputContext context = new InputContext())
            {
                //var remote = context.ReaderDataDtos.Where(x => x.EmployeeDtoId == Id && x.StartDate.Value.Month == month && x.StartDate.Value.Year == year)
                //    .Select(e => new { e.Duration ,e.StartDate.Value}).ToList();
                //List<int?> duration=remote
                //    .Select(e=>e.Duration) .ToList();


                var employeeRecords = context.EmployeeDtos
                    .Include(e => e.ReaderDataDtos)
                    .FirstOrDefault(e => e.Id == Id);

                var distinct2 = employeeRecords.ReaderDataDtos
     .Where(x => x.StartDate != null&& x.StartDate.Value.Month== month) // StartDate null değilse
     .DistinctBy(x => x.StartDate.Value.Day) // StartDate'e göre benzersiz olanları al
     .ToList();


                //            var distinctDays = employeeRecords.ReaderDataDtos
                //.Where(item => item.StartDate.HasValue
                //               && item.StartDate.Value.Month == month
                //               && item.StartDate.Value.Year == year
                //               && item.StartDate.Value.Day != null) // Ek kontrol
                //.Select(item => item.StartDate?.Day)
                //.Where(day => day.HasValue) // 
                //.Distinct()
                //.ToList();
                //            if (distinctDays.Count()>0)
                //            {
                //                Console.WriteLine("distinct başarılı");
                //            }
                if (employeeRecords != null)
                {

                    foreach (var item in employeeRecords.ReaderDataDtos)
                    {
                        //var date1 = item.StartDate.Value.Day;

                        if (item.StartDate.HasValue && item.StartDate.Value.Month == month && item.StartDate.Value.Year == year)
                        {
                            if (item.Duration!=null)
                            {
                                var duration1 = item.Duration;
                                duration2 += (Int32)duration1;
                            }
                           

                          
                            
                        }                           
                        
                    }
                    int averageduration;
                    if (distinct2.Any()) // distinct2 koleksiyonunda en az bir öğe varsa
                    {
                        averageduration = duration2 / distinct2.Count();
                    }
                    else
                    {
                        
                        averageduration = 0; // Veya başka bir varsayılan değer
                    }


                    result.Add(averageduration);
                }


                //result = result.Where(d => d != 0).ToList();
                return result;

            }
        }
        public void UpdateDataForSameId()
        {
            using (InputContext context = new InputContext())
            {
                var previousDayDataWithStartDateOnly = context.ReaderDataDtos
                  .Where(rd => rd.StartDate != null && rd.EndDate == null) // Önceki günün verilerini bulur: StartDate var, EndDate yok
                  .ToList();

                foreach (var item in previousDayDataWithStartDateOnly)
                {
                    var previousDayData = context.ReaderDataDtos.FirstOrDefault(rd =>
                      rd.EmployeeDtoId == item.EmployeeDtoId && // Çalışanın verilerini alırken Employee ID'sini kontrol etmek önemli
                      rd.StartDate == null && rd.EndDate != null &&
                      rd.EndDate.Value.Date.AddDays(-1) == item.StartDate.Value.Date);

                    if (previousDayData != null)
                    {
                        // Eğer önceki günün datalarında StartDate var ve EndDate yoksa, yeni gelen verinin EndDate değerini ata
                        previousDayData.StartDate = item.StartDate;
                        var duration = (int)(previousDayData.EndDate.Value - item.StartDate.Value).TotalSeconds;
                        previousDayData.Duration = duration;
                        context.Remove(item);
                        // Duration hesaplama veya diğer işlemler burada yapılabilir
                    }
                }

                context.SaveChanges();
            }
        }

        private static ReaderDataDto CombineData(ReaderDataDto previousDayData, ReaderDataDto currentDayData)
        {
            return new ReaderDataDto
            {
                EmployeeDtoId = previousDayData.EmployeeDtoId,
                StartDate = previousDayData.StartDate,
                EndDate = currentDayData.EndDate,
                // Diğer alanları da burada kopyalayabilir veya ayarlayabilirsiniz.
            };
        }




        public void DeleteEntryWithStartDateOnly()
        {
            using (InputContext context = new InputContext())
            {
                var allEmployees = context.EmployeeDtos
                    .Include(e => e.ReaderDataDtos)
                    .ToList();

                foreach (var employee in allEmployees)
                {
                    var entriesWithStartDateOnly = employee.ReaderDataDtos
                        .Where(rd => rd.StartDate != null && rd.EndDate != null) // Başlangıç ve bitiş tarihine sahip olanları filtrele
                        .GroupBy(rd => rd.StartDate) 
                        .SelectMany(grp => grp.Skip(1)) // İkinci tarihi olan girişten başlayarak
                        .ToList();

                    foreach (var entry in entriesWithStartDateOnly)
                    {
                        context.ReaderDataDtos.Remove(entry);
                    }
                }

                context.SaveChanges();
            }
        }

    }
}

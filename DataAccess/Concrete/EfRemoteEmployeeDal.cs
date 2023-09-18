using Core.DataAccess.EntityFramework;
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
            using (InputContext context=new InputContext())
            {
                var result = from emp in context.EmployeeDtos
                             join rdr in context.ReaderDataDtos
                             on emp.Id equals rdr.EmployeeDtoId
                             select new CombinedDataDto
                             {
                                Id=emp.Id,
                                FirstName=emp.FirstName,
                                LastName=emp.LastName,
                                StartDate=rdr.StartDate, 
                                EndDate=rdr.EndDate,
                                CalculatedDuration=rdr.Duration,
                                RemoteEmployeeDtoId=rdr.EmployeeDtoId
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
                    .SingleOrDefault(e => e.Id == Id);
                if (employeeRecords!=null)
                {
                   
                    foreach (var item in employeeRecords.ReaderDataDtos)
                    {
                        if (item.StartDate.Value.Month==month && item.StartDate.Value.Year==year)
                        {
                        if (item.StartDate.Value != item.StartDate.Value)
                        {
                            day += 1;
                        }
                            var duration1 = item.Duration;
                            
                            duration2 += (Int32)duration1;
                        }
                    }
                    int averageduration = duration2 / day;
                    result.Add(averageduration);
                }

                //result = result.Where(d => d != 0).ToList();
                return result;

            }
        }
    }
}

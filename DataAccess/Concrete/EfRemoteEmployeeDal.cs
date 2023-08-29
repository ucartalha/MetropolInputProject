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

        public List<int> GetDurationByName(string name, int month)
        {
            using (InputContext context = new InputContext())
            {
                var remoteEmployeeDurations = (
                    from emp in context.EmployeeDtos
                    join rdr in context.ReaderDataDtos on emp.Id equals rdr.EmployeeDtoId
                    where emp.FirstName == name && rdr.StartDate.Value.Month== month
                    select rdr.Duration.Value
                ).ToList();

                return remoteEmployeeDurations;
            }
        }
    }
}

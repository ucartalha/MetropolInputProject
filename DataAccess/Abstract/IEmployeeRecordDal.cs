using Core.DataAccess;
using Core.Utilites.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IEmployeeRecordDal: IEntityRepository<EmployeeRecord>
    {
        public IResult DeleteByDateRange(DateTime startDate, DateTime endDate);
        public List<PersonalEmployeeDto> GetEmployeeDetail(string Name);

        public List<TimeSpan> GetWorkingHoursByName(string name,int month);
       
    }
}

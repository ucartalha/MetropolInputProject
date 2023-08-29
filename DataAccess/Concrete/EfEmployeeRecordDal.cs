using Core.DataAccess.EntityFramework;
using Core.Utilites.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfEmployeeRecordDal : EfEntityRepositoryBase<EmployeeRecord, InputContext>, IEmployeeRecordDal
    {
         

        public List<PersonalEmployeeDto> GetEmployeeDetail(string Name)
        { 
            //using (InputContext context = new InputContext())
            //{
            //    var result = from e in context.EmployeeRecords
            //                 join r in context.RemoteWorkEmployees
            //                 on e.Name.ToLower() equals r.FirstName.ToLower()
            //                 where e.Name == Name.ToLower()
            //                 select new PersonalEmployeeDto
            //                 {
            //                     FullName = e.Name + e.SurName,
            //                     OfficeDate = e.Date,
            //                     Duration = r.Duration != null && r.Duration > 0 ? r.Duration : 0,
            //                     RemoteDate = r.LogDate != null ? r.LogDate : DateTime.MinValue,
            //                     WorkingHour = e.WorkingHour
            //                 };
            //}
            using (InputContext context = new InputContext())
                {
                    //var overShiftsList = context.OverShifts.ToList(); // OverShifts tablosunu bellekte bir liste olarak yükle
                    List<PersonalEmployeeDto> personalEmployeetDtoList = new List<PersonalEmployeeDto>();

                    var remote = context.RemoteWorkEmployees.Where(x => x.FirstName == Name ).Select(x => new PersonalEmployeeDto
                    {
                        FullName = x.FirstName+x.LastName,
                        RemoteDuration = x.RemoteDuration,
                        OfficeDate = null,
                        RemoteDate = x.LogDate,
                        WorkingHour = null
                    }).ToList();
                personalEmployeetDtoList.AddRange(remote); 

                    var office = context.EmployeeRecords.Where(x => x.Name == Name).Select(x => new PersonalEmployeeDto
                    {
                        FullName = x.Name,
                        RemoteDuration = null,
                        OfficeDate = x.Date,
                        RemoteDate = null,
                        WorkingHour = x.WorkingHour
                    }).ToList();
                personalEmployeetDtoList.AddRange(office);


                    return personalEmployeetDtoList;


                }

                
            
        }
        //public List<PersonalEmployeeDto> GetEmployeeDetail(string name)
        //{
        //    using (InputContext context = new InputContext())
        //    {
        //        var employeeRecords = context.EmployeeRecords
        //            .Where(e => e.Name.Contains(name) || e.SurName.Contains(name))
        //            .Select(e => new PersonalEmployeeDto { FullName = e.Name + " " + e.SurName })
        //            .ToList();

        //        if (!context.RemoteWorkEmployees.Any())
        //        {
        //            return employeeRecords;
        //        }
        //        else
        //        {
        //            var result = from e in employeeRecords
        //                         join r in context.RemoteWorkEmployees
        //                         on e.FullName equals r.FirstName + " " + r.LastName
        //                         select new PersonalEmployeeDto
        //                         {
        //                             FullName = e.FullName,
        //                             Duration = r.Duration,
        //                             WorkingHour = e.WorkingHour
        //                         };

        //            return result.ToList();
        //        }
        //    }
        //}
        public IResult DeleteByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (InputContext context = new InputContext())
                {
                    var recordsToDelete = context.EmployeeRecords
                        .Where(e => e.Date >= startDate.Date && e.Date < endDate.Date)
                        .ToList();

                    context.EmployeeRecords.RemoveRange(recordsToDelete);
                    context.SaveChanges();
                }
                return new SuccessResult("Records deleted successfully.");
            }
            catch (Exception ex)
            {

                return new ErrorResult($"An error occurred while deleting records: {ex.Message}");
            }
        }
        public List<TimeSpan> GetWorkingHoursByName(string name, int month)
        {
            using (InputContext context = new InputContext()) 
            {
                var employeeRecords = context.EmployeeRecords
                    .Where(e => e.Name == name && e.Date.Month == month)
                    .Select(e => new { e.WorkingHour, e.Date })
                    .ToList();

                List<TimeSpan> workingHours = employeeRecords
                    .Select(e => e.WorkingHour)
                    .ToList();

                return workingHours;
            }
        }
    }


    }



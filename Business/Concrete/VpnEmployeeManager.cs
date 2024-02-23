using Business.Abstract;
using Castle.Components.DictionaryAdapter.Xml;
using Core.Extensions;
using Core.Utilites.Helpers;
using Core.Utilites.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class VpnEmployeeManager : IVpnEmployeeService
    {
        IVpnEmployeeDal _vpnemployee;
        InputContext _dbContext;
        public VpnEmployeeManager(IVpnEmployeeDal vpnEmployeeDal, InputContext context)
        {
            _vpnemployee = vpnEmployeeDal;
            _dbContext = context;
        }
        public IResult Add(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentNullException("No file uploaded.");
                }

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var configuration = new ExcelReaderConfiguration
                    {
                        FallbackEncoding = Encoding.GetEncoding("UTF-8"),
                    };

                    using (var reader = ExcelReaderFactory.CreateReader(stream, configuration))
                    {
                        var records = new List<VpnEmployee>();
                        while (reader.Read())
                        {
                            try
                            {
                                var DateValue = reader.GetValue(0);
                                var tunnelip = reader.GetValue(8);

                                //string tunnelip = tunnelipValue != null ? Convert.ToString(tunnelipValue) : string.Empty;
                                if (DateValue == null || tunnelip == null || string.IsNullOrEmpty(tunnelip.ToString()) || DateValue.Equals("Log Tarihi  [date1]"))
                                {
                                    continue; // Eğer veri uygun değilse, bir sonraki satıra geçiyoruz
                                }

                                var logDate = reader.GetValue(0) != null ? Convert.ToDateTime(reader.GetValue(0)) : DateTime.MinValue;
                                var userValue = reader.GetValue(1) != null ? reader.GetString(1) : string.Empty;
                                
                                //!= null ? Convert.ToInt32(reader.GetValue(3)) : 0
                                var bytesOutValue = reader.GetValue(3);
                                if (bytesOutValue.ToString().IsNullOrEmpty())
                                {
                                    continue;
                                }
                                    
                                int bytesOut = 0;
                                if (bytesOutValue != null && int.TryParse(bytesOutValue.ToString(), out var tempBytesOut))
                                {
                                    bytesOut = tempBytesOut;
                                }



                                //!= null ? Convert.ToInt32(reader.GetValue(4)) : 0
                                var bytesInValue = reader.GetValue(4);
                                int bytesIn = 0;
                                if (bytesInValue != null && int.TryParse(bytesInValue.ToString(), out var tempBytesIn))
                                {
                                    bytesIn = tempBytesIn;
                                }

                                var durationValue = reader.GetValue(5) != null ? Convert.ToInt32(reader.GetValue(5)) : 0;
                                
                                //if (logDate == null || durationValue == null)
                                //    continue;



                                var nameComponents = userValue.Split('.');
                                var firstName = nameComponents.Length > 0 ? nameComponents[0] : "";
                                var lastName = nameComponents.Length > 1 ? nameComponents[1] : "";
                                var matchingRemoteId = GetByFullName(firstName, lastName);


                                var employee = new VpnEmployee
                                {
                                    LogDate = logDate,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Group = reader.GetValue(2) != null ? reader.GetString(3) : string.Empty,
                                    Bytesout = bytesOut,
                                    Bytesin = bytesIn,
                                    Duration = durationValue,
                                    RemoteEmployeeId = matchingRemoteId.Id
                                };

                                // İlk giriş tarihini kontrol et
                                if ((employee.FirstRecord == null || logDate < employee.FirstRecord) || bytesOut==null)
                                {
                                    employee.FirstRecord = logDate.AddSeconds(-durationValue);
                                }

                                // Son çıkış tarihini kontrol et
                                if (employee.LastRecord == null || employee.FirstRecord > employee.LastRecord||bytesOut!=null)
                                {
                                    employee.LastRecord = logDate;
                                }

                                records.Add(employee);


                                //var matchingRemoteId = GetByFullName(firstName, lastName);
                                //records.Add(new VpnEmployee
                                //{
                                //    LogDate = logDate,
                                //    FirstName = firstName,
                                //    LastName = lastName,
                                //    Group = reader.GetValue(3) != null ? reader.GetString(3) : string.Empty,
                                //    Bytesout = bytesOut,
                                //    Bytesin = bytesIn,
                                //    Duration = durationValue,
                                //    RemoteEmployeeId=matchingRemoteId.Id,
                                //    FirstRecord=DateTime.MinValue,
                                //    LastRecord=DateTime.MinValue

                                //});
                                //records.ToList();

                            }
                            catch (Exception ex)
                            {
                                return new ErrorResult("An error occurred while processing data: " + ex.Message);
                            }
                        }
                        //try
                        //{
                        //    var existingEmployeesDictionary = _dbContext.VpnEmployees.ToDictionary(e => e.FirstName + "_" + e.LastName, e => e);
                        //    var denden = new List<VpnEmployee>();
                        //    for (int i = 0; i < records.Count; i++)
                        //    {
                        //        var employee = denden.FirstOrDefault(d => d.FirstName == records[i].FirstName);
                        //        if (records[i].RemoteEmployeeId!=null)
                        //        {

                        //        }
                        //    }
                        //}
                        //catch (Exception)
                        //{

                        //    throw;
                        //}

                        var existingRecords = new List<VpnEmployee>();
                        existingRecords = _vpnemployee.GetAll();
                        var distinctRecords = records
                            .GroupBy(r => new { r.LogDate, r.FirstName, r.LastName, r.Bytesin, r.Bytesout, r.Duration,r.RemoteEmployeeId })
                            .Select(g => g.First())
                            .ToList();

                        var recordsToAdd = distinctRecords
                            .Where(r => !existingRecords.Any(er =>
                                er.LogDate == r.LogDate &&
                                er.FirstName == r.FirstName &&
                                er.LastName == r.LastName &&
                                er.Duration == r.Duration &&
                                er.Bytesin == r.Bytesin &&
                                er.Bytesout == r.Bytesout &&
                                er.RemoteEmployeeId== r.RemoteEmployeeId
                            ))
                            .ToList();

                        foreach (var empRecors in recordsToAdd)
                        {
                            _vpnemployee.Add(empRecors);
                        }
                        
                       

                        _dbContext.SaveChanges();

                        

                        var uploadedFile = new UploadedFile
                        {
                            FileName = file.FileName,
                            FileSize = file.Length,
                            UploadTime = DateTime.Now,
                            ContentHash = HashHelper.ComputeFileHash(file)
                        };

                        var existingUploadedFile = _dbContext.UploadedFiles.FirstOrDefault(uf => uf.ContentHash == uploadedFile.ContentHash);
                        if (existingUploadedFile != null)
                        {
                            return new ErrorResult("File already exists.");
                        }
                        else
                        {
                            _dbContext.UploadedFiles.Add(uploadedFile);
                            _dbContext.SaveChanges();
                        }
                        _vpnemployee.TransformToData();
                        return new SuccessResult("Excel file processed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ErrorResult("An error occurred: " + ex.Message);
            }
        }


        private RemoteEmployee GetByFullName(string firstName, string lastName)
        {
            var result = _dbContext.EmployeeDtos.FirstOrDefault(e =>
             e.FirstName.Contains(firstName) && e.LastName.Contains(lastName));

            if (result == null)
            {
                // Eğer veri bulunamazsa, yeni bir RemoteEmployee nesnesi oluşturup kaydet
                var newEmployee = new RemoteEmployee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    // Diğer özellikleri de gerekirse burada tanımlayabilirsiniz
                };

                _dbContext.EmployeeDtos.Add(newEmployee);
                _dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydet

                return newEmployee;
            }

            // Eğer veri bulunursa mevcut RemoteEmployee nesnesini döndür
            return result;
        }
    }
}

using Business.Abstract;
using Core.Utilites.Helpers;
using Core.Utilites.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using ExcelDataReader; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RemoteWorkEmployeeManager : IRemoteWorkEmployeeService
    {
        
        IRemoteEmployee _remoteEmployeeDal;
        InputContext _dbContext;
        public RemoteWorkEmployeeManager(IRemoteEmployee remoteEmployee, InputContext context)
        {
            
            _remoteEmployeeDal = remoteEmployee;
            _dbContext = context;
        }
        public IResult Add(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException("No file uploaded.");
            }

            // Dosya adını hashleyerek kaydetme
            string fileName = HashHelper.ComputeFileHash(file);

            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var configuration = new ExcelReaderConfiguration
                    {
                        // Karakter kodlamasını burada belirleyin
                        FallbackEncoding = Encoding.GetEncoding("UTF-8"), // Örnek olarak UTF-8 karakter kodlaması kullanılıyor
                                                                          // Diğer yapılandırma seçeneklerini de burada belirleyebilirsiniz
                    };

                    using (var reader = ExcelReaderFactory.CreateReader(stream, configuration))
                    {
                        var remoteEmployees = new List<RemoteDtos>();

                        while (reader.Read())
                        {
                            try
                            {
                                // Excel'den veriyi okuyoruz
                                var logDateValue = reader.GetValue(0); // Log tarih değerini al
                                var eventValueObj = reader.GetValue(1); // Event değerini al

                                // Gerekli kontrolleri yapıyoruz: Tarih ve event değeri boş olmamalı ve event değeri 24 veya 25 olmalı
                                if (logDateValue == null || eventValueObj == null || !int.TryParse(eventValueObj.ToString(), out int eventValue) || (eventValue != 24 && eventValue != 25))
                                {
                                    continue; // Eğer veri uygun değilse, bir sonraki satıra geçiyoruz
                                }


                                var firstName = string.Empty;
                                var lastName = string.Empty;

                                var userValue = reader.GetValue(2);
                                var fullName = userValue != null ? userValue.ToString() : "";
                                var nameComponents = fullName.Split('\\');

                                if (nameComponents.Length == 2)
                                {
                                    var domain = nameComponents[0]; // METROPOLCARD
                                    var FullName = nameComponents[1]; // birhan.demirbas

                                    var fullNameComponents = FullName.Split('.');
                                    firstName = fullNameComponents.Length > 0 ? fullNameComponents[0] : "";
                                    lastName = fullNameComponents.Length > 1 ? fullNameComponents[1] : "";

                                    // firstName ve lastName değişkenlerini kullanabilirsiniz.
                                }
                                else
                                {
                                    // Uygun olmayan format durumunda işlemler yapılabilir.
                                }
                                // Ad ve soyad değişkenlerini tanımlıyoruz
                                //var firstName = string.Empty;
                                //var lastName = string.Empty;
                                //var userValue = reader.GetValue(2);
                                //if (userValue != null)
                                //{
                                //    var nameComponents = userValue.ToString().Split('.');
                                //    firstName = nameComponents.Length > 0 ? nameComponents[0] : "";
                                //    lastName = nameComponents.Length > 1 ? nameComponents[1] : "";
                                //}


                                // Süre hesaplama için gerekli değişkenleri tanımlıyoruz
                                TimeSpan remoteDuration = TimeSpan.Zero;
                                DateTime logDate = DateTime.MinValue; // logDate değişkenine varsayılan bir değer atıyoruz

                                // Eğer logDateValue uygun bir tarih formatında ise logDate'e çeviriyoruz
                                if (logDateValue != null && DateTime.TryParse(logDateValue.ToString(), out logDate))
                                {
                                    // Veri uygunsa logDate özelliğine ekliyoruz
                                    logDate = logDate.ToUniversalTime().AddHours(3); // Zamanı UTC zamanına çeviriyoruz
                                }

                                int TimeRemote = (int)remoteDuration.TotalSeconds;



                                var Response = new RemoteDtos
                                {
                                    FirstName = firstName,
                                    LastName = lastName,
                                    EventID = eventValue,
                                    LogDate = logDate
                                };
                                remoteEmployees.Add(Response);
                                remoteEmployees.ToList();
                                var startDate = DateTime.MinValue;
                                var endDate = DateTime.MinValue;
                                if (eventValue == 25)
                                {

                                    startDate = logDate;

                                }
                                else if (eventValue == 24)
                                {
                                    endDate = logDate;
                                    if (endDate == startDate)
                                    {
                                        endDate = DateTime.MinValue;
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                return new ErrorResult("Hata oluştu: " + ex.Message);
                            }
                        }
                        try
                        {
                            var existingEmployeesDictionary = _dbContext.EmployeeDtos.ToDictionary(e => e.FirstName, e => e);

                            var denden = new List<RemoteEmployee>();

                            for (int i = 0; i < remoteEmployees.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(remoteEmployees[i].FirstName) && !string.IsNullOrEmpty(remoteEmployees[i].LastName))
                                {
                                    var employee = denden.FirstOrDefault(d => d.FirstName == remoteEmployees[i].FirstName);

                                    if (employee == null)
                                    {
                                        employee = new RemoteEmployee
                                        {
                                            FirstName = remoteEmployees[i].FirstName,
                                            LastName = remoteEmployees[i].LastName,
                                            ReaderDataDtos = new List<ReaderDataDto>()
                                        };
                                        denden.Add(employee);
                                    }

                                    if (remoteEmployees[i].EventID == 25)
                                    {
                                        if (remoteEmployees[i].LogDate != null)
                                        {
                                            employee.ReaderDataDtos.Add(new ReaderDataDto
                                            {
                                                StartDate = remoteEmployees[i].LogDate,
                                                Duration = 0,
                                                EndDate = null
                                            });
                                        }
                                    }
                                    else if (remoteEmployees[i].EventID == 24)
                                    {
                                        if (remoteEmployees[i].LogDate != null)
                                        {
                                            var existingReaderData = employee.ReaderDataDtos.FirstOrDefault(d => d.EndDate == null);
                                            if (existingReaderData != null)
                                            {
                                                existingReaderData.EndDate = remoteEmployees[i].LogDate;
                                            }
                                            else
                                            {
                                                employee.ReaderDataDtos.Add(new ReaderDataDto
                                                {
                                                    StartDate = null,
                                                    Duration = null,
                                                    EndDate = remoteEmployees[i].LogDate
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (var employeeDto in denden)
                            {
                                if (existingEmployeesDictionary.TryGetValue(employeeDto.FirstName, out var existingEmployee))
                                {
                                    foreach (var readerDataDto in employeeDto.ReaderDataDtos)
                                    {
                                        if (readerDataDto.StartDate.HasValue && !readerDataDto.EndDate.HasValue)
                                        {
                                            var existingReaderData = existingEmployee.ReaderDataDtos.FirstOrDefault(rd => rd.StartDate == readerDataDto.StartDate);

                                            if (existingReaderData != null)
                                            {
                                                
                                                readerDataDto.EndDate = readerDataDto.StartDate.Value.Date.Add(new TimeSpan(23, 59, 59));
                                                existingReaderData.Duration = (int)(readerDataDto.EndDate - readerDataDto.StartDate).Value.TotalSeconds;
                                                
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (var employeeDto in denden)
                            {
                                var existingEmployee = _dbContext.EmployeeDtos.FirstOrDefault(e => e.FirstName == employeeDto.FirstName && e.LastName ==employeeDto.LastName);

                                if (existingEmployee == null)
                                {
                                    existingEmployee = new RemoteEmployee
                                    {
                                        FirstName = employeeDto.FirstName,
                                        LastName = employeeDto.LastName,
                                        ReaderDataDtos = new List<ReaderDataDto>()
                                    };
                                    _dbContext.EmployeeDtos.Add(existingEmployee);
                                }

                                foreach (var readerDataDto in employeeDto.ReaderDataDtos)
                                {
                                    var existingReaderData = existingEmployee.ReaderDataDtos.FirstOrDefault(rd => rd.StartDate == readerDataDto.StartDate);

                                    if (existingReaderData != null)
                                    {
                                        existingReaderData.EndDate = readerDataDto.EndDate;
                                        existingReaderData.Duration = readerDataDto.StartDate.HasValue && readerDataDto.EndDate.HasValue
                                            ? (int)(readerDataDto.EndDate.Value - readerDataDto.StartDate.Value).TotalSeconds
                                            : 0;
                                    }
                                    else if (readerDataDto.StartDate.HasValue && !readerDataDto.EndDate.HasValue)
                                    {
                                        var defaultEndDate = readerDataDto.StartDate.Value.Date.Add(new TimeSpan(23, 59, 59)); // Varsayılan saat 23:59:59
                                        var newReaderData = new ReaderDataDto
                                        {
                                            StartDate = readerDataDto.StartDate,
                                            EndDate = defaultEndDate,
                                            Duration = (int)(defaultEndDate-readerDataDto.StartDate.Value).TotalSeconds
                                        };

                                        existingEmployee.ReaderDataDtos.Add(newReaderData);
                                    }
                                    else if (readerDataDto.EndDate.HasValue && !readerDataDto.StartDate.HasValue)
                                    {
                                        var defaultStartDate = readerDataDto.EndDate.Value.Date.Add(new TimeSpan(00, 00, 00)); // Varsayılan saat 23:59:59
                                        var newReaderData = new ReaderDataDto
                                        {
                                            StartDate = defaultStartDate,
                                            EndDate = readerDataDto.EndDate,
                                            Duration = readerDataDto.Duration > 18000 ? (int?)null : (int?)(readerDataDto.EndDate.Value - defaultStartDate).TotalSeconds
                                        };
                                        if (newReaderData.Duration>18000)
                                        {
                                            newReaderData.Duration = null;
                                        }
                                        existingEmployee.ReaderDataDtos.Add(newReaderData);

                                    }
                                    else
                                    {
                                        var newReaderData = new ReaderDataDto
                                        {
                                            StartDate = readerDataDto.StartDate,
                                            EndDate = readerDataDto.EndDate,
                                            Duration = readerDataDto.StartDate.HasValue && readerDataDto.EndDate.HasValue
                                                ? (int)(readerDataDto.EndDate.Value - readerDataDto.StartDate.Value).TotalSeconds
                                                : 0
                                        };

                                        existingEmployee.ReaderDataDtos.Add(newReaderData);
                                    }
                                }
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
                                return new ErrorResult("Dosya zaten var.");
                            }
                            else
                            {
                                _dbContext.UploadedFiles.Add(uploadedFile);
                                _dbContext.SaveChanges();
                            }

                            return new SuccessResult("Excel dosyası başarıyla işlendi.");

                        }
                        catch (Exception ex)
                        {
                            Type exceptionType = ex.GetType();


                            string errorMessage = ex.Message;


                            if (ex.InnerException != null)
                            {

                                Type innerExceptionType = ex.InnerException.GetType();


                                string innerErrorMessage = ex.InnerException.Message;
                            }

                            return new ErrorResult("Veritabanına kaydedilirken bir hata oluştu: " + ex.Message);
                        }





                    }
                }
            }
            return new SuccessResult("Excel dosyası başarıyla işlendi.");


        }

        public IDataResult<List<RemoteWorkEmployee>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<CombinedDataDto>> GetAllWithLogs()
        {
            return new SuccessDataResult<List<CombinedDataDto>>(_remoteEmployeeDal.GetAllWithLogs());
        }

        



    }


}


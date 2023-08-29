using Business.Abstract;
using Core.Utilites.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PersonalManager : IPersonalService
    {
        IPersonalDal _personalDal;
        IEmployeeRecordDal _employeeDal;
        IRemoteEmployee _remoteEmployee;
        public PersonalManager(IPersonalDal personalDal, IEmployeeRecordDal employeeDal, IRemoteEmployee remoteEmployee)
        {
            _personalDal = personalDal;
            _employeeDal = employeeDal;
            _remoteEmployee = remoteEmployee;
        }

        public IDataResult<List<Personal>> GetAll()
        {                        
                return new SuccessDataResult<List<Personal>>(_personalDal.GetAll());            
        }

        //public IDataResult<Personal> ProcessMonthlyAverage(string name, int month)
        //{
        //    // İlgili aydaki çalışma saatlerini getir
        //    List<TimeSpan> workingHours = _employeeDal.GetWorkingHoursByName(name, month);

        //    if (workingHours.Count > 0)
        //    {
        //        // Ay içindeki toplam süreyi hesapla
        //        TimeSpan totalHours = TimeSpan.Zero;
        //        foreach (TimeSpan hour in workingHours)
        //        {
        //            totalHours += hour;
        //        }
        //        DateTime date = new DateTime(DateTime.Now.Year, month, 1);
        //        // Ortalama saat hesapla
        //        TimeSpan monthlyAverage = TimeSpan.FromTicks(totalHours.Ticks / workingHours.Count);

        //        // Personal nesnesini oluştur ve verileri doldur
        //        Personal personal = new Personal();
        //        personal.Name = name;
        //        personal.AverageHour = monthlyAverage;
        //        personal.Date = date;

        //        // İşlem başarılı ise Personal nesnesini dön
        //        return new SuccessDataResult<Personal>(personal, "Aylık ortalama başarıyla hesaplandı.");
        //    }
        //    else
        //    {
        //        // İsim için çalışma saatleri bulunamadı durumunu işleyebilirsiniz
        //        return new ErrorDataResult<Personal>("İsimle eşleşen çalışma saatleri bulunamadı.");
        //    }
        //}
        public IDataResult<List<Personal>> ProcessMonthlyAverage(string name, int month)
        {
            List<Personal> personalList = new List<Personal>();
            DateTime currentDate = DateTime.Now;
            int currentYear = currentDate.Year;
            int previousMonth1 = month - 1;
            int previousMonth2 = month - 2;

            if (previousMonth1 <= 0)
            {
                previousMonth1 += 12;
                currentYear--;
            }

            if (previousMonth2 <= 0)
            {
                previousMonth2 += 12;
                currentYear--;
            }

            for (int i = 0; i < 3; i++)
            {
                int targetMonth = month - i;
                if (targetMonth <= 0)
                {
                    targetMonth += 12;
                }

                List<TimeSpan> workingHours = _employeeDal.GetWorkingHoursByName(name, targetMonth);

                if (workingHours.Count > 0)
                {
                    // Ay içindeki toplam süreyi hesapla
                    TimeSpan totalHours = TimeSpan.Zero;
                    foreach (TimeSpan hour in workingHours)
                    {
                        totalHours += hour;
                    }

                    // Ortalama saat hesapla
                    TimeSpan monthlyAverage = TimeSpan.FromTicks(totalHours.Ticks / workingHours.Count);

                    List<int> duration = _remoteEmployee.GetDurationByName(name, targetMonth);
                    int totalDuration = 0;

                    if (duration.Count > 0)
                    {
                        foreach (int second in duration)
                        {
                            totalDuration += second;
                        }
                    }

                    int averageDuration = duration.Count > 0 ? totalDuration / duration.Count : 0;


                    // Personal nesnesini oluştur ve verileri doldur
                    Personal personal = new Personal();
                    personal.Name = name;
                    personal.AverageHour = monthlyAverage;
                    personal.RemoteHour = averageDuration;
                    personal.Date = new DateTime(currentYear, targetMonth, 1);

                    personalList.Add(personal);
                }
            }

            if (personalList.Count > 0)
            {
                return new SuccessDataResult<List<Personal>>(personalList, "Önceki 3 ayın ortalama saatleri başarıyla hesaplandı.");
            }
            else
            {
                // İsim için çalışma saatleri bulunamadı durumunu işleyebilirsiniz
                return new ErrorDataResult<List<Personal>>("İsimle eşleşen çalışma saatleri bulunamadı.");
            }
        }
    }
}

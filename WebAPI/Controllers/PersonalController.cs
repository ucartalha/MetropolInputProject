using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalController : Controller
    {
        IPersonalService _services;
        public PersonalController(IPersonalService personalService)
        {
                _services = personalService;
        }
        [HttpGet("process-monthly-average")]
        public IActionResult ProcessMonthlyAverage(string name, int month)
        {
            var result = _services.ProcessMonthlyAverage(name, month);

            if (result.Success)
            {
                return Ok(result); // Başarılı ise Personal nesnesini döndür
            }
            else
            {
                return BadRequest(result.Message); // Hata durumunda hata mesajını döndür
            }
        }
    }
}

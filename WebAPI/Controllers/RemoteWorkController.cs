using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteWorkController : Controller
    {
        IRemoteWorkEmployeeService _services;
        public RemoteWorkController(IRemoteWorkEmployeeService service)
        {
            _services = service;
        }
        [HttpPost("add")]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length==10* 1024 *1024)
                {
                    return BadRequest("Dosya boyutu 10 Megabayt'tan büyük olamaz");
                }
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var result= _services.Add(file);
                if (result.Success)
                {
                    return Ok("File uploaded successfully.");
                }
                return BadRequest("Dosya formatı uygun değil" + result.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Hata durumunda ilgili işlemleri gerçekleştirin (örn. hata loglama)
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while uploading the file: {ex.Message}");
            }
        }
        
        [HttpGet("getall")]
        public IActionResult GetAll() 
        {
            var result = _services.GetAllWithLogs();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllWithLogs")]
        public IActionResult GetAllWithLogs() 
        {
            var result= _services.GetAllWithLogs();
            if(result.Success)
            { return Ok(result); }
            return BadRequest(result);
        }
        [HttpPost("updatelogdate")]
        public IActionResult UpdateLogDate(int readerDataId, DateTime? newStartDate, DateTime? newEndDate) 
        {
            try { 
            var result = _services.UpdateReaderData(readerDataId, newStartDate, newEndDate);
            if (result.Success)
            {
                return Ok(new { success = true, message = "Veri başarıyla güncellendi." });
            }
            return BadRequest(new { success = false, message = result.Message });
        }
            catch {
                return BadRequest(new { success = false, message = "Bir hata oluştu: "});
            }
            }

        [HttpGet("GetAllwithSumDuration")]
        public IActionResult GetAllWithSumDuration()
        {
            var result = _services.GetAllWithoutNull();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

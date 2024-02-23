using Autofac.Core;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VpnEmployeeController : Controller
    {
        IVpnEmployeeService _employeService;
        public VpnEmployeeController(IVpnEmployeeService employeeService)
        {
            _employeService = employeeService;
        }
        [HttpPost("add")]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                var result = _employeService.Add(file);

                if (result.Success)
                {
                    return Ok("File uploaded successfully.");
                }
                return BadRequest(result.Message);
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
        private bool DosyaZatenVar(string dosyaAdi)
        {

            return false; // Dosya yoksa varsayılan olarak false döndürüldü.
        }
    }
}

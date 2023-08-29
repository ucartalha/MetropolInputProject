using Business.Abstract;
using Core.Utilites.Helpers;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        IEmployeeRecordService _services;
        public EmployeeController(IEmployeeRecordService service)
        {
            _services = service;
        }
         

        [HttpGet("getdetails")] 
        public IActionResult Get(string name)
        {
            var result=_services.GetPersonalDetails(name);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                _services.Add(file);
                return Ok("File uploaded successfully.");
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

        [HttpPost("deletebydate")]
        public IActionResult DeleteByDate(DateTime startDate, DateTime endDate)
        {
            if(_services.DeleteByDateRange(startDate, endDate).Success)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("getbyaveragehour")]
        public IActionResult GetAverageHour(string name,double average)
        {
            if (_services.GetAverageHour(name,average).Success)
            {
                return Ok();
            }
                
            return BadRequest();
        }
        [HttpGet("getbyname")]
        public IActionResult GetByName(string name)
        {
            var result = _services.GetByName(name);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}


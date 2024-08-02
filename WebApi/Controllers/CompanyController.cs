using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger logger)
        {

            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }
        // GET api/<controller>
        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            _logger.LogInformation("Get all the companies.");
            var items = await _companyService.GetAllCompaniesAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(items);
        }

        // GET api/<controller>/5
        public async Task<CompanyDto> Get(string companyCode)
        {
            _logger.LogInformation($"Get the companies {companyCode}.");
            var item = await _companyService.GetCompanyByCodeAsync(companyCode);
            if (item == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return _mapper.Map<CompanyDto>(item);
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            if (companyDto == null)
            {
                return BadRequest("Company data is null.");
            }

            var company = _mapper.Map<CompanyInfo>(companyDto);
            bool result;

            try
            {
                result = await _companyService.SaveCompanyAsync(company);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return InternalServerError(e);
            }

            if (result)
            {
                return Created(new Uri(Request.RequestUri + "/" + company.CompanyCode), company);
            }
            else
            {
                return BadRequest("Failed to save company data.");
            }
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(string companyCode, [FromBody] CompanyDto companyDto)
        {
            if (companyDto == null || companyCode != companyDto.CompanyCode)
            {
                return BadRequest("Invalid data.");
            }

            var company = _mapper.Map<CompanyInfo>(companyDto);
            var result = await _companyService.SaveCompanyAsync(company);

            if (result)
            {
                return Ok(company);
            }
            else
            {
                return BadRequest("Failed to update company data.");
            }
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string companyCode)
        {
            var result = await _companyService.DeleteCompanyAsync(companyCode);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to delete company.");
            }
        }
    }
}
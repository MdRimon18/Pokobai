using Domain.Services.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorInMvc.Controllers.Api
{
  
    [ApiController]
    [Route("api/Country")]
    public class CountryController : ControllerBase
    {

        private readonly CountryServiceV2 _countryServicev2;
        public CountryController(CountryServiceV2 countryServicev2)
        {

            _countryServicev2 = countryServicev2;

        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllCountries(string? search, int page, int pageSize)
        { 
            var countries = (await _countryServicev2.Get(null, null, null, null, null, null, null, null, null, null, page, pageSize)).ToList();
            if (countries.Count == 0)
            {
                return Ok(new
                {
                    countries = countries,
                    currentPage = page,
                    totalPages = 0,
                    totalRecord = 0
                });
            }
            var totalRecord = countries[0].total_row;
            var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            return Ok(new
            {
                countries = countries,
                currentPage = page,
                totalPages,
                totalRecord
            });
        }
    }
}

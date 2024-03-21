using AviaSearcher.AviaSearchService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Staff.API.Controllers;
using WebAPI.DTO;
using WebAPI.Models;

namespace AviaSearcher.Controllers
{
   
    [ApiController]
    public class AviaSearchController : BaseController
    {
        private IAviaSearchService _aviaSearchService;

        public AviaSearchController(IAviaSearchService aviaSearchService)
        {
            _aviaSearchService = aviaSearchService;

        }

        [HttpPost]
        public async Task<QueryResult<FlightDTO>> GetFlights([FromBody] TableMetaData tableMeta)
            => await _aviaSearchService.GetFlights(tableMeta, await HttpContext.GetTokenAsync("access_token")).ConfigureAwait(false);

        [HttpPost]
        public async Task<int> CreateReservation(ReservationDTO model)
           => await _aviaSearchService.CreateReservation(model, CurUserProfile, await HttpContext.GetTokenAsync("access_token")).ConfigureAwait(false);
    }
}

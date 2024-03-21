using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;
using WebAPI.Models;

namespace AviaSearcher.AviaSearchService
{
    public interface IAviaSearchService
    {
        Task<QueryResult<FlightDTO>> GetFlights( TableMetaData tableMeta,string token);
        Task<int> CreateReservation(ReservationDTO model, UserProfile userProfile,string token);
    }
}

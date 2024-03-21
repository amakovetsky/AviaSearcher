using Nancy.Json;
using Newtonsoft.Json;
using WebAPI.Clients;
using WebAPI.DTO;
using WebAPI.Helper;
using WebAPI.Models;
using WebAPI.Redis;

namespace AviaSearcher.AviaSearchService
{
    public class AviaSearchService : IAviaSearchService
    {
        private readonly IAviaDataClient    _aviaDataClient;
        private readonly IAviaData2Client _aviaData2Client;
        private readonly IRedisService _redisCache;

        public AviaSearchService(IAviaDataClient aviaDataClient , IAviaData2Client aviaData2Client, IRedisService redisCache)
        {
            _aviaDataClient = aviaDataClient;
            _aviaData2Client = aviaData2Client;
            _redisCache = redisCache;
        }

        public async Task<QueryResult<FlightDTO>> GetFlights(TableMetaData tableMeta, string token)
        {
            IQueryable<FlightDTO> query;
            var cash_key = ParseFilter(tableMeta);
            
            var res = await  _redisCache.GetDataAsync(cash_key);
            if (res != null)
            {
                var cash = JsonConvert.DeserializeObject<QueryResult<FlightDTO>>(res);
                query = cash.Items.AsQueryable();
            }
            else
            {
                var collection1 = await _aviaDataClient.GetFlights(tableMeta, token);
                collection1.Items.ForEach(v => v.Source = WebAPI.Enums.Source.Source1);
                
                var collection2 = await _aviaData2Client.GetFlights(tableMeta, token);
                collection1.Items.ForEach(v => v.Source = WebAPI.Enums.Source.Source2);

                query = collection1.Items.Union(collection2.Items).AsQueryable();
            }
           
            // filter
            query = query.FilterQueryableWithDelegate(tableMeta, AviaSearchHelper.AviaSearchHelper.FilterExpressionList);
            // sort
            if (tableMeta.sortField != null)
                query = query.ApplyOrdering(tableMeta, tableMeta.sortField, AviaSearchHelper.AviaSearchHelper.SortExpressionList());
            else query = query.ApplyOrdering(tableMeta, tableMeta.sortField != null ? tableMeta.sortField : "date", AviaSearchHelper.AviaSearchHelper.SortExpressionList());

            QueryResult<FlightDTO> result = new QueryResult<FlightDTO>();
            result.TotalItems = query.Count();

            List<FlightDTO> datas = query
                .Skip(tableMeta.first)
                .Take(tableMeta.rows)
                .ToList();

            result.Items = datas                
                .ToList();

            if (result.Items.Count > 0)
            {

                await _redisCache.SetDataAsync(cash_key, result, new TimeSpan(24, 0, 0));
            }

            return result;
        }

                     
            


        public async Task<int> CreateReservation(ReservationDTO model, UserProfile userProfile, string token)
        {
            model.User = userProfile.UserName;
            if(model.Source == WebAPI.Enums.Source.Source1 )
                  return await _aviaDataClient.CreateReservation(model, token);
            if (model.Source == WebAPI.Enums.Source.Source2)
                return await _aviaData2Client.CreateReservation(model, token);

            return 0;
        }

        public static string ParseFilter(
                       TableMetaData utData
            )
        {
            string result = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic item_filter = serializer.Deserialize<object>(utData.filters);
            foreach (dynamic item in item_filter)
            {
                string value = "";
                string key = item.Key;
                foreach (dynamic item2 in item.Value)
                {
                    value += item2.Value;
                    break;
                }
                
                result += key +"-"+ value;
                
            }
            return result;
        }
    }
}

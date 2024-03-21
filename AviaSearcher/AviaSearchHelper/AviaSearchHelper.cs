using System.Linq.Expressions;
using WebAPI.DTO;


namespace AviaSearcher.AviaSearchHelper
{
    public class AviaSearchHelper
    {
        public static Dictionary<string, Expression<Func<FlightDTO, bool>>> FilterExpressionList(string strFilter, int intFilter)
        {
            return new Dictionary<string, Expression<Func<FlightDTO, bool>>>()
            {
                ["id"] = v => v.Id == intFilter,
                ["date"] = f => f.Date.Day == Convert.ToDateTime(strFilter).Day && f.Date.Month == Convert.ToDateTime(strFilter).Month,
                ["price"] = f => f.Price == intFilter,
                ["company"] = f => (int)f.AviaCompanyId == intFilter,
                ["transplants"] = f => f.Transplants == intFilter,
            };
        }
        public static Dictionary<string, Expression<Func<FlightDTO, object>>> SortExpressionList()
        {
            return new Dictionary<string, Expression<Func<FlightDTO, object>>>()
            {
                ["id"] = v => v.Id,
                ["date"] = v => v.Date,
                ["company"] = v => v.AviaCompanyId,
                ["price"] = v => v.Price,
            };
        }
    }
}

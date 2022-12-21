using MediatR;
using sd.Statisztika.Application.Dtos;
using sd.Statisztika.Application.ViewModels;

namespace sd.Statisztika.Application.Queries
{
    public class GetStatsQuery : IRequest<GetStatsViewModel>
    {
        public GetStatsQuery(string playerId)
        {
            PlayerId = playerId;
        }
        public  string PlayerId { get; set; }
    }
}

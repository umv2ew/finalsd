using MediatR;
using sd.Statisztika.Application.ViewModels;

namespace sd.Statisztika.Application.Queries;

public class GetStatsQuery(string playerId) : IRequest<GetStatsViewModel>
{
    public string PlayerId { get; set; } = playerId;
}

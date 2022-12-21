using MediatR;
using Microsoft.AspNetCore.Mvc;
using sd.Statisztika.Application.Commands;
using sd.Statisztika.Application.Dtos;
using sd.Statisztika.Application.Queries;
using sd.Statisztika.Application.ViewModels;

namespace sd.Statisztika.Web.Controllers
{
    [Route("[controller]")]
    public class StatisticsController : Controller
    {
        private readonly ISender _mediator;
        public StatisticsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetStatisticsById")]
        public async Task<GetStatsViewModel> GetStatisticsById(string id)
        {
            return await _mediator.Send(new GetStatsQuery(id));
        }

        [HttpPut]
        [Route("UpdateStatistics")]
        public async Task<Unit> UpdateStatistics(UpdateStatisticsDto dto)
        {
            return await _mediator.Send(new UpdateStatisticsCommand(dto));
        }
    }
}

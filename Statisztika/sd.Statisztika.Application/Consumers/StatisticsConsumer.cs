using MassTransit;
using MediatR;
using Newtonsoft.Json;
using sd.Jatek.Integration;
using sd.Statisztika.Application.Commands;
using sd.Statisztika.Application.Dtos;

namespace sd.Statisztika.Application.Consumers
{
    public class StatisticsConsumer : IConsumer<StatisticsIntegrationDto>
    {
        private readonly ISender _mediator;
        public StatisticsConsumer(ISender mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<StatisticsIntegrationDto> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            var dto = JsonConvert.DeserializeObject<UpdateStatisticsDto>(jsonMessage);
            await _mediator.Send(new UpdateStatisticsCommand(dto));
        }
    }
}

using MassTransit;
using MediatR;
using Newtonsoft.Json;
using sd.Jatek.Integration;
using sd.Statisztika.Application.Commands;
using sd.Statisztika.Application.Dtos;

namespace sd.Statisztika.Application.Consumers;

public class StatisticsConsumer(ISender mediator) : IConsumer<StatisticsIntegrationDto>
{
    private readonly ISender _mediator = mediator;

    public async Task Consume(ConsumeContext<StatisticsIntegrationDto> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        var dto = JsonConvert.DeserializeObject<UpdateStatisticsDto>(jsonMessage)
            ?? throw new Exception("Statistics serialization failed");

        await _mediator.Send(new UpdateStatisticsCommand(dto));
    }
}

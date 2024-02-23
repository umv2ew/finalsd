using MediatR;
using sd.Statisztika.Application.Dtos;

namespace sd.Statisztika.Application.Commands;

public class UpdateStatisticsCommand(UpdateStatisticsDto dto) : IRequest<Unit>
{
    public UpdateStatisticsDto Dto { get; set; } = dto;
}
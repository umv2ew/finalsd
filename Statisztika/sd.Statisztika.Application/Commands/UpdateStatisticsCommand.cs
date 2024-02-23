using MediatR;
using sd.Statisztika.Application.Dtos;

namespace sd.Statisztika.Application.Commands;

public class UpdateStatisticsCommand(UpdateStatisticsDto dto) : IRequest
{
    public UpdateStatisticsDto Dto { get; set; } = dto;
}
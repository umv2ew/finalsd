using MediatR;
using sd.Statisztika.Application.Dtos;

namespace sd.Statisztika.Application.Commands
{
    public class UpdateStatisticsCommand : IRequest
    {
        public UpdateStatisticsCommand(UpdateStatisticsDto dto)
        {
            Dto = dto;
        }
        public UpdateStatisticsDto Dto { get; set; }
    }
}
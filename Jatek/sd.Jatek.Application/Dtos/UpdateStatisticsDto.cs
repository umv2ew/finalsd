using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sd.Jatek.Application.Dtos
{
    public class UpdateStatisticsDto
    {
        public string PlayerId { get; set; }
        public bool IsWon { get; set; }
        public int Points { get; set; }
    }
}

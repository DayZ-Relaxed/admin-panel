using System.ComponentModel.DataAnnotations;

namespace DayZRelaxed.Models
{
    public class Statistics
    {
        public int StatisticsId { get; set; }
        public int StatisticsType { get; set; }
        public string Description { get; set; }
        public DateTime DateWritten { get; set; }
        public int Value { get; set; }
    }
}

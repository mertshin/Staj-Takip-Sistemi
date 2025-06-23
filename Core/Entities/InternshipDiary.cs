using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class InternshipDiary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string WorkDone { get; set; }
        public string Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int InternshipApplicationId { get; set; }
        public InternshipApplication InternshipApplication { get; set; }
    }
}

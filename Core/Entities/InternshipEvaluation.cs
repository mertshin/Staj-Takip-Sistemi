using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class InternshipEvaluation
    {
        public int Id { get; set; }
        public DateTime EvaluationDate { get; set; }
        public int WorkPerformance { get; set; } // 1-10
        public int TimeManagement { get; set; } // 1-10
        public int Teamwork { get; set; } // 1-10
        public int ProblemSolving { get; set; } // 1-10
        public int LearningAbility { get; set; } // 1-10
        public string GeneralEvaluation { get; set; }
        public bool SuccessStatus { get; set; }

        public int InternshipApplicationId { get; set; }
        public InternshipApplication InternshipApplication { get; set; }

        public int AdvisorId { get; set; }
        public Advisor Advisor { get; set; }
    }
}

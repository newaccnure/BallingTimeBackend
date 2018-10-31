using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallingTimeBackend.Models
{
    public class TrainingProgram
    {
        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
        public int DribblingDrillId { get; set; }
        public DribblingDrill DribblingDrill { get; set; }
    }
}

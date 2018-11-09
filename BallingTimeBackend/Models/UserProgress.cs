using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallingTimeBackend.Models
{
    public class UserProgress
    {
        public double AverageSpeed { get; set; }

        public double RepeationsPerSecond { get; set; }

        public double Accuracy { get; set; }

        public bool isCompleted { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int DribblingDrillId { get; set; }
        public DribblingDrill DribblingDrill { get; set; }
    }
}

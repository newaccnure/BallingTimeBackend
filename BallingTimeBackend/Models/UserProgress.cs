using System;
using System.ComponentModel.DataAnnotations;

namespace BallingTimeBackend.Models
{
    public class UserProgress
    {
        public double AverageSpeed { get; set; }

        public double RepeationsPerSecond { get; set; }

        public double Accuracy { get; set; }

        public bool IsCompleted { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int DribblingDrillId { get; set; }
        public DribblingDrill DribblingDrill { get; set; }
    }
}

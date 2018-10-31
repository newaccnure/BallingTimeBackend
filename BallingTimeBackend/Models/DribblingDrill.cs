using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BallingTimeBackend.Models
{
    public class DribblingDrill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string VideoReference { get; set; }

        public ICollection<UserProgress> UserProgresses { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
        public DribblingDrill()
        {
            UserProgresses = new List<UserProgress>();
            TrainingPrograms = new List<TrainingProgram>();
        }
    }
}

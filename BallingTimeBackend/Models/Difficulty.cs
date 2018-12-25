using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallingTimeBackend.Models
{
    public class Difficulty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SecondsForExercise { get; set; }

        // rated from 1 to 10
        public int DifficultyLevel { get; set; }

        public ICollection<TrainingProgram> TrainingPrograms { get; set; }

        public Difficulty() {
            TrainingPrograms = new List<TrainingProgram>();
        }
    }
}

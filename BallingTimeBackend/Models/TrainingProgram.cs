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

namespace BallingTimeBackend.Data_for_frontend
{
    public class Drill_Info
    {
        public bool IsCompleted { get; set; }

        public int SecondsForExercise { get; set; }

        public int DifficultyId { get; set; }

        public int DrillId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string VideoReference { get; set; }
    }
}

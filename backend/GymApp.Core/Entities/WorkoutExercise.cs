namespace GymApp.Core.Entities
{
    public class WorkoutExercise
    {
        public int WeId { get; set; }
        public int WpId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int RestSeconds { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual WorkoutPlan? WorkoutPlan { get; set; }
    }
}

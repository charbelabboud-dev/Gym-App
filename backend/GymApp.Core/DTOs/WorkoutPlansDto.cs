using System;
using System.Collections.Generic;

namespace GymApp.Core.DTOs
{
    // What the frontend sees - full workout plan with exercises
    public class WorkoutPlanDto
    {
        public int WpId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    }

    // What coach sends when creating a plan
    public class CreateWorkoutPlanDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public string CoachCode { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CreateExerciseDto> Exercises { get; set; } = new List<CreateExerciseDto>();
    }

    // What the frontend sees for each exercise
    public class ExerciseDto
    {
        public int WeId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int RestSeconds { get; set; }
    }

    // What coach sends when adding an exercise
    public class CreateExerciseDto
    {
        public string ExerciseName { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int RestSeconds { get; set; }
    }

    // For updating a plan (without exercises)
    public class UpdateWorkoutPlanDto
    {
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import workoutPlanService from "../services/WorkoutPlanService";
import PageHeader from "../Components/common/PageHeader";
import Card from "../Components/common/Card";
import Button from "../Components/common/Button";
import Loading from "../Components/common/Loading";
import EmptyState from "../Components/common/EmptyState";
import Alert from "../Components/common/Alert";
import "../Styles/global.css";

function WorkoutPlansPage() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [selectedPlan, setSelectedPlan] = useState(null);

  const clientCode = user?.clientCode;

  useEffect(() => {
    if (clientCode) {
      loadPlans();
    }
  }, );

  const loadPlans = async () => {
    try {
      setLoading(true);
      const result = await workoutPlanService.getClientPlans(clientCode);
      if (result.success) {
        setPlans(result.data);
      } else {
        setError("Failed to load workout plans");
      }
    } catch (err) {
      setError("Error connecting to server");
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  if (!clientCode) {
    return (
      <div className="page-container">
        <div className="page-content">
          <PageHeader title="Workout Plans" />
          <Alert type="error" message="Unable to find your client profile." />
          <Button onClick={() => navigate("/dashboard")} variant="primary">
            Back to Dashboard
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="page-container">
      <div className="page-content">
        <PageHeader title="💪 Workout Plans" />

        <Alert type="error" message={error} onClose={() => setError("")} />

        {loading ? (
          <Loading />
        ) : plans.length === 0 ? (
          <EmptyState
            icon="💪"
            title="No workout plans yet"
            message="Your coach will assign you a workout plan soon!"
          />
        ) : (
          <div>
            {plans.map((plan) => (
              <Card key={plan.wpId}>
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "flex-start",
                    flexWrap: "wrap",
                  }}
                >
                  <div style={{ flex: 1 }}>
                    <h3 style={{ marginBottom: "10px", color: "#2c3e50" }}>
                      {plan.planName}
                    </h3>
                    <p>
                      <strong>👨‍🏫 Coach:</strong> {plan.coachName}
                    </p>
                    <p>
                      <strong>📅 Duration:</strong> {formatDate(plan.startDate)}{" "}
                      - {formatDate(plan.endDate)}
                    </p>
                    <p>
                      <strong>📋 Exercises:</strong>{" "}
                      {plan.exercises?.length || 0}
                    </p>
                  </div>
                  <Button
                    onClick={() =>
                      setSelectedPlan(
                        selectedPlan?.wpId === plan.wpId ? null : plan,
                      )
                    }
                    variant="primary"
                  >
                    {selectedPlan?.wpId === plan.wpId
                      ? "Hide Exercises"
                      : "View Exercises"}
                  </Button>
                </div>

                {selectedPlan?.wpId === plan.wpId && (
                  <div
                    style={{
                      marginTop: "20px",
                      borderTop: "1px solid #eee",
                      paddingTop: "15px",
                    }}
                  >
                    <h4>📋 Exercises</h4>
                    <div className="table-container">
                      <table className="table">
                        <thead>
                          <tr>
                            <th>Exercise</th>
                            <th style={{ textAlign: "center" }}>Sets</th>
                            <th style={{ textAlign: "center" }}>Reps</th>
                            <th style={{ textAlign: "center" }}>Rest</th>
                          </tr>
                        </thead>
                        <tbody>
                          {plan.exercises?.map((exercise, idx) => (
                            <tr key={idx}>
                              <td>
                                <strong>{exercise.exerciseName}</strong>
                              </td>
                              <td style={{ textAlign: "center" }}>
                                {exercise.sets}
                              </td>
                              <td style={{ textAlign: "center" }}>
                                {exercise.reps}
                              </td>
                              <td style={{ textAlign: "center" }}>
                                {exercise.restSeconds}s
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  </div>
                )}
              </Card>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default WorkoutPlansPage;

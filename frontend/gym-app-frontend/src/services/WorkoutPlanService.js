import api from './api';

const workoutPlanService = {
    // Get workout plans for a client
    getClientPlans: async (clientCode) => {
        try {
            const response = await api.get(`/WorkoutPlan/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching workout plans:', error);
            throw error;
        }
    },

    // Get workout plans for a coach
    getCoachPlans: async (coachCode) => {
        try {
            const response = await api.get(`/WorkoutPlan/coach/${coachCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching coach plans:', error);
            throw error;
        }
    },

    // Create a new workout plan
    createWorkoutPlan: async (planData) => {
        try {
            const response = await api.post('/WorkoutPlan', planData);
            return response.data;
        } catch (error) {
            console.error('Error creating workout plan:', error);
            throw error;
        }
    },

    // Get a single plan by ID
    getPlanById: async (planId) => {
        try {
            const response = await api.get(`/WorkoutPlan/${planId}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching plan:', error);
            throw error;
        }
    }
};

export default workoutPlanService;
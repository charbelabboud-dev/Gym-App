import api from './api';

const goalService = {
    getClientGoals: async (clientCode) => {
        try {
            const response = await api.get(`/Goal/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching goals:', error);
            throw error;
        }
    },
    
    createGoal: async (goalData) => {
        try {
            const response = await api.post('/Goal', goalData);
            return response.data;
        } catch (error) {
            console.error('Error creating goal:', error);
            throw error;
        }
    },
    
    updateProgress: async (goalId, progressData) => {
        try {
            const response = await api.put(`/Goal/${goalId}/progress`, progressData);
            return response.data;
        } catch (error) {
            console.error('Error updating progress:', error);
            throw error;
        }
    },
    
    deleteGoal: async (goalId) => {
        try {
            const response = await api.delete(`/Goal/${goalId}`);
            return response.data;
        } catch (error) {
            console.error('Error deleting goal:', error);
            throw error;
        }
    }
};

export default goalService;
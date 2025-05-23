import api from './api';

const progressService = {
    // Get progress history for a client
    getClientProgress: async (clientCode) => {
        try {
            const response = await api.get(`/Progress/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching progress:', error);
            throw error;
        }
    },

    // Add new progress entry
    addProgress: async (progressData) => {
        try {
            const response = await api.post('/Progress', progressData);
            return response.data;
        } catch (error) {
            console.error('Error adding progress:', error);
            throw error;
        }
    },

    // Delete a progress entry
    deleteProgress: async (progressId) => {
        try {
            const response = await api.delete(`/Progress/${progressId}`);
            return response.data;
        } catch (error) {
            console.error('Error deleting progress:', error);
            throw error;
        }
    }
};

export default progressService;
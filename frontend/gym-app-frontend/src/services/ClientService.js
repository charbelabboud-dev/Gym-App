import api from './api';

const clientService = {
    // Get clients assigned to a coach
    getCoachClients: async (coachCode) => {
        try {
            const response = await api.get(`/Clients/coach/${coachCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching coach clients:', error);
            throw error;
        }
    },

    // Get a single client's details
    getClientDetails: async (clientCode) => {
        try {
            const response = await api.get(`/Clients/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching client details:', error);
            throw error;
        }
    },

    getDietitianClients: async (dietitianCode) => {
    try {
        const response = await api.get(`/Clients/dietitian/${dietitianCode}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching dietitian clients:', error);
        throw error;
    }
},
    // Get all clients (for dropdown)
    getAllClients: async () => {
        try {
            const response = await api.get('/Clients');
            return response.data;
        } catch (error) {
            console.error('Error fetching clients:', error);
            throw error;
        }
    }
};

export default clientService;
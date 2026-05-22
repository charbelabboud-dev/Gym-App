import api from './api';

const coachService = {
    // Get all coaches
    getAllCoaches: async () => {
        try {
            const response = await api.get('/Coach');
            return response.data;
        } catch (error) {
            console.error('Error fetching coaches:', error);
            throw error;
        }
    },

    // Get top rated coaches
getTopCoaches: async (limit = 10) => {
    try {
        const response = await api.get(`/Coach/top-rated?limit=${limit}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching top coaches:', error);
        throw error;
    }
},
    // Get coaches by specialty
    getCoachesBySpecialty: async (specialty) => {
        try {
            const response = await api.get(`/Coach/specialty/${encodeURIComponent(specialty)}`);
            return response.data;
        } catch (error) {
            console.error('Error filtering coaches:', error);
            throw error;
        }
    }
};

export default coachService;
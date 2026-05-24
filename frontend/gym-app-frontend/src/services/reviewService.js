import api from './api';

const reviewService = {
    // Get reviews for a coach
    getCoachReviews: async (coachCode) => {
        try {
            const response = await api.get(`/Review/coach/${coachCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching coach reviews:', error);
            throw error;
        }
    },

    // Get coach rating summary
    getCoachRating: async (coachCode) => {
        try {
            const response = await api.get(`/Review/coach/${coachCode}/rating`);
            return response.data;
        } catch (error) {
            console.error('Error fetching coach rating:', error);
            throw error;
        }
    },

    // Get reviews by a client
    getClientReviews: async (clientCode) => {
        try {
            const response = await api.get(`/Review/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching client reviews:', error);
            throw error;
        }
    },

    // Add a review
    addReview: async (reviewData) => {
        try {
            const response = await api.post('/Review', reviewData);
            return response.data;
        } catch (error) {
            console.error('Error adding review:', error);
            throw error;
        }
    }
};

export default reviewService;
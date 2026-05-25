import api from './api';

const dietPlanService = {
    // Get diet plans for a client
    getClientPlans: async (clientCode) => {
        try {
            const response = await api.get(`/DietPlan/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching diet plans:', error);
            throw error;
        }
    },

    // Get diet plans for a dietitian
    getDietitianPlans: async (dietitianCode) => {
        try {
            const response = await api.get(`/DietPlan/dietitian/${dietitianCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching dietitian plans:', error);
            throw error;
        }
    },

    createDietPlan: async (planData) => {
    try {
        const response = await api.post('/DietPlan', planData);
        return response.data;
    } catch (error) {
        console.error('Error creating diet plan:', error);
        throw error;
    }
},
    // Get a single plan by ID
    getPlanById: async (planId) => {
        try {
            const response = await api.get(`/DietPlan/${planId}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching plan:', error);
            throw error;
        }
    }
};

export default dietPlanService;
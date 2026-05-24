import api from './api';

const sessionService = {
    // Book a new session
    bookSession: async (sessionData) => {
        try {
            const response = await api.post('/Session/book', sessionData);
            return response.data;
        } catch (error) {
            console.error('Error booking session:', error);
            throw error;
        }
    },

    // Get sessions for a client
    getClientSessions: async (clientCode) => {
        try {
            const response = await api.get(`/Session/client/${clientCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching client sessions:', error);
            throw error;
        }
    },

    // Get sessions for a coach
    getCoachSessions: async (coachCode) => {
        try {
            const response = await api.get(`/Session/coach/${coachCode}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching coach sessions:', error);
            throw error;
        }
    },

    // Reschedule a session
    rescheduleSession: async (sessionId, newDateTime, duration) => {
        try {
            const response = await api.put(`/Session/${sessionId}/reschedule`, {
                date: newDateTime,
                duration: duration
            });
            return response.data;
        } catch (error) {
            console.error('Error rescheduling session:', error);
            throw error;
        }
    },
// Add this to sessionService
updateSessionStatus: async (sessionId, status) => {
    try {
        const response = await api.put(`/Session/${sessionId}/status`, { status });
        return response.data;
    } catch (error) {
        console.error('Error updating session status:', error);
        throw error;
    }
},
// Update session status (for coaches)
updateSessionStatus: async (sessionId, status) => {
    try {
        const response = await api.put(`/Session/${sessionId}/status`, { status });
        return response.data;
    } catch (error) {
        console.error('Error updating session status:', error);
        throw error;
    }
},
    // Cancel a session
    cancelSession: async (sessionId) => {
        try {
            const response = await api.delete(`/Session/${sessionId}/cancel`);
            return response.data;
        } catch (error) {
            console.error('Error cancelling session:', error);
            throw error;
        }
    }
};

export default sessionService;
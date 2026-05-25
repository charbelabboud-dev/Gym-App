import api from './api';

const notificationService = {
    // Get notifications for user
    getUserNotifications: async (userId) => {
        try {
            const response = await api.get(`/Notification/user/${userId}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching notifications:', error);
            throw error;
        }
    },

    // Get unread count
    getUnreadCount: async (userId) => {
        try {
            const response = await api.get(`/Notification/user/${userId}/unread-count`);
            return response.data;
        } catch (error) {
            console.error('Error fetching unread count:', error);
            throw error;
        }
    },

    // Mark notification as read
    markAsRead: async (notificationId) => {
        try {
            const response = await api.put(`/Notification/${notificationId}/read`);
            return response.data;
        } catch (error) {
            console.error('Error marking as read:', error);
            throw error;
        }
    },

    // Mark all as read
    markAllAsRead: async (userId) => {
        try {
            const response = await api.put(`/Notification/user/${userId}/read-all`);
            return response.data;
        } catch (error) {
            console.error('Error marking all as read:', error);
            throw error;
        }
    }
};

export default notificationService;
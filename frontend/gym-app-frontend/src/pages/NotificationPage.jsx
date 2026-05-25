import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import notificationService from '../services/notificationService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function NotificationsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [notifications, setNotifications] = useState([]);
    const [filteredNotifications, setFilteredNotifications] = useState([]);
    const [filter, setFilter] = useState('all'); // 'all', 'unread', 'read'
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const userId = user?.userId;

    useEffect(() => {
        if (userId) {
            loadNotifications();
        }
    }, [userId]);

    useEffect(() => {
        filterNotifications();
    }, [filter, notifications]);

    const loadNotifications = async () => {
        try {
            setLoading(true);
            const result = await notificationService.getUserNotifications(userId);
            if (result.success) {
                setNotifications(result.data);
                setFilteredNotifications(result.data);
            } else {
                setError('Failed to load notifications');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const filterNotifications = () => {
        if (filter === 'all') {
            setFilteredNotifications(notifications);
        } else if (filter === 'unread') {
            setFilteredNotifications(notifications.filter(n => !n.isRead));
        } else if (filter === 'read') {
            setFilteredNotifications(notifications.filter(n => n.isRead));
        }
    };

    const handleMarkAsRead = async (notificationId) => {
        try {
            const result = await notificationService.markAsRead(notificationId);
            if (result.success) {
                loadNotifications();
                setSuccess('Notification marked as read');
                setTimeout(() => setSuccess(''), 2000);
            }
        } catch (err) {
            setError('Error marking as read');
        }
    };

    const handleMarkAllAsRead = async () => {
        try {
            const result = await notificationService.markAllAsRead(userId);
            if (result.success) {
                loadNotifications();
                setSuccess('All notifications marked as read');
                setTimeout(() => setSuccess(''), 2000);
            }
        } catch (err) {
            setError('Error marking all as read');
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const now = new Date();
        const diffMs = now - date;
        const diffMins = Math.floor(diffMs / 60000);
        const diffHours = Math.floor(diffMs / 3600000);
        const diffDays = Math.floor(diffMs / 86400000);

        if (diffMins < 1) return 'Just now';
        if (diffMins < 60) return `${diffMins} minutes ago`;
        if (diffHours < 24) return `${diffHours} hours ago`;
        return `${diffDays} days ago`;
    };

    const getTypeIcon = (type) => {
        switch(type) {
            case 'Session': return '📅';
            case 'Progress': return '📊';
            case 'Workout': return '💪';
            case 'Diet': return '🥗';
            default: return '🔔';
        }
    };

    if (!userId) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Notifications" />
                    <Alert type="error" message="Unable to find your profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    const unreadCount = notifications.filter(n => !n.isRead).length;
    const readCount = notifications.filter(n => n.isRead).length;

    return (
        <div className="page-container">
            <div className="page-content">
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px', flexWrap: 'wrap', gap: '15px' }}>
                    <PageHeader title="🔔 Notifications" showBack={true} />
                    <div style={{ display: 'flex', gap: '10px' }}>
                        <Button 
                            onClick={() => setFilter('all')} 
                            variant={filter === 'all' ? 'primary' : 'secondary'}
                            style={{ padding: '8px 16px' }}
                        >
                            All ({notifications.length})
                        </Button>
                        <Button 
                            onClick={() => setFilter('unread')} 
                            variant={filter === 'unread' ? 'primary' : 'secondary'}
                            style={{ padding: '8px 16px' }}
                        >
                            Unread ({unreadCount})
                        </Button>
                        <Button 
                            onClick={() => setFilter('read')} 
                            variant={filter === 'read' ? 'primary' : 'secondary'}
                            style={{ padding: '8px 16px' }}
                        >
                            Read ({readCount})
                        </Button>
                        {unreadCount > 0 && (
                            <Button onClick={handleMarkAllAsRead} variant="success">
                                Mark All Read
                            </Button>
                        )}
                    </div>
                </div>
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {loading ? (
                    <Loading />
                ) : filteredNotifications.length === 0 ? (
                    <EmptyState 
                        icon="🔔"
                        title={
                            filter === 'unread' ? 'No unread notifications' :
                            filter === 'read' ? 'No read notifications' :
                            'No notifications'
                        }
                        message={
                            filter === 'unread' ? 'You\'ve read everything!' :
                            filter === 'read' ? 'Mark some notifications as read to see them here' :
                            'You\'re all caught up! Check back later for updates.'
                        }
                        action={
                            filter !== 'all' && (
                                <Button onClick={() => setFilter('all')} variant="primary">
                                    View All
                                </Button>
                            )
                        }
                    />
                ) : (
                    <div>
                        <p style={{ marginBottom: '15px', color: '#7f8c8d' }}>
                            {filter === 'unread' ? `${filteredNotifications.length} unread notification(s)` :
                             filter === 'read' ? `${filteredNotifications.length} read notification(s)` :
                             `${notifications.length} total notification(s)`}
                        </p>
                        {filteredNotifications.map((notification) => (
                            <Card key={notification.notId}>
                                <div style={{ 
                                    display: 'flex', 
                                    justifyContent: 'space-between', 
                                    alignItems: 'flex-start',
                                    opacity: notification.isRead ? 0.7 : 1
                                }}>
                                    <div style={{ display: 'flex', gap: '15px', flex: 1 }}>
                                        <div style={{ fontSize: '24px' }}>
                                            {getTypeIcon(notification.type)}
                                        </div>
                                        <div style={{ flex: 1 }}>
                                            <h4 style={{ margin: '0 0 5px 0', color: '#2c3e50' }}>
                                                {notification.title}
                                                {!notification.isRead && (
                                                    <span style={{
                                                        display: 'inline-block',
                                                        marginLeft: '8px',
                                                        width: '8px',
                                                        height: '8px',
                                                        backgroundColor: '#dc3545',
                                                        borderRadius: '50%',
                                                        verticalAlign: 'middle'
                                                    }}></span>
                                                )}
                                            </h4>
                                            <p style={{ margin: '5px 0', color: '#555' }}>{notification.message}</p>
                                            <p style={{ margin: '5px 0', fontSize: '12px', color: '#999' }}>
                                                {formatDate(notification.createdDate)}
                                            </p>
                                        </div>
                                    </div>
                                    {!notification.isRead && (
                                        <Button 
                                            onClick={() => handleMarkAsRead(notification.notId)}
                                            variant="primary"
                                            style={{ padding: '4px 12px', fontSize: '12px' }}
                                        >
                                            Mark Read
                                        </Button>
                                    )}
                                </div>
                            </Card>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default NotificationsPage;
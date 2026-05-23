import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import sessionService from '../services/SessionService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function MySessionsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [sessions, setSessions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const clientCode = user?.clientCode;

    useEffect(() => {
        if (clientCode) {
            loadSessions();
        }
    }, [clientCode]);

    const loadSessions = async () => {
        try {
            setLoading(true);
            const result = await sessionService.getClientSessions(clientCode);
            if (result.success) {
                setSessions(result.data);
            } else {
                setError('Failed to load sessions');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const handleCancel = async (sessionId) => {
        if (!window.confirm('Are you sure you want to cancel this session?')) return;
        
        try {
            const result = await sessionService.cancelSession(sessionId);
            if (result.success) {
                setSuccess('Session cancelled successfully');
                loadSessions();
                setTimeout(() => setSuccess(''), 3000);
            } else {
                setError(result.message || 'Failed to cancel session');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error cancelling session');
        }
    };

    const handleReschedule = (session) => {
        navigate('/reschedule-session', { state: { session } });
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            weekday: 'short',
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    const getStatusColor = (status) => {
        switch(status) {
            case 'Scheduled': return '#28a745';
            case 'Completed': return '#17a2b8';
            case 'Cancelled': return '#dc3545';
            default: return '#6c757d';
        }
    };

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Sessions" />
                    <Alert type="error" message="Unable to find your client profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="📋 My Sessions" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {loading ? (
                    <Loading />
                ) : sessions.length === 0 ? (
                    <EmptyState 
                        icon="📅"
                        title="No sessions booked"
                        message="Book your first session with a coach to get started!"
                        action={
                            <Button onClick={() => navigate('/coaches')} variant="primary">
                                Browse Coaches
                            </Button>
                        }
                    />
                ) : (
                    <div>
                        <p style={{ marginBottom: '20px', color: '#7f8c8d' }}>
                            You have {sessions.length} session(s)
                        </p>
                        {sessions.map((session) => (
                            <Card key={session.sesId}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div style={{ flex: 1 }}>
                                        <h3 style={{ marginBottom: '10px', color: '#2c3e50' }}>{session.coachName}</h3>
                                        <p><strong>📅 Date:</strong> {formatDate(session.date)}</p>
                                        <p><strong>⏱️ Duration:</strong> {session.duration} minutes</p>
                                        <p><strong>🏷️ Type:</strong> {session.sessionType}</p>
                                        {session.description && (
                                            <p><strong>📝 Notes:</strong> {session.description}</p>
                                        )}
                                    </div>
                                    <div style={{ textAlign: 'right' }}>
                                        <span style={{
                                            display: 'inline-block',
                                            padding: '4px 12px',
                                            borderRadius: '20px',
                                            backgroundColor: getStatusColor(session.status),
                                            color: 'white',
                                            fontSize: '12px',
                                            fontWeight: 'bold',
                                            marginBottom: '10px'
                                        }}>
                                            {session.status}
                                        </span>
                                        {session.status === 'Scheduled' && (
                                            <div>
                                                <Button 
                                                    onClick={() => handleReschedule(session)} 
                                                    variant="warning"
                                                    style={{ marginRight: '8px' }}
                                                >
                                                    Reschedule
                                                </Button>
                                                <Button 
                                                    onClick={() => handleCancel(session.sesId)} 
                                                    variant="danger"
                                                >
                                                    Cancel
                                                </Button>
                                            </div>
                                        )}
                                    </div>
                                </div>
                            </Card>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default MySessionsPage;
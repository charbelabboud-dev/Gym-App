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

function CoachSessionsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [sessions, setSessions] = useState([]);
    const [filter, setFilter] = useState('upcoming'); // upcoming, past
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const coachCode = user?.coachCode;

    useEffect(() => {
        if (coachCode) {
            loadSessions();
        }
    }, [coachCode]);

    const loadSessions = async () => {
        try {
            setLoading(true);
            const result = await sessionService.getCoachSessions(coachCode);
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

    const handleMarkCompleted = async (sessionId) => {
        if (!window.confirm('Mark this session as completed?')) return;
        
        try {
            // You'll need to add this endpoint in backend
            const result = await sessionService.updateSessionStatus(sessionId, 'Completed');
            if (result.success) {
                setSuccess('Session marked as completed');
                loadSessions();
                setTimeout(() => setSuccess(''), 3000);
            } else {
                setError(result.message || 'Failed to update session');
            }
        } catch (err) {
            setError('Error updating session');
        }
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

    const getFilteredSessions = () => {
        const now = new Date();
        if (filter === 'upcoming') {
            return sessions.filter(s => 
                s.status === 'Scheduled' && new Date(s.date) > now
            );
        } else {
            return sessions.filter(s => 
                s.status === 'Completed' || (s.status === 'Scheduled' && new Date(s.date) < now)
            );
        }
    };

    if (!coachCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Sessions" />
                    <Alert type="error" message="Unable to find your coach profile. Please contact support." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    const filteredSessions = getFilteredSessions();
    const upcomingCount = sessions.filter(s => s.status === 'Scheduled' && new Date(s.date) > new Date()).length;

    return (
        <div className="page-container">
            <div className="page-content">
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px', flexWrap: 'wrap', gap: '15px' }}>
                    <PageHeader title="📅 My Sessions" showBack={true} />
                    <div style={{ display: 'flex', gap: '10px' }}>
                        <Button 
                            onClick={() => setFilter('upcoming')} 
                            variant={filter === 'upcoming' ? 'primary' : 'secondary'}
                        >
                            Upcoming ({upcomingCount})
                        </Button>
                        <Button 
                            onClick={() => setFilter('past')} 
                            variant={filter === 'past' ? 'primary' : 'secondary'}
                        >
                            Past & Completed
                        </Button>
                    </div>
                </div>
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {loading ? (
                    <Loading />
                ) : filteredSessions.length === 0 ? (
                    <EmptyState 
                        icon="📅"
                        title={filter === 'upcoming' ? 'No upcoming sessions' : 'No past sessions'}
                        message={filter === 'upcoming' ? 'You have no upcoming sessions scheduled.' : 'No completed sessions yet.'}
                    />
                ) : (
                    <div>
                        {filteredSessions.map((session) => (
                            <Card key={session.sesId}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div style={{ flex: 1 }}>
                                        <h3 style={{ marginBottom: '10px', color: '#2c3e50' }}>{session.clientName}</h3>
                                        <p><strong>📅 Date:</strong> {formatDate(session.date)}</p>
                                        <p><strong>⏱️ Duration:</strong> {session.duration} minutes</p>
                                        <p><strong>🏷️ Type:</strong> {session.sessionType}</p>
                                        {session.description && (
                                            <p><strong>📝 Notes:</strong> {session.description}</p>
                                        )}
                                        <p><strong>📌 Status:</strong> 
                                            <span style={{
                                                display: 'inline-block',
                                                marginLeft: '8px',
                                                padding: '2px 8px',
                                                borderRadius: '12px',
                                                fontSize: '12px',
                                                backgroundColor: session.status === 'Scheduled' ? '#ffc107' : '#28a745',
                                                color: session.status === 'Scheduled' ? '#333' : 'white'
                                            }}>
                                                {session.status}
                                            </span>
                                        </p>
                                    </div>
                                    {session.status === 'Scheduled' && new Date(session.date) < new Date() && (
                                        <Button 
                                            onClick={() => handleMarkCompleted(session.sesId)}
                                            variant="success"
                                        >
                                            Mark Completed
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

export default CoachSessionsPage;
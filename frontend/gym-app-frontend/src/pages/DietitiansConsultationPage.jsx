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

function DietitianConsultationsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [sessions, setSessions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const dietitianCode = user?.dietitianCode;

    useEffect(() => {
        if (dietitianCode) {
            loadConsultations();
        }
    }, [dietitianCode]);

    const loadConsultations = async () => {
        try {
            setLoading(true);
            // You'll need to add endpoint to get dietitian sessions
            const result = await sessionService.getDietitianSessions(dietitianCode);
            if (result.success) {
                setSessions(result.data);
            } else {
                setError('Failed to load consultations');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    if (!dietitianCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Consultations" />
                    <Alert type="error" message="Unable to find your dietitian profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="📅 Consultation Requests" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {loading ? (
                    <Loading />
                ) : sessions.length === 0 ? (
                    <EmptyState 
                        icon="📅"
                        title="No consultations"
                        message="You have no consultation requests yet."
                    />
                ) : (
                    <div>
                        {sessions.map((session) => (
                            <Card key={session.sesId}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div>
                                        <h3>{session.clientName}</h3>
                                        <p><strong>📅 Date:</strong> {formatDate(session.date)}</p>
                                        <p><strong>⏱️ Duration:</strong> {session.duration} minutes</p>
                                        {session.description && (
                                            <p><strong>📝 Notes:</strong> {session.description}</p>
                                        )}
                                        <p><strong>Status:</strong> {session.status}</p>
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

export default DietitianConsultationsPage;
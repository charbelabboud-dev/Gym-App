import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import sessionService from '../services/SessionService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function RescheduleSessionPage() {
    const navigate = useNavigate();
    const location = useLocation();
    const { user } = useAuth();
    
    const session = location.state?.session;
    
    const [formData, setFormData] = useState({
        date: '',
        time: '10:00',
        duration: session?.duration || 60
    });
    
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    if (!session) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Reschedule Session" />
                    <Alert type="error" message="No session selected." />
                    <Button onClick={() => navigate('/my-sessions')} variant="primary">
                        Back to Sessions
                    </Button>
                </div>
            </div>
        );
    }

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setSuccess('');

        const dateTime = `${formData.date}T${formData.time}:00`;

        try {
            const result = await sessionService.rescheduleSession(
                session.sesId,
                dateTime,
                parseInt(formData.duration)
            );
            
            if (result.success) {
                setSuccess('Session rescheduled successfully! Redirecting...');
                setTimeout(() => {
                    navigate('/my-sessions');
                }, 2000);
            } else {
                setError(result.message || 'Failed to reschedule session');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error rescheduling session');
        } finally {
            setLoading(false);
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

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="Reschedule Session" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                <Card>
                    <div style={{ 
                        background: '#f8f9fa', 
                        padding: '15px', 
                        borderRadius: '12px', 
                        marginBottom: '20px' 
                    }}>
                        <h3 style={{ marginBottom: '10px', color: '#2c3e50' }}>Current Session</h3>
                        <p><strong>Coach:</strong> {session.coachName}</p>
                        <p><strong>Date & Time:</strong> {formatDate(session.date)}</p>
                        <p><strong>Duration:</strong> {session.duration} minutes</p>
                    </div>

                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label>New Date</label>
                            <input
                                type="date"
                                name="date"
                                value={formData.date}
                                onChange={handleChange}
                                required
                                min={new Date().toISOString().split('T')[0]}
                                className="form-control"
                            />
                        </div>

                        <div className="form-group">
                            <label>New Time</label>
                            <select
                                name="time"
                                value={formData.time}
                                onChange={handleChange}
                                className="form-control"
                            >
                                <option value="09:00">09:00 AM</option>
                                <option value="10:00">10:00 AM</option>
                                <option value="11:00">11:00 AM</option>
                                <option value="12:00">12:00 PM</option>
                                <option value="13:00">01:00 PM</option>
                                <option value="14:00">02:00 PM</option>
                                <option value="15:00">03:00 PM</option>
                                <option value="16:00">04:00 PM</option>
                                <option value="17:00">05:00 PM</option>
                                <option value="18:00">06:00 PM</option>
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Duration</label>
                            <select
                                name="duration"
                                value={formData.duration}
                                onChange={handleChange}
                                className="form-control"
                            >
                                <option value="30">30 minutes</option>
                                <option value="45">45 minutes</option>
                                <option value="60">60 minutes</option>
                                <option value="90">90 minutes</option>
                            </select>
                        </div>

                        <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
                            <Button 
                                type="submit" 
                                variant="primary" 
                                disabled={loading}
                            >
                                {loading ? 'Rescheduling...' : 'Confirm Reschedule'}
                            </Button>
                            <Button 
                                type="button" 
                                variant="secondary" 
                                onClick={() => navigate('/my-sessions')}
                            >
                                Cancel
                            </Button>
                        </div>
                    </form>
                </Card>
            </div>
        </div>
    );
}

export default RescheduleSessionPage;
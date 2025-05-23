import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import sessionService from '../services/SessionService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function BookSessionPage() {
    const navigate = useNavigate();
    const location = useLocation();
    const { user } = useAuth();
    
    const selectedCoach = location.state?.coach || null;
    
    const [formData, setFormData] = useState({
        coachCode: selectedCoach?.coCode || '',
        coachName: selectedCoach?.fullName || '',
        date: '',
        time: '10:00',
        duration: 60,
        sessionType: 'Training',
        description: ''
    });
    
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const clientCode = user?.clientCode;

    // Check if user has client code
    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Book a Session" />
                    <Alert type="error" message="Unable to find your client profile. Please contact support." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    // Check if coach is selected
    if (!selectedCoach) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Book a Session" />
                    <Alert type="error" message="No coach selected. Please choose a coach first." />
                    <Button onClick={() => navigate('/coaches')} variant="primary">Browse Coaches</Button>
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

        const sessionData = {
            clientCode: clientCode,
            coachCode: formData.coachCode,
            sessionType: formData.sessionType,
            description: formData.description,
            date: dateTime,
            duration: parseInt(formData.duration)
        };

        try {
            const result = await sessionService.bookSession(sessionData);
            if (result.success) {
                setSuccess('Session booked successfully! Redirecting...');
                setTimeout(() => {
                    navigate('/my-sessions');
                }, 2000);
            } else {
                setError(result.message || 'Failed to book session');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error booking session');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="Book a Session" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                <Card>
                    <form onSubmit={handleSubmit}>
                        {/* Coach Info Card */}
                        <div style={{ 
                            background: '#f8f9fa', 
                            padding: '15px', 
                            borderRadius: '12px', 
                            marginBottom: '20px' 
                        }}>
                            <h3 style={{ marginBottom: '10px', color: '#2c3e50' }}>Selected Coach</h3>
                            <p><strong>Name:</strong> {selectedCoach.fullName}</p>
                            <p><strong>Specialty:</strong> {selectedCoach.specialty}</p>
                            <p><strong>Phone:</strong> {selectedCoach.phone}</p>
                            <p><strong>Email:</strong> {selectedCoach.email}</p>
                        </div>

                        <div className="form-group">
                            <label>Session Type</label>
                            <select
                                name="sessionType"
                                value={formData.sessionType}
                                onChange={handleChange}
                                className="form-control"
                            >
                                <option value="Training">🏋️ Training</option>
                                <option value="Consultation">📋 Consultation</option>
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Date</label>
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
                            <label>Time</label>
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

                        <div className="form-group">
                            <label>Notes (optional)</label>
                            <textarea
                                name="description"
                                value={formData.description}
                                onChange={handleChange}
                                rows="3"
                                placeholder="Any specific goals or questions for the coach?"
                                className="form-control"
                            />
                        </div>

                        <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
                            <Button 
                                type="submit" 
                                variant="success" 
                                disabled={loading}
                            >
                                {loading ? 'Booking...' : 'Confirm Booking'}
                            </Button>
                            <Button 
                                type="button" 
                                variant="secondary" 
                                onClick={() => navigate('/coaches')}
                            >
                                Choose Different Coach
                            </Button>
                        </div>
                    </form>
                </Card>
            </div>
        </div>
    );
}

export default BookSessionPage;
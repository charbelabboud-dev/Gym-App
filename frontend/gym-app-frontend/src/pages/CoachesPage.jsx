import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import coachService from '../services/coachService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function CoachesPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [coaches, setCoaches] = useState([]);
    const [filteredCoaches, setFilteredCoaches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [specialty, setSpecialty] = useState('');
    const [specialties] = useState(['All', 'Strength Training', 'Yoga', 'Cardio', 'Nutrition']);

    useEffect(() => {
        loadCoaches();
    }, []);

    const loadCoaches = async () => {
        try {
            setLoading(true);
            const result = await coachService.getAllCoaches();
            if (result.success) {
                setCoaches(result.data);
                setFilteredCoaches(result.data);
            } else {
                setError('Failed to load coaches');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const handleFilter = async () => {
        if (specialty === 'All' || !specialty) {
            setFilteredCoaches(coaches);
        } else {
            try {
                setLoading(true);
                const result = await coachService.getCoachesBySpecialty(specialty);
                if (result.success) {
                    setFilteredCoaches(result.data);
                }
            } catch (err) {
                setError('Error filtering coaches');
            } finally {
                setLoading(false);
            }
        }
    };

    const handleBookSession = (coach) => {
        navigate('/book-session', { state: { coach } });
    };

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="👨‍🏫 Our Coaches" />

                <Alert type="error" message={error} onClose={() => setError('')} />

                <div className="filter-bar">
                    <div className="filter-group">
                        <label>Specialty</label>
                        <select value={specialty} onChange={(e) => setSpecialty(e.target.value)}>
                            {specialties.map(s => <option key={s} value={s}>{s}</option>)}
                        </select>
                    </div>
                    <Button onClick={handleFilter} variant="primary">Filter</Button>
                    <Button onClick={() => { setSpecialty('All'); loadCoaches(); }} variant="secondary">Reset</Button>
                </div>

                {loading ? (
                    <Loading />
                ) : filteredCoaches.length === 0 ? (
                    <EmptyState 
                        icon="👨‍🏫"
                        title="No coaches found"
                        message="Try a different specialty or check back later"
                    />
                ) : (
                    <div>
                        <p style={{ marginBottom: '20px', color: '#7f8c8d' }}>
                            Found {filteredCoaches.length} coach(es)
                        </p>
                        {filteredCoaches.map((coach, index) => (
                            <Card key={index}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div>
                                        <h3 style={{ marginBottom: '10px', color: '#2c3e50' }}>{coach.fullName}</h3>
                                        <span style={{
                                            display: 'inline-block',
                                            background: '#e7f3ff',
                                            color: '#0066cc',
                                            padding: '4px 12px',
                                            borderRadius: '20px',
                                            fontSize: '12px',
                                            marginBottom: '12px'
                                        }}>
                                            {coach.specialty}
                                        </span>
                                        <p><strong>📞</strong> {coach.phone}</p>
                                        <p><strong>✉️</strong> {coach.email}</p>
                                        <p><strong>📍</strong> {coach.address}</p>
                                    </div>
                                    <Button onClick={() => handleBookSession(coach)} variant="success">
                                        Book Session
                                    </Button>
                                </div>
                            </Card>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default CoachesPage;
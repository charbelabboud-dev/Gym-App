import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import coachService from '../services/coachService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function CoachRankingsPage() {
    const navigate = useNavigate();
    
    const [coaches, setCoaches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [filter, setFilter] = useState('rating'); // rating, popular

    useEffect(() => {
        loadTopCoaches();
    }, []);

    const loadTopCoaches = async () => {
        try {
            setLoading(true);
            const result = await coachService.getTopCoaches();
            if (result.success) {
                setCoaches(result.data);
            } else {
                setError('Failed to load rankings');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const renderStars = (rating) => {
        const fullStars = Math.floor(rating);
        const hasHalfStar = rating % 1 >= 0.5;
        const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);
        
        return (
            <>
                {'⭐'.repeat(fullStars)}
                {hasHalfStar && '½'}
                {'☆'.repeat(emptyStars)}
                <span style={{ marginLeft: '8px', color: '#666', fontSize: '14px' }}>
                    ({rating.toFixed(1)})
                </span>
            </>
        );
    };

    if (loading) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Coach Rankings" />
                    <Loading />
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="🏆 Top Rated Coaches" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {coaches.length === 0 ? (
                    <div className="empty-state">
                        <div className="empty-state-icon">🏆</div>
                        <h3>No coaches yet</h3>
                        <p>Check back later for top-rated coaches!</p>
                    </div>
                ) : (
                    <div>
                        <p style={{ marginBottom: '20px', color: '#7f8c8d' }}>
                            Showing top {coaches.length} coaches
                        </p>
                        {coaches.map((coach, index) => (
                            <Card key={index}>
                                <div style={{ display: 'flex', alignItems: 'center', gap: '20px', flexWrap: 'wrap' }}>
                                    <div style={{ 
                                        fontSize: '32px', 
                                        fontWeight: 'bold', 
                                        color: index === 0 ? '#FFD700' : index === 1 ? '#C0C0C0' : index === 2 ? '#CD7F32' : '#bdc3c7',
                                        minWidth: '60px',
                                        textAlign: 'center'
                                    }}>
                                        #{index + 1}
                                    </div>
                                    <div style={{ flex: 1 }}>
                                        <h3 style={{ marginBottom: '5px' }}>{coach.fullName}</h3>
                                        <div style={{ marginBottom: '5px' }}>
                                            <span style={{
                                                display: 'inline-block',
                                                background: '#e7f3ff',
                                                color: '#0066cc',
                                                padding: '2px 8px',
                                                borderRadius: '12px',
                                                fontSize: '12px'
                                            }}>
                                                {coach.specialty}
                                            </span>
                                        </div>
                                        <div style={{ marginBottom: '5px' }}>
                                            {renderStars(coach.rating)}
                                        </div>
                                        <p style={{ margin: '5px 0', fontSize: '14px', color: '#666' }}>
                                            📞 {coach.phone} | ✉️ {coach.email}
                                        </p>
                                    </div>
                                    <Button 
                                        onClick={() => navigate('/book-session', { state: { coach } })}
                                        variant="success"
                                    >
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

export default CoachRankingsPage;
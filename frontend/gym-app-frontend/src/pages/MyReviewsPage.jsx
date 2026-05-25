import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import reviewService from '../services/reviewService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function MyReviewsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [reviews, setReviews] = useState([]);
    const [rating, setRating] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const coachCode = user?.coachCode;

    useEffect(() => {
        if (coachCode) {
            loadReviews();
            loadRating();
        }
    }, [coachCode]);

    const loadReviews = async () => {
        try {
            setLoading(true);
            const result = await reviewService.getCoachReviews(coachCode);
            if (result.success) {
                setReviews(result.data);
            } else {
                setError('Failed to load reviews');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const loadRating = async () => {
        try {
            const result = await reviewService.getCoachRating(coachCode);
            if (result.success) {
                setRating(result.data);
            }
        } catch (err) {
            console.error('Error loading rating:', err);
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        });
    };

    const renderStars = (ratingValue) => {
        return '⭐'.repeat(ratingValue) + '☆'.repeat(5 - ratingValue);
    };

    if (!coachCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Reviews" />
                    <Alert type="error" message="Unable to find your coach profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="⭐ My Reviews" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {/* Rating Summary */}
                {rating && rating.totalReviews > 0 && (
                    <Card>
                        <div style={{ textAlign: 'center' }}>
                            <h2 style={{ fontSize: '48px', color: '#ffc107', marginBottom: '10px' }}>
                                {rating.averageRating.toFixed(1)}
                            </h2>
                            <div style={{ fontSize: '24px', marginBottom: '5px' }}>
                                {renderStars(Math.round(rating.averageRating))}
                            </div>
                            <p style={{ color: '#7f8c8d' }}>
                                Based on {rating.totalReviews} review(s)
                            </p>
                            <div style={{ display: 'flex', justifyContent: 'center', gap: '20px', marginTop: '15px' }}>
                                <div>
                                    <strong>5 ★</strong>: {rating.rating5Count}
                                </div>
                                <div>
                                    <strong>4 ★</strong>: {rating.rating4Count}
                                </div>
                                <div>
                                    <strong>3 ★</strong>: {rating.rating3Count}
                                </div>
                                <div>
                                    <strong>2 ★</strong>: {rating.rating2Count}
                                </div>
                                <div>
                                    <strong>1 ★</strong>: {rating.rating1Count}
                                </div>
                            </div>
                        </div>
                    </Card>
                )}

                {loading ? (
                    <Loading />
                ) : reviews.length === 0 ? (
                    <EmptyState 
                        icon="⭐"
                        title="No reviews yet"
                        message="You haven't received any reviews from clients yet."
                    />
                ) : (
                    <div>
                        <p style={{ marginBottom: '20px', color: '#7f8c8d' }}>
                            {reviews.length} review(s) from your clients
                        </p>
                        {reviews.map((review) => (
                            <Card key={review.revId}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div style={{ flex: 1 }}>
                                        <div style={{ display: 'flex', alignItems: 'center', gap: '10px', marginBottom: '10px' }}>
                                            <h3 style={{ margin: 0 }}>{review.clientName}</h3>
                                            <div style={{ fontSize: '18px' }}>
                                                {renderStars(review.rating)}
                                            </div>
                                        </div>
                                        {review.comment && (
                                            <p style={{ margin: '10px 0', fontStyle: 'italic', color: '#555' }}>
                                                "{review.comment}"
                                            </p>
                                        )}
                                        <p style={{ margin: '5px 0', fontSize: '12px', color: '#999' }}>
                                            {formatDate(review.createdDate)}
                                        </p>
                                        {review.sessionId && (
                                            <p style={{ margin: '5px 0', fontSize: '12px', color: '#007bff' }}>
                                                Session #{review.sessionId}
                                            </p>
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

export default MyReviewsPage;
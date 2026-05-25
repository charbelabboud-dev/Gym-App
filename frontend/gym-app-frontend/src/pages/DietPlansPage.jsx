import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import dietPlanService from '../services/DietPlanService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function DietPlansPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [plans, setPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const clientCode = user?.clientCode;

    useEffect(() => {
        if (clientCode) {
            loadPlans();
        }
    }, [clientCode]);

    const loadPlans = async () => {
        try {
            setLoading(true);
            const result = await dietPlanService.getClientPlans(clientCode);
            if (result.success) {
                setPlans(result.data);
            } else {
                setError('Failed to load diet plans');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const formatDate = (dateString) => {
        if (!dateString) return 'N/A';
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        });
    };

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Diet Plans" />
                    <Alert type="error" message="Unable to find your client profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="🥗 Diet Plans" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {loading ? (
                    <Loading />
                ) : plans.length === 0 ? (
                    <EmptyState 
                        icon="🥗"
                        title="No diet plans yet"
                        message="Your dietitian will assign you a meal plan soon!"
                    />
                ) : (
                    <div>
                        {plans.map((plan) => (
                            <Card key={plan.dietId}>
                                <h3 style={{ marginBottom: '10px', color: '#28a745' }}>🍎 {plan.description}</h3>
                                <p><strong>👩‍⚕️ Dietitian:</strong> {plan.dietitianName}</p>
                                <p><strong>📅 Duration:</strong> {formatDate(plan.startDate)} - {formatDate(plan.endDate)}</p>
                                <p><strong>⏱️ Days:</strong> {plan.durationDays} days</p>
                                {plan.caloriesTarget && (
                                    <p><strong>🔥 Target Calories:</strong> {plan.caloriesTarget} kcal/day</p>
                                )}
                            </Card>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default DietPlansPage;
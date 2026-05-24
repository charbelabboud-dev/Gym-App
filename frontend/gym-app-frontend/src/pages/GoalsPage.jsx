import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import GoalService from '../services/GoalService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import goalService from '../services/GoalService';
import '../Styles/global.css';

function GoalsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [goals, setGoals] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [showForm, setShowForm] = useState(false);
    
    const [formData, setFormData] = useState({
        goalType: 'Weight',
        targetValue: '',
        targetDate: ''
    });

    const clientCode = user?.clientCode;

    useEffect(() => {
        if (clientCode) {
            loadGoals();
        }
    }, [clientCode]);

    const loadGoals = async () => {
        try {
            setLoading(true);
            const result = await GoalService.getClientGoals(clientCode);
            if (result.success) {
                setGoals(result.data);
            } else {
                setError('Failed to load goals');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setSuccess('');

        const goalData = {
            clientCode: clientCode,
            goalType: formData.goalType,
            targetValue: parseFloat(formData.targetValue),
            targetDate: formData.targetDate || null
        };

        try {
            const result = await goalService.createGoal(goalData);
            if (result.success) {
                setSuccess('Goal created successfully!');
                setShowForm(false);
                setFormData({ goalType: 'Weight', targetValue: '', targetDate: '' });
                loadGoals();
                setTimeout(() => setSuccess(''), 3000);
            } else {
                setError(result.message || 'Failed to create goal');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error creating goal');
        } finally {
            setLoading(false);
        }
    };

    const handleUpdateProgress = async (goalId, currentValue) => {
        try {
            const result = await goalService.updateProgress(goalId, { currentValue: parseFloat(currentValue) });
            if (result.success) {
                setSuccess('Progress updated!');
                loadGoals();
                setTimeout(() => setSuccess(''), 3000);
            }
        } catch (err) {
            setError('Error updating progress');
        }
    };

    const getGoalIcon = (type) => {
        switch(type) {
            case 'Weight': return '⚖️';
            case 'Sessions': return '📅';
            case 'Workouts': return '💪';
            case 'BodyFat': return '📊';
            default: return '🎯';
        }
    };

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Goals" />
                    <Alert type="error" message="Unable to find your client profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                    <PageHeader title="🎯 My Fitness Goals" showBack={true} />
                    <Button onClick={() => setShowForm(!showForm)} variant="success">
                        {showForm ? 'Cancel' : '+ New Goal'}
                    </Button>
                </div>
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {/* Create Goal Form */}
                {showForm && (
                    <Card>
                        <h3>Set New Goal</h3>
                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label>Goal Type</label>
                                <select
                                    name="goalType"
                                    value={formData.goalType}
                                    onChange={handleChange}
                                    className="form-control"
                                >
                                    <option value="Weight">⚖️ Weight Goal (kg)</option>
                                    <option value="Sessions">📅 Sessions Goal (per month)</option>
                                    <option value="Workouts">💪 Workout Goal (per week)</option>
                                    <option value="BodyFat">📊 Body Fat Goal (%)</option>
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Target Value</label>
                                <input
                                    type="number"
                                    step="0.1"
                                    name="targetValue"
                                    value={formData.targetValue}
                                    onChange={handleChange}
                                    required
                                    placeholder="Enter your target"
                                    className="form-control"
                                />
                            </div>

                            <div className="form-group">
                                <label>Target Date (optional)</label>
                                <input
                                    type="date"
                                    name="targetDate"
                                    value={formData.targetDate}
                                    onChange={handleChange}
                                    className="form-control"
                                />
                            </div>

                            <Button type="submit" variant="primary">Create Goal</Button>
                        </form>
                    </Card>
                )}

                {/* Goals List */}
                {loading ? (
                    <Loading />
                ) : goals.length === 0 ? (
                    <EmptyState 
                        icon="🎯"
                        title="No goals set"
                        message="Set your first fitness goal to start tracking!"
                    />
                ) : (
                    <div>
                        {goals.map((goal) => (
                            <Card key={goal.id}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap' }}>
                                    <div>
                                        <h3>{getGoalIcon(goal.goalType)} {goal.goalType} Goal</h3>
                                        <p><strong>Target:</strong> {goal.targetValue} {goal.goalType === 'Weight' ? 'kg' : goal.goalType === 'BodyFat' ? '%' : ''}</p>
                                        {goal.currentValue && (
                                            <p><strong>Current:</strong> {goal.currentValue} {goal.goalType === 'Weight' ? 'kg' : goal.goalType === 'BodyFat' ? '%' : ''}</p>
                                        )}
                                        {goal.targetDate && (
                                            <p><strong>Target Date:</strong> {new Date(goal.targetDate).toLocaleDateString()}</p>
                                        )}
                                        <p><strong>Progress:</strong> {goal.progressPercentage}%</p>
                                        <div style={{ backgroundColor: '#e0e0e0', borderRadius: '10px', height: '10px', width: '100%', marginTop: '10px' }}>
                                            <div style={{ backgroundColor: '#28a745', borderRadius: '10px', height: '10px', width: `${goal.progressPercentage}%` }}></div>
                                        </div>
                                    </div>
                                    <div>
                                        {!goal.isCompleted && (
                                            <div>
                                                <input
                                                    type="number"
                                                    step="0.1"
                                                    placeholder="Update value"
                                                    id={`progress-${goal.id}`}
                                                    style={{ padding: '8px', marginRight: '8px', width: '100px' }}
                                                />
                                                <Button 
                                                    onClick={() => {
                                                        const value = document.getElementById(`progress-${goal.id}`).value;
                                                        if (value) handleUpdateProgress(goal.id, value);
                                                    }}
                                                    variant="primary"
                                                    size="small"
                                                >
                                                    Update
                                                </Button>
                                            </div>
                                        )}
                                        {goal.isCompleted && (
                                            <span style={{ color: '#28a745', fontWeight: 'bold' }}>✓ Completed!</span>
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

export default GoalsPage;
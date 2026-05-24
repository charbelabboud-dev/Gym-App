import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import workoutPlanService from '../services/WorkoutPlanService';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function CreateWorkoutPlanPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    
    const coachCode = user?.coachCode;
    
    const [formData, setFormData] = useState({
        clientCode: '',
        planName: '',
        startDate: '',
        endDate: '',
        exercises: [
            { exerciseName: '', sets: 3, reps: 10, restSeconds: 60 }
        ]
    });

    useEffect(() => {
        if (coachCode) {
            loadClients();
        }
    }, );

    const loadClients = async () => {
        try {
            setLoading(true);
            const result = await clientService.getCoachClients(coachCode);
            if (result.success) {
                setClients(result.data);
            }
        } catch (err) {
            setError('Failed to load clients');
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleExerciseChange = (index, field, value) => {
        const updatedExercises = [...formData.exercises];
        updatedExercises[index][field] = value;
        setFormData({ ...formData, exercises: updatedExercises });
    };

    const addExercise = () => {
        setFormData({
            ...formData,
            exercises: [
                ...formData.exercises,
                { exerciseName: '', sets: 3, reps: 10, restSeconds: 60 }
            ]
        });
    };

    const removeExercise = (index) => {
        const updatedExercises = formData.exercises.filter((_, i) => i !== index);
        setFormData({ ...formData, exercises: updatedExercises });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSubmitting(true);
        setError('');
        setSuccess('');

        // Validate at least one exercise
        const hasValidExercise = formData.exercises.some(ex => ex.exerciseName.trim());
        if (!hasValidExercise) {
            setError('Please add at least one exercise');
            setSubmitting(false);
            return;
        }

        const planData = {
            clientCode: formData.clientCode,
            coachCode: coachCode,
            planName: formData.planName,
            startDate: formData.startDate + 'T00:00:00',
            endDate: formData.endDate + 'T00:00:00',
            exercises: formData.exercises.filter(ex => ex.exerciseName.trim())
        };

        try {
            const result = await workoutPlanService.createWorkoutPlan(planData);
            if (result.success) {
                setSuccess('Workout plan created successfully!');
                setTimeout(() => {
                    navigate('/my-clients');
                }, 2000);
            } else {
                setError(result.message || 'Failed to create workout plan');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error creating workout plan');
        } finally {
            setSubmitting(false);
        }
    };

    if (!coachCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Create Workout Plan" />
                    <Alert type="error" message="Unable to find your coach profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="📝 Create Workout Plan" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {loading ? (
                    <Loading />
                ) : (
                    <Card>
                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label>Select Client *</label>
                                <select
                                    name="clientCode"
                                    value={formData.clientCode}
                                    onChange={handleChange}
                                    required
                                    className="form-control"
                                >
                                    <option value="">-- Select a client --</option>
                                    {clients.map(client => (
                                        <option key={client.clCode} value={client.clCode}>
                                            {client.fullName} ({client.email})
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Plan Name *</label>
                                <input
                                    type="text"
                                    name="planName"
                                    value={formData.planName}
                                    onChange={handleChange}
                                    required
                                    placeholder="e.g., Beginner Strength Program"
                                    className="form-control"
                                />
                            </div>

                            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px' }}>
                                <div className="form-group">
                                    <label>Start Date *</label>
                                    <input
                                        type="date"
                                        name="startDate"
                                        value={formData.startDate}
                                        onChange={handleChange}
                                        required
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>End Date *</label>
                                    <input
                                        type="date"
                                        name="endDate"
                                        value={formData.endDate}
                                        onChange={handleChange}
                                        required
                                        className="form-control"
                                    />
                                </div>
                            </div>

                            <h3 style={{ margin: '20px 0 15px 0' }}>Exercises</h3>
                            
                            {formData.exercises.map((exercise, index) => (
                                <div key={index} style={{ 
                                    border: '1px solid #ddd', 
                                    borderRadius: '8px', 
                                    padding: '15px', 
                                    marginBottom: '15px',
                                    position: 'relative'
                                }}>
                                    <div style={{ display: 'grid', gridTemplateColumns: '1fr auto', gap: '10px', marginBottom: '10px' }}>
                                        <input
                                            type="text"
                                            placeholder="Exercise name *"
                                            value={exercise.exerciseName}
                                            onChange={(e) => handleExerciseChange(index, 'exerciseName', e.target.value)}
                                            required
                                            className="form-control"
                                        />
                                        {formData.exercises.length > 1 && (
                                            <Button 
                                                type="button"
                                                variant="danger"
                                                onClick={() => removeExercise(index)}
                                                style={{ padding: '8px 16px' }}
                                            >
                                                Remove
                                            </Button>
                                        )}
                                    </div>
                                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '10px' }}>
                                        <div>
                                            <label style={{ fontSize: '12px' }}>Sets</label>
                                            <input
                                                type="number"
                                                value={exercise.sets}
                                                onChange={(e) => handleExerciseChange(index, 'sets', parseInt(e.target.value))}
                                                min="1"
                                                className="form-control"
                                            />
                                        </div>
                                        <div>
                                            <label style={{ fontSize: '12px' }}>Reps</label>
                                            <input
                                                type="number"
                                                value={exercise.reps}
                                                onChange={(e) => handleExerciseChange(index, 'reps', parseInt(e.target.value))}
                                                min="1"
                                                className="form-control"
                                            />
                                        </div>
                                        <div>
                                            <label style={{ fontSize: '12px' }}>Rest (seconds)</label>
                                            <input
                                                type="number"
                                                value={exercise.restSeconds}
                                                onChange={(e) => handleExerciseChange(index, 'restSeconds', parseInt(e.target.value))}
                                                min="0"
                                                className="form-control"
                                            />
                                        </div>
                                    </div>
                                </div>
                            ))}

                            <Button 
                                type="button"
                                variant="secondary"
                                onClick={addExercise}
                                style={{ marginBottom: '20px' }}
                            >
                                + Add Another Exercise
                            </Button>

                            <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
                                <Button 
                                    type="submit" 
                                    variant="primary" 
                                    disabled={submitting}
                                >
                                    {submitting ? 'Creating...' : 'Create Workout Plan'}
                                </Button>
                                <Button 
                                    type="button" 
                                    variant="secondary" 
                                    onClick={() => navigate('/my-clients')}
                                >
                                    Cancel
                                </Button>
                            </div>
                        </form>
                    </Card>
                )}
            </div>
        </div>
    );
}

export default CreateWorkoutPlanPage;
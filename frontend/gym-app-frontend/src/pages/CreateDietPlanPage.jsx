import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import dietPlanService from '../services/DietPlanService';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function CreateDietPlanPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    
    const dietitianCode = user?.dietitianCode;
    
    const [formData, setFormData] = useState({
        clientCode: '',
        description: '',
        startDate: '',
        endDate: '',
        caloriesTarget: ''
    });

    useEffect(() => {
        if (dietitianCode) {
            loadClients();
        }
    }, [dietitianCode]);

    const loadClients = async () => {
        try {
            setLoading(true);
            const result = await clientService.getDietitianClients(dietitianCode);
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

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSubmitting(true);
        setError('');
        setSuccess('');

        const planData = {
            clientCode: formData.clientCode,
            dietitianCode: dietitianCode,
            description: formData.description,
            startDate: formData.startDate + 'T00:00:00',
            endDate: formData.endDate + 'T00:00:00',
            caloriesTarget: formData.caloriesTarget ? parseInt(formData.caloriesTarget) : null
        };

        try {
            const result = await dietPlanService.createDietPlan(planData);
            if (result.success) {
                setSuccess('Diet plan created successfully!');
                setTimeout(() => {
                    navigate('/dietitian-clients');
                }, 2000);
            } else {
                setError(result.message || 'Failed to create diet plan');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error creating diet plan');
        } finally {
            setSubmitting(false);
        }
    };

    if (!dietitianCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Create Diet Plan" />
                    <Alert type="error" message="Unable to find your dietitian profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="📝 Create Diet Plan" />
                
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
                                <label>Plan Description *</label>
                                <textarea
                                    name="description"
                                    value={formData.description}
                                    onChange={handleChange}
                                    required
                                    rows="3"
                                    placeholder="e.g., High protein meal plan for muscle gain..."
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

                            <div className="form-group">
                                <label>Target Calories (optional)</label>
                                <input
                                    type="number"
                                    name="caloriesTarget"
                                    value={formData.caloriesTarget}
                                    onChange={handleChange}
                                    placeholder="e.g., 2000"
                                    className="form-control"
                                />
                            </div>

                            <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
                                <Button 
                                    type="submit" 
                                    variant="primary" 
                                    disabled={submitting}
                                >
                                    {submitting ? 'Creating...' : 'Create Diet Plan'}
                                </Button>
                                <Button 
                                    type="button" 
                                    variant="secondary" 
                                    onClick={() => navigate('/dietitian-clients')}
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

export default CreateDietPlanPage;
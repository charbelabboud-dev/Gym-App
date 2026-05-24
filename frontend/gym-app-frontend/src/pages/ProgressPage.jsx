import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import progressService from '../services/ProgressService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function ProgressPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [progress, setProgress] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [showForm, setShowForm] = useState(false);
    
    const [formData, setFormData] = useState({
        weight: '',
        bodyFatPercentage: '',
        chest: '',
        waist: '',
        notes: '',
        entryDate: new Date().toISOString().split('T')[0]
    });

    const clientCode = user?.clientCode;

    useEffect(() => {
        if (clientCode) {
            loadProgress();
        }
    }, [clientCode]);

    const loadProgress = async () => {
        try {
            setLoading(true);
            const result = await progressService.getClientProgress(clientCode);
            if (result.success) {
                setProgress(result.data);
            } else {
                setError('Failed to load progress');
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

        const progressData = {
            clientCode: clientCode,
            weight: formData.weight ? parseFloat(formData.weight) : null,
            bodyFatPercentage: formData.bodyFatPercentage ? parseFloat(formData.bodyFatPercentage) : null,
            chest: formData.chest ? parseFloat(formData.chest) : null,
            waist: formData.waist ? parseFloat(formData.waist) : null,
            notes: formData.notes,
            entryDate: formData.entryDate + 'T00:00:00'
        };

        try {
            const result = await progressService.addProgress(progressData);
            if (result.success) {
                setSuccess('Progress added successfully!');
                setShowForm(false);
                setFormData({
                    weight: '',
                    bodyFatPercentage: '',
                    chest: '',
                    waist: '',
                    notes: '',
                    entryDate: new Date().toISOString().split('T')[0]
                });
                loadProgress();
                setTimeout(() => setSuccess(''), 3000);
            } else {
                setError(result.message || 'Failed to add progress');
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Error adding progress');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (progressId) => {
        if (!window.confirm('Are you sure you want to delete this entry?')) return;
        
        try {
            const result = await progressService.deleteProgress(progressId);
            if (result.success) {
                setSuccess('Progress entry deleted');
                loadProgress();
                setTimeout(() => setSuccess(''), 3000);
            }
        } catch (err) {
            setError('Error deleting entry');
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

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Progress Tracking" />
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
    <PageHeader title="📊 Progress Tracking" showBack={true} />
    <Button onClick={() => setShowForm(!showForm)} variant="success">
        {showForm ? 'Cancel' : '+ Add Progress'}
    </Button>
</div>
                
                <Alert type="error" message={error} onClose={() => setError('')} />
                <Alert type="success" message={success} onClose={() => setSuccess('')} />

                {/* Add Progress Form */}
                {showForm && (
                    <Card>
                        <h3>Add Progress Entry</h3>
                        <form onSubmit={handleSubmit}>
                            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '15px' }}>
                                <div className="form-group">
                                    <label>Date</label>
                                    <input
                                        type="date"
                                        name="entryDate"
                                        value={formData.entryDate}
                                        onChange={handleChange}
                                        required
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Weight (kg)</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="weight"
                                        value={formData.weight}
                                        onChange={handleChange}
                                        placeholder="e.g., 75.5"
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Body Fat %</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="bodyFatPercentage"
                                        value={formData.bodyFatPercentage}
                                        onChange={handleChange}
                                        placeholder="e.g., 18.5"
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Chest (cm)</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="chest"
                                        value={formData.chest}
                                        onChange={handleChange}
                                        placeholder="e.g., 95"
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Waist (cm)</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="waist"
                                        value={formData.waist}
                                        onChange={handleChange}
                                        placeholder="e.g., 80"
                                        className="form-control"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Notes</label>
                                    <input
                                        type="text"
                                        name="notes"
                                        value={formData.notes}
                                        onChange={handleChange}
                                        placeholder="How do you feel?"
                                        className="form-control"
                                    />
                                </div>
                            </div>
                            <div style={{ marginTop: '20px' }}>
                                <Button type="submit" variant="primary">Save Entry</Button>
                            </div>
                        </form>
                    </Card>
                )}

                {/* Progress History */}
                {loading && progress.length === 0 ? (
                    <Loading />
                ) : progress.length === 0 ? (
                    <EmptyState 
                        icon="📊"
                        title="No progress entries yet"
                        message="Click 'Add Progress' to start tracking your fitness journey!"
                    />
                ) : (
                    <div>
                        <h3>Progress History</h3>
                        <div className="table-container">
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th>Date</th>
                                        <th>Weight (kg)</th>
                                        <th>Body Fat %</th>
                                        <th>Chest (cm)</th>
                                        <th>Waist (cm)</th>
                                        <th>Notes</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {progress.map((entry) => (
                                        <tr key={entry.id}>
                                            <td>{formatDate(entry.entryDate)}</td>
                                            <td>{entry.weight || '-'}</td>
                                            <td>{entry.bodyFatPercentage || '-'}</td>
                                            <td>{entry.chest || '-'}</td>
                                            <td>{entry.waist || '-'}</td>
                                            <td>{entry.notes || '-'}</td>
                                            <td>
                                                <Button 
                                                    onClick={() => handleDelete(entry.id)} 
                                                    variant="danger"
                                                    style={{ padding: '4px 12px', fontSize: '12px' }}
                                                >
                                                    Delete
                                                </Button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

export default ProgressPage;
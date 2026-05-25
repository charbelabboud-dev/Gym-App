import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import progressService from '../services/ProgressService';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function ClientProgressPage() {
    const { clientCode } = useParams();
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [progress, setProgress] = useState([]);
    const [client, setClient] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    // Determine user role and back path
    const userRole = user?.role;
    const backPath = userRole === 'Dietitian' ? '/dietitian-clients' : '/my-clients';

    useEffect(() => {
        if (clientCode) {
            loadData();
        }
    }, [clientCode]);

    const loadData = async () => {
        try {
            setLoading(true);
            
            const clientResult = await clientService.getClientDetails(clientCode);
            if (clientResult.success) {
                setClient(clientResult.data);
            }
            
            const progressResult = await progressService.getClientProgress(clientCode);
            if (progressResult.success) {
                setProgress(progressResult.data);
            }
        } catch (err) {
            setError('Error loading data');
        } finally {
            setLoading(false);
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

    const formatWeight = (weight) => {
        if (!weight) return '-';
        return `${weight} kg`;
    };

    const formatBodyFat = (value) => {
        if (!value) return '-';
        return `${value}%`;
    };

    const formatMeasurement = (value) => {
        if (!value) return '-';
        return `${value} cm`;
    };

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Client Progress" />
                    <Alert type="error" message="No client selected." />
                    <Button onClick={() => navigate(backPath)} variant="primary">Back to Clients</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title={`📊 ${client?.fullName || 'Client'}'s Progress`} />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {loading ? (
                    <Loading />
                ) : progress.length === 0 ? (
                    <EmptyState 
                        icon="📊"
                        title="No progress entries"
                        message="This client hasn't added any progress entries yet."
                        action={
                            <Button onClick={() => navigate(backPath)} variant="primary">
                                Back to Clients
                            </Button>
                        }
                    />
                ) : (
                    <div>
                        {/* Summary Stats */}
                        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '15px', marginBottom: '25px' }}>
                            <Card>
                                <h4>Latest Weight</h4>
                                <p style={{ fontSize: '24px', fontWeight: 'bold', color: '#2c3e50' }}>
                                    {progress[0]?.weight ? `${progress[0].weight} kg` : '-'}
                                </p>
                            </Card>
                            <Card>
                                <h4>Latest Body Fat</h4>
                                <p style={{ fontSize: '24px', fontWeight: 'bold', color: '#2c3e50' }}>
                                    {progress[0]?.bodyFatPercentage ? `${progress[0].bodyFatPercentage}%` : '-'}
                                </p>
                            </Card>
                            <Card>
                                <h4>Total Entries</h4>
                                <p style={{ fontSize: '24px', fontWeight: 'bold', color: '#2c3e50' }}>
                                    {progress.length}
                                </p>
                            </Card>
                        </div>

                        {/* Progress History Table */}
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
                                    </tr>
                                </thead>
                                <tbody>
                                    {progress.map((entry) => (
                                        <tr key={entry.id}>
                                            <td>{formatDate(entry.entryDate)}</td>
                                            <td>{formatWeight(entry.weight)}</td>
                                            <td>{formatBodyFat(entry.bodyFatPercentage)}</td>
                                            <td>{formatMeasurement(entry.chest)}</td>
                                            <td>{formatMeasurement(entry.waist)}</td>
                                            <td>{entry.notes || '-'}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                )}
                
                <div style={{ marginTop: '20px' }}>
                    <Button onClick={() => navigate(backPath)} variant="secondary">
                        ← Back to Clients
                    </Button>
                </div>
            </div>
        </div>
    );
}

export default ClientProgressPage;
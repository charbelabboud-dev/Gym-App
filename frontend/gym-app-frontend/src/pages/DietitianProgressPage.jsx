import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import progressService from '../services/ProgressService';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function DietitianProgressPage() {
    const { clientCode } = useParams();
    const { user } = useAuth();
    const navigate = useNavigate();


        console.log('User:', user);  // ← Add this
    console.log('ClientCode from URL:', clientCode);  // ← Add this
    console.log('DietitianCode:', user?.dietitianCode);  // ← Add this
    
    const [progress, setProgress] = useState([]);
    const [client, setClient] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const dietitianCode = user?.dietitianCode;

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

    if (!dietitianCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Client Progress" />
                    <Alert type="error" message="Unable to find your dietitian profile." />
                    <Button onClick={() => navigate('/dietitian-clients')} variant="primary">Back to Clients</Button>
                </div>
            </div>
        );
    }

    if (!clientCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="Client Progress" />
                    <Alert type="error" message="No client selected." />
                    <Button onClick={() => navigate('/dietitian-clients')} variant="primary">Back to Clients</Button>
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
                            <Button onClick={() => navigate('/dietitian-clients')} variant="primary">
                                Back to Clients
                            </Button>
                        }
                    />
                ) : (
                    <div>
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
                                            <td>{entry.weight || '-'}</td>
                                            <td>{entry.bodyFatPercentage || '-'}</td>
                                            <td>{entry.chest || '-'}</td>
                                            <td>{entry.waist || '-'}</td>
                                            <td>{entry.notes || '-'}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                )}
                
                <div style={{ marginTop: '20px' }}>
                    <Button onClick={() => navigate('/dietitian-clients')} variant="secondary">
                        ← Back to Clients
                    </Button>
                </div>
            </div>
        </div>
    );
}

export default DietitianProgressPage;
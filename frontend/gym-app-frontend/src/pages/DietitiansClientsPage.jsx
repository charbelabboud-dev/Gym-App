import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function DietitianClientsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const dietitianCode = user?.dietitianCode;

    useEffect(() => {
        if (dietitianCode) {
            loadClients();
        }
    }, [dietitianCode]);

    const loadClients = async () => {
        try {
            setLoading(true);
            // You'll need to add this endpoint in backend
            const result = await clientService.getDietitianClients(dietitianCode);
            if (result.success) {
                setClients(result.data);
            } else {
                setError('Failed to load clients');
            }
        } catch (err) {
            setError('Error connecting to server');
        } finally {
            setLoading(false);
        }
    };

    if (!dietitianCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Clients" />
                    <Alert type="error" message="Unable to find your dietitian profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="👥 My Nutrition Clients" />
                
                <Alert type="error" message={error} onClose={() => setError('')} />

                {loading ? (
                    <Loading />
                ) : clients.length === 0 ? (
                    <EmptyState 
                        icon="👥"
                        title="No clients yet"
                        message="You don't have any assigned clients yet."
                    />
                ) : (
                    <div>
                        <p style={{ marginBottom: '20px', color: '#7f8c8d' }}>
                            You have {clients.length} client(s)
                        </p>
                        <div className="table-container">
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th>Client</th>
                                        <th>Email</th>
                                        <th>Phone</th>
                                        <th>Registered</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {clients.map((client) => (
                                        <tr key={client.clCode}>
                                            <td><strong>{client.fullName}</strong></td>
                                            <td>{client.email}</td>
                                            <td>{client.phone}</td>
                                            <td>{new Date(client.registerDate).toLocaleDateString()}</td>
                                            <td>
                                                <Button 
                                                    onClick={() => navigate(`/dietitian-progress/${client.clCode}`)}
                                                    variant="primary"
                                                    style={{ padding: '4px 12px', fontSize: '12px' }}
                                                >
                                                    View Progress
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

export default DietitianClientsPage;
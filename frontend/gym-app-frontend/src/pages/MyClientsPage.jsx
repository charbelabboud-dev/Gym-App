import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import clientService from '../services/clientService';
import PageHeader from '../Components/common/PageHeader';
import Card from '../Components/common/Card';
import Button from '../Components/common/Button';
import Loading from '../Components/common/Loading';
import EmptyState from '../Components/common/EmptyState';
import Alert from '../Components/common/Alert';
import '../Styles/global.css';

function    MyClientsPage() {
    const { user } = useAuth();
    const navigate = useNavigate();
    
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const coachCode = user?.coachCode;

    useEffect(() => {
        if (coachCode) {
            loadClients();
        }
    }, [coachCode]);

    const loadClients = async () => {
        try {
            setLoading(true);
            const result = await clientService.getCoachClients(coachCode);
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

    if (!coachCode) {
        return (
            <div className="page-container">
                <div className="page-content">
                    <PageHeader title="My Clients" />
                    <Alert type="error" message="Unable to find your coach profile." />
                    <Button onClick={() => navigate('/dashboard')} variant="primary">Back to Dashboard</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="page-container">
            <div className="page-content">
                <PageHeader title="👥 My Clients" />
                
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
                                        <th>Sessions</th>
                                        <th>Completed</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {clients.map((client) => (
                                        <tr key={client.clCode}>
                                            <td><strong>{client.fullName}</strong></td>
                                            <td>{client.email}</td>
                                            <td>{client.phone}</td>
                                            <td>{new Date(client.registerDate).toLocaleDateString()}</td>
                                            <td style={{ textAlign: 'center' }}>{client.totalSessions}</td>
                                            <td style={{ textAlign: 'center' }}>{client.completedSessions}</td>
                                            <td>
                                                <Button 
                                                    onClick={() => navigate(`/client-progress/${client.clCode}`)}
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

export default MyClientsPage;
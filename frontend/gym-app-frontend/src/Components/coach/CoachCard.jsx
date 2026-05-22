import React from 'react';

function CoachCard({ coach, onBookSession }) {
    return (
        <div style={{
            border: '1px solid #ddd',
            borderRadius: '8px',
            padding: '15px',
            marginBottom: '15px',
            backgroundColor: 'white',
            boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
        }}>
            <h3 style={{ margin: '0 0 10px 0', color: '#333' }}>{coach.fullName}</h3>
            
            <div style={{ marginBottom: '10px' }}>
                <span style={{ 
                    backgroundColor: '#007bff', 
                    color: 'white', 
                    padding: '3px 8px', 
                    borderRadius: '4px',
                    fontSize: '12px'
                }}>
                    {coach.specialty}
                </span>
            </div>
            
            <p style={{ margin: '5px 0' }}>
                <strong>Phone:</strong> {coach.phone}
            </p>
            <p style={{ margin: '5px 0' }}>
                <strong>Email:</strong> {coach.email}
            </p>
            <p style={{ margin: '5px 0' }}>
                <strong>Address:</strong> {coach.address}
            </p>
            
            <button
                onClick={() => onBookSession(coach)}
                style={{
                    marginTop: '10px',
                    padding: '8px 16px',
                    backgroundColor: '#28a745',
                    color: 'white',
                    border: 'none',
                    borderRadius: '4px',
                    cursor: 'pointer'
                }}
            >
                Book Session
            </button>
        </div>
    );
}

export default CoachCard;
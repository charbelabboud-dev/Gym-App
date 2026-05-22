import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';


function Header() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    // Don't show header on login/register pages
    if (location.pathname === '/login' || location.pathname === '/register') {
        return null;
    }

    return (
        <header className="app-header">
            <div className="header-content">
                <div className="header-logo" onClick={() => navigate('/dashboard')}>
                    <span className="logo-icon">🏋️</span>
                    <span className="logo-text">GymApp</span>
                </div>
                
                <div className="header-user">
                    <span className="user-name">Welcome, {user?.fullName?.split(' ')[0] || 'User'}</span>
                    <button onClick={handleLogout} className="logout-header-btn">
                        Logout
                    </button>
                </div>
            </div>
        </header>
    );
}

export default Header;
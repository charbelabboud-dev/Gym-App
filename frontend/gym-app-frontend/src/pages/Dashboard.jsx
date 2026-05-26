import React, { useState, useEffect, useRef } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import sessionService from '../services/SessionService';
import workoutPlanService from '../services/WorkoutPlanService';
import progressService from '../services/ProgressService';
import '../Styles/Dashboard.css';

function Dashboard() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    
    const [stats, setStats] = useState({ sessions: 0, workouts: 0, progress: 0 });
    const [loading, setLoading] = useState(true);

    // use a ref so the effect only fires once — prevents the oscillation flicker
    const clientCodeRef = useRef(user?.clientCode);
    const roleRef = useRef(user?.role);

    useEffect(() => {
        if (clientCodeRef.current && roleRef.current === 'Client') {
            fetchStats();
        } else {
            setLoading(false);
        }
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const fetchStats = async () => {
        try {
            const sessionsResult = await sessionService.getClientSessions(clientCodeRef.current);
            const completedSessions = sessionsResult.success
                ? sessionsResult.data.filter(s => s.status === 'Completed').length : 0;

            const workoutsResult = await workoutPlanService.getClientPlans(clientCodeRef.current);
            const workoutsCount = workoutsResult.success ? workoutsResult.data.length : 0;

            const progressResult = await progressService.getClientProgress(clientCodeRef.current);
            const progressCount = progressResult.success ? progressResult.data.length : 0;
            const progressPercentage = Math.min(progressCount * 10, 100);

            setStats({ sessions: completedSessions, workouts: workoutsCount, progress: progressPercentage });
        } catch (error) {
            console.error('Error fetching stats:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleLogout = () => { logout(); navigate('/login'); };

    const features = {
        client: [
            { icon: '👨‍🏫', title: 'Browse Coaches',    desc: 'Find the perfect coach for your goals',  path: '/coaches' },
            { icon: '📋', title: 'My Sessions',       desc: 'View and manage your bookings',           path: '/my-sessions' },
            { icon: '💪', title: 'Workout Plans',     desc: 'Follow your personalized workouts',       path: '/workout-plans' },
            { icon: '🥗', title: 'Diet Plans',        desc: 'Track your nutrition journey',            path: '/diet-plans' },
            { icon: '📊', title: 'Track Progress',    desc: 'Monitor your fitness growth',             path: '/progress' },
            { icon: '🔔', title: 'Notifications',     desc: 'View your updates',                       path: '/notifications' },
            { icon: '🎯', title: 'My Goals',          desc: 'Set and track fitness goals',             path: '/goals' },
            { icon: '🏆', title: 'Top Coaches',       desc: 'See the best rated coaches',              path: '/coach-rankings' },
        ],
        coach: [
            { icon: '📅', title: 'My Sessions',         desc: 'View and manage sessions',      path: '/coach-sessions' },
            { icon: '👥', title: 'My Clients',           desc: 'Manage your clients',           path: '/my-clients' },
            { icon: '📝', title: 'Create Workout Plan',  desc: 'Design training programs',      path: '/create-workout-plan' },
            { icon: '📊', title: 'Client Progress',      desc: 'Track client achievements',     path: '/my-clients' },
            { icon: '⭐', title: 'My Reviews',           desc: 'See what clients say',          path: '/my-reviews' },
        ],
        dietitian: [
            { icon: '👥', title: 'My Clients',      desc: 'Manage your nutrition clients',  path: '/dietitian-clients' },
            { icon: '📅', title: 'Consultations',   desc: 'View consultation requests',     path: '/dietitian-consultations' },
            { icon: '📝', title: 'Create Diet Plan',desc: 'Design meal programs',            path: '/create-diet-plan' },
            { icon: '📊', title: 'Client Progress', desc: 'Track nutrition goals',           path: '/dietitian-clients' },
        ],
    };

    const currentFeatures = features[user?.role?.toLowerCase()] || features.client;
    const sectionTitle =
        user?.role === 'Client'   ? 'Your Fitness Toolkit' :
        user?.role === 'Coach'    ? 'Coach Dashboard' :
        'Dietitian Dashboard';

    return (
        <div className="dashboard-container">
            {/* Giant background watermark */}
            <div className="dashboard-watermark" aria-hidden="true">FITLINK</div>

            <div className="dashboard-content">

                {/* Header */}
                <div className="dashboard-header">
                    <div className="welcome-text">
                        <span className="welcome-eyebrow">WELCOME BACK</span>
                        <h1>{user?.fullName}!</h1>
                    </div>
                    <button onClick={handleLogout} className="logout-btn">Logout</button>
                </div>

                {/* Stats */}
                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon">📅</div>
                        <div className="stat-value">{loading ? '—' : stats.sessions}</div>
                        <div className="stat-label">Sessions Completed</div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon">💪</div>
                        <div className="stat-value">{loading ? '—' : stats.workouts}</div>
                        <div className="stat-label">Active Workouts</div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon">📈</div>
                        <div className="stat-value">{loading ? '—' : `${stats.progress}%`}</div>
                        <div className="stat-label">Progress Goal</div>
                    </div>
                </div>

                {/* Features */}
                <h2 className="section-title">{sectionTitle}</h2>

                <div className="feature-grid">
                    {currentFeatures.map((feature, index) => (
                        <div key={index} className="feature-card" onClick={() => navigate(feature.path)}>
                            <div className="feature-glow" />
                            <div className="feature-icon">{feature.icon}</div>
                            <div className="feature-title">{feature.title}</div>
                            <div className="feature-desc">{feature.desc}</div>
                        </div>
                    ))}
                </div>

            </div>
        </div>
    );
}

export default Dashboard;

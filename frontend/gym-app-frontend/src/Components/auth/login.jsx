import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import Alert from '../../Components/common/Alert';
import '../../Styles/global.css';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPass, setShowPass] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    const result = await login(email, password);
    if (result.success) {
      navigate('/dashboard');
    } else {
      setError(result.error);
    }
    setLoading(false);
  };

  return (
    <div className="auth-container">

      {/* ── Left visual panel ── */}
      <div className="auth-visual">
        <div className="auth-visual-image" />
        <div className="auth-visual-overlay" />
        <div className="auth-visual-overlay-bottom" />

        {/* Logo */}
        <div className="auth-visual-logo">
          <div className="auth-visual-logo-icon">🏋️</div>
          <span className="auth-visual-logo-text">DIET<span>HUB</span></span>
        </div>

        {/* Bottom content */}
        <div className="auth-visual-content">
          <h1 className="auth-visual-headline">
            BUILD YOUR<br/>
            <span>DREAM</span><br/>
            PHYSIQUE
          </h1>
          <p className="auth-visual-sub">
            Expert coaches, personalised diet plans, and progress tracking — all in one place.
          </p>
          <div className="auth-visual-stats">
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">2.4K+</span>
              <span className="auth-visual-stat-label">Members</span>
            </div>
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">180+</span>
              <span className="auth-visual-stat-label">Coaches</span>
            </div>
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">98%</span>
              <span className="auth-visual-stat-label">Satisfaction</span>
            </div>
          </div>
        </div>
      </div>

      {/* ── Right form panel ── */}
      <div className="auth-form-panel">
        <div className="auth-card">
          <div className="auth-header">
            <h1>WELCOME BACK</h1>
            <p>Sign in to continue your journey</p>
          </div>

          <Alert type="error" message={error} onClose={() => setError('')} />

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Email Address</label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                placeholder="you@example.com"
              />
            </div>

            <div className="form-group">
              <label>Password</label>
              <div style={{ position: 'relative' }}>
                <input
                  type={showPass ? 'text' : 'password'}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  placeholder="Enter your password"
                  style={{ paddingRight: '48px' }}
                />
                <button
                  type="button"
                  onClick={() => setShowPass(!showPass)}
                  style={{
                    position: 'absolute',
                    right: '14px',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    background: 'none',
                    border: 'none',
                    cursor: 'pointer',
                    color: 'var(--muted)',
                    fontSize: '16px',
                    padding: 0,
                    width: 'auto',
                  }}
                >
                  {showPass ? '🙈' : '👁️'}
                </button>
              </div>
            </div>

            <button type="submit" className="btn-submit" disabled={loading}>
              {loading ? 'SIGNING IN...' : 'SIGN IN →'}
            </button>
          </form>

          <div className="auth-footer">
            <p>
              Don't have an account?{' '}
              <Link to="/register">Create one free</Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Login;

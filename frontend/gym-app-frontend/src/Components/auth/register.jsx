import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import Alert from '../../Components/common/Alert';
import '../../Styles/global.css';

const ROLES = [
  { value: 'Client',    icon: '💪', label: 'Client' },
  { value: 'Coach',     icon: '🏋️', label: 'Coach' },
  { value: 'Dietitian', icon: '🥗', label: 'Dietitian' },
];

function Register() {
  const [formData, setFormData] = useState({
    email: '', password: '', fullName: '', role: 'Client', phone: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPass, setShowPass] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });
  const setRole = (role) => setFormData({ ...formData, role });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    const result = await register(formData);
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

        <div className="auth-visual-logo">
          <div className="auth-visual-logo-icon">🏋️</div>
          <span className="auth-visual-logo-text">FIT<span>LINK</span></span>
        </div>

        <div className="auth-visual-content">
          <h1 className="auth-visual-headline">
            YOUR<br/>
            <span>FITNESS</span><br/>
            STARTS NOW
          </h1>
          <p className="auth-visual-sub">
            Join thousands who've already transformed their body with DietHub's elite coaching network.
          </p>
          <div className="auth-visual-stats">
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">Free</span>
              <span className="auth-visual-stat-label">To Join</span>
            </div>
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">3 Min</span>
              <span className="auth-visual-stat-label">Setup</span>
            </div>
            <div className="auth-visual-stat">
              <span className="auth-visual-stat-num">24/7</span>
              <span className="auth-visual-stat-label">Support</span>
            </div>
          </div>
        </div>
      </div>

      {/* ── Right form panel ── */}
      <div className="auth-form-panel">
        <div className="auth-card">
          <div className="auth-header">
            <h1>JOIN DIETHUB</h1>
            <p>Create your free account today</p>
          </div>

          <Alert type="error" message={error} onClose={() => setError('')} />

          {/* Role picker */}
          <div className="form-group">
            <label>I am a</label>
            <div className="role-picker">
              {ROLES.map((r) => (
                <div
                  key={r.value}
                  className={`role-option ${formData.role === r.value ? 'active' : ''}`}
                  onClick={() => setRole(r.value)}
                >
                  <div className="role-option-icon">{r.icon}</div>
                  <div className="role-option-label">{r.label}</div>
                </div>
              ))}
            </div>
          </div>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Full Name</label>
              <input
                type="text"
                name="fullName"
                value={formData.fullName}
                onChange={handleChange}
                required
                placeholder="John Doe"
              />
            </div>

            <div className="form-group">
              <label>Email Address</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
                placeholder="you@example.com"
              />
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '14px' }}>
              <div className="form-group">
                <label>Password</label>
                <div style={{ position: 'relative' }}>
                  <input
                    type={showPass ? 'text' : 'password'}
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    required
                    placeholder="Min 6 chars"
                    style={{ paddingRight: '44px' }}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPass(!showPass)}
                    style={{
                      position: 'absolute', right: '12px', top: '50%',
                      transform: 'translateY(-50%)', background: 'none',
                      border: 'none', cursor: 'pointer', color: 'var(--muted)',
                      fontSize: '14px', padding: 0, width: 'auto',
                    }}
                  >
                    {showPass ? '🙈' : '👁️'}
                  </button>
                </div>
              </div>

              <div className="form-group">
                <label>Phone (optional)</label>
                <input
                  type="tel"
                  name="phone"
                  value={formData.phone}
                  onChange={handleChange}
                  placeholder="+1 555 0100"
                />
              </div>
            </div>

            <button type="submit" className="btn-submit" disabled={loading}>
              {loading ? 'CREATING ACCOUNT...' : 'CREATE ACCOUNT →'}
            </button>
          </form>

          <div className="auth-footer">
            <p>
              Already have an account?{' '}
              <Link to="/login">Sign in here</Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Register;

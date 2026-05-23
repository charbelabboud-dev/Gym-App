import React from 'react';
import { useNavigate } from 'react-router-dom';

function PageHeader({ title, showBack = true }) {
    const navigate = useNavigate();

    return (
        <div className="page-header">
            <div className="page-header-left">
                {showBack && (
                    <button onClick={() => navigate(-1)} className="back-btn">
                        ← Back
                    </button>
                )}
                <h1>{title}</h1>
            </div>
        </div>
    );
}

export default PageHeader;
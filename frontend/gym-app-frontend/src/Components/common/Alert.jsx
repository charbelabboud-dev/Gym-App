import React from 'react';

function Alert({ type, message, onClose }) {
    if (!message) return null;

    return (
        <div className={`alert alert-${type}`}>
            {message}
            {onClose && (
                <button onClick={onClose} style={{ float: 'right', background: 'none', border: 'none', cursor: 'pointer' }}>
                    ✕
                </button>
            )}
        </div>
    );
}

export default Alert;
import React from 'react';

function Card({ children, onClick, className = '' }) {
    return (
        <div className={`card ${className}`} onClick={onClick}>
            {children}
        </div>
    );
}

export default Card;
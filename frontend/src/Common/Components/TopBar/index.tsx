const TopBar = () => (
    <div className="bg-light border-bottom py-1 d-none d-md-block" style={{ fontSize: '0.85rem' }}>
        <div className="container d-flex justify-content-end align-items-center">
            <a href="tel:+5491100000000" className="text-dark text-decoration-none me-3">
                <span className="material-icons" style={{ fontSize: '1rem', verticalAlign: 'text-bottom' }}>phone</span> 
                +54 9 (341) 550-6271
            </a>
            <a href="mailto:info@csingenieria.com" className="text-dark text-decoration-none">
                <span className="material-icons" style={{ fontSize: '1rem', verticalAlign: 'text-bottom' }}>email</span>
                info@csingenieria.com.ar
            </a>
        </div>
    </div>
);

export default TopBar;
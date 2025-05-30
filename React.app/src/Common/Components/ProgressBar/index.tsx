interface ProgressBarProps {
    visible: number;
}

const ProgressBar: React.FC<ProgressBarProps> = ({ visible }) => {
    if (!visible) return null;

    return (
        <div
            className="position-fixed top-0 start-0 w-100 h-100 bg-light bg-opacity-75 d-flex justify-content-center align-items-center"
            style={{ zIndex: 1050 }}
        >
            <div className="spinner-border text-primary" role="status" style={{ width: '4rem', height: '4rem' }}>
                <span className="visually-hidden">Loading...</span>
            </div>
        </div>
    );
};

export default ProgressBar;

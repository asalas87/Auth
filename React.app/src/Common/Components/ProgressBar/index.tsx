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
    //if (!visible) return null;

    //return (
    //    <div
    //        className="position-fixed top-0 start-0 w-100"
    //        style={{ zIndex: 2000, height: '5px' }}
    //    >
    //        <div
    //            className="progress"
    //            style={{ height: '100%' }}
    //        >
    //            <div
    //                className="progress-bar progress-bar-striped progress-bar-animated bg-primary"
    //                role="progressbar"
    //                style={{ width: '100%' }}
    //            />
    //        </div>
    //    </div>
    //);
};

export default ProgressBar;

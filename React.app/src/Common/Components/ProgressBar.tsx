interface ProgressBarProps {
    progress: number;
}

const ProgressBar: React.FC<ProgressBarProps> = ({ progress }) => {
    return (
        <div className="progress mt-3">
            <div className="progress-bar" role="progressbar" style={{ width: `${progress}%` }}>
                {progress}%
            </div>
        </div>
    );
};

export default ProgressBar;

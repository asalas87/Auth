import React, { useState } from "react";
import { useUpload } from "../Hooks/useUpload";
import ProgressBar from "../../Common/Components/ProgressBar";

const FileUpload = () => {
    const [file, setFile] = useState<File | null>(null);
    const { handleUpload, progress, loading } = useUpload();

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            setFile(event.target.files[0]);
        }
    };

    const onUploadClick = () => {
        if (!file) return;
        const formData = new FormData();
        formData.append("file", file);
        handleUpload(formData);
    };

    return (
        <div className="container mt-4">
            <input
                type="file"
                onChange={handleFileChange}
                className="form-control"
                disabled={loading}
                accept="application/pdf"
            />
            <button
                onClick={onUploadClick}
                className="btn btn-primary mt-2"
                disabled={loading}
            >
                {loading ? "Uploading..." : "Upload"}
            </button>
            {progress > 0 && <ProgressBar progress={progress} visible={true} />}
        </div>
    );
};

export default FileUpload;

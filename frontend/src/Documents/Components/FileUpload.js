import React, { useState } from "react";
import { useUpload } from "../Hooks/useUpload";
import ProgressBar from "../../Common/Components/ProgressBar";
const FileUpload = () => {
    const [file, setFile] = useState(null);
    const { handleUpload, progress, loading } = useUpload();
    const handleFileChange = (event) => {
        if (event.target.files) {
            setFile(event.target.files[0]);
        }
    };
    const onUploadClick = () => {
        if (!file)
            return;
        const formData = new FormData();
        formData.append("file", file);
        handleUpload(formData);
    };
    return (React.createElement("div", { className: "container mt-4" },
        React.createElement("input", { type: "file", onChange: handleFileChange, className: "form-control", disabled: loading, accept: "application/pdf" }),
        React.createElement("button", { onClick: onUploadClick, className: "btn btn-primary mt-2", disabled: loading }, loading ? "Uploading..." : "Upload"),
        progress > 0 && React.createElement(ProgressBar, { progress: progress, visible: true })));
};
export default FileUpload;

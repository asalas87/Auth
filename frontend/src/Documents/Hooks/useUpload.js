import { useState } from "react";
//import { uploadDocument } from "../Services/DocumentService";
export const useUpload = () => {
    const [progress, setProgress] = useState(0);
    const [loading, setLoading] = useState(false);
    const handleUpload = async (formData) => {
        setLoading(true);
        try {
            //await uploadDocument(formData);
        }
        finally {
            setLoading(false);
            setProgress(0);
        }
    };
    return { handleUpload, loading, progress, setProgress };
};

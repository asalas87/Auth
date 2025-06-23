import { useState } from "react";
//import { uploadDocument } from "../Services/DocumentService";

export const useUpload = () => {
    const [progress, setProgress] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(false);

    const handleUpload = async (formData: FormData): Promise<void> => {
        setLoading(true);
        try {
            //await uploadDocument(formData);
        } finally {
            setLoading(false);
            setProgress(0);
        }
    };

    return { handleUpload, loading, progress, setProgress };
};

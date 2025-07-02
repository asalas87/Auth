import { useState } from "react";
import { create } from "../Services/DocumentService";
import { IDocumentDTO } from "../Interfaces/IDocumentDTO";

export const useUpload = () => {
    const [progress, setProgress] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(false);

    const handleUpload = async (formData: IDocumentDTO): Promise<void> => {
        setLoading(true);
        try {
            await create(formData);
        } finally {
            setLoading(false);
            setProgress(0);
        }
    };

    return { handleUpload, loading, progress, setProgress };
};

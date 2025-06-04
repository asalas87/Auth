import { createContext, useState } from "react";
import { IDocumentDTO } from "../Interfaces/IDocumentDTO";

interface DocumentsContextType {
    documents: IDocumentDTO[];
    addDocument: (doc: IDocumentDTO) => void;
}

export const DocumentsContext = createContext<DocumentsContextType | undefined>(undefined);

export const DocumentsProvider = ({ children }: { children: React.ReactNode }) => {
    const [documents, setDocuments] = useState<IDocumentDTO[]>([]);

    const addDocument = (doc: IDocumentDTO) => {
        setDocuments((prev) => [...prev, doc]);
    };

    return (
        <DocumentsContext.Provider value={{ documents, addDocument }}>
            {children}
        </DocumentsContext.Provider>
    );
};

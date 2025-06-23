import React, { createContext, useState } from "react";
export const DocumentsContext = createContext(undefined);
export const DocumentsProvider = ({ children }) => {
    const [documents, setDocuments] = useState([]);
    const addDocument = (doc) => {
        setDocuments((prev) => [...prev, doc]);
    };
    return (React.createElement(DocumentsContext.Provider, { value: { documents, addDocument } }, children));
};

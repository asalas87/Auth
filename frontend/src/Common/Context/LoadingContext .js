import React, { createContext, useContext, useState } from 'react';
const LoadingContext = createContext({ loading: false, setLoading: () => { } });
export const useLoading = () => useContext(LoadingContext);
export const LoadingProvider = ({ children }) => {
    const [loading, setLoading] = useState(false);
    return (React.createElement(LoadingContext.Provider, { value: { loading, setLoading } }, children));
};

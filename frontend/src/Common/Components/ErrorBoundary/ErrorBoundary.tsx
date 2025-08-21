import React from "react";
import { toast } from "react-toastify";

interface State {
  hasError: boolean;
  error?: Error;
}

class CrudTableErrorBoundary extends React.Component<{ children: React.ReactNode }, State> {
  constructor(props: { children: React.ReactNode }) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, info: React.ErrorInfo) {
    console.error("CrudTable Error:", error, info);
    toast.error("Ocurrió un error en la tabla. Intenta nuevamente.");
  }

  handleRetry = () => {
    this.setState({ hasError: false, error: undefined });
  };

  render() {
    if (this.state.hasError) {
      return (
        <div className="p-4 text-center">
          <p className="text-danger fw-bold">Algo salió mal en la tabla, refresque la pantalla</p>
          {/* <button className="btn btn-primary mt-2" onClick={this.handleRetry}>
            Reintentar
          </button> */}
        </div>
      );
    }

    return this.props.children;
  }
}

export default CrudTableErrorBoundary;

import AuthView from './Security/Views/AuthView';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
    return (
        <div>
            <ToastContainer />
            <AuthView />
        </div>
    );
}

export default App;

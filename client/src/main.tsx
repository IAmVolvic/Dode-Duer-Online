import { StrictMode } from "react";
import ReactDOM from 'react-dom/client'
import '@assets/styles/styles.css';
import 'jotai-devtools/styles.css';
import App from "./app/App";


ReactDOM.createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <App/>
    </StrictMode>
)
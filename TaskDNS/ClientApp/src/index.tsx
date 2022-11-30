// @ts-nocheck
import { BrowserRouter } from 'react-router-dom'
import ReactDOM from 'react-dom/client'
import { App } from './App'
import './css/page.css'

const rootElement = document.getElementById('root');
const root = ReactDOM.createRoot(rootElement);

root.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
)
import { Routes, Route, Navigate, Link } from 'react-router-dom'
import InvoicesPage from './pages/InvoicesPage'
import NewInvoicePage from './pages/NewInvoicePage'


export default function App() {
return (
<div className="container">
<header className="header">
<h1>Invoice Management</h1>
<nav>
<Link to="/invoices">Invoices</Link>
<Link to="/invoices/new">New Invoice</Link>
</nav>
</header>
<Routes>
<Route path="/" element={<Navigate to="/invoices" replace />} />
<Route path="/invoices" element={<InvoicesPage />} />
<Route path="/invoices/new" element={<NewInvoicePage />} />
<Route path="*" element={<div className="card"><h2>Not Found</h2></div>} />
</Routes>
</div>
)
}
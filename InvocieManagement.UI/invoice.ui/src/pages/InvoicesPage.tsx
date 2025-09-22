import React, { useEffect, useMemo, useState } from 'react'
import { getInvoices } from '../api'
import type { InvoiceRead } from '../types/invoiceTypes'
import InlineInvoiceForm from '../shared/InlineInvoiceForm'

export default function InvoicesPage() {
  const [customerName, setCustomerName] = useState('')
  const [startDate, setStartDate] = useState('')
  const [endDate, setEndDate] = useState('')
  const [sortBy, setSortBy] = useState<'id' | 'customerName' | 'invoiceDate' | 'totalAmount'>('invoiceDate')
  const [sortDir, setSortDir] = useState<'asc' | 'desc'>('desc')
  const [pageNumber, setPageNumber] = useState(1)
  const [pageSize, setPageSize] = useState(10)

  const [items, setItems] = useState<InvoiceRead[]>([])
  const [totalPages, setTotalPages] = useState(0)
  const [totalCount, setTotalCount] = useState(0)
  const [loading, setLoading] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const query = useMemo(() => ({
    pageNumber,
    pageSize,
    sortBy,
    sortDir,
    customerName: customerName || null,
    startDate: startDate || null,
    endDate: endDate || null,
  }), [pageNumber, pageSize, sortBy, sortDir, customerName, startDate, endDate])

  useEffect(() => {
    (async () => {
      setLoading(true); setErr(null)
      try {
        const res = await getInvoices(query)
        setItems(res.items)
        setTotalPages(res.totalPages)
        setTotalCount(res.totalCount)
      } catch (e: any) {
        setErr(e?.message || 'Failed to load invoices')
      } finally {
        setLoading(false)
      }
    })()
  }, [query])

  return (
    <div className="stack">
      <section className="card">
        <h2>Search</h2>
        <div className="filters">
          <input placeholder="Customer name" value={customerName} onChange={e => setCustomerName(e.target.value)} />
          <input type="date" value={startDate} onChange={e => setStartDate(e.target.value)} />
          <input type="date" value={endDate} onChange={e => setEndDate(e.target.value)} />
          <select value={sortBy} onChange={e => setSortBy(e.target.value as any)}>
            <option value="invoiceDate">Invoice Date</option>
            <option value="customerName">Customer</option>
            <option value="totalAmount">Total</option>
            <option value="id">Id</option>
          </select>
          <select value={sortDir} onChange={e => setSortDir(e.target.value as any)}>
            <option value="desc">Desc</option>
            <option value="asc">Asc</option>
          </select>
          <select value={pageSize} onChange={e => { setPageSize(Number(e.target.value)); setPageNumber(1) }}>
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
          </select>
          <button onClick={() => setPageNumber(1)}>Apply</button>
        </div>
      </section>

      <section className="card">
        <h2>Invoices</h2>
        {err && <p className="error">{err}</p>}
        {loading ? <p>Loading…</p> : items.length === 0 ? <p>No results.</p> : (
          <table className="table">
            <thead>
              <tr>
                <th>#</th>
                <th>Customer</th>
                <th>Date</th>
                <th>Total</th>
              </tr>
            </thead>
            <tbody>
              {items.map(inv => (
                <React.Fragment key={inv.id}>
                  <tr>
                    <td>{inv.id}</td>
                    <td>{inv.customerName}</td>
                    <td>{new Date(inv.invoiceDate).toLocaleDateString()}</td>
                    <td>{inv.totalAmount.toFixed(2)}</td>
                  </tr>
                  <tr>
                    <td colSpan={4}>
                      <table className="inner">
                        <thead>
                          <tr>
                            <th>Description</th>
                            <th>Qty</th>
                            <th>Unit</th>
                            <th>Line Total</th>
                          </tr>
                        </thead>
                        <tbody>
                          {inv.lines.map(line => (
                            <tr key={line.id}>
                              <td>{line.description}</td>
                              <td>{line.quantity}</td>
                              <td>{line.unitPrice.toFixed(2)}</td>
                              <td>{line.lineTotal.toFixed(2)}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </td>
                  </tr>
                </React.Fragment>
              ))}
            </tbody>
          </table>
        )}
        <div className="pager">
          <div>Found {totalCount}</div>
          <button disabled={pageNumber <= 1} onClick={() => setPageNumber(p => p - 1)}>Prev</button>
          <span>Page {pageNumber} / {Math.max(1, totalPages)}</span>
          <button disabled={pageNumber >= totalPages} onClick={() => setPageNumber(p => p + 1)}>Next</button>
        </div>
      </section>

      <section className="card">
        <h2>Quick add (Inline Invoice)</h2>
        <InlineInvoiceForm onCreated={() => { setPageNumber(1) }} />
      </section>
    </div>
  )
}

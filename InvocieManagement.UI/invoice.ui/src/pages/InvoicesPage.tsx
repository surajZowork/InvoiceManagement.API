import React, { useEffect, useMemo, useState } from 'react'
import { getInvoices } from '../api'
import type { InvoiceRead } from '../types/invoiceTypes'

export default function InvoicesPage() {
  const [customerName, setCustomerName] = useState('')
  const [startDate, setStartDate] = useState('') // yyyy-mm-dd
  const [endDate, setEndDate] = useState('')
  const [sortBy, setSortBy] = useState<'id' | 'customerName' | 'invoiceDate' | 'totalAmount'>('invoiceDate')
  const [sortDir, setSortDir] = useState<'asc' | 'desc'>('desc')
  const [pageNumber, setPageNumber] = useState(1)
  const [pageSize, setPageSize] = useState(10)

  const [data, setData] = useState<{ items: InvoiceRead[]; totalPages: number; totalCount: number }>({ items: [], totalPages: 0, totalCount: 0 })
  const [loading, setLoading] = useState(false)
  const [expandedRows, setExpandedRows] = useState<Set<number>>(new Set())

  const toggleRowExpansion = (invoiceId: number) => {
    setExpandedRows(prev => {
      const newSet = new Set(prev)
      if (newSet.has(invoiceId)) {
        newSet.delete(invoiceId)
      } else {
        newSet.add(invoiceId)
      }
      return newSet
    })
  }

  const query = useMemo(() => ({ customerName: customerName || null, startDate: startDate || null, endDate: endDate || null, sortBy, sortDir, pageNumber, pageSize }), [customerName, startDate, endDate, sortBy, sortDir, pageNumber, pageSize])

  useEffect(() => {
    (async () => {
      setLoading(true)
      try {
        const res = await getInvoices(query)
        setData({ items: res.items, totalPages: res.totalPages, totalCount: res.totalCount })
      } finally {
        setLoading(false)
      }
    })()
  }, [query.pageNumber, query.pageSize, query.sortBy, query.sortDir, query.customerName, query.startDate, query.endDate])

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
        {loading ? <p>Loading…</p> : data.items.length === 0 ? <p>No results.</p> : (
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
              {data.items.map(inv => (
                <React.Fragment key={inv.id}>
                  <tr className="expandable" onClick={() => toggleRowExpansion(inv.id)}>
                    <td>
                      <span className="expand-icon">{expandedRows.has(inv.id) ? '▼' : '▶'}</span>
                      {inv.id}
                    </td>
                    <td>{inv.customerName}</td>
                    <td>{new Date(inv.invoiceDate).toLocaleDateString()}</td>
                    <td>{inv.totalAmount.toFixed(2)}</td>
                  </tr>
                  {expandedRows.has(inv.id) && (
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
                  )}
                </React.Fragment>
              ))}
            </tbody>
          </table>
        )}
        <div className="pager">
          <button disabled={pageNumber <= 1} onClick={() => setPageNumber(p => p - 1)}>Prev</button>
          <span>Page {pageNumber} / {Math.max(1, data.totalPages)}</span>
          <button disabled={pageNumber >= data.totalPages} onClick={() => setPageNumber(p => p + 1)}>Next</button>
        </div>
      </section>
    </div>
  )
}
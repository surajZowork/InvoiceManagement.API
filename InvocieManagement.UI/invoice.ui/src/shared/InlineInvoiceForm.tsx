import React, { useState } from 'react'
import { createInvoice } from '../api'
import type { InvoiceLineCreate } from '../types/invoiceTypes'

export default function InlineInvoiceForm({ onCreated }: { onCreated: () => void }) {
  const [customerName, setCustomerName] = useState('')
  const [date, setDate] = useState('')
  const [lines, setLines] = useState<InvoiceLineCreate[]>([{ description: '', quantity: 1, unitPrice: 0 }])
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const setLine = (i: number, line: InvoiceLineCreate) => setLines(ls => ls.map((l, idx) => idx === i ? line : l))
  const addLine = () => setLines(ls => [...ls, { description: '', quantity: 1, unitPrice: 0 }])
  const removeLine = (i: number) => setLines(ls => ls.filter((_, idx) => idx !== i))

  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setBusy(true); setError(null)
    try {
      await createInvoice({ customerName, invoiceDate: date ? new Date(date).toISOString() : undefined, lines })
      setCustomerName(''); setDate(''); setLines([{ description: '', quantity: 1, unitPrice: 0 }])
      onCreated()
    } catch (err: any) {
      const msg = err?.response?.data?.title || (err?.response?.data?.errors ? JSON.stringify(err.response.data.errors) : err?.message)
      setError(msg || 'Failed to save')
    } finally { setBusy(false) }
  }

  return (
    <form onSubmit={submit} className="inline-form">
      <div className="row">
        <input placeholder="Customer" value={customerName} onChange={e => setCustomerName(e.target.value)} required />
        <input type="date" value={date} onChange={e => setDate(e.target.value)} />
        <button type="button" onClick={addLine}>+ Line</button>
      </div>
      {lines.map((l, i) => (
        <div className="row" key={i}>
          <input placeholder="Description" value={l.description} onChange={e => setLine(i, { ...l, description: e.target.value })} required />
          <input type="number" min={1} value={l.quantity} onChange={e => setLine(i, { ...l, quantity: Number(e.target.value) })} required />
          <input type="number" min={0} step={0.01} value={l.unitPrice} onChange={e => setLine(i, { ...l, unitPrice: Number(e.target.value) })} required />
          <button type="button" onClick={() => removeLine(i)}>Remove</button>
        </div>
      ))}
      {error && <p className="error">{error}</p>}
      <button type="submit" disabled={busy}>{busy ? 'Saving…' : 'Save Invoice'}</button>
    </form>
  )
}

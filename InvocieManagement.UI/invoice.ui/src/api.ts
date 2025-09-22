import axios from 'axios'
import type { InvoiceCreate, InvoiceRead, PagedResult } from './types/invoiceTypes'

const api = axios.create({ baseURL: '/' }) // proxied to API

export async function createInvoice(payload: InvoiceCreate) {
  const res = await api.post<InvoiceRead>('/api/invoices', payload)
  return res.data
}

export async function getInvoices(params: {
  pageNumber?: number
  pageSize?: number
  sortBy?: 'id' | 'customerName' | 'invoiceDate' | 'totalAmount'
  sortDir?: 'asc' | 'desc'
  customerName?: string | null
  startDate?: string | null // yyyy-mm-dd
  endDate?: string | null   // yyyy-mm-dd
}) {
  const res = await api.get<PagedResult<InvoiceRead>>('/api/invoices', { params })
  return res.data
}

export async function getInvoice(id: number) {
  const res = await api.get<InvoiceRead>(`/api/invoices/${id}`)
  return res.data
}

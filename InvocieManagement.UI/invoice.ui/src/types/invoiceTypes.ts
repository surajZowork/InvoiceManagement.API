export type InvoiceLineCreate = {
description: string
quantity: number
unitPrice: number
}

export type InvoiceCreate = {
customerName: string
invoiceDate?: string // ISO string
lines: InvoiceLineCreate[]
}


export type InvoiceLineRead = InvoiceLineCreate & {
id: number
lineTotal: number
}


export type InvoiceRead = {
id: number
customerName: string
invoiceDate: string
totalAmount: number
lines: InvoiceLineRead[]
}


export type PagedResult<T> = {
items: T[]
pageNumber: number
pageSize: number
totalCount: number
totalPages: number
}
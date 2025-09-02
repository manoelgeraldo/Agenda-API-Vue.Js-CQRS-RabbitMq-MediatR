
// src/stores/contacts.js
import { defineStore } from 'pinia'
import api from '../services/api'

export const useContacts = defineStore('contacts', {
  state: () => ({
    list: [],
    total: 0,

    page: 1,
    pageSize: 10,

    q: '',

    sortField: null,
    sortOrder: 0,

    isLoading: false,

    _reqSeq: 0,
  }),

  getters: {
    first: (s) => (s.page - 1) * s.pageSize,
  },

  actions: {

    async fetch(page = this.page, opts = {}) {

      const mySeq = ++this._reqSeq
      this.isLoading = true

      const params = {
        q: this.q || undefined,
        page,
        pageSize: opts.pageSize ?? this.pageSize,
        sortField: opts.sortField ?? this.sortField ?? undefined,

        sortOrder:
          opts.sortOrder ?? this.sortOrder
            ? (this.sortOrder === 1 ? 'asc' : this.sortOrder === -1 ? 'desc' : undefined)
            : undefined,
      }

      try {
        const { data } = await api.get('/contacts', { params })

        if (mySeq === this._reqSeq) {
          this.list = data.items
          this.total = data.total
          this.page = data.page
        }
        return data
      } finally {
        if (mySeq === this._reqSeq) {
          this.isLoading = false
        }
      }
    },

    async create(payload) {
      await api.post('/contacts', payload)
      await this.fetch(1)
    },

    async update(id, payload) {
      await api.put(`/contacts/${id}`, payload)
      await this.fetch(this.page)
    },

    async remove(id) {
      await api.delete(`/contacts/${id}`)
      const next = this.list.length > 1 ? this.page : Math.max(1, this.page - 1)
      await this.fetch(next)
    },

    // Helpers para DataTable / Toolbar:

    setQuery(q) {
      this.q = (q || '').trim()
      return this.fetch(1)
    },

    setPage(nextPage) {
      return this.fetch(nextPage)
    },

    setPageSize(size) {
      this.pageSize = size
      return this.fetch(1)
    },

    setSort(field, order) {
      this.sortField = field || null
      this.sortOrder = order ?? 0
      return this.fetch(1)
    },
  },
})


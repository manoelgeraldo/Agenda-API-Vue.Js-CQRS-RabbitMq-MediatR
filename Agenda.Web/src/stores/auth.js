import { defineStore } from 'pinia'
import api from '../services/api'

export const useAuth = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token'),
    user: JSON.parse(localStorage.getItem('user') || 'null')
  }),
  getters: {
    isAuthenticated: (state) => !!state.token
  },
  actions: {
    async login(email, password) {
      const { data } = await api.post('/auth/login', { email, password })

      this.token = data.token
      this.user = data.user ?? null

      localStorage.setItem('token', this.token)
      if (this.user) {
        localStorage.setItem('user', JSON.stringify(this.user))
      }

      api.defaults.headers.common.Authorization = `Bearer ${this.token}`
    },
    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      delete api.defaults.headers.common.Authorization
    },
    init() {
      if (this.token) {
        api.defaults.headers.common.Authorization = `Bearer ${this.token}`
      }
    }
  }
})

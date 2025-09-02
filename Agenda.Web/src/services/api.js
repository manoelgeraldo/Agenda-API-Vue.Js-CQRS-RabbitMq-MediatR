
import axios from 'axios'

const BASE_URL =
  import.meta?.env?.VITE_API_BASE_URL?.trim?.() ||
  '/api'

const api = axios.create({
  baseURL: BASE_URL
})

let isRedirectingOn401 = false

api.interceptors.request.use((cfg) => {
  
  const isSameApi =
    !cfg.baseURL || (cfg.baseURL && String(cfg.baseURL).startsWith(BASE_URL))

  if (isSameApi) {
    const token = localStorage.getItem('token')
   
    cfg.headers = cfg.headers || {}
    
    if (token && !cfg.headers.Authorization) {
      cfg.headers.Authorization = `Bearer ${token}`
    }
  }

  return cfg
})


api.interceptors.response.use(
  (res) => res,
  async (err) => {
    const status = err?.response?.status
    const isAxiosCancel = axios.isCancel?.(err)
    if (isAxiosCancel) return Promise.reject(err)

    if (!status) {
      return Promise.reject(err)
    }

    if (status === 401 && !isRedirectingOn401) {
      isRedirectingOn401 = true
      try {

        const { useAuth } = await import('../stores/auth')
        const auth = useAuth()
        auth.logout()

        const routerMod = await import('../router')
        const router = routerMod.default

        const current = router.currentRoute.value?.fullPath || '/'

        if (router.currentRoute.value?.path !== '/login') {
          router.push({ path: '/login', query: { redirect: current } })
        }
      } catch {
          window.location.href = '/login'
      } finally {
        setTimeout(() => { isRedirectingOn401 = false }, 0)
      }
    }

    return Promise.reject(err)
  }
)

export default api

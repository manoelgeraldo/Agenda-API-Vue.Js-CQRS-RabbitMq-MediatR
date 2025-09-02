import { createRouter, createWebHistory } from 'vue-router'
import LoginView from './views/LoginView.vue'
import ContactsView from './views/ContactsView.vue'
import { useAuth } from './stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/login', component: LoginView },
    { path: '/', redirect: '/contacts' },
    { path: '/contacts', component: ContactsView, meta: { requiresAuth: true } }
  ]
})


router.beforeEach((to, from, next) => {
  const auth = useAuth()
  const isLogged = auth.isAuthenticated

  if (to.meta.requiresAuth && !isLogged) {
    return next({ path: '/login', query: { redirect: to.fullPath } })
  }

  if (to.path === '/login' && isLogged) {
    return next('/contacts')
  }

  next()
})

export default router

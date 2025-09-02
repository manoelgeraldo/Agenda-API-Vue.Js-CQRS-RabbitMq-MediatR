
import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'

import 'primeicons/primeicons.css'

import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import ToastService from 'primevue/toastservice'
import ConfirmationService from 'primevue/confirmationservice'

const app = createApp(App)
const pinia = createPinia()

app.use(PrimeVue, { theme: { preset: Aura } })
app.use(pinia)

import { useAuth } from './stores/auth'
const auth = useAuth()
auth.init()

app.use(ToastService)
app.use(ConfirmationService)
app.use(router)

app.mount('#app')


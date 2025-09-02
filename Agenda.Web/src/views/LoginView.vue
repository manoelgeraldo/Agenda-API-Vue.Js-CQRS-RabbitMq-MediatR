
<template>
  <div class="card flex justify-center">
    <Form :resolver="resolver" @submit="onFormSubmit" class="flex flex-col gap-4 w-full sm:w-64">

      <FormField v-slot="$field" name="email" initialValue="" class="flex flex-col gap-2 p-2 formField">
        <InputText v-bind="$field" type="email" placeholder="Email" :disabled="loading" />
        <Message v-if="$field?.invalid" severity="error" size="small" variant="simple">
          {{ $field.error?.message }}
        </Message>
      </FormField>

      <FormField v-slot="$field" name="password" initialValue="" class="formField">
        <section class="flex flex-col gap-2">
          <Password v-bind="$field" :feedback="false" toggleMask placeholder="Password" :disabled="loading" fluid />
          <Message v-if="$field?.invalid" severity="error" size="small" variant="simple">
            {{ $field.error?.message }}
          </Message>
        </section>
      </FormField>

      <Button type="submit" :loading="loading" label="Entrar" />
    </Form>
  </div>
</template>

<script setup>
  import { Form, FormField } from '@primevue/forms'
  import { zodResolver } from '@primevue/forms/resolvers/zod'
  import Message from 'primevue/message'
  import InputText from 'primevue/inputtext'
  import Password from 'primevue/password'
  import Button from 'primevue/button'
  import { z } from 'zod'
  import { ref } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useAuth } from '../stores/auth'
  import { useToast } from 'primevue/usetoast'

  const auth = useAuth()
  const toast = useToast()
  const route = useRoute()
  const router = useRouter()

  const loading = ref(false)

  const resolver = zodResolver(
    z.object({
      email: z.string().min(1, { message: 'Email é obrigatório.' }).email({ message: 'Email inválido.' }),
      password: z.string().min(1, { message: 'Senha é obrigatória.' })
    })
  )

  const onFormSubmit = async ({ valid, values }) => {
    if (!valid || loading.value) return
    loading.value = true
    try {
      await auth.login(values.email, values.password)
      toast.add({ severity: 'success', summary: 'Login efetuado' })
      router.push(route.query.redirect || '/contacts')
    } catch (e) {
      toast.add({ severity: 'error', summary: 'Credenciais inválidas' })
    } finally {
      loading.value = false
    }
  }
</script>

<style>
  .formField {
    padding-bottom: 10px;
  }
</style>


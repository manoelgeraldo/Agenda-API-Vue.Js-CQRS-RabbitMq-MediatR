<template>
  <Dialog v-model:visible="dialogVisible"
          :modal="true"
          :draggable="false"
          :closable="!saving"
          :dismissableMask="!saving"
          :breakpoints="{ '960px': '50vw', '640px': '90vw' }"
          :style="{ maxWidth: '90vw' }"
          :header="editing ? 'Editar Contato' : 'Novo Contato'">
    <Form @submit.prevent="submit" class="flex flex-col gap-4">
      <!-- Nome -->
      <div class="flex flex-col gap-1 pb-1">
        <label class="pr-2" for="name">Nome</label>
        <InputText id="name"
                   v-model="form.name"
                   :disabled="saving"
                   :invalid="!!errors.name"
                   placeholder="Ex.: Maria Silva" />
        <small v-if="errors.name" class="p-error">{{ errors.name }}</small>
      </div>

      <!-- E-mail -->
      <div class="flex flex-col gap-1 pb-1">
        <label class="pr-2" for="email">E-mail</label>
        <InputText id="email"
                   v-model="form.email"
                   :disabled="saving"
                   :invalid="!!errors.email"
                   placeholder="Ex.: maria@email.com" />
        <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
      </div>

      <!-- Telefone -->
      <div class="flex flex-col gap-1 pb-1">
        <label class="pr-1" for="phone">Telefone</label>
        <InputMask id="phone"
                   v-model="form.phone"
                   mask="(99) 9999-9999?9"
                   :unmask="true"
                   :disabled="saving"
                   :invalid="!!errors.phone"
                   placeholder="(81) 99999-0000" />
        <small v-if="errors.phone" class="p-error">{{ errors.phone }}</small>
      </div>

      <!-- Ações -->
      <div class="flex justify-between gap-2 mt-2">
        <Button type="button"
                label="Cancelar"
                severity="secondary"
                v-bind:disabled="saving"
                @click="dialogVisible = false" class="mr-3"/>
        <Button type="submit"
                :label="editing ? 'Salvar alterações' : 'Salvar'"
                :loading="saving" />
      </div>
    </Form>
  </Dialog>
</template>


<script setup lang="js">
  import { reactive, watch, computed } from 'vue'
  import { z } from 'zod'

  import Dialog from 'primevue/dialog'
  import InputText from 'primevue/inputtext'
  import InputMask from 'primevue/inputmask'
  import Button from 'primevue/button'

  const props = defineProps({
    modelValue: { type: Object, required: true }, // { name, email, phone }
    visible: { type: Boolean, default: false },
    editing: { type: Boolean, default: false },
    saving: { type: Boolean, default: false },
  })

  const emit = defineEmits(['save', 'update:visible'])

  // estado local
  const form = reactive({ name: '', email: '', phone: '' })
  const errors = reactive({ name: '', email: '', phone: '' })

  // sincroniza quando modelValue mudar
  watch(
    () => props.modelValue,
    (val) => {
      form.name = val?.name ?? ''
      form.email = val?.email ?? ''
      // aceita já normalizado (somente dígitos) ou formato livre
      const raw = String(val?.phone ?? '')
      form.phone = raw.replace(/\D/g, '') // mantém só dígitos no estado
      clearErrors()
    },
    { immediate: true, deep: true }
  )

  const dialogVisible = computed({
    get: () => props.visible,
    set: (v) => emit('update:visible', v),
  })

  // Zod: 10 (fixo) ou 11 (celular) dígitos com DDD
  const schema = z.object({
    name: z.string().trim().min(1, 'Nome é obrigatório'),
    email: z.string().trim().min(1, 'E-mail é obrigatório').email('E-mail inválido'),
    phone: z
      .string()
      .trim()
      .regex(/^\d+$/, 'Telefone deve conter apenas números')
      .refine((v) => v.length === 10 || v.length === 11, 'Telefone deve ter 10 ou 11 dígitos'),
  })

  function clearErrors() {
    errors.name = ''
    errors.email = ''
    errors.phone = ''
  }

  function validate() {
    clearErrors()
    const parsed = schema.safeParse({
      name: form.name,
      email: form.email,
      phone: form.phone, // já são só dígitos por causa do unmask
    })
    if (parsed.success) return true

    for (const issue of parsed.error.issues) {
      const path = issue.path?.[0]
      if (path && errors[path] === '') errors[path] = issue.message
    }
    return false
  }

  function submit() {
    if (!validate()) return
    // envia normalizado (somente dígitos)
    emit('save', {
      name: form.name.trim(),
      email: form.email.trim(),
      phone: form.phone.trim(),
    })
  }
</script>

<style>
  .pb-1 {
    padding-bottom: 10px;
  }

  .pr-1 {
    padding-right: 10px;
  }

  .pr-2 {
    padding-right: 25px;
  }

  .mr-3 {
    margin-right: 30px;
  }
</style>


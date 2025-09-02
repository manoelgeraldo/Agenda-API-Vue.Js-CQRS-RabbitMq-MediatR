<template>
  <div class="p-5">
    <!-- Toolbar -->
    <div class="flex justify-between mb-3" style="padding-bottom: 30px;">
      <div class="flex items-center gap-2">
        <Button label="Novo"
                icon="pi pi-plus"
                @click="newContact"
                v-bind:disabled="busy" style="margin-right: 20px;"/>

        <InputText v-model="search" placeHolder="Digite..." style="width: 400px;" @keyup.enter="store.setQuery(search)" />
      </div>
    </div>

    <!-- DataTable -->
    <ContactTable :items="store.list"
                  :total="store.total"
                  :page="store.page"
                  :pageSize="store.pageSize"
                  :loading="store.isLoading"
                  :sortField="store.sortField"
                  :sortOrder="store.sortOrder"
                  @page="store.setPage"
                  @sort="({ field, order }) => store.setSort(field, order)"
                  @edit="edit"
                  @remove="confirmRemove" />

    <!-- Formulário -->
    <ContactForm v-if="show"
                 :modelValue="formData"
                 :visible="show"
                 :editing="!!editing"
                 :saving="saving"
                 @save="save"
                 @update:visible="val => show = val" />

    <ConfirmDialog />
  </div>
</template>


<script setup lang="js">
  import { ref, onMounted, watch, computed } from 'vue'
  import { useContacts } from '../stores/contacts'
  import ContactForm from '../components/ContactForm.vue'
  import ContactTable from '../components/ContactTable.vue'
  import { useToast } from 'primevue/usetoast'
  import { useConfirm } from 'primevue/useconfirm'

  import Button from 'primevue/button'
  import IconField from 'primevue/iconfield'
  import InputIcon from 'primevue/inputicon'
  import InputText from 'primevue/inputtext'
  import ConfirmDialog from 'primevue/confirmdialog'

  const store = useContacts()
  const toast = useToast()
  const confirm = useConfirm()

  const show = ref(false)
  const editing = ref(null)
  const formData = ref({ name: '', email: '', phone: '' })
  const saving = ref(false)

  const search = ref(store.q ?? '')
  let searchTimer = null

  const busy = computed(() => store.isLoading || saving.value)

  onMounted(async () => {
    try {
      await store.fetch()
    } catch {
      toast.add({ severity: 'error', summary: 'Falha ao carregar contatos' })
    }
  })

  function newContact() {
    editing.value = null
    formData.value = { name: '', email: '', phone: '' }
    show.value = true
  }

  async function save(payload) {
    if (saving.value) return
    saving.value = true
    try {
      if (editing.value) {
        await store.update(editing.value, payload)
        toast.add({ severity: 'success', summary: 'Contato atualizado' })
      } else {
        await store.create(payload)
        toast.add({ severity: 'success', summary: 'Contato criado' })
      }
      show.value = false
      await store.fetch(store.page)
    } catch {
      toast.add({ severity: 'error', summary: 'Erro ao salvar' })
    } finally {
      saving.value = false
    }
  }

  function edit(row) {
    editing.value = row.id
    formData.value = { name: row.name, email: row.email, phone: row.phone }
    show.value = true
  }

  function confirmRemove(row) {
    confirm.require({
      message: `Excluir o contato "${row.name}"?`,
      header: 'Confirmar exclusão',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Excluir',
      rejectLabel: 'Cancelar',
      acceptClass: 'p-button-danger',
      accept: () => remove(row),
    })
  }

  async function remove(row) {
    try {
      await store.remove(row.id)
      toast.add({ severity: 'success', summary: 'Excluído' })
      const hasItems = store.list && store.list.length > 0
      const nextPage = hasItems ? store.page : Math.max(1, (store.page || 1) - 1)
      await store.fetch(nextPage)
    } catch {
      toast.add({ severity: 'error', summary: 'Erro ao excluir' })
    }
  }

  async function doSearch(goToFirstPage = true) {
    store.q = search.value?.trim?.() ?? ''
    try {
      await store.fetch(goToFirstPage ? 1 : store.page)
    } catch {
      toast.add({ severity: 'error', summary: 'Erro na busca' })
    }
  }

  watch(search, () => {
    if (busy.value) return
    clearTimeout(searchTimer)
    searchTimer = setTimeout(() => doSearch(true), 400)
  })

  watch(show, (v) => {
    if (!v) {
      editing.value = null
      formData.value = { name: '', email: '', phone: '' }
    }
  })
</script>



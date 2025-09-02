<template>
  <DataTable :value="items"
             :loading="loading"
             paginator
             :rows="pageSize"
             :totalRecords="total"
             :first="(page - 1) * pageSize"
             :sortField="sortField"
             :sortOrder="sortOrder"
             @page="onPage"
             @sort="onSort"
             removableSort>
    <Column field="name" header="Nome" sortable />
    <Column field="email" header="E-mail" sortable />
    <Column field="phone" header="Telefone" />
    <Column header="Ações" :exportable="false" style="width:150px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" class="p-button-text p-button-sm mr-2" @click="$emit('edit', data)" />
        <Button icon="pi pi-trash" class="p-button-text p-button-danger p-button-sm" @click="$emit('remove', data)" />
      </template>
    </Column>
  </DataTable>
</template>


<script setup>
  import DataTable from 'primevue/datatable'
  import Column from 'primevue/column'
  import Button from 'primevue/button'

  const props = defineProps({
    items: Array,
    total: Number,
    page: Number,
    pageSize: Number,
    loading: Boolean,
    sortField: { type: String, default: null },
    sortOrder: { type: Number, default: 0 }, // 1 | -1 | 0
  })

  const emit = defineEmits(['page', 'sort', 'edit', 'remove'])

  function onPage(e) {
    emit('page', e.page + 1)
  }
  function onSort(e) {
    emit('sort', { field: e.sortField, order: e.sortOrder })
  }
</script>

<template>
  <input
    class="form-control"
    ref="inputRef"
    :value="formattedValue"
  >
</template>

<script>
import { defineComponent } from 'vue'
import useCurrencyInput from 'vue-currency-input'

export default defineComponent({
  name: 'CurrencyInput',
  props: {
    modelValue: Number
  },
  setup () {
    const { formattedValue, inputRef } = useCurrencyInput({ currency: 'USD', currencyDisplay: 'hidden', precision: 2, valueRange: { min: 0, max: 9999 }, autoDecimalDigits: true })

    return { inputRef, formattedValue }
  }
})
</script>

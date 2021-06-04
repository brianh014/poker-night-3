<template>
  <div class="row">
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Gray.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">1&#162;</span>
        <input v-model="gray" @change="emitTotal()" v-on:keyup="gray = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Gray 1 cent">
      </div>
    </div>
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Red.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">5&#162;</span>
        <input v-model="red" @change="emitTotal()" v-on:keyup="red = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Red 5 cent">
      </div>
    </div>
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Green.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">25&#162;</span>
        <input v-model="green" @change="emitTotal()" v-on:keyup="green = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Green 25 cent">
      </div>
    </div>
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Black.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">100&#162;</span>
        <input v-model="black" @change="emitTotal()" v-on:keyup="black = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Black 100 cent">
      </div>
    </div>
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Purple.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">500&#162;</span>
        <input v-model="purple" @change="emitTotal()" v-on:keyup="purple = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Purple 500 cent">
      </div>
    </div>
    <div class="col-6 col-md-4 text-center">
      <div class="chip-image">
        <img src="@/assets/Poker_Chip_Yellow.svg" />
      </div>
      <div class="input-group chip-input">
        <span class="input-group-text">1000&#162;</span>
        <input v-model="yellow" @change="emitTotal()" v-on:keyup="yellow = $event.target.value; emitTotal()" type="number" class="form-control" aria-label="Yellow 1000 cent">
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'ChipCounter',
  data () {
    return {
      gray: 0,
      red: 0,
      green: 0,
      black: 0,
      purple: 0,
      yellow: 0
    }
  },
  methods: {
    total (): number {
      return this.gray * 0.01 + this.red * 0.05 + this.green * 0.25 + this.black * 1 + this.purple * 5 + this.yellow * 10;
    },
    emitTotal () {
      this.$emit('newTotal', this.total());
    }
  }
})
</script>

<style lang="scss" scoped>
.chip-image {
  padding: 0px 30px;
}
.chip-input {
  max-width: 130px;
  margin: auto;
  padding: 10px 0px 20px 0px;
  input {
    text-align: center;
  }
}
</style>

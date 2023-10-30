import { defineStore } from 'pinia'

export const useUTubeStore = defineStore('utube', {
  state: () => {
    return {
      drawer: true,
      drawerItems: [
        { title: 'Home', value: 'home', props: {"prepend-icon": "mdi-home-city", rounded: "xl" }},
        { title: 'History', value: 'home', props: {"prepend-icon": "mdi-history", rounded: "xl" }},
        { title: 'Watch Later', value: 'home', props: {"prepend-icon": "mdi-clock-outline", rounded: "xl" }},
        { title: 'Liked Videos', value: 'home' , props: {"prepend-icon": "mdi-thumb-up", rounded: "xl" }},
        { type: 'divider' },
        { title: 'Settings', value: 'home', props: {"prepend-icon": "mdi-cog", rounded: "xl" }}
      ]
    }
  },
  getters: {
    isDrawerOpen() {
      return this.drawer;
    }
  },
  actions: {
    toggleDrawer() {
      this.drawer = !this.drawer;
    }
  }
})

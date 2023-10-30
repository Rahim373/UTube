<script>
import { RouterView } from 'vue-router'
import { useUTubeStore } from '../stores/uTubeStore';

export default {
    name: "layout-component",
    data: () => ({
        drawer: true,
        rail: true,
    }),
    setup() {
        return {
            store: useUTubeStore()
        }
    },
    components: [
        RouterView,
    ],
    watch: {
        rail(newVal) {
            this.store.toggleDrawer(newVal)
        },
    }
}

</script>

<template>
    <v-app>
        <v-layout>
            <v-app-bar style="position:fixed" elevation="0" >
                <v-app-bar-nav-icon variant="text" @click.stop="rail = !rail"></v-app-bar-nav-icon>
                <v-toolbar-title>uTube</v-toolbar-title>
                <v-spacer></v-spacer>
                <v-btn variant="text" icon="mdi-magnify"></v-btn>
                <v-btn variant="text" icon="mdi-filter"></v-btn>
                <v-btn variant="text" icon="mdi-dots-vertical"></v-btn>
            </v-app-bar>

            <v-navigation-drawer :rail="rail" v-model="drawer" style="position:fixed" location="left" permanent>
                <v-list :items="store.drawerItems"></v-list>
            </v-navigation-drawer>

            <v-main>
                <v-content>
                    <RouterView class="padding"></RouterView>
                </v-content>
            </v-main>
        </v-layout>

    </v-app>
</template>

<style>
.padding {
    padding: 16px;
}

.fixed-bar {
    position: sticky;
    position: -webkit-sticky; /* for Safari */
    top: 6em;
    z-index: 2;
  }
</style>
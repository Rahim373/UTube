<template>
    <div class="card-root" :style="{ width: width + 'px' }">
        <v-card :width="width">
            <v-img :height="height" :src="thumbnail" cover class="text-white">
                <div class="duration">{{ duration }}</div>
            </v-img>

        </v-card>
        <div class="card-detail">
            <v-list>
                <v-list-item :title="title" :subtitle="channel" :prepend-avatar="avatar">
                    <v-list-item-subtitle>{{ views }} views • {{ uploaded }}</v-list-item-subtitle>
                </v-list-item>
            </v-list>
        </div>
    </div>
</template>

<style>
.card-root {
    cursor: pointer;
}

.duration {
    background-color: black;
    width: fit-content;
    padding: 1px 5px;
    font-size: small;
    border-radius: 5px;
    position: absolute;
    bottom: .5em;
    right: .5em;
}
</style>

<script>

import { useUTubeStore } from '../../stores/uTubeStore';

export default {
    name: 'large-card-component',
    props: ['duration', 'title', 'channel', 'avatar', 'views', 'uploaded', 'thumbnail'],
    setup() {
        return {
            store: useUTubeStore()
        }
    },
    computed: {
        width() {
            return this.store.isDrawerOpen ? 338 : 400;
        },
        height() {
            return this.store.isDrawerOpen ? 190 : 225;
        }
    }
}
</script>

<template>
    <VideoPlayer controls
    src="http://localhost:30011/utube-bucket/b389be12-8b0a-48b2-a0f5-442f4eca090e/temp/b389be12-8b0a-48b2-a0f5-442f4eca090e.m3u8"
    displayCurrentQuality="true"
    type="application/x-mpegURL">
    </VideoPlayer>
</template>

<script>
import 'video.js/dist/video-js.css'
import { VideoPlayer } from '@videojs-player/vue'

// https://demo.unified-streaming.com/k8s/features/stable/video/tears-of-steel/tears-of-steel.ism/.m3u8
export default {
    name: 'video-detail-view',
    data() {
        return {
            video: {}
        }
    },
    components: {
        VideoPlayer
    },
    methods: {

    },
    computed: {

    },
    created() {
    }
}
</script>

<style>
.customClassName {
    width: 60vw;
}
</style>
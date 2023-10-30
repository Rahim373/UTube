<template>
    <div>
        <v-infinite-scroll :height="600" :items="videos" :onLoad="getData">
            <div class="video-grid" :style="{ gridTemplateColumns: gridColumnTemplate }">
                <LargeCard v-for="video in videos" :key="video.id" :title="video.title" :uploaded="video.uploaded"
                    :channel="video.channel" :views="video.views" :thumbnail="video.thumbnail" :avatar="video.avatar"
                    :duration="video.duration" @click="goToVideo(video.id)"></LargeCard>
            </div>
        </v-infinite-scroll>
    </div>
</template>

<script>
import LargeCard from '../components/videoCards/LargeCard.vue';
import { useUTubeStore } from '../stores/uTubeStore';
import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
dayjs.extend(relativeTime);

export default {
    name: 'Home-view',
    setup() {
        return {
            store: useUTubeStore()
        }
    },
    data() {
        return {
            videos: []
        }
    },
    metaInfo: {
        title: 'uTube | Home'
    },
    components: {
        LargeCard
    },
    methods: {
        goToVideo(id) {
            console.log(id)
            if (id) {

                this.$router.push(`/watch?v=${id}`);
            }
        },
        async getData() {
            var response = await fetch('https://localhost:53973/api/videos?pageLength=50&pageNumber=1');
            let data = await response.json();
            this.videos = [...this.videos, ...data.map(i => {
                return {
                    id: i.videoId,
                    avatar: i.avatar,
                    title: i.title,
                    thumbnail: i.defaultThumbnail,
                    channel: i.channel,
                    uploaded: dayjs(i.uploadedOn).fromNow(),
                    views: i.views,
                    duration: i.duration
                }
            })];
        }
    },
    computed: {
        gridColumnTemplate() {
            return 'auto auto auto' + (this.store.isDrawerOpen ? ' auto' : '');
        }
    },
    created() {
        this.getData();
    }
}
</script>

<style>
.video-grid {
    display: grid;
    row-gap: 32px;
}
</style>
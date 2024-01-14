export interface Video {
    id: string
    videoId: string
    uploadedOn: string
    title: string
    description: string
    tags: any
    defaultThumbnail: string
    avatar: string
    channel: string
    thumbnails: Array<string>
    playlist: string
    duration: string
    views: number,
    subTitles?: string
}

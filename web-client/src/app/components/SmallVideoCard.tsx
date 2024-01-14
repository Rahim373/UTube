import Link from "next/link";
import { Video } from "../models/Video";
import VideoInfo from "./VideoInfo";
import { ApiRoutes } from "../constants/Api";

export default function LargeVideoCard({video} : {video: Video}) {
  const thumbnail = "https://picsum.photos/32";
  const poster = `${ApiRoutes.Storage}/${video.defaultThumbnail}`;

  return (
    <Link href={`/video/${encodeURIComponent(video.videoId)}`}>
      <div className="flex flex-row items-stretch gap-2">
        <div className="basis-2/5">
          <div
            className="thumbnail border border-gray-200 rounded-xl"
            style={{ backgroundImage: `url(${poster})` }}
          ></div>
        </div>
        <div className="basis-3/5">
          <VideoInfo
            title={video.title}
            channel={video.channel}
            channelThumbnail={thumbnail}
            views={video?.views}
            uploadedOn={video.uploadedOn}
          />
        </div>
      </div>
    </Link>
  );
}

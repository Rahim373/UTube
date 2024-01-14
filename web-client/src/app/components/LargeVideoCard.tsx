import Link from "next/link";
import { Video } from "../models/Video";
import * as dayjs from "dayjs";
import VideoInfo from "./VideoInfo";
import { ApiRoutes } from "../constants/Api";

import relativeTime from "dayjs/plugin/relativeTime";
dayjs.extend(relativeTime);

export default function LargeVideoCard(props: any) {
  const thumbnail = "https://picsum.photos/32";
  const video: Video = props.video;
  const poster = `${ApiRoutes.Storage}/${video.defaultThumbnail}`;

  return (
    <Link href={`/video/${encodeURIComponent(video.videoId)}`}>
      <div className="">
        <div
          className="thumbnail border border-gray-200 rounded-xl"
          style={{ backgroundImage: `url(${poster})` }}
        ></div>
        <div className="mt-3">
          <VideoInfo
            title={video?.title}
            channel={video?.channel}
            channelThumbnail={thumbnail}
            views={video?.views}
            uploadedOn={video?.uploadedOn}
          />
        </div>
      </div>
    </Link>
  );
}

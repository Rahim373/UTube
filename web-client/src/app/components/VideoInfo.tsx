import VideoTitle from "./VideoTitle";
import dayjs from "dayjs";
import Views from "./Views";
import relativeTime from "dayjs/plugin/relativeTime";
dayjs.extend(relativeTime);

export default function VideoInfo({
  title,
  channel,
  channelThumbnail,
  views,
  uploadedOn,
}: {
  title: string;
  channel: string;
  channelThumbnail: string;
  views: number;
  uploadedOn: string;
}) {
  return (
    <div className="flex flex-row">
      <div
        className="channel-thumbnail flex-shrink-0"
        style={{ backgroundImage: `url(${channelThumbnail})` }}
      ></div>
      <div>
        <VideoTitle>{title}</VideoTitle>
        <span className="channel-info">{channel}</span>
        <br />
        <span className="channel-info">
          <Views views={views} /> . {dayjs(uploadedOn).fromNow()}
        </span>
      </div>
    </div>
  );
}

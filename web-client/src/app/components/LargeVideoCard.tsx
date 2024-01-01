import VideoTitle from "./VideoTitle";

export default function LargeVideoCard() {
  const poster = "https://picsum.photos/300/200";
  const thumbnail = "https://picsum.photos/32";

  return (
    <div className="">
      <div
        className="thumbnail"
        style={{ backgroundImage: `url(${poster})` }}
      ></div>
      <div className="mt-3 flex flex-row">
        <div
          className="channel-thumbnail flex-shrink-0"
          style={{ backgroundImage: `url(${thumbnail})` }}
        ></div>
        <div>
          <VideoTitle>
            নির্বাচনে আইনশৃঙ্খলা বাহিনীর চাহিদা ১ হাজার ৭১ কোটি টাকা! | National
            Election | Ekhon TV
          </VideoTitle>
          <span className="channel-info">Test channel</span>
          <br />
          <span className="channel-info">4.5k views . 2 hours ago</span>
        </div>
      </div>
    </div>
  );
}

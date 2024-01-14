"use client";

import { Video } from "@/app/models/Video";
import { useEffect, useState } from "react";
import ReactHlsPlayer from "@ducanh2912/react-hls-player";
import VideoTitle from "@/app/components/VideoTitle";
import Button from "@/app/components/Button";
import dayjs from "dayjs";
import Views from "@/app/components/Views";
import VerticalVideoList from "@/app/components/VerticalVideoList";
import { ApiRoutes } from "@/app/constants/Api";

import localizedFormat from "dayjs/plugin/localizedFormat";
dayjs.extend(localizedFormat);

export default function Page({ params }: { params: { id: string } }) {
  const [video, setVideo] = useState<Video>();

  const thumbnail = "https://picsum.photos/32";

  useEffect(() => {
    const getData = async () => {
      const response = await fetch(`${ApiRoutes.Videos}/${params.id}`);
      const data: Video = await response.json();
      setVideo(data);
    };

    getData().catch((e) => console.error(e));
  }, []);

  return (
    <div className="flex ">
      {video && (
        <div className="w-4/6 pr-4">
          <ReactHlsPlayer
            className="rounded-2xl"
            src={`${ApiRoutes.Storage}/${video?.playlist}`}
            width="100%"
            autoPlay={false}
            controls={true}
          />

          <div className="mt-4">
            <VideoTitle $largeTitle>{video?.title}</VideoTitle>
            <div className="mt-3 flex flex-row">
              <div
                className="channel-thumbnail-lg flex-shrink-0"
                style={{ backgroundImage: `url(${thumbnail})` }}
              ></div>
              <div>
                <VideoTitle>{video?.channel}</VideoTitle>
                <span className="channel-info">231k subscriber</span>
              </div>
              <div className="mx-8">
                <Button $highlight>Subscribe</Button>
              </div>
            </div>
          </div>

          <div className="mt-4 bg-gray-200 p-4 rounded-2xl text-sm">
            <p className="font-semibold mb-4">
              <span>
                <Views views={video.views} />
              </span>
              <span className="ml-2">
                {dayjs(video?.uploadedOn).format("ll")}
              </span>
            </p>
            <p className="whitespace-pre-wrap">{video?.description}</p>
          </div>
        </div>
      )}
      <div className="w-2/6 pl-4">
        <VerticalVideoList />
      </div>
    </div>
  );
}

"use client";

import SmallVideoCard from "./SmallVideoCard";
import useGridColumnCount from "../hooks/useGridColumnCount";
import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import { ApiRoutes } from "../constants/Api";

export default function VerticalVideoList() {
  var colCount = useGridColumnCount();
  const [videos, setVideos] = useState<Array<Video>>([]);

  useEffect(() => {
    const fetchData = async () => {
      var response = await fetch(ApiRoutes.Videos);
      var data = await response.json();
      setVideos([...videos, ...data]);
    }

    fetchData().catch(e => console.error(e));
  }, []);

  return (
    <div>
      <div className="flex flex-col gap-4">
        { videos.map((video, index) => <SmallVideoCard video={video} key={index} />) }
      </div>
    </div>
  );
}

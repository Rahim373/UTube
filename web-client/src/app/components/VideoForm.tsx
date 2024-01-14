import { useEffect, useState } from "react";
import { Video } from "../models/Video";
import Input from "./Input";
import ThumbnailSelector from "./ThumbnailSelector";
import { ApiRoutes } from "../constants/Api";

export default function VideoForm({
  videoId,
}: {
  videoId: string | undefined;
}) {
  const [video, setVideo] = useState<Video>();

  const getVideo = async (callback: any) => {
    const response = await fetch(
      `${ApiRoutes.Videos}/${videoId}`
    );
    const data: Video = await response.json();
    if (!data.thumbnails) {
        data.thumbnails = [];
    }
    data.thumbnails.push(data.defaultThumbnail);
    data.thumbnails.push("0dca03af-7215-4b63-8e4d-3654203dd77b/thumbnail/04742f06-8ca1-4d77-8816-42f7916d8146.png");
    data.thumbnails.push("aa76919d-d479-4c6e-89e4-0542d24037a8/thumbnail/225dedf8-d399-4d09-8c67-23033b35d491.png");
    data.thumbnails.push("7bacdb33-149b-488a-8f42-ac9f98c51661/thumbnail/eed95e97-40a4-4a0b-96a2-e01cd9c6a737.png");

    callback(data);
  };

  useEffect(() => {
    getVideo((data: Video) => setVideo(data)).catch((err) =>
      console.error(err)
    );
  }, [videoId]);

  useEffect(() => {
    const interval = setInterval(() => {
      getVideo((data: Video) => setVideo(data)).catch((err) =>
        console.error(err)
      );
    }, 5000);

    return () => {
      clearInterval(interval);
    };
  }, [videoId]);

  const onThumbnailSelected = (thumbnail: string) => {

  };

  return (
    <div>
      <div>
        <Input name="title" label="Video Title" value={video?.title} />
        <Input
          as="textarea"
          name="description"
          label="Video Desciption"
          value={video?.description}
        />
        <Input name="tags" label="Tags" value={video?.tags} />
        <Input
          name="duration"
          disabled
          label="Duration"
          value={video?.duration}
        />

        <ThumbnailSelector
          defaultThumbnail={video?.defaultThumbnail}
          thumbnails={video?.thumbnails}
          onChange={onThumbnailSelected}
        />
      </div>
    </div>
  );
}

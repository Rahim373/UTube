"use client";

import { useState } from "react";
import VideoUploader from "./VideoUploader";
import VideoUploadResponse from "../../models/VideoUploadResponse";
import VideoForm from "@/app/components/VideoForm";

export default function Upload() {
  const [fileUploaded, setFileUploaded] = useState<boolean>(false);
  const [videoId, setVideoId] = useState<string>();

  const onVideoUploaded = (
    uploaded: boolean,
    uploadResponse: VideoUploadResponse
  ): void => {
    setFileUploaded(uploaded);
    setVideoId(uploadResponse?.videoId);
  };

  return (
    <div className="w-11/12 mx-auto">
      <section>
        <VideoUploader
          enabled={fileUploaded}
          onVideoUploaded={onVideoUploaded}
        />
      </section>

      <section>{fileUploaded && <VideoForm videoId={videoId} />}</section>
    </div>
  );
}

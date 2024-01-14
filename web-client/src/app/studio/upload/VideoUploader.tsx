"use client";

import { FilePond } from "react-filepond";
import "filepond/dist/filepond.min.css";
import { useState } from "react";
import { ApiRoutes } from "@/app/constants/Api";
import { ActualFileObject, FilePondFile, FilePondInitialFile } from "filepond";
import VideoUploadResponse from "@/app/models/VideoUploadResponse";

export default function VideoUploader({
  enabled,
  onVideoUploaded,
}: {
  enabled: boolean;
  onVideoUploaded: (enabled: boolean, videoId: VideoUploadResponse) => void;
}) {
  const [files, setFiles] = useState<Array<ActualFileObject>>([]);

  const handleUpdateFiles = (fileItems: FilePondFile[]) => {
    setFiles(
      fileItems.map((fileItem: FilePondFile): ActualFileObject => fileItem.file)
    );
  };

  const handleProcessFile = async (
    fieldName: string,
    file: Blob,
    metadata: { [key: string]: any },
    load: (p: string | { [key: string]: any }) => void,
    error: (errorText: string) => void
  ) => {
    try {
      const formData = new FormData();
      formData.append("file", file);

      var myHeaders = new Headers();
      myHeaders.append("Content-Type", "multipart/form-data");
      myHeaders.append("Accept", "text/plain");

      var response = await fetch(ApiRoutes.FileUpload, {
        method: "POST",
        body: formData,
      });
      var data = await response.json();
      load(data.videoId);

      onVideoUploaded(true, data);
    } catch (err) {
      console.error(err);
      error("Upload failed");
    }
  };

  return (
    <FilePond
      files={files}
      onupdatefiles={handleUpdateFiles}
      allowMultiple={false}
      maxFiles={1}
      server={{
        process: handleProcessFile,
      }}
      allowRevert={false}
      credits={false}
      labelIdle='Drag & Drop your files or <span class="filepond--label-action">Browse</span>'
      disabled={enabled}
    />
  );
}

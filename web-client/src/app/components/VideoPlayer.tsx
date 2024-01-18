"use client";

import ReactHlsPlayer from "@ducanh2912/react-hls-player";

export default function VideoPlayer({ src }: { src: string }) {
  return (
    <ReactHlsPlayer
      className="rounded-2xl"
      src={src}
      width="100%"
      autoPlay={false}
      controls={true}
    />
  );
}

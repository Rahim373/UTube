"use client";

import { useEffect, useRef } from "react";
import videojs from "video.js";
import "video.js/dist/video-js.css";
import options from "../api/auth/[...nextauth]/options";
import Player from "video.js/dist/types/player";

export default function VideoPlayer({ src }: { src: string }) {
  const videoRef = useRef<HTMLDivElement>(null);
  const playerRef = useRef<Player | null>(null);

  useEffect(() => {
    if (!playerRef.current) {
      const options = {
        autoplay: true,
        controls: true,
        responsive: true,
        disablePictureInPicture: false,
        fluid: true,
        playbackRates: [0.25, 0.5, 1, 1.5, 2],
        sources: [
          {
            src: src,
            type: "application/x-mpegURL",
          },
        ],
        experimentalSvgIcons: true,
        controlBar: {
          skipButtons: {
            forward: 5,
            backward: 5,
          },
        },
      };

      const videoElement = document.createElement("video-js");
      videoElement.classList.add("vjs-big-play-centered");
      videoRef.current?.appendChild(videoElement);

      const player = videojs(videoElement, options, () => {
        videojs.log("Player ready");
      });
      playerRef.current = player;
    }
  }, [options, videoRef]);

  useEffect(() => {
    const player = playerRef.current;

    return () => {
      if (player && !player.isDisposed()) {
        player.dispose();
        playerRef.current = null;
      }
    };
  }, [playerRef]);

  return (
    <div data-vjs-player>
      <div ref={videoRef} className="rounded-lg" />
    </div>
  );
}

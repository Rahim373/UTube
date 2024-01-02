"use client";

import { useState, useEffect } from "react";
import { WindowDimension } from "../models/WindowDimension";

function getWindowDimensions(): WindowDimension {
  let innerHeight = 0;
  let innerWidth = 0;

  if (typeof window !== "undefined") {
    innerWidth = window.innerWidth;
    innerHeight = window.innerHeight;
  }

  return {
    height: innerHeight,
    width: innerWidth,
  };
}

export default function useWindowDimentions(): WindowDimension {
  const [dimension, setDimension] = useState(getWindowDimensions());

  useEffect(() => {
    function handleResize() {
      setDimension(getWindowDimensions());
    }

    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return dimension;
}

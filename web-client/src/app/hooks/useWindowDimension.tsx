"use client";

import { useState, useEffect } from "react";
import { WindowDimension } from "../models/WindowDimension";

function getWindowDimensions(): WindowDimension {
  const { innerWidth, innerHeight } = window;
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

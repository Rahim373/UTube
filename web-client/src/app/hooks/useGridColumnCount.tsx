"use client";

import { useEffect, useState } from "react";
import useWindowDimentions from "./useWindowDimension";

export default function useGridColumnCount(): number {
  const [colCount, setColCount] = useState(0);
  const { width } = useWindowDimentions();

  useEffect(() => {
    if (width <= 790) {
      setColCount(1);
    } else if (width <= 900) {
      setColCount(2);
    } else if (width <= 1920) {
      setColCount(3);
    } else if (width <= 2500) {
      setColCount(4);
    } else {
      setColCount(5);
    }
  }, [width]);

  return colCount;
}

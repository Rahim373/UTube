"use client";

import LargeVideoCard from "./LargeVideoCard";
import useGridColumnCount from "../hooks/useGridColumnCount";

export default function VideoGrid() {
  var colCount = useGridColumnCount();

  return (
    <div>
      <div className={`grid grid-cols-${colCount} gap-4`}>
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
        <LargeVideoCard />
      </div>
    </div>
  );
}

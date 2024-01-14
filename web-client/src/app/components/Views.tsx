"use client";

export default function Views({ views }: { views: number }) {
  return (
    <span>
      {views} {views > 1 ? "views" : "view"}
    </span>
  );
}

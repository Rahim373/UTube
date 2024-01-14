import { useState } from "react";
import { ApiRoutes } from "../constants/Api";

export default function ThumbnailSelector({
  defaultThumbnail,
  thumbnails,
  onChange,
}: {
  defaultThumbnail?: string;
  thumbnails?: string[];
  onChange: any;
}) {
  const [selectedThumb, setSelectedThumb] = useState<string | undefined>(
    defaultThumbnail
  );
  const selectThumb = (src: string) => {
    if (selectedThumb != src) {
      setSelectedThumb(src);
      onChange(src);
    }
  };

  return (
    <div>
      <label>Thumbnail</label>
      <div className="flex gap-4 mt-2">
        {thumbnails?.map((src, index) => {
          const isSelected = src.toLowerCase() == selectedThumb?.toLowerCase();
          const selectedCss = isSelected
            ? "border-red-500 shadow-md"
            : "border-gray-200";

          return (
            <div
              key={index}
              className="w-48 relative"
              onClick={(e) => selectThumb(src)}
            >
              <div
                className={`thumbnail border absolute rounded-xl ${selectedCss}`}
                style={{
                  backgroundImage: `url(${ApiRoutes.Storage}/${src})`,
                }}
              >
                {isSelected && (
                  <div className="shadow-md absolute w-full bottom-0 z-10">
                    <p className="text-white text-center font-bold uppercase text-sm bg-red-500 py-2">
                      Selected
                    </p>
                  </div>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

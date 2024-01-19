import { Video } from "@/app/models/Video";
import VideoTitle from "@/app/components/VideoTitle";
import Button from "@/app/components/Button";
import dayjs from "dayjs";
import Views from "@/app/components/Views";
import VerticalVideoList from "@/app/components/VerticalVideoList";
import { ApiRoutes } from "@/app/constants/Api";
import localizedFormat from "dayjs/plugin/localizedFormat";
import VideoPlayer from "@/app/components/VideoPlayer";
import { Metadata, ResolvingMetadata } from "next";
dayjs.extend(localizedFormat);

/**
 Returns video information
 * @param id Video id
 * @returns Video object
 */
const getVideo = async (id: string): Promise<Video> => {
  const response = await fetch(`${ApiRoutes.Videos}/${id}`);
  const data: Video = await response.json();
  return data;
};

/**
 * Generate the metadata for the current page
 * @param params params
 * @param parent parent
 * @returns Metadata
 */
export async function generateMetadata(
  { params, searchParams }: Props,
  parent: ResolvingMetadata
): Promise<Metadata> {
  const id = params.id;
  // fetch data
  const video = await getVideo(id);
  const previousImages = (await parent).openGraph?.images || [];

  return {
    title: video.title,
    openGraph: {
      images: [
        `${ApiRoutes.Storage}/${video.defaultThumbnail}`,
        ...previousImages,
      ],
    },
  };
}

export default async function Page(props: any) {
  const id: string = props.params.id;
  console.log(id);
  const video = await getVideo(id);
  const thumbnail = "https://picsum.photos/32";

  return (
    <div className="flex ">
      {video && (
        <div className="w-4/6 pr-4">
          <VideoPlayer
            src={`${ApiRoutes.Storage}/${video?.playlist}`}
          ></VideoPlayer>

          <div className="mt-4">
            <VideoTitle $largeTitle>{video?.title}</VideoTitle>
            <div className="mt-3 flex flex-row">
              <div
                className="channel-thumbnail-lg flex-shrink-0"
                style={{ backgroundImage: `url(${thumbnail})` }}
              ></div>
              <div>
                <VideoTitle>{video?.channel}</VideoTitle>
                <span className="channel-info">231k subscriber</span>
              </div>
              <div className="mx-8">
                <Button $highlight>Subscribe</Button>
              </div>
            </div>
          </div>

          <div className="mt-4 bg-gray-200 p-4 rounded-2xl text-sm">
            <p className="font-semibold mb-4">
              <span>
                <Views views={video.views} />
              </span>
              <span className="ml-2">
                {dayjs(video?.uploadedOn).format("ll")}
              </span>
            </p>
            <p className="whitespace-pre-wrap">{video?.description}</p>
          </div>
        </div>
      )}
      <div className="w-2/6 pl-4">
        <VerticalVideoList />
      </div>
    </div>
  );
}

import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import TopBar from "./components/TopBar";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "uTube",
  description: "A self video hosting platform",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body suppressHydrationWarning={true} className={inter.className}>
        <div>
          <div className="absolute h-14 w-full top-0 border-b border-gray-100">
            <TopBar />
          </div>
          <div className="absolute top-14 w-full h-[calc(100%-56px)] flex">
            <div className="bg-white w-full overflow-y-auto max-h-full p-4">
              <div className="max-w-screen-2xl content-around m-auto">
                {children}
              </div>
            </div>
          </div>
        </div>
      </body>
    </html>
  );
}

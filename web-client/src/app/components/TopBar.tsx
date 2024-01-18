import Image from "next/image";
import logo from "../../assets/img/utube-logo.png";
import Link from "next/link";
import Logout from "./Logout";
import { getServerSession } from "next-auth";
import options from "../api/auth/[...nextauth]/options";

export default async function TopBar() {
  const session = await getServerSession(options);

  return (
    <div className="flex items-center content-stretch justify-between h-14 px-4">
      <div className="align-middle">
        <Link href="/">
          <Image src={logo} width={98} alt="uTube Logo" priority={false} />
        </Link>
      </div>
      <div>
        <input
          className="py-2 px-5 shadow-inner outline-none w-96 border border-gray-200 rounded-full"
          placeholder="Search"
        />
      </div>
      <div>
        <Link href="/studio/upload">Upload Video</Link>
        {session && <Logout></Logout>}
      </div>
    </div>
  );
}

import Image from "next/image";
import logo from "../../assets/img/utube-logo.png";

export default function TopBar(): any {
  return (
    <div className="flex items-center content-stretch justify-between h-14 px-4">
      <div className="align-middle">
        <Image src={logo} width={98} alt="uTube Logo" priority={false} />
      </div>
      <div>
        <input
          className="py-2 px-5 shadow-inner outline-none w-96 border border-gray-200 rounded-full"
          placeholder="Search"
        />
      </div>
      <div>right</div>
    </div>
  );
}

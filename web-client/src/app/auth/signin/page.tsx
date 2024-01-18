"use client";
import { signIn, useSession } from "next-auth/react";
import { useEffect } from "react";
import { useRouter, useSearchParams } from "next/navigation";

export default function Signin() {
  const router = useRouter();
  const { status } = useSession();
  const params = useSearchParams();

  useEffect(() => {
    console.log(status);

    if (status === "unauthenticated") {
      console.log("No JWT");
      console.log(status);
      void signIn("webclient");
    } else if (status === "authenticated") {
      void router.push(params?.get("callbackUrl") || "/");
    }
  }, [status]);

  return <div></div>;
}

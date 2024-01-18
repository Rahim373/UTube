"use client";

import { signOut } from "next-auth/react";
import Button from "./Button";

export default function Logout() {
  return (
    <Button
      $highlight
      onClick={(e) => {
        signOut();
      }}
    >
      Logout
    </Button>
  );
}

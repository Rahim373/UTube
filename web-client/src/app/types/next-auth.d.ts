import { DefaultSession } from 'next-auth';
import NextAuth from 'next-auth/next';

declare module 'next-auth' {
    interface Session extends DefaultSession {
        access_token: any & DefaultSession["user"];
    }

    interface User {
        access_token: any
        & DefaultSession["user"];
    }
}


declare module "next-auth/jwt" {
    /** Returned by the `jwt` callback and `getToken`, when using JWT sessions */
    interface JWT {
        /** OpenID ID Token */
        access_token?: string;
    }
}

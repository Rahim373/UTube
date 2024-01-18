import type { Awaitable, DefaultSession, NextAuthOptions, Session, User } from "next-auth"
import { JWT } from "next-auth/jwt";
import IdentityServer4 from "next-auth/providers/identity-server4"

const options: NextAuthOptions = {
    // pages: {
    //     signIn: "/auth/signin"
    // },
    providers: [
        IdentityServer4({
            id: "webclient",
            name: "Identity Server",
            authorization: { params: { scope: "openid profile video-service.full-access" } },
            issuer: "http://localhost:53901",
            clientId: "webclient",
            clientSecret: "d7f02357090218c43f6e381745189aeb4ff9ca8e0e65499f3b740e2c51ef2aff",
        }),
    ],
    secret: "yRrRw7CAZGHGA/R/1o/C+MT+FoQnOq/xsmO/njMTt60=",
    debug: true,
    callbacks: {
        jwt: async ({ token, account }) => {
            if (account?.access_token) {
                token.access_token = account?.access_token;
            }
            return token;
        },
        session: async ({ session, token }) => {
            if (token) {
                session.access_token = token.access_token;
            }
            session.access_token = token.access_token;
            return Promise.resolve(session);
        },
    }
}

export default options;

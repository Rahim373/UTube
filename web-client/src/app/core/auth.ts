import NextAuth from 'next-auth';
import IdentityServer4Provider from "next-auth/providers/identity-server4";

const authOptions = {
    providers: [
        IdentityServer4Provider({
            id: "identity-server4",
            name: "IdentityServer4",
            issuer: "http://localhost:53901",
            clientId: "webclient",
            clientSecret: "d7f02357090218c43f6e381745189aeb4ff9ca8e0e65499f3b740e2c51ef2aff"
        }),
    ]
};

export default NextAuth(authOptions);

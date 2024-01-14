/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        remotePatterns: [
            {
                protocol: "https",
                hostname: "picsum.photos"
            }
        ]
    },
    compiler: {
        styledComponents: true
    },
    output: 'standalone'
}

module.exports = nextConfig

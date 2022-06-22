self.assetsInclude = [];
self.assetsExclude = [/\.scp\.css$/];
self.defaultUrl = '/';
self.prohibitedUrls = [];
self.assetsUrl = '/service-worker-assets.js';

// more about SRI (Subresource Integrity) here: https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity
// online tool to generate integrity hash: https://www.srihash.org/   or   https://laysent.github.io/sri-hash-generator/
// using only js to generate hash: https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/digest
self.externalAssets = [
    {
        "url": "/"
    },
];

self.importScripts('_content/Bit.Tooling.Bswup/bit-bswup.sw.js');
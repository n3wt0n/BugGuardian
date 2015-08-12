using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Extensions
{
    internal static class HttpClientExtensions
    {
        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
            => await client.PatchAsync(new Uri(requestUri), content, CancellationToken.None);

        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
            => await client.PatchAsync(requestUri, content, CancellationToken.None);

        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
            => await client.PatchAsync(new Uri(requestUri), content, cancellationToken);

        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            if (cancellationToken == CancellationToken.None)
                return await client.SendAsync(request);

            return await client.SendAsync(request, cancellationToken);
        }
    }
}

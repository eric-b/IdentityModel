using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdentityModel.Client
{
    public static class HttpRequestMessageTokenExtensions
    {
        public static void SetTokenRequest(this HttpRequestMessage httpRequest, TokenRequest request)
        {
            if (!request.Parameters.TryGetValue(OidcConstants.TokenRequest.ClientId, out _))
            {
                if (request.ClientId.IsMissing()) throw new InvalidOperationException("client_id is missing");
            }

            if (request.Address.IsPresent())
            {
                httpRequest.RequestUri = new Uri(request.Address);
            }

            httpRequest.Method = HttpMethod.Post;

            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ClientCredentialsHelper.PopulateClientCredentials(request, httpRequest);
            httpRequest.Content = new FormUrlEncodedContent(request.Parameters);
        }
    }
}
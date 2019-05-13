using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdentityModel.Client
{
    public static class HttpRequestMessageTokenIntrospectionExtensions
    {
        public static void SetTokenIntrospectionRequest(this HttpRequestMessage httpRequest, TokenIntrospectionRequest request)
        {
            if (request.Address.IsPresent())
            {
                httpRequest.RequestUri = new Uri(request.Address);
            }

            httpRequest.Method = HttpMethod.Post;

            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var clone = request.Clone();
            ClientCredentialsHelper.PopulateClientCredentials(clone, httpRequest);

            clone.Parameters.AddRequired(OidcConstants.TokenIntrospectionRequest.Token, request.Token);
            clone.Parameters.AddOptional(OidcConstants.TokenIntrospectionRequest.TokenTypeHint, request.TokenTypeHint);

            httpRequest.Content = new FormUrlEncodedContent(clone.Parameters);
        }
    }
}
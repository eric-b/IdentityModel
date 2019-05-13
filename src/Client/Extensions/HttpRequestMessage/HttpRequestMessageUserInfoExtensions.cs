using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdentityModel.Client
{
    public static class HttpRequestMessageUserInfoExtensions
    {
        public static void SetUserInfoRequest(this HttpRequestMessage httpRequest, UserInfoRequest request)
        {
            if (request.Token.IsMissing()) throw new ArgumentNullException(nameof(request.Token));

            if (request.Address.IsPresent())
            {
                httpRequest.RequestUri = new Uri(request.Address);
            }
            
            httpRequest.Method = HttpMethod.Get;
            
            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.SetBearerToken(request.Token);
        }
    }
}
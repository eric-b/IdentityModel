using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdentityModel.Client
{
    public static class HttpRequestMessageTokenExtensions
    {
        public static void SetClientCredentialsTokenRequest(this HttpRequestMessage httpRequest, ClientCredentialsTokenRequest request)
        {
            var clone = request.Clone<ClientCredentialsTokenRequest>();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            httpRequest.SetTokenRequest(clone);
        }

        public static void SetDeviceTokenRequest(this HttpRequestMessage httpRequest, DeviceTokenRequest request)
        {
            var clone = request.Clone<DeviceTokenRequest>();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.DeviceCode);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.DeviceCode, request.DeviceCode);

            httpRequest.SetTokenRequest(clone);
        }

        public static void SetPasswordTokenRequest(this HttpRequestMessage httpRequest, PasswordTokenRequest request)
        {
            var clone = request.Clone<PasswordTokenRequest>();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.Password);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.UserName, request.UserName);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Password, request.Password, allowEmpty: true);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            httpRequest.SetTokenRequest(clone);
        }

        public static void SetAuthorizationCodeTokenRequest(this HttpRequestMessage httpRequest, AuthorizationCodeTokenRequest request)
        {
            var clone = request.Clone<AuthorizationCodeTokenRequest>();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.AuthorizationCode);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Code, request.Code);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.RedirectUri, request.RedirectUri);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.CodeVerifier, request.CodeVerifier);

            httpRequest.SetTokenRequest(clone);
        }

        public static void SetRefreshTokenRequest(this HttpRequestMessage httpRequest, RefreshTokenRequest request)
        {
            var clone = request.Clone<RefreshTokenRequest>();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.RefreshToken);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.RefreshToken, request.RefreshToken);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            httpRequest.SetTokenRequest(clone);
        }

        public static void SetTokenRequest(this HttpRequestMessage httpRequest, TokenRequest request)
        {
            if (!request.Parameters.ContainsKey(OidcConstants.TokenRequest.GrantType))
            {
                request.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, request.GrantType);
            }

            if (!request.Parameters.ContainsKey(OidcConstants.TokenRequest.ClientId))
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
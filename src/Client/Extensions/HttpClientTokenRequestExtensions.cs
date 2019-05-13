// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Client
{
    /// <summary>
    /// HttpClient extensions for OAuth token requests
    /// </summary>
    public static class HttpClientTokenRequestExtensions
    {
        /// <summary>
        /// Sends a token request using the client_credentials grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestClientCredentialsTokenAsync(this HttpMessageInvoker client, ClientCredentialsTokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetClientCredentialsTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the urn:ietf:params:oauth:grant-type:device_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestDeviceTokenAsync(this HttpMessageInvoker client, DeviceTokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetDeviceTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the password grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestPasswordTokenAsync(this HttpMessageInvoker client, PasswordTokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetPasswordTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the authorization_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestAuthorizationCodeTokenAsync(this HttpMessageInvoker client, AuthorizationCodeTokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetAuthorizationCodeTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the refresh_token grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestRefreshTokenAsync(this HttpMessageInvoker client, RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetRefreshTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestTokenAsync(this HttpMessageInvoker client, TokenRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetTokenRequest(request);

            return await client.SendInternalAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            //var clone = request.Clone<TokenRequest>();

            //if (!clone.Parameters.ContainsKey(OidcConstants.TokenRequest.GrantType))
            //{
            //    clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, request.GrantType);
            //}

            //return await client.RequestTokenInternalAsync(clone, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="address">The address.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">parameters</exception>
        public static async Task<TokenResponse> RequestTokenRawAsync(this HttpMessageInvoker client, string address, IDictionary<string, string> parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var request = new TokenRequest()
            {
                Address = address,
                Parameters = parameters
            };

            return await client.RequestTokenAsync(request).ConfigureAwait(false);
        }

        //internal static async Task<TokenResponse> RequestTokenInternalAsync(this HttpMessageInvoker client, ProtocolRequest request, CancellationToken cancellationToken = default)
        //{
        //    var httpRequest = new HttpRequestMessage();
        //    httpRequest.SetTokenRequest(request as TokenRequest);

        //    HttpResponseMessage response;
        //    try
        //    {
        //        response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ProtocolResponse.FromException<TokenResponse>(ex);
        //    }

        //    return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response).ConfigureAwait(false);
        //}

        internal static async Task<TokenResponse> SendInternalAsync(this HttpMessageInvoker client, HttpRequestMessage httpRequest, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response).ConfigureAwait(false);
        }
    }
}
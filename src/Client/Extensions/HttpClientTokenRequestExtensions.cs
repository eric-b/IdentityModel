// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        public static Task<TokenResponse> RequestClientCredentialsTokenAsync(this HttpMessageInvoker client, ClientCredentialsTokenRequest request, CancellationToken cancellationToken = default)
        {
            return RequestClientCredentialsTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request using the client_credentials grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestClientCredentialsTokenAsync(this HttpMessageInvoker client,
                                                                                   ClientCredentialsTokenRequest request,
                                                                                   Func<HttpRequestMessage, Task> requestVisitor,
                                                                                   CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the urn:ietf:params:oauth:grant-type:device_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenResponse> RequestDeviceTokenAsync(this HttpMessageInvoker client, DeviceTokenRequest request, CancellationToken cancellationToken = default)
        {
            return RequestDeviceTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request using the urn:ietf:params:oauth:grant-type:device_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestDeviceTokenAsync(this HttpMessageInvoker client, DeviceTokenRequest request, Func<HttpRequestMessage, Task> requestVisitor, CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.DeviceCode);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.DeviceCode, request.DeviceCode);

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the password grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenResponse> RequestPasswordTokenAsync(this HttpMessageInvoker client, PasswordTokenRequest request, CancellationToken cancellationToken = default)
        {
            return RequestPasswordTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request using the password grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestPasswordTokenAsync(this HttpMessageInvoker client,
                                                                          PasswordTokenRequest request,
                                                                          Func<HttpRequestMessage, Task> requestVisitor,
                                                                          CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.Password);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.UserName, request.UserName);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Password, request.Password, allowEmpty: true);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the authorization_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenResponse> RequestAuthorizationCodeTokenAsync(this HttpMessageInvoker client,
                                                                             AuthorizationCodeTokenRequest request,
                                                                             CancellationToken cancellationToken = default)
        {
            return RequestAuthorizationCodeTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request using the authorization_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestAuthorizationCodeTokenAsync(this HttpMessageInvoker client,
                                                                                   AuthorizationCodeTokenRequest request,
                                                                                   Func<HttpRequestMessage, Task> requestVisitor,
                                                                                   CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.AuthorizationCode);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Code, request.Code);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.RedirectUri, request.RedirectUri);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.CodeVerifier, request.CodeVerifier);

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request using the refresh_token grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenResponse> RequestRefreshTokenAsync(this HttpMessageInvoker client, RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            return RequestRefreshTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request using the refresh_token grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestRefreshTokenAsync(this HttpMessageInvoker client, RefreshTokenRequest request, Func<HttpRequestMessage, Task> requestVisitor, CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.RefreshToken);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.RefreshToken, request.RefreshToken);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenResponse> RequestTokenAsync(this HttpMessageInvoker client, TokenRequest request, CancellationToken cancellationToken = default)
        {
            return RequestTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestTokenAsync(this HttpMessageInvoker client, TokenRequest request, Func<HttpRequestMessage, Task> requestVisitor, CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            if (!clone.Parameters.ContainsKey(OidcConstants.TokenRequest.GrantType))
            {
                clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, request.GrantType);
            }

            return await client.RequestTokenAsync(clone, requestVisitor, cancellationToken).ConfigureAwait(false);
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
        public static Task<TokenResponse> RequestTokenRawAsync(this HttpMessageInvoker client, string address, IDictionary<string, string> parameters, CancellationToken cancellationToken = default)
        {
            return RequestTokenRawAsync(client, address, parameters, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="address">The address.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">parameters</exception>
        public static async Task<TokenResponse> RequestTokenRawAsync(this HttpMessageInvoker client,
                                                                     string address,
                                                                     IDictionary<string, string> parameters,
                                                                     Func<HttpRequestMessage, Task> requestVisitor,
                                                                     CancellationToken cancellationToken = default)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var request = new TokenRequest()
            {
                Address = address,
                Parameters = parameters
            };

            return await client.RequestTokenAsync(request, requestVisitor).ConfigureAwait(false);
        }

        internal static Task<TokenResponse> RequestTokenAsync(this HttpMessageInvoker client, ProtocolRequest request, CancellationToken cancellationToken = default)
        {
            return RequestTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        internal static async Task<TokenResponse> RequestTokenAsync(this HttpMessageInvoker client,
                                                                    ProtocolRequest request,
                                                                    Func<HttpRequestMessage, Task> requestVisitor,
                                                                    CancellationToken cancellationToken = default)
        {
            request.Prepare();
            request.Method = HttpMethod.Post;

            if (requestVisitor != null)
            {
                await requestVisitor(request).ConfigureAwait(false);
            }

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response).ConfigureAwait(false);
        }
    }
}
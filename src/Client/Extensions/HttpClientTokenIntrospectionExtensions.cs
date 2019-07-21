// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Client
{
    /// <summary>
    /// HttpClient extensions for OAuth token introspection
    /// </summary>
    public static class HttpClientTokenIntrospectionExtensions
    {
        /// <summary>
        /// Sends an OAuth token introspection request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<TokenIntrospectionResponse> IntrospectTokenAsync(this HttpMessageInvoker client, TokenIntrospectionRequest request, CancellationToken cancellationToken = default)
        {
            return IntrospectTokenAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends an OAuth token introspection request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenIntrospectionResponse> IntrospectTokenAsync(this HttpMessageInvoker client,
                                                                                  TokenIntrospectionRequest request,
                                                                                  Func<HttpRequestMessage, Task> requestVisitor,
                                                                                  CancellationToken cancellationToken = default)
        {
            var clonedRequest = request.Clone();

            clonedRequest.Method = HttpMethod.Post;
            clonedRequest.Parameters.AddRequired(OidcConstants.TokenIntrospectionRequest.Token, request.Token);
            clonedRequest.Parameters.AddOptional(OidcConstants.TokenIntrospectionRequest.TokenTypeHint, request.TokenTypeHint);
            clonedRequest.Prepare();

            if (requestVisitor != null)
            {
                await requestVisitor(clonedRequest).ConfigureAwait(false);
            }

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenIntrospectionResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<TokenIntrospectionResponse>(response).ConfigureAwait(false);
        }
    }
}
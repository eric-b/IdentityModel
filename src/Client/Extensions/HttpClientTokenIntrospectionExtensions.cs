// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Internal;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public static async Task<TokenIntrospectionResponse> IntrospectTokenAsync(this HttpMessageInvoker client, TokenIntrospectionRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.SetTokenIntrospectionRequest(request);

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenIntrospectionResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<TokenIntrospectionResponse>(response).ConfigureAwait(false);
        }
    }
}
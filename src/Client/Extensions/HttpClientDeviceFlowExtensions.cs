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
    /// HttpClient extensions for OIDC userinfo
    /// </summary>
    public static class HttpClientDeviceFlowExtensions
    {
        /// <summary>
        /// Sends a userinfo request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<DeviceAuthorizationResponse> RequestDeviceAuthorizationAsync(this HttpMessageInvoker client, DeviceAuthorizationRequest request, CancellationToken cancellationToken = default)
        {
            return RequestDeviceAuthorizationAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a userinfo request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<DeviceAuthorizationResponse> RequestDeviceAuthorizationAsync(this HttpMessageInvoker client,
                                                                                              DeviceAuthorizationRequest request,
                                                                                              Func<HttpRequestMessage, Task> requestVisitor,
                                                                                              CancellationToken cancellationToken = default)
        {
            var clonedRequest = request.Clone();

            clonedRequest.Parameters.AddOptional(OidcConstants.AuthorizeRequest.Scope, request.Scope);
            clonedRequest.Method = HttpMethod.Post;
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
                return ProtocolResponse.FromException<DeviceAuthorizationResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<DeviceAuthorizationResponse>(response).ConfigureAwait(false);
        }
    }
}
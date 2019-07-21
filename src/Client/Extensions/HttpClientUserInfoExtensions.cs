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
    public static class HttpClientUserInfoExtensions
    {
        /// <summary>
        /// Sends a userinfo request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<UserInfoResponse> GetUserInfoAsync(this HttpMessageInvoker client, UserInfoRequest request, CancellationToken cancellationToken = default)
        {
            return GetUserInfoAsync(client, request, requestVisitor: null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a userinfo request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="requestVisitor">Action called before request is sent to <paramref name="client"/> (ignored if null).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<UserInfoResponse> GetUserInfoAsync(this HttpMessageInvoker client,
                                                                    UserInfoRequest request,
                                                                    Func<HttpRequestMessage, Task> requestVisitor,
                                                                    CancellationToken cancellationToken = default)
        {
            if (request.Token.IsMissing()) throw new ArgumentNullException(nameof(request.Token));

            var clonedRequest = request.Clone();

            clonedRequest.Method = HttpMethod.Get;
            clonedRequest.SetBearerToken(request.Token);
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
                return ProtocolResponse.FromException<UserInfoResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<UserInfoResponse>(response).ConfigureAwait(false);
        }
    }
}
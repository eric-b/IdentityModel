using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityModel.Client
{
    /// <summary>
    /// Interface for discovery cache
    /// </summary>
    public interface IDiscoveryCache
    {
        /// <summary>
        /// Gets or sets the duration of the cache.
        /// </summary>
        /// <value>
        /// The duration of the cache.
        /// </value>
        TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Retrieves the discovery document
        /// </summary>
        /// <returns></returns>
        Task<DiscoveryDocumentResponse> GetAsync();

        /// <summary>
        /// Retrieves the discovery document
        /// </summary>
        /// <param name="visitor">Action called before request is sent.</param>
        /// <returns></returns>
        Task<DiscoveryDocumentResponse> GetAsync(Func<HttpRequestMessage, Task> visitor);

        /// <summary>
        /// Forces a refresh on the next get.
        /// </summary>
        void Refresh();
    }
}
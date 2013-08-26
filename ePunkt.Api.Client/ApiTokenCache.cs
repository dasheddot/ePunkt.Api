using ePunkt.Api.Responses;
using System.Collections.Generic;

namespace ePunkt.Api.Client
{
    public class ApiTokenCache
    {
        private readonly Dictionary<int, ApiTokenResponse> _cachedTokens = new Dictionary<int, ApiTokenResponse>();

        public void AddToken(int mandatorId, ApiTokenResponse tokenResponse)
        {
            _cachedTokens[mandatorId] = tokenResponse;
        }

        public bool HasCachedToken(int mandatorId)
        {
            return _cachedTokens.ContainsKey(mandatorId);
        }

        public ApiTokenResponse GetCachedToken(int mandatorId)
        {
            if (HasCachedToken(mandatorId))
                return _cachedTokens[mandatorId];
            return null;
        }
    }
}

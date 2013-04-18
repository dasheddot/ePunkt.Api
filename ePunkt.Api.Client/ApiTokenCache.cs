using System.Collections.Generic;

namespace ePunkt.Api.Client
{
    public class ApiTokenCache
    {
        private readonly Dictionary<int, ApiToken> _cachedTokens = new Dictionary<int, ApiToken>();

        public void AddToken(int mandatorId, ApiToken token)
        {
            _cachedTokens[mandatorId] = token;
        }

        public bool HasCachedToken(int mandatorId)
        {
            return _cachedTokens.ContainsKey(mandatorId);
        }

        public ApiToken GetCachedToken(int mandatorId)
        {
            if (HasCachedToken(mandatorId))
                return _cachedTokens[mandatorId];
            return null;
        }
    }
}

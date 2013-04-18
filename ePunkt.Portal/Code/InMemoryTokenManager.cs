using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using ePunkt.SocialConnector;

namespace ePunkt.Portal
{
    public class InMemoryTokenManager : ITokenManager
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public InMemoryTokenManager(string consumerKey, string consumerSecret)
        {
            _consumerSecret = consumerSecret;
            _consumerKey = consumerKey;
        }

        public string ConsumerKey
        {
            get { return _consumerKey; }
        }

        public string ConsumerSecret
        {
            get { return _consumerSecret; }
        }

        private Dictionary<string, string> Dictionary
        {
            get
            {
                var cache = MemoryCache.Default;
                var dictionary = cache["inMemoryTokenManager"] as Dictionary<string, string>;
                if (dictionary == null)
                {
                    dictionary = new Dictionary<string, string>();
                    cache.Add("inMemoryTokenManager", dictionary, DateTime.Now.AddYears(1));
                }

                return dictionary;
            }
        }

        public void DeleteTokenSecret(string token)
        {
            Dictionary.Remove(token);
        }

        public string GetTokenSecret(string token)
        {
            return Dictionary.ContainsKey(token) ? Dictionary[token] : null;
        }

        public void StoreTokenSecret(string token, string secret)
        {
            Dictionary[token] = secret;
        }
    }
}
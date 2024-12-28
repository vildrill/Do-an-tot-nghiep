﻿    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    namespace Web1.Others
    {
        public class PayLib
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private SortedList<string, string> _requestData = new SortedList<string, string>(new PayCompare());
            private SortedList<string, string> _responseData = new SortedList<string, string>(new PayCompare());

            public PayLib(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            //------------------------REQUEST DATA----------------------------------------
            public void AddRequestData(string key, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _requestData.Add(key, value);
                }
            }

            public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
            {
                StringBuilder data = new StringBuilder();
                foreach (KeyValuePair<string, string> kv in _requestData)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                    {
                        data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                }
                string queryString = data.ToString();

                baseUrl += "?" + queryString;
                string signData = queryString;
                if (signData.Length > 0)
                {
                    signData = signData.Remove(data.Length - 1, 1);
                }
                string vnp_SecureHash = Util.HmacSHA512(vnp_HashSecret, signData);
                baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

                return baseUrl;
            }

            //------------------------RESPONSE DATA----------------------------------------
            public void AddResponseData(string key, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _responseData.Add(key, value);
                }
            }

            public string GetResponseData(string key)
            {
                if (_responseData.TryGetValue(key, out var retValue))
                {
                    return retValue;
                }
                else
                {
                    return string.Empty;
                }
            }

            private string GetResponseData()
            {
                StringBuilder data = new StringBuilder();
                if (_responseData.ContainsKey("vnp_SecureHashType"))
                {
                    _responseData.Remove("vnp_SecureHashType");
                }
                if (_responseData.ContainsKey("vnp_SecureHash"))
                {
                    _responseData.Remove("vnp_SecureHash");
                }
                foreach (KeyValuePair<string, string> kv in _responseData)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                    {
                        data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                }
                if (data.Length > 0)
                {
                    data.Remove(data.Length - 1, 1);
                }
                return data.ToString();
            }

            public bool ValidateSignature(string inputHash, string secretKey)
            {
                string rspRaw = GetResponseData();
                string myChecksum = Util.HmacSHA512(secretKey, rspRaw);
                return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
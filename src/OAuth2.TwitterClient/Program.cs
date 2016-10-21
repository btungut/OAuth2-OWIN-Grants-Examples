// Copyright (C) 2016 Collaborators of https://github.com/btungut/OAuth2-OWIN-Grants-Examples repository.
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see;
// https://github.com/btungut/OAuth2-OWIN-Grants-Examples/blob/master/LICENSE.md
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.TwitterClient
{
    class Program
    {
        const string BASE_URL = "https://api.twitter.com";
        const string CONSUMER_KEY = "PUT_HERE"; //A.K.A. CLIENT KEY
        const string CONSUMER_SECRET = "PUT_HERE"; //A.K.A. CLIENT SECRET

        static void Main(string[] args)
        {
            /*
             * These lines look like a bit dirty.
             * Don't forgot it just a OAuth2.0 real life using example :)
             */
            using (HttpClient client = new HttpClient())
            {
                //Authenticating
                HttpRequestMessage request = GetRequestMessage("/oauth2/token", HttpMethod.Post);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedCredentials());
                request.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                HttpResponseMessage response = client.SendAsync(request).Result;
                var tokenResponse = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, new
                {
                    token_type="",
                    access_token=""
                });

                //Fetch user timeline
                request = GetRequestMessage("/1.1/statuses/user_timeline.json?screen_name=btungut", HttpMethod.Get);
                request.Headers.Authorization = new AuthenticationHeaderValue(tokenResponse.token_type, tokenResponse.access_token);

                response = client.SendAsync(request).Result;
                string feeds = response.Content.ReadAsStringAsync().Result;
            }
        }

        private static string GetEncodedCredentials()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CONSUMER_KEY}:{CONSUMER_SECRET}"));
        }

        private static HttpRequestMessage GetRequestMessage(string endpoint, HttpMethod method, object content = null)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, new Uri(string.Concat(BASE_URL, endpoint)));

            if (content != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            return message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;


namespace Utils.Facebook
{
    public class Facebook
    {
        private static HttpClient client = new HttpClient();

        public string FacebookAppId { get; set; }
        public string FacebookAccessToken { get; set; }

        private string facebookFeedUrl = "https://graph.facebook.com/v2.12/{0}/feed?fields=id%2Ccreated_time%2Cpicture%2Cmessage%2Clink&access_token={1}";

        public Facebook(string appId,string accessToken){
            this.FacebookAppId = appId;
            this.FacebookAccessToken = accessToken;
        }

        public FacebookFeed GetFeed(){
            try
            {
                var url = String.Format(facebookFeedUrl, this.FacebookAppId, this.FacebookAccessToken);
                var responseMessage = client.GetAsync(url).Result;
                var body = responseMessage.Content.ReadAsStringAsync().Result;
                var jsonData = FacebookFeed.FromJson(body);
                return jsonData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public partial class FacebookFeed
        {
            [JsonProperty("data")]
            public Datum[] Data { get; set; }

            [JsonProperty("paging")]
            public Paging Paging { get; set; }
        }

        public partial class Datum
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("created_time")]
            public string CreatedTime { get; set; }

            [JsonProperty("picture")]
            public string Picture { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }
        }

        public partial class Paging
        {
            [JsonProperty("cursors")]
            public Cursors Cursors { get; set; }

            [JsonProperty("next")]
            public string Next { get; set; }
        }

        public partial class Cursors
        {
            [JsonProperty("before")]
            public string Before { get; set; }

            [JsonProperty("after")]
            public string After { get; set; }
        }

        public partial class FacebookFeed
        {
            public static FacebookFeed FromJson(string json){
                return JsonConvert.DeserializeObject<FacebookFeed>(json, Facebook.Converter.Settings);
            }
        }

        public class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
        }
    }
}
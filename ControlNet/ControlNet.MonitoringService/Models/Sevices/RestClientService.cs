using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public static class RestClientService<T> where T : class
    {
        #region Fields

        #endregion

        #region Methods

        public static RestClient GetRestClient(string serverUrl)
        {
            return new RestClient(serverUrl);
        }

        public static async Task SendPostRequest(string serverUrl, string route, T bodyData)
        {
            var client = GetRestClient(serverUrl);

            var request = new RestRequest(route)
                .AddJsonBody(JsonConvert.SerializeObject(bodyData));

            var response = await client.PostAsync<Message>(request);
        }

        #endregion
    }
}

using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public static class WebApiHttpClientExtentions
    {
        public static async Task<HttpResponse<T>?> GetWebApiModel<T>(this IHttpClientFactory factory, string relativeUri, string jwtToken = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUri);
            if (jwtToken != "")
            {
                request.Headers.Add("Authorization", $"Bearer {jwtToken}");
            }

            var client = factory.CreateClient("WebAPI");
            var response = await client.SendAsync(request);
            var httpResponse = new HttpResponse<T>() { StatusCode = response.StatusCode };
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                httpResponse.Model = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }

            return httpResponse;
        }

        public static async Task<HttpResponse<T1>?> PostWebApiModel<T1,T2>(this IHttpClientFactory factory, string relativeUri, T2 model, string jwtToken = "")
        {
            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string strModel = JsonConvert.SerializeObject(model, Formatting.Indented);
            using var response = await client.PostAsync(relativeUri, new StringContent(strModel, Encoding.UTF8, "application/json"));
            var httpResponse = new HttpResponse<T1>() { StatusCode = response.StatusCode };
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                httpResponse.Model = JsonConvert.DeserializeObject<T1>(reader.ReadToEnd());
            }
            return httpResponse;
        }

        // PUT
        public static async Task<HttpResponse<T1>?> PutWebApiModel<T1, T2>(this IHttpClientFactory factory, string relativeUri, T2 model, string jwtToken = "")
        {
            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            string strModel = JsonConvert.SerializeObject(model, Formatting.Indented);
            using var response = await client.PutAsync(relativeUri, new StringContent(strModel, Encoding.UTF8, "application/json"));
            var httpResponse = new HttpResponse<T1>() { StatusCode = response.StatusCode };

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                httpResponse.Model = JsonConvert.DeserializeObject<T1>(reader.ReadToEnd());
            }
            return httpResponse;
        }


        // DELETE
        public static async Task<HttpResponse<T>?> DeleteWebApiModel<T>(this IHttpClientFactory factory, string relativeUri, string jwtToken = "")
        {
            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            using var response = await client.DeleteAsync(relativeUri);
            var httpResponse = new HttpResponse<T>() { StatusCode = response.StatusCode };
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                httpResponse.Model = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            return httpResponse;
        }
    }
}

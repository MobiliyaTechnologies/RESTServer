namespace EnergyManagementScheduler.WebJob.Utilities
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using EnergyManagementScheduler.WebJob.Contracts;
    using Newtonsoft.Json;

    public static class AzureMLClient
    {
        /// <summary>
        /// Calls the azure ml asynchronous.
        /// </summary>
        /// <param name="requrestData">The requrest data.</param>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="auth">The authentication.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task<AnomalyServiceResponse> CallAzureMLAsync(AnomalyServiceRequest requrestData, string apiUrl, string auth)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                client.BaseAddress = new Uri(apiUrl);
                try
                {
                    HttpResponseMessage response =  client.PostAsJsonAsync("", requrestData).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        AnomalyServiceResponse responseReceived = JsonConvert.DeserializeObject<AnomalyServiceResponse>(result);
                        return responseReceived;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                        // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                        Console.WriteLine(response.Headers.ToString());

                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);

                        return null;
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}

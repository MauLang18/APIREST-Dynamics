using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Dynamics.Controllers
{
    [Route("dynamics")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //Api para obtener datos por el idtra de manera basica
        [HttpGet]
        [Route("Get")]
        public async Task<string> GetAsync(string name)
        {
            //id del cliente de azure
            string clientId = "";
            //id secreta de la api en azure
            string clientSecrets = "";
            //link para el inicio de sesion
            string authority = "";
            //link de dynamics
            string crmUrl = "";
            string response2 = "";

            string accessToken = string.Empty;

            ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
            accessToken = result.AccessToken;
            Console.WriteLine(accessToken);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                /*HttpResponseMessage whoAmIResponse = await httpClient.GetAsync("api/data/v9.2/WhoAmI");
                whoAmIResponse.EnsureSuccessStatusCode();
                var response = await whoAmIResponse.Content.ReadAsStringAsync();*/

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=title&$filter=contains(title,'{name}')&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();
                var response = await httpResponseMessaje.Content.ReadAsStringAsync();

                response2 = response;

                Console.WriteLine(response);
            }

            return response2;
        }

        [HttpPatch]
        [Route("Patch")]
        public async Task<string> PatchAsync(string id, string bcf)
        {
            string clientId = "";
            string clientSecrets = "";
            string authority = "";
            string crmUrl = "";
            string response2 = "";

            string accessToken = string.Empty;

            ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
            accessToken = result.AccessToken;
            Console.WriteLine(accessToken);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                dynamic content = new JObject();
                content.new_bcf = bcf;
                HttpContent httpContent = new StringContent(content.ToString(), UnicodeEncoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Prefer","return=representation");
                httpClient.DefaultRequestHeaders.Add("If-Match", "*");
                HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync($"api/data/v9.1/{entityName}({id})", httpContent);
                var response = httpResponseMessage.EnsureSuccessStatusCode();
                HttpStatusCode statusCode;
                statusCode = response.StatusCode;

                if ((int)response.StatusCode == 200)
                {
                    response2 = "Actualizado";
                }
                //response2 = httpContent.ToString();

                Console.WriteLine(response);
            }

            return response2;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<string> CreateAsync(string bcf)
        {
            string clientId = "";
            string clientSecrets = "";
            string authority = "";
            string crmUrl = "";
            string response2 = "";

            string accessToken = string.Empty;

            ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
            accessToken = result.AccessToken;
            Console.WriteLine(accessToken);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                dynamic content = new JObject();
                content.new_bcf = bcf;
                HttpContent httpContent = new StringContent(content.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"api/data/v9.1/{entityName}", httpContent);
                var response = httpResponseMessage.EnsureSuccessStatusCode();
                HttpStatusCode statusCode;
                statusCode = response.StatusCode;

                if ((int)response.StatusCode == 200)
                {
                    response2 = "Actualizado";
                }
                //response2 = httpContent.ToString();

                Console.WriteLine(response);
            }

            return response2;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<string> DeleteAsync(string id, string bcf)
        {
            string clientId = "";
            string clientSecrets = "";
            string authority = "";
            string crmUrl = "";
            string response2 = "";

            string accessToken = string.Empty;

            ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
            accessToken = result.AccessToken;
            Console.WriteLine(accessToken);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync($"api/data/v9.1/{entityName}({id})");
                var response = httpResponseMessage.EnsureSuccessStatusCode();
                HttpStatusCode statusCode;
                statusCode = response.StatusCode;

                if ((int)response.StatusCode == 200)
                {
                    response2 = "Actualizado";
                }
                //response2 = httpContent.ToString();

                Console.WriteLine(response);
            }

            return response2;
        }
    }
}

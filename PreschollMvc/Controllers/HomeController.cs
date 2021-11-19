using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace PreschollMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Authorize]
        public async Task<ActionResult> CallApi_DD() {

            // discover endpoints from metadata
            var oidcDiscoveryResult = await DiscoveryClient.GetAsync("https://localhost:44394");

            // request token
            var tokenClient = new TokenClient(oidcDiscoveryResult.TokenEndpoint, "mitsos", "1554db43-3015-47a8-a748-55bd76b6af48");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("app.api.weather");
            var token = tokenResponse.AccessToken;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://localhost:44332/weatherforecast");


            string content;
            if (!response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
                ViewBag.Json = content;
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
                ViewBag.Json = JArray.Parse(content).ToString();
            }
            
            return View("Json");
        }

        public async Task<ActionResult> CallMitsos()
        {
            //https://stackoverflow.com/questions/42643786/can-we-restrict-users-in-identity-server4-to-specific-applications


            // discover endpoints from metadata
            var oidcDiscoveryResult = await DiscoveryClient.GetAsync("https://localhost:44394");

            // request token
            var tokenClient = new TokenClient(oidcDiscoveryResult.TokenEndpoint, "mitsos", "1554db43-3015-47a8-a748-55bd76b6af48");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("app.api.weather");
            var token = tokenResponse.AccessToken;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://localhost:44332/weatherforecast");


            string content;
            if (!response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
                ViewBag.Json = content;
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
                ViewBag.Json = JArray.Parse(content).ToString();
            }

            return View("Json");
        }

    }
}
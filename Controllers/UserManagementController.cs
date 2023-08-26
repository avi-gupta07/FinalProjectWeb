using FinalProjectWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;

namespace FinalProjectWeb.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserManagementController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44368/");
        }

        // GET: UserManagement
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            // Convert the User object to HttpContent using JsonContent
            HttpContent httpContent = JsonContent.Create(user);

           // Send the POST request with HttpContent
            HttpResponseMessage response = await _httpClient.PostAsync("api/Account/Register", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                // Handle error responses
                return View("Error");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(User user)
        {
            var formData = new Dictionary<string, string>()
            {
                {"grant_type","password" },
                {"username",user.Email},
                { "password",user.Password}
            };

            var content = new FormUrlEncodedContent(formData);
            // Send the POST request with HttpContent
            HttpResponseMessage response = await _httpClient.PostAsync("Token", content);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(token);

                // Extracting the value of the token
                if (tokenDictionary.TryGetValue("access_token", out string accessToken))
                {
                    // Store the access token in the session
                    Session["AccessToken"] = accessToken;
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Handle missing token
                    return View("Error");
                }
                                
            }
            else
            {
                // Handle error responses
                return View("Error");
            }
        }

        public ActionResult NewPassword()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(User user)
        {
            HttpContent httpContent = JsonContent.Create(user);

            if (Session["AccessToken"] is string accessToken)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _httpClient.PostAsync("api/Account/ForgotPassword", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    // Handle error responses
                    return View("Error");
                }

            }

            return View("Error");
         
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace FinalProjectWeb.Models
{
    public class RegisterUser
    {
        public User User { get; set; }
        private readonly HttpClientCertificate _httpClient;

        //public RegisterUser() {
        //    _httpClient = new HttpClient();
        //}
    }
}
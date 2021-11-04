using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Voleska.Services
{
    public class GooglereCaptchaService
    {
        private ReCAPTCHASettings _settings;

        public GooglereCaptchaService(IOptions<ReCAPTCHASettings> settings) {
            _settings = settings.Value;
        }

       
        public virtual async Task<GoogleResponse> ReCaptchaVarification(string _Token) {

            GooglereCaptchaData _myData = new GooglereCaptchaData {
                response = _Token,
                secret = _settings.ReCAPTCHA_Secret_Key
            };

            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, errors) =>
                            {
                                return true;
                            };
            var odgovor = _Token;
            var skrivnost = _settings.ReCAPTCHA_Secret_Key;
            
            HttpClient client = new HttpClient();

            var response = await client.GetStringAsync(string.Format
    ("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
    skrivnost, odgovor));

            var capresp = JsonConvert.DeserializeObject<GoogleResponse>(response);

            return capresp;
        }
    }

    public class GooglereCaptchaData { 
        public string response { get; set; } // token
        public string secret { get; set; } // secret key
    }

    public class GoogleResponse { 
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}

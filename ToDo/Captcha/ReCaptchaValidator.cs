using Newtonsoft.Json.Linq;
using Serilog;
using System.Net;

namespace ToDo.Captcha
{
    public class ReCaptchaValidator : ICaptchaValidator
    {
        private const string RemoteAddress = "";
        private readonly string _secretKey;
        private readonly double acceptableScore;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReCaptchaValidator(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClientFactory = httpClient;
            _secretKey = configuration["ReCaptcha:SecretKey"];
            acceptableScore = double.Parse(configuration["ReCaptcha:AcceptableScore"]);
        }
        public async Task<bool> IsCaptchaPassedAsync(string token)
        {
            dynamic responce = await GetCaptchaResultDataAsync(token);
            if (responce.success == "true")
            {
                return System.Convert.ToDouble(responce.score) >= acceptableScore;
            }
            return false;
        }

        public async Task<JObject> GetCaptchaResultDataAsync (string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("responce", token)
            });
            using var httpClient = _httpClientFactory.CreateClient();
            var res = await httpClient.PostAsync(RemoteAddress, content);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                Log.Error(new HttpRequestException(res.ReasonPhrase),"Error!");
            }
            var jsonResult = await res.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResult);
        }
    }
}

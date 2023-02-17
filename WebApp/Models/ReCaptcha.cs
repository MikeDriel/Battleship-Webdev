using Newtonsoft.Json.Linq;

namespace WebApp.Models
{


    public class ReCaptcha
    {
        private readonly HttpClient _captchaClient;

        public ReCaptcha(HttpClient client)
        {
            _captchaClient = client;
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await _captchaClient.PostAsync($"?secret=6Ld35o0kAAAAAGrx6bhs54NsNqblS-PcklJjWxRo&response={captcha}", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
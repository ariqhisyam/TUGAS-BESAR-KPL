

namespace UnitTest_MenuDriver
{
    internal class LoginDriver
    {
        private HttpClient httpClient;

        public LoginDriver(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal async Task LoginAsync(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
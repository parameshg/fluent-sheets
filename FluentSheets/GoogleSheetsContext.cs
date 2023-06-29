using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace FluentSheets
{
    public class GoogleSheetsContext
    {
        internal SheetsService Service { get; }

        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        public GoogleSheetsContext(string application, string credentials = @".\service-account.json")
        {
            Service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = GetCredentialsFromFile(credentials), ApplicationName = application });
        }

        private GoogleCredential GetCredentialsFromFile(string credentials)
        {
            GoogleCredential credential;

            using (var stream = new FileStream(credentials, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return credential;
        }
    }
}
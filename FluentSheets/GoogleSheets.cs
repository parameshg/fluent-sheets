using EnsureThat;
using Google.Apis.Sheets.v4;
using MapsterMapper;

namespace FluentSheets
{
    public class GoogleSheets
    {
        private const string RANGE = "A:Z";

        private string? SheetId { get; set; }

        private string? TabName { get; set; }

        private IMapper? Mapper { get; set; }

        private SheetsService? Api { get; set; }

        private GoogleSheets() { }

        public static GoogleSheets Context(GoogleSheetsContext context)
        {
            return new GoogleSheets { Api = EnsureArg.IsNotNull(EnsureArg.IsNotNull(context).Service) };
        }

        public GoogleSheets Map(IMapper mapper)
        {
            Mapper = EnsureArg.IsNotNull(mapper);

            return this;
        }

        public GoogleSheets Sheet(string sheetId)
        {
            SheetId = EnsureArg.IsNotEmptyOrWhiteSpace(sheetId);

            return this;
        }

        public GoogleSheets Tab(string tabName)
        {
            TabName = EnsureArg.IsNotEmptyOrWhiteSpace(tabName);

            return this;
        }

        public async Task<IList<T>> GetRecords<T>(string range = RANGE)
        {
            var result = new List<T>();

            if (Api != null && !string.IsNullOrWhiteSpace(SheetId) && !string.IsNullOrWhiteSpace(TabName))
            {
                var request = Api.Spreadsheets.Values.Get(SheetId, $"{TabName}!{RANGE}");

                if (request != null)
                {
                    var response = await request.ExecuteAsync();

                    if (response != null && response.Values != null)
                    {
                        foreach (var value in response.Values)
                        {
                            if (value != null)
                            {
                                result.Add(Mapper.Map<T>(value));
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<T> First<T>(string range = RANGE)
        {
            var result = default(T);

            if (!string.IsNullOrWhiteSpace(range))
            {
                result = (await GetRecords<T>(range)).FirstOrDefault();
            }

            return result;
        }

        public async Task<T> Last<T>(string range = RANGE)
        {
            var result = default(T);

            if (!string.IsNullOrWhiteSpace(range))
            {
                result = (await GetRecords<T>(range)).LastOrDefault();
            }

            return result;
        }
    }
}
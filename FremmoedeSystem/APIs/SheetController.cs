namespace FremmødeSystem.APIs {
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Google.Apis.Util.Store;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class SheetController {
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public const string ApplicationName = "Spedsbjerg";
        public const string spreadsheetId = "";
        private static BaseClientService.Initializer Initializer;
        private static SheetsService sheetService;
        private static UserCredential credential;
        private const string secretPath = "Credential.json";
        private const string credPath = "auth/token.json";

        private static bool initialized = false;

        public static void Init() {
            initialized = true;
            string credJson = File.ReadAllText(secretPath);
            Initializer = new BaseClientService.Initializer();
            Initializer.ApiKey = "";
            Initializer.ApplicationName = ApplicationName;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromFile(secretPath).Secrets,
                Scopes,
                user: "speedyspedsbjerg@gmail.com",
                CancellationToken.None,
                new FileDataStore(credPath, true)
                ).Result;

            Initializer.HttpClientInitializer = credential;
            sheetService = new SheetsService(Initializer);
        }


        public static SpreadsheetsResource.ValuesResource.GetRequest? getRequest(string range) {
            if(!initialized) Init();
            Console.WriteLine(Initializer.ApplicationName);
            return sheetService.Spreadsheets.Values.Get(spreadsheetId, range);
        }

        public static bool Post(List<IList<object>> data, int row, string sheetName) {
            var valueRange = new ValueRange {
                Values = data
            };
            string range = $"{sheetName}!D{row}";
            var request = sheetService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var response = request.Execute();
            if(response != null) {
                return true;
            }
            else return false;
        }

    }
}

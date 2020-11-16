using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.Collections;

public static class GoogleSheetsConnector
{
    static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    static string ApplicationName = "Google Sheets API .NET Quickstart";

    [MenuItem("Twindrums/Google Sheets/Download")]
    static void DownloadSheets()
    {
        UserCredential credential;

        using (var stream =
            new FileStream("/Users/mraue/Downloads/credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
             Debug.Log("Credential file saved to: " + credPath);
        }

        // Create Google Sheets API service.
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Define request parameters.
        String spreadsheetId = "13A57GnX7n__-quw8nZ7xQOiM3Y1Q_IL0j1MsUpBwx2Y";
        String range = "Engineer!A:E";
        SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);

        // Prints the names and majors of students in a sample spreadsheet:
        // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {            
            foreach (var row in values)
            {
                foreach (var item in row)
                    Debug.Log(item);
                // Print columns A and E, which correspond to indices 0 and 4.                
            }
        }
        else
        {
            Debug.Log("No data found.");
        }        
    }
}

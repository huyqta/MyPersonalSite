using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MySite.Controllers
{
    public class BaseController : Controller
    {


        private UserCredential GetCredentials()
        {
            try
            {
                string[] Scopes = { DriveService.Scope.DriveReadonly };


                UserCredential credential;

                using (var stream = new FileStream(@"client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, "credentials/drive-dotnet-quickstart.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);

                }

                return credential;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return null;
            }

        }

        private void UploadImage(string path, DriveService service, string folderUpload)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "image/*";

            fileMetadata.Parents = new List<string>
            {
                folderUpload
            };


            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/*");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            //textBox1.Text += ("File ID: " + file.Id);

        }

        public void GetCredentialGoogle()
        {
            string fileName = @"C:\Users\HUYQTA\Downloads\111.jpg";
            string ApplicationName = "Drive API .NET Quickstart";

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetCredentials(),
                ApplicationName = ApplicationName,
            });

            string folderid;
            //get folder id by name
            var fileMetadatas = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "TEST",
                MimeType = "application/vnd.google-apps.folder"
            };
            var requests = service.Files.Create(fileMetadatas);
            requests.Fields = "id";
            var files = requests.Execute();
            folderid = files.Id;


            Thread thread = new Thread(() =>
            {
                UploadImage(fileName, service, folderid);

            });
            thread.IsBackground = true;
            thread.Start();



            //string pageToken = null;

            //do
            //{
            //    ListFiles(service, ref pageToken);

            //} while (pageToken != null);
        }

    }
}
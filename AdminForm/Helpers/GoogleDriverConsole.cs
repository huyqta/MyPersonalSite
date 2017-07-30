using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminForm.Helpers
{
    public class GoogleDriverConsole
    {
        public string FileId { get; set; }

        string[] Scopes = { DriveService.Scope.Drive };
        string ApplicationName = "My Site";

        public UserCredential GetCredentials()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, "client_secreta.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return credential;
        }

        public void UploadFile()
        {

            UserCredential credential;

            credential = GetCredentials();

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            string folderid;
            var requestFolder = service.Files.List();
            requestFolder.Q = "mimeType = 'application/vnd.google-apps.folder' and name contains 'EBOOK'";
            requestFolder.Spaces = "drive";
            requestFolder.Fields = "nextPageToken, files(id, name)";
            var result = requestFolder.Execute();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialog1.FileNames)
                {
                    //Thread thread = new Thread(() =>
                    //{
                    //UploadImage(filename, service, folderid);
                    //var a = requests.Resp;
                    IList<string> folders = new List<string>();
                    folders.Add(result.Files.FirstOrDefault().Id);
                    var fileMetadatas = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = filename.Split('\\').Last().ToString(),
                        Parents = folders,
                    };
                    FilesResource.CreateMediaUpload request;
                    using (var stream = new System.IO.FileStream(filename,
                                            System.IO.FileMode.Open))
                    {
                        request = service.Files.Create(
                            fileMetadatas, stream, "image/jpeg");

                        request.Fields = "id";
                        request.Upload();
                        var file = request.ResponseBody;
                        var a = fileMetadatas;
                        this.FileId = file.Id;
                    }
                    //});
                    //thread.IsBackground = true;
                    //thread.Start();

                }

            }

            //string pageToken = null;

            //do
            //{
            //    ListFiles(service, ref pageToken);

            //} while (pageToken != null);
        }

        public void DownloadFile(string fileId)
        {
            UserCredential credential;

            credential = GetCredentials();

            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var request = driveService.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();
            
            request.Download(stream);
            //var fileStream = new FileStream(@"C:/image.jpg", FileMode.Create, FileAccess.Write);
            //stream.CopyTo(fileStream);
            //fileStream.Dispose();
            Image img = System.Drawing.Image.FromStream(stream);

            img.Save(@"C:/image.jpg", ImageFormat.Jpeg);


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
    }
}

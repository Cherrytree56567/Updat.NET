using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace updat.NET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 form = new Form1();

                var webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleteCallback);
                webClient.DownloadFileAsync(new Uri("https://github.com/Cherrytree56567/Updat.NET-Updates/archive/refs/heads/main.zip"), "Def.zip");
                Application.Run(form);

                void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
                {
                    if (e.ProgressPercentage < 99)
                    {
                        form.label1.Text = $" {e.ProgressPercentage}% ";
                    }
                    if (e.ProgressPercentage > 99)
                    {
                        form.label1.Text = $"{e.ProgressPercentage}%";
                    }
                }

                void DownloadCompleteCallback(object sender, AsyncCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        form.label2.Text = "Error Downloading";
                    }
                    else
                    {
                        form.label2.Text = "        Installing!   ";
                        string extractPath = args[0];

                        using (var archive = new ZipArchive(File.OpenRead("Def.zip"), ZipArchiveMode.Read))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                var entryPath = Path.Combine(extractPath, entry.FullName);

                                if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\") || entry.FullName.EndsWith(@"\"))
                                {
                                    Directory.CreateDirectory(entryPath);
                                    continue;
                                }

                                using (var stream = entry.Open())
                                {
                                    using (var fileStream = File.Create(entryPath))
                                    {
                                        stream.CopyTo(fileStream);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

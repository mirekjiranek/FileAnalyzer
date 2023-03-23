using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileAnalyzer
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AnalyzeButton_Click(object sender, EventArgs e)
        {
            string directoryPath = DirectoryTextBox.Text.Trim();
            if (Directory.Exists(directoryPath))
            {
                List<FileDetails> previousFiles = GetPreviousFiles();
                List<FileDetails> files = GetFiles(directoryPath, previousFiles);
                List<FileDetails> modifiedFiles = new List<FileDetails>();

                foreach (var file in previousFiles)
                {
                    var matchingFile = files.FirstOrDefault(f => f.Path == file.Path);
                    if (matchingFile != null && file.Hash != matchingFile.Hash)
                    {
                        matchingFile.Version = file.Version + 1;
                        modifiedFiles.Add(matchingFile);
                    }
                }

                List<FileDetails> newFiles = files.Where(f => !previousFiles.Any(pf => pf.Path == f.Path)).ToList();
                List<FileDetails> deletedFiles = previousFiles.Where(pf => !files.Any(f => f.Path == pf.Path)).ToList();


                SaveFiles(files);

                ResultLabel.Text = GenerateResultMessage(files, newFiles, deletedFiles, modifiedFiles);
            }
            else
            {
                ResultLabel.Text = "Directory path is not valid.";
            }
        }

        private List<FileDetails> GetFiles(string directoryPath, List<FileDetails> previousFiles)
        {
            List<FileDetails> files = new List<FileDetails>();

            foreach (string file in Directory.GetFiles(directoryPath)) 
            {
                var previousFile = previousFiles.FirstOrDefault(x => x.Path == file);
                if (previousFile == null)
                {
                    var newFileDetails = new FileDetails()
                    {
                        Path = file,
                        Hash = GetFileHash(file),
                        Version = 1
                    };

                    files.Add(newFileDetails);
                }
                else
                {
                    string newHash = GetFileHash(file);
                    if (previousFile.Hash != newHash)
                    {
                        var previousFileDetails = new FileDetails()
                        {
                            Path = previousFile.Path,
                            Hash = newHash,
                            Version = previousFile.Version,
                        };
                        files.Add(previousFileDetails);
                    }
                    else
                    {
                        files.Add(previousFile);
                    }
                }
            }

            foreach (string directory in Directory.GetDirectories(directoryPath))
            {
                files.AddRange(GetFiles(directory, previousFiles));
            }

            return files;
        }

        private string GetFileHash(string filePath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        private List<FileDetails> GetPreviousFiles()
        {
            List<FileDetails> files = new List<FileDetails>();

            string filePath = Server.MapPath("~/App_Data/files.json");

            if (!ApplicationSettings.InitialValue && File.Exists(filePath))
            {
                string fileJson = File.ReadAllText(filePath);
                files = JsonConvert.DeserializeObject<List<FileDetails>>(fileJson);
            }
            else
            {
                string fileJson = JsonConvert.SerializeObject(files, Formatting.Indented);
                File.WriteAllText(filePath, fileJson);
            }

            return files;
        }

        private void SaveFiles(List<FileDetails> files)
        {
            string filePath = Server.MapPath("~/App_Data/files.json");
            string fileJson = JsonConvert.SerializeObject(files, Formatting.Indented);
            File.WriteAllText(filePath, fileJson);
        }

        private string GenerateResultMessage(List<FileDetails> currentFiles, List<FileDetails> newFiles, List<FileDetails> deletedFiles, List<FileDetails> modifiedFiles)
        {
            string result = "";
            if (ApplicationSettings.InitialValue)
            {
                ApplicationSettings.InitialValue = false;
                result += "<b>Direcory loaded, list of files:</b><br/>";
                foreach (var file in currentFiles)
                {
                    result += $"{file.Path}<br/>";
                }
                return result;
            }

            if (currentFiles.Count == 0)
            {
                result += "Empty directory";
            }

            if (newFiles.Count > 0)
            {
                result += "<b>Nové soubory:</b><br/>";
                foreach (var file in newFiles)
                {
                    result += $"[A] {file.Path} (verze {file.Version})<br/>";
                }
            }

            if (deletedFiles.Count > 0)
            {
                result += "<b>Odstraněné soubory a adresáře:</b><br/>";
                foreach (var file in deletedFiles)
                {
                    result += $"[D] {file.Path}<br/>";
                }
            }

            if (modifiedFiles.Count > 0)
            {
                result += "<b>Změněné soubory:</b><br/>";
                foreach (var file in modifiedFiles)
                {
                    result += $"[M] {file.Path} (verze {file.Version})<br/>";
                }
            }

            if (result == "")
            {
                result = "Žádná změna.";
            }

            return result;
        }
    }
}
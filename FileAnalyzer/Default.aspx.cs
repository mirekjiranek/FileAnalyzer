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
            List<FileDetails> files = GetFiles(directoryPath);
            List<FileDetails> previousFiles = GetPreviousFiles();

            List<FileDetails> newFiles = files.Except(previousFiles).ToList();
            List<FileDetails> deletedFiles = previousFiles.Except(files).ToList();
            List<FileDetails> modifiedFiles = new List<FileDetails>();

            foreach (FileDetails file in previousFiles.Intersect(files))
            {
                if (file.Hash != GetFileHash(file.Path))
                {
                    file.Version++;
                    modifiedFiles.Add(file);
                }
            }

            SaveFiles(files);

            ResultLabel.Text = GenerateResultMessage(newFiles, deletedFiles, modifiedFiles);
        }

        private List<FileDetails> GetFiles(string directoryPath)
        {
            List<FileDetails> files = new List<FileDetails>();

            foreach (string file in Directory.GetFiles(directoryPath))
            {
                FileDetails fileDetails = new FileDetails()
                {
                    Path = file,
                    Hash = GetFileHash(file),
                    Version = 1
                };

                files.Add(fileDetails);
            }

            foreach (string directory in Directory.GetDirectories(directoryPath))
            {
                files.AddRange(GetFiles(directory));
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

            if (ViewState["Files"] != null)
            {
                files = (List<FileDetails>)ViewState["Files"];
            }

            return files;
        }

        private void SaveFiles(List<FileDetails> files)
        {
            ViewState["Files"] = files;
        }

        private string GenerateResultMessage(List<FileDetails> newFiles, List<FileDetails> deletedFiles, List<FileDetails> modifiedFiles)
        {
            string result = "";

            if (newFiles.Count > 0)
            {
                result += "<b>Nové soubory:</b><br/>";
                foreach (FileDetails file in newFiles)
                {
                    result += $"[A] {file.Path} (verze {file.Version})<br/>";
                }
            }

            if (deletedFiles.Count > 0)
            {
                result += "<b>Odstraněné soubory a adresáře:</b><br/>";
                foreach (FileDetails file in deletedFiles)
                {
                    result += $"[D] {file.Path}<br/>";
                }
            }

            if (modifiedFiles.Count > 0)
            {
                result += "<b>Změněné soubory:</b><br/>";
                foreach (FileDetails file in modifiedFiles)
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
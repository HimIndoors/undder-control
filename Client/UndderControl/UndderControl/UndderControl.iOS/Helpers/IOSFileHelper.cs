using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UndderControl.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(UndderControl.iOS.Helpers.IOSFileHelper))]

namespace UndderControl.iOS.Helpers
{
    class IOSFileHelper : IFileHelper
    {
        public Task<bool> ExistsAsync(string filename)
        {
            var filepath = GetFilePath(filename);
            var exists = File.Exists(filepath);
            return Task.FromResult(exists);
        }

        public Task<DateTime> LastModifiedAsync(string filename)
        {
            var filepath = GetFilePath(filename);
            var lastModified = File.GetLastWriteTime(filepath);
            return Task.FromResult(lastModified);
        }

        public async Task WriteTextAsync(string filename, string text)
        {
            var filepath = GetFilePath(filename);
            using (var streamWriter = File.CreateText(filepath))
            {
                await streamWriter.WriteAsync(text);
            }
        }

        public async Task<string> ReadTextAsync(string filename)
        {
            var filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public Task<IEnumerable<string>> GetFilesAsync()
        {
            var files = Directory.GetFiles(GetDocsPath());
            return Task.FromResult<IEnumerable<string>>(files);
        }

        public Task DeleteAsync(string filename)
        {
            File.Delete(GetFilePath(filename));
            return Task.CompletedTask;
        }

        // Private methods.
        string GetFilePath(string filename)
        {
            return Path.Combine(GetDocsPath(), filename);
        }

        private string GetDocsPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
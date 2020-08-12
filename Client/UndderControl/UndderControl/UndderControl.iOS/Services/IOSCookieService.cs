using System;
using System.IO;
using System.Linq;
using Foundation;
using UIKit;
using UndderControl.iOS.Services;
using UndderControl.Services;
using WebKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSCookieService))]
namespace UndderControl.iOS.Services
{
    public class IOSCookieService : ICookieService
    {
        public void Clear()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                NSHttpCookieStorage.SharedStorage.RemoveCookiesSinceDate(NSDate.DistantPast);
                WKWebsiteDataStore.DefaultDataStore.FetchDataRecordsOfTypes(WKWebsiteDataStore.AllWebsiteDataTypes, (NSArray records) =>
                {
                    for (nuint i = 0; i < records.Count; i++)
                    {
                        var record = records.GetItem<WKWebsiteDataRecord>(i);

                        WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypes(record.DataTypes,
                            new[] { record }, () => { Console.Write($"deleted: {record.DisplayName}"); });
                    }
                });

                NSUrlCache.SharedCache.RemoveAllCachedResponses();
            }
            else
            {

                // Remove the basic cache.
                NSUrlCache.SharedCache.RemoveAllCachedResponses();
                var cookies = NSHttpCookieStorage.SharedStorage.Cookies;

                foreach (var c in cookies)
                {
                    NSHttpCookieStorage.SharedStorage.DeleteCookie(c);
                }
            }

            try
            {
                // Clear web cache
                DeleteLibraryFolderContents("Caches");

                // Remove all cookies stored by the site. This includes localStorage, sessionStorage, and WebSQL/IndexedDB.
                DeleteLibraryFolderContents("Cookies");

                // Removes all app cache storage.
                DeleteLibraryFolder("WebKit");

            }
            catch (Exception ex)
            {
                App.UnhandledException(ex, $"Error deleting cache {ex.Message}");
            }

        }

        private void DeleteLibraryFolder(string folderName)
        {
            var manager = NSFileManager.DefaultManager;
            var library = manager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User).First();
            var dir = Path.Combine(library.Path, folderName);

            manager.Remove(dir, out NSError error);
            if (error != null)
            {
                App.UnhandledException(new Exception(error.Description), error.Description);
            }
        }

        private void DeleteLibraryFolderContents(string folderName)
        {
            var manager = NSFileManager.DefaultManager;
            var library = manager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User).First();
            var dir = Path.Combine(library.Path, folderName);
            var contents = manager.GetDirectoryContent(dir, out NSError error);
            if (error != null)
            {
                App.UnhandledException(new Exception(error.Description), error.Description);
            }

            foreach (var c in contents)
            {
                try
                {
                    manager.Remove(Path.Combine(dir, c), out NSError errorRemove);
                    if (errorRemove != null)
                    {
                        App.UnhandledException(new Exception(error.Description), error.Description);
                    }
                }
                catch (Exception ex)
                {
                    App.UnhandledException(ex, $"Error deleting folder contents: {folderName}{Environment.NewLine}{ex.Message}");
                }
            }
        }
    }
}
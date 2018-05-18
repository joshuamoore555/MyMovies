using System;
using System.Collections.Generic;
using System.IO;
using WMPLib;

namespace MyMovies.MovieRetriever
{
    class FileRetriever
    {
        public FileRetriever()
        {

        }

        public List<string> GetMovies(int min, int max)
        {
            List<string> movieFiles = new List<string>();
            var player = new WindowsMediaPlayer();
            TimeSpan minLimit = new TimeSpan(0, 0, min, 0, 0);
            TimeSpan maxLimit = new TimeSpan(0, 0, max, 0, 0);

            //returns files from directory and sub directories
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = path.Replace("Documents", "Vuze Downloads");
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);

            //only take videos bigger than 15 mins
            foreach (string filepath in files)
            {
                if (filepath.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) || filepath.EndsWith(".wmv", StringComparison.OrdinalIgnoreCase) || filepath.EndsWith(".avi", StringComparison.OrdinalIgnoreCase) || filepath.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase))
                {
                    var clip = player.newMedia(filepath);
                    TimeSpan t = TimeSpan.FromSeconds(clip.duration);
                    if (t >= minLimit && t <= maxLimit)
                    {
                        movieFiles.Add(filepath);
                    }
                }
            }
            movieFiles.Sort();
            return movieFiles;
        }
    }
}

using System.IO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace TextSearcherN
{
    public class TextSearcher
    {
        public string textToSearch
        {
            set
            {
                text = value;
            }
        }

        public bool checkSubDirectories { get; set; } = false;

        /// <summary>
        /// baseDirectory is the directory where we have to start searching
        /// </summary>
        public string baseDirectory
        {
            set
            {
                directory = value;
            }
        }

        public string resultFile
        {
            get
            {
                return result;
            }
        }

        public long elapsedMilliseconds { get; set; } = 0;

        public void search()
        {
            Random r = new Random();
            result = Path.Combine(Path.GetTempPath(), "result-" + r.Next(1000, 999999) + ".txt");

            StreamWriter writer = new StreamWriter(result);
            
            Stopwatch sw = new Stopwatch();

            sw.Start();


            writer.WriteLine("To search a process press ctrl + f and digit \"result from *process name*\".\nIf there isn't result for a process it's because there were 0 matches and if you can find the process it's because there was a problem with the memory dump because of the application or user permissions.\n\n\n");
            
            if (checkSubDirectories)
            {
                string[] subDirectories = Directory.GetDirectories(directory);
                Parallel.ForEach<string>(subDirectories, (dir) =>
                {
                    foreach (string file in Directory.EnumerateFileSystemEntries(dir))
                    {
                        try
                        {
                            if (!fileEmpty(file))
                            {
                                writer.WriteLine("Result from " + file.Replace(".txt", string.Empty) + "\n" + String.Join(Environment.NewLine, File.ReadAllLines(file).Where<string>((line) => line.ToLower().Contains(text.ToLower()))));
                            }

                        }
                        catch (Exception) { }
                    }
                });


            }
            else
            {
                string[] files = Directory.GetFiles(directory);
                Parallel.ForEach<string>(files, (file) =>
                {
                    try
                    {
                        if (!fileEmpty(file))
                        {
                            writer.WriteLine("Result from " + file.Replace(".txt", string.Empty) + "\n" + String.Join(Environment.NewLine, File.ReadAllLines(file).Where<string>((line) => line.ToLower().Contains(text.ToLower()))));
                        }
                    }
                    catch { }
                });
            }


            writer.Close();
            sw.Stop();

            elapsedMilliseconds = sw.ElapsedMilliseconds;
        }


        bool fileEmpty(string file)
        {
            FileInfo fi = new FileInfo(file);

            if (fi.Length > 0)
                return false;
            else
            {
                return true;
            }
        }

        string result;
        string text;
        string directory;
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Jawbreaker_Deobfuscator
{
    class Program
    {
        private static string[] regexs = {
            "(\")([\\w]*)(\")",
            "[a-zA-Z]{9,12}"
        };

        private static void Exist(string x)
        {
            if (!File.Exists(x))
            {
                Console.WriteLine("This File Doesn't Exist :");
                Console.ReadLine();
                Environment.Exit(1337); // flushed lmaoooooooooooooooooooo
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Python File Path: ");
            string file = Console.ReadLine().Replace("\"", string.Empty);
            Exist(file);

            string Jawbreaked = Regex.Match(File.ReadAllText(file), regexs[0]).ToString().Replace("\"", string.Empty);

            string output1 = String.Format("{0}Jawbreaked.py", Path.GetTempPath());

            File.WriteAllText(output1, Oof.Base64Decode(Oof.Base32ToBytes(Oof.HexToText(Jawbreaked))).Replace("exec", "print")); //Oof

            Console.WriteLine("Specify your python.exe File Path: ");
            string PIP = Console.ReadLine().Replace("\"", string.Empty);

            Exist(PIP);

            ProcessStartInfo proc = new ProcessStartInfo()
            {
                FileName = PIP,
                Arguments = output1,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                LoadUserProfile = true
            };
            Process process = Process.Start(proc);
            using (StreamReader reader = process.StandardOutput)
            {
                string err = process.StandardError.ReadToEnd();
                if(err.Length > 0)
                {
                    Console.WriteLine(String.Format("Error:\n{0}", err));
                    Console.ReadLine();
                    Environment.Exit(1337);
                }
                string Jawbreaked2 = Regex.Match(reader.ReadToEnd(), regexs[1]).ToString().Replace("\"", string.Empty);
                string[] fileSplit = file.Split('\\');
                string oofDownloaded = new WebClient().DownloadString(String.Format("https://hastebin.com/raw/{0}", Jawbreaked2));
                File.WriteAllText(file.Replace(fileSplit[fileSplit.Length - 1], "Oofed.py"), String.Format("# Oofed with https://github.com/HideakiAtsuyo/Jawbreaked\n{0}", oofDownloaded));
            }

            File.Delete(output1);

            Console.WriteLine("Oofed !");
            Console.ReadLine();
        }
    }
}

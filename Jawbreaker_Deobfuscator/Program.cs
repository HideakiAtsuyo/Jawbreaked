using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
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
                File.WriteAllText(file.Replace(file.Split('\\')[file.Split('\\').Length - 1], "Oofed.py"), String.Format("# Oofed with https://github.com/HideakiAtsuyo/Jawbreaked\n{0}", new WebClient().DownloadString(String.Format("https://hastebin.com/raw/{0}", Jawbreaked2))));
            }

            File.Delete(output1);

            Console.WriteLine("Oofed !");
            Console.ReadLine();
        }
    }

    public static class Oof
    {
        public static string HexToText(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] rawBytes = new byte[hex.Length / 2];
            for (int i = 0; i < rawBytes.Length; i++)
                rawBytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return Encoding.ASCII.GetString(rawBytes);
        }
        public static string Base32ToBytes(string base32)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            List<byte> output = new List<byte>();
            char[] bytes = base32.ToCharArray();
            for (int bitIndex = 0; bitIndex < base32.Length * 5; bitIndex += 8)
            {
                int dualbyte = alphabet.IndexOf(bytes[bitIndex / 5]) << 10;
                if (bitIndex / 5 + 1 < bytes.Length)
                    dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 1]) << 5;
                if (bitIndex / 5 + 2 < bytes.Length)
                    dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 2]);

                dualbyte = 0xff & (dualbyte >> (15 - bitIndex % 5 - 8));
                output.Add((byte)(dualbyte));
            }
            return Encoding.ASCII.GetString(output.ToArray()).Replace("?", string.Empty);
        }

        public static string Base64Decode(string x)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(x));
        }
    }
}
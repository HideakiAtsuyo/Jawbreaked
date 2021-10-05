using System;
using System.Collections.Generic;
using System.Text;

namespace Jawbreaker_Deobfuscator
{
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class Client
{
    static void Main(string[] args)
    {
        Console.WriteLine("Wprowadź wiadomość do wysłania:");
        string message = Console.ReadLine();

        try
        {
            string encryptedMessage = EncryptWithHMAC(message);
            Console.WriteLine($"Zaszyfrowana wiadomość: {encryptedMessage}");

            TcpClient client = new TcpClient("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(encryptedMessage);
            stream.Write(data, 0, data.Length);
            stream.Flush();

            Console.WriteLine("Wiadomość została wysłana do serwera.");
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    static string EncryptWithHMAC(string plainText) //szyfrowanie wiadmosci 
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456");
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456");
            

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(plainText);
                    }
                }

                byte[] iv = aes.IV;
                byte[] encrypted = ms.ToArray();

                byte[] hmac; //obliczanie HMAC
                using (HMACSHA256 hmacSha = new HMACSHA256(Encoding.UTF8.GetBytes("hmacsecretkey123")))
                {
                    hmac = hmacSha.ComputeHash(encrypted);
                }

                byte[] result = new byte[iv.Length + encrypted.Length + hmac.Length];
                Array.Copy(iv, 0, result, 0, iv.Length);
                Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);
                Array.Copy(hmac, 0, result, iv.Length + encrypted.Length, hmac.Length);
                return Convert.ToBase64String(result);
            }
        }
    }  

}


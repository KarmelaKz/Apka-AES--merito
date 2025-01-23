using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Szyfrowanie AES w aplikacji klient-serwer
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


class Server
{
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Serwer nasłuchuje na porcie 5000...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Klient połączony.");
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Otrzymano zaszyfrowaną wiadomość: {receivedMessage}");

                try
                {
                    string decryptedMessage = DecryptWithHMAC(receivedMessage);
                    Console.WriteLine($"Odszyfrowana wiadomość: {decryptedMessage}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas deszyfrowania: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Nie odebrano danych.");
            }

            client.Close();
        }
    }

    // Funkcja deszyfrowania 
    static string DecryptWithHMAC(string cipherText)
    {
        byte[] combined = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] hmac = new byte[32];
        byte[] encrypted = new byte[combined.Length - iv.Length - hmac.Length];

        Array.Copy(combined, 0, iv, 0, iv.Length);
        Array.Copy(combined, iv.Length, encrypted, 0, encrypted.Length);
        Array.Copy(combined, iv.Length + encrypted.Length, hmac, 0, hmac.Length);

        using (HMACSHA256 hmacSha = new HMACSHA256(Encoding.UTF8.GetBytes("hmacsecretkey123")))  //weryfikacja 
        {
            byte[] computedHmac = hmacSha.ComputeHash(encrypted);
            if (!computedHmac.SequenceEqual(hmac))
            {
                throw new Exception("Niepoprawna integralność wiadomości (HMAC niezgodny).");
            }
        }


        using (Aes aes = Aes.Create())  //odszyfrowywanie wiadomości 
        {
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456");
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

}

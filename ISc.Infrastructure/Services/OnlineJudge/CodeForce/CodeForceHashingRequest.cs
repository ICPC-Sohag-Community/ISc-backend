using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Infrastructure.Services.OnlineJudge.CodeForce
{
    public static class CodeForceHashingRequest
    {
        public static string GenerateSig(string queryString,string source,string apisecret)
        {
            string rand = new Random().Next(100000, 999999).ToString();
            string link = rand + source + apisecret + '#' + apisecret;

            byte[] hashValue = SHA512.HashData(Encoding.UTF8.GetBytes(link));
            string hashText = BitConverter.ToString(hashValue).Replace("-", "").ToLower();

            while (hashText.Length < 32)
            {
                hashText = "0" + hashText;
            }

            return rand + hashText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Comman.Enums;
using ISc.Shared.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISc.Infrastructure.Services.OnlineJudge.CodeForce
{
    public static class CodeForceHandlingRequest
    {
        public static string GenerateSig(string queryString,string apiSecret)
        {
            string rand = new Random().Next(100000, 999999).ToString();
            string link = rand + "/" + queryString + '#' + apiSecret;

            byte[] hashValue = SHA512.HashData(Encoding.UTF8.GetBytes(link));
            string hashText = BitConverter.ToString(hashValue).Replace("-", "").ToLower();

            while (hashText.Length < 32)
            {
                hashText = "0" + hashText;
            }

            return rand + hashText;
        }

        /// <summary>
        /// first value is key second one is value
        /// </summary>
        /// <param name="app setting configuration"></param>
        /// <param name="community name"></param>
        /// <returns></returns>
        /// <exception cref="SerivceErrorException"></exception>
        public static KeyValuePair<string,string> GetSheetKeysFactory(IConfiguration config,Community community,ILogger log)
        {
            var codeForceConfig = config.GetSection("CodeForceSettings");

            switch (community)
            {
                case Community.Sohag:
                    return new KeyValuePair<string, string>(codeForceConfig["SohagApiKey"]!, codeForceConfig["SohagApiSecret"]!);
                case Community.Assuit:
                    return new KeyValuePair<string, string>(codeForceConfig["AssuitApiKey"]!, codeForceConfig["AssuitApiSecret"]!);
            }

            log.LogCritical($"Couldn't find api key or api secret for {community}");

            throw new SerivceErrorException("CodeForce configurations not found!");
        }

        public static string GenerateTimeInUnix()
        {
            return (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000).ToString();
        }
    }
}

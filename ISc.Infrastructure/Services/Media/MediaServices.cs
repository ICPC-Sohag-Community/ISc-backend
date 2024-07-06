﻿using ISc.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Infrastructure.Services.Media
{
    internal class MediaServices : IMediaServices
    {
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _configuration;

        public MediaServices(
            IWebHostEnvironment host,
            IConfiguration configuration)
        {
            _host = host;
            _configuration = configuration;
        }

        public async Task DeleteAsync(string url)
        {
            var imageNameToDelete = Path.GetFileNameWithoutExtension(url);
            var ext = Path.GetExtension(url);
            var oldImagePath = $@"{_configuration["ImageSavePath"]}\Images\{imageNameToDelete}{ext}";

            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }

            await Task.CompletedTask;
        }

        public string? GetUrl(string? url)
        {
            if (url.IsNullOrEmpty()) return null;

            return _configuration["ImageSavePath"]!.ToString() + @"/" + url;
        }

        public Task<string> SaveAsync(IFormFile? media)
        {
            if (media is null)
            {
                return Task.FromResult("");
            }

            var extension = Path.GetExtension(media.FileName).ToLower();

            var uniqueFileName = Guid.NewGuid().ToString() + extension;

            var uploadsFolder = Path.Combine("wwwroot", "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                media.CopyTo(stream);
                stream.Dispose();
            }

            return Task.FromResult(uniqueFileName);
        }

        public async Task<string> UpdateAsync(string oldUrl, IFormFile newMedia)
        {
            if (oldUrl == null && newMedia == null)
            {
                return "";
            }

            if (newMedia == null)
            {
                return oldUrl;
            }

            if (oldUrl == null)
            {
                return await SaveAsync(newMedia)!;
            }

            await DeleteAsync(oldUrl);
            return await SaveAsync(newMedia)!;
        }

    }
}

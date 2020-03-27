﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebFeatures.WebApi.ModelBinders
{
    internal class BytesModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Metadata.ModelType == typeof(byte[]) ? new BytesModelBinder() : null;
        }
    }

    internal class BytesModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string modeName = bindingContext.IsTopLevelObject
                ? bindingContext.BinderModelName ?? bindingContext.FieldName
                : bindingContext.ModelName;

            var res = await TryGetFirstNotEmptyFileAsync(bindingContext.HttpContext.Request, modeName);
            if (!res.Success)
                return;

            bindingContext.ModelState.SetModelValue(modeName, null, null);

            byte[] bytes = await ReadBytesAsync(res.File);

            bindingContext.Result = ModelBindingResult.Success(bytes);
        }

        private async Task<(bool Success, IFormFile File)> TryGetFirstNotEmptyFileAsync(HttpRequest request, string fileName)
        {
            if (!request.HasFormContentType)
                return (false, null);

            IFormCollection form = await request.ReadFormAsync();

            foreach (IFormFile file in form.Files)
            {
                if (file.Length == 0 && string.IsNullOrEmpty(file.FileName))
                {
                    continue;
                }

                if (file.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return (true, file);
                }
            }

            return (false, null);
        }

        private async Task<byte[]> ReadBytesAsync(IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}

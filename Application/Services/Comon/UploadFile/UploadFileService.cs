using Common;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Comon.UploadFile
{
    public class UploadFileService : IRequestHandler<RequestUploadFile, ResultDto<string>>
    {
        private readonly IHostingEnvironment _environment;
        public UploadFileService(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public Task<ResultDto<string>> Handle(RequestUploadFile request, CancellationToken cancellationToken)
        {
            if (request.file == null)
            {
                return Task.FromResult(new ResultDto<string> { Message = "فایل خالی است" });
            }
            string folder = @"ProductImages\";
            string path = Path.Combine(_environment.WebRootPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Guid.NewGuid() + request.file.FileName;
            string filePath = Path.Combine(path, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                request.file.CopyTo(fileStream);
            }
            return Task.FromResult(new ResultDto<string>
            {
                Data = folder + fileName,
                IsSuccess = true,
                Message = "فایل آپلود شد"
            });
        }
    }
    public record RequestUploadFile(IFormFile file) : IRequest<ResultDto<string>>;
}

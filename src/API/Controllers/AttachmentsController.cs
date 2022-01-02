using API.ViewModels.Attachment;
using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Models.Utils.Utility;

namespace API.Controllers
{
    public class AttachmentsController : BaseApiController
    {
        private readonly AttachmentService _attachmentService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AttachmentsController> _logger;
        private readonly IConfiguration _configuration;

        public AttachmentsController(AttachmentService attachmentService,
            IMapper mapper, IWebHostEnvironment env,
            ILogger<AttachmentsController> logger,
            IConfiguration configuration)
        {
            _attachmentService = attachmentService;
            _mapper = mapper;
            _env = env;
            _logger = logger;
            _configuration = configuration;
        }

        // GET /attachments/?commandId={id}
        [HttpGet]
        public async Task<ActionResult<List<AttachmentRead>>> GetByCommandId([FromQuery] long id)
        {
            var result = await _attachmentService.GetByCommandId(id);
            var attachmentsRead = _mapper.Map<List<AttachmentRead>>(result);

            return Ok(attachmentsRead);
        }

        // DELETE /attachments/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _attachmentService.Delete(id);

            if (!result)
                return NotFound();
            return Ok();
        }

        // Create/Upload Attachment 
        // POST /attachments/
        [HttpPost]
        public async Task<ActionResult<AttachmentRead>> Create([FromForm] AttachmentCreate attachmentCreate)
        {
            if (attachmentCreate == null)
                return BadRequest();

            var types = Enum.GetValues<AttachmentType>();

            if (!IsValidExtension(attachmentCreate.File, types))
                return BadRequest();

            var (uploaded, fileName) = UploadFile(attachmentCreate.File);

            if (uploaded)
            {
                var attachmentModel = _mapper.Map<Attachment>(attachmentCreate);

                AttachmentType ext = GetExtensionType(attachmentCreate.File, types);
                attachmentModel.Type = ext;
                attachmentModel.Name = fileName;

                var result = await _attachmentService.Create(attachmentModel);

                if (result == null)
                    return BadRequest();

                var attachmentRead = _mapper.Map<AttachmentRead>(result);
                return Created("", attachmentRead);
            }

            return BadRequest();
        }

        private (bool, string) UploadFile(IFormFile file)
        {
            try
            {
                string uploadsDirName = _configuration["UploadsDirName"];
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(_env.WebRootPath, uploadsDirName);
                var filePath = Path.Combine(uploads, uniqueFileName);

                if (!Directory.Exists(Path.Combine(_env.WebRootPath, uploadsDirName)))
                {
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, uploadsDirName));
                }
                using var fileStream = new FileStream(filePath, FileMode.Create);
                fileStream.Position = 0;
                file.CopyTo(fileStream);

                return (true, Path.Combine(uploadsDirName, uniqueFileName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (false, null);
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }

        private bool IsValidExtension(IFormFile file, AttachmentType[] types)
            => string.IsNullOrEmpty(Path.GetExtension(file.FileName).TrimStart('.').ToUpper()) ? false : types.Any(t => t.ToString() == Path.GetExtension(file.FileName).TrimStart('.').ToUpper());

        private AttachmentType GetExtensionType(IFormFile file, AttachmentType[] types)
            => types.First(t => t.ToString() == Path.GetExtension(file.FileName).TrimStart('.').ToUpper());
    }
}

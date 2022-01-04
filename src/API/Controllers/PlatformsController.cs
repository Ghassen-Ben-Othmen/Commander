using API.ViewModels.Platform;
using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PlatformsController : BaseApiController
    {
        private readonly PlatformService _platformService;
        private readonly IMapper _mapper;

        public PlatformsController(PlatformService platformService, IMapper mapper)
        {
            _platformService = platformService;
            _mapper = mapper;
        }

        // GET /platforms/
        [HttpGet]
        public async Task<ActionResult<List<PlatformRead>>> GetAll([FromQuery] uint page, [FromQuery] uint size)
        {
            var platforms = await _platformService.All(page, size);
            if (platforms == null)
                return BadRequest();

            var result = _mapper.Map<List<PlatformRead>>(platforms);
            return Ok(result);
        }

        // GET /platform/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformRead>> GetById(long id)
        {
            var platform = await _platformService.GetById(id);

            if (platform == null)
                return NotFound();

            var result = _mapper.Map<PlatformRead>(platform);
            return Ok(result);
        }

        // POST /platforms/
        [HttpPost]
        public async Task<ActionResult<PlatformRead>> Add(PlatformCreate platformCreate)
        {
            if (platformCreate == null)
                return BadRequest();

            var platformModel = _mapper.Map<Platform>(platformCreate);
            var result = await _platformService.Create(platformModel);

            if (result == null)
                return BadRequest();

            var platformRead = _mapper.Map<PlatformRead>(result);
            return CreatedAtAction(nameof(GetById), new { Id = platformRead.Id}, platformRead);

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PlatformRead>> Update(long id, PlatformUpdate platformUpdate)
        {
            if (platformUpdate == null)
                return BadRequest();

            platformUpdate.Id = id;
            var platformModel = _mapper.Map<Platform>(platformUpdate);

            var result = await _platformService.PartialUpdate(platformModel);

            if (result == null)
                return NotFound();

            var platformRead = _mapper.Map<PlatformRead>(result);
            return Ok(platformRead);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _platformService.Delete(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}

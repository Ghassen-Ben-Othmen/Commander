using API.ViewModels.Command;
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
    public class CommandsController : BaseApiController
    {
        private readonly CommandService _commandService;
        private readonly IMapper _mapper;

        public CommandsController(CommandService commandService, IMapper mapper)
        {
            _commandService = commandService;
            _mapper = mapper;
        }

        // GET /commands/
        [HttpGet]
        public async Task<ActionResult<List<CommandRead>>> GetAll([FromQuery] long? platformId, [FromQuery] uint page, [FromQuery] uint size)
        {
            var commands = await _commandService.All(platformId, page, size);
            if (commands == null)
                return BadRequest();

            var result = _mapper.Map<List<CommandRead>>(commands);
            return Ok(result);
        }

        // GET /commands/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandRead>> GetById(long id)
        {
            var command = await _commandService.GetById(id);
            if (command == null)
                return NotFound();

            var result = _mapper.Map<CommandRead>(command);
            return Ok(result);
        }

        // POST /commands/
        [HttpPost]
        public async Task<ActionResult<CommandRead>> Add(CommandCreate commandCreate)
        {
            if (commandCreate == null)
                return BadRequest();

            var commandModel = _mapper.Map<Command>(commandCreate);
            var commandCreated = await _commandService.Create(commandModel);

            var result = _mapper.Map<CommandRead>(commandCreated);
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
        }

        // PATCH /commands/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult<CommandRead>> Update(long id, CommandUpdate commandUpdate)
        {
            var commandToUpdate = await _commandService.GetById(id);

            if (commandToUpdate == null)
                return NotFound();

            _mapper.Map(commandUpdate, commandToUpdate);

            var result = await _commandService.PartialUpdate(commandToUpdate);

            var commandRead = _mapper.Map<CommandRead>(result);

            return Ok(commandRead);
        }

        // DELETE /commands/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _commandService.Delete(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}

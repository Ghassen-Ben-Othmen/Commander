using API.ViewModels.Argument;
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
    public class ArgumentsController : BaseApiController
    {
        private readonly ArgumentService _argumentService;
        private readonly IMapper _mapper;

        public ArgumentsController(ArgumentService argumentService, IMapper mapper)
        {
            _argumentService = argumentService;
            _mapper = mapper;
        }

        // POST /arguments/
        [HttpPost]
        public async Task<ActionResult<ArgumentRead>> Add(ArgumentCreate argumentCreate)
        {
            if (argumentCreate == null)
                return BadRequest();

            var argumentModel = _mapper.Map<Argument>(argumentCreate);
            var result = await _argumentService.Create(argumentModel);

            if (result == null)
                return BadRequest();

            var argumentRead = _mapper.Map<ArgumentRead>(result);
            return Created("", argumentRead);
        }

        // PATCH /arguments/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult<ArgumentRead>> Update(long id, ArgumentUpdate argumentUpdate)
        {
            if (argumentUpdate == null)
                return BadRequest();

            var argument = await _argumentService.GetById(id);

            _mapper.Map(argumentUpdate, argument);
            var result = await _argumentService.PartialUpdate(argument);
            var argumentRead = _mapper.Map<ArgumentRead>(result);

            if (argumentRead == null)
                return NotFound();
            return Ok(argumentRead);
        }

        // DELETE /arguments/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _argumentService.Delete(id);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}

using GithubIntegration.AppService.Contract.Interface;
using GithubIntegration.AppService.Contract.Request;
using GithubIntegration.AppService.Contract.ViewModel;
using GithubIntegration.Infra.CrossCutting.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GithubIntegration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitRepositoryController
        : ControllerBase
    {
        private readonly IGitRepositorioAppService _service;

        public GitRepositoryController(IGitRepositorioAppService service)
        {
            _service = service;
        }

        // GET api/<GitRepositoryController>/5
        [HttpGet("GetByUser/{user}")]
        public async Task<IActionResult> GetByUser(string user)
        {
            try
            {
                var result = await _service.GetRepositoriesByUser(user).ConfigureAwait(false);
                if (!result.IsNull())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<GitRepositoryController>/5
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var result = await _service.GetRepositoriesByName(name).ConfigureAwait(false);
                if (!result.IsNull())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<GitRepositoryController>/5
        [HttpGet("GetByOwnerAndRepository")]
        public async Task<IActionResult> GetByOwnerAndRepository([FromQuery]RepositoryRequest requestRepository)
        {
            try
            {
                var result = await _service.GetRepositoriesByOwnerAndRepository(requestRepository).ConfigureAwait(false);
                if (!result.IsNull())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<GitRepositoryController>/5
        [HttpGet("GetContributorsByRepository")]
        public async Task<IActionResult> GetContributorsByRepository([FromQuery] RepositoryRequest requestRepository)
        {
            try
            {
                var result = await _service.GetContributorsRepository(requestRepository).ConfigureAwait(false);
                if (!result.IsNull())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<GitRepositoryController>/5
        [HttpGet("GetFavoriteRepositories")]
        public async Task<IActionResult> GetFavoriteRepositories()
        {
            try
            {
                var result = await _service.GetFavoriteRepositories().ConfigureAwait(false);
                if (!result.IsNull())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new { });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<GitRepositoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FavoriteRepositoryVM favoriteVM)
        {
            try
            {
                var result = await _service.Add(favoriteVM).ConfigureAwait(false);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Impossível salvar arquivo");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<GitRepositoryController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<GitRepositoryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
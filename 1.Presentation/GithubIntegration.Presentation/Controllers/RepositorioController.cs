using GithubIntegration.AppService.Contract.Interface;
using GithubIntegration.AppService.Contract.Request;
using GithubIntegration.AppService.Contract.View;
using GithubIntegration.Infra.CrossCutting.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubIntegration.Presentation.Controllers
{
    public class RepositorioController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IGitRepositorioAppService _gitRepositorioAppService;

        private readonly string _uriGetRepositoriesByUser;
        private readonly string _uriGetRepositoriesByName;
        private readonly string _uriGetRepositoryByOwnerAndRepository;
        private readonly string _uriGetContributorsRepository;
        private readonly string _repositoryFavoritePath;

        public RepositorioController(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IGitRepositorioAppService gitRepositorioAppService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _gitRepositorioAppService = gitRepositorioAppService;

            _uriGetRepositoriesByUser = _configuration.GetSection("GithubIntegration")["RepositoriesByUserUri"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _uriGetRepositoriesByName = _configuration.GetSection("GithubIntegration")["RepositoriesByNameUri"].ThrowIfNull("Uri Repositories By Name Github").ToString();
            _uriGetRepositoryByOwnerAndRepository = _configuration.GetSection("GithubIntegration")["RepositoryByOwnerAndRepository"].ThrowIfNull("Uri Repositories By User and Repository Github").ToString();
            _uriGetContributorsRepository = _configuration.GetSection("GithubIntegration")["RepositoryContributors"].ThrowIfNull("Uri Contributors By User and Repository Github").ToString();
            _repositoryFavoritePath = _configuration.GetSection("GithubIntegration")["RepositoryFavoritePath"].ThrowIfNull("Favorite Repositories").ToString();
        }

        // GET: RepositorioController
        public async Task<ActionResult<RepositoryRequest[]>> Index()
        {
            return View(await _gitRepositorioAppService.GetFavoriteRepositories().ConfigureAwait(false));
        }

        // GET: RepositorioController/Details/5
        public async Task<ActionResult<GitRepositoryView>> Details(RepositoryRequest requestRepository)
        {
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetRepositoryByOwnerAndRepository, requestRepository.nameRepository, requestRepository.loginOwner));
                var response = await _clientFactory.CreateClient("apiGitIntegration").SendAsync(request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    return View(await JsonSerializer.DeserializeAsync<GitRepositoryView>(responseStream));
                }
                else
                {
                    return View();
                }
            }

            return View();
        }

        // GET: RepositorioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepositorioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RepositorioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RepositorioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RepositorioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RepositorioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using GithubIntegration.AppService.Contract.Interface;
using GithubIntegration.AppService.Contract.Request;
using GithubIntegration.AppService.Contract.View;
using GithubIntegration.AppService.Contract.ViewModel;
using GithubIntegration.Infra.CrossCutting.Helper;
using GithubIntegration.Infra.CrossCutting.Helper.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubIntegration.AppService
{
    public class GitRepositorioAppService
        : IGitRepositorioAppService
    {
        private readonly string _uriGetRepositoriesByUser;
        private readonly string _uriGetRepositoriesByName;
        private readonly string _uriGetRepositoryByOwnerAndRepository;
        private readonly string _uriGetContributorsRepository;
        private readonly string _repositoryFavoritePath;

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public GitRepositorioAppService(
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;

            _uriGetRepositoriesByUser = _configuration.GetSection("GithubIntegration")["RepositoriesByUserUri"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _uriGetRepositoriesByName = _configuration.GetSection("GithubIntegration")["RepositoriesByNameUri"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _uriGetRepositoryByOwnerAndRepository = _configuration.GetSection("GithubIntegration")["RepositoryByOwnerAndRepository"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _uriGetContributorsRepository = _configuration.GetSection("GithubIntegration")["RepositoryContributors"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _repositoryFavoritePath = _configuration.GetSection("GithubIntegration")["RepositoryFavoritePath"].ThrowIfNull("Uri Repositories By User Github").ToString();
        }

        public async Task<GitRepositoryView[]> GetRepositoriesByUser(string user)
        {
            if (!user.IsNullOrEmpty())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetRepositoriesByUser, user));

                    var client = _clientFactory.CreateClient("github");

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        using var responseStream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<GitRepositoryView[]>(responseStream);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Usuário");
            }
        }

        public async Task<GitRepositoriesView> GetRepositoriesByName(string name)
        {
            if (!name.IsNullOrEmpty())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetRepositoriesByName, name));
                    var response = await _clientFactory.CreateClient("github").SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        using var responseStream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<GitRepositoriesView>(responseStream);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Nome do Repositório");
            }
        }

        public async Task<GitRepositoryView> GetRepositoriesByOwnerAndRepository(RepositoryRequest requestRepository)
        {
            if (!requestRepository.IsNull() && !requestRepository.loginOwner.IsNullOrEmpty() && !requestRepository.nameRepository.IsNullOrEmpty())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetRepositoryByOwnerAndRepository, requestRepository.loginOwner, requestRepository.nameRepository));
                    var response = await _clientFactory.CreateClient("github").SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        using var responseStream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<GitRepositoryView>(responseStream);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Owner e Nome do Repositório");
            }
        }

        public async Task<GitContributor[]> GetContributorsRepository(RepositoryRequest requestRepository)
        {
            if (!requestRepository.IsNull() && !requestRepository.loginOwner.IsNullOrEmpty() && !requestRepository.nameRepository.IsNullOrEmpty())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetContributorsRepository, requestRepository.loginOwner, requestRepository.nameRepository));
                    var response = await _clientFactory.CreateClient("github").SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        using var responseStream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<GitContributor[]>(responseStream);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException("Owner e Nome do Repositório");
            }
        }

        public async Task<bool> Add(FavoriteRepositoryVM vm)
        {
            vm.nameRepository = vm.nameRepository?.ToLowerInvariant();
            vm.loginOwner = vm.loginOwner?.ToLowerInvariant();

            if (!vm.IsNull() && !vm.loginOwner.IsNullOrEmpty() && !vm.nameRepository.IsNullOrEmpty())
            {
                var pathFile = _repositoryFavoritePath + "favorites.json";
                var fileExists = await Task.FromResult(File.Exists(pathFile)).ConfigureAwait(false);

                IList <FavoriteRepositoryVM> favorites = await Task.FromResult(fileExists
                ? new List<FavoriteRepositoryVM>(JsonSerializer.Deserialize<FavoriteRepositoryVM[]>(File.ReadAllText(pathFile)))
                : new List<FavoriteRepositoryVM>(1) { vm }).ConfigureAwait(false);

                // Valida se o favorito já está contido no json local
                if (!favorites.Any(f => f.loginOwner.Equals(vm.loginOwner) && f.nameRepository.Equals(vm.nameRepository)))
                {
                    favorites.Add(vm);
                }
                else if (fileExists && favorites.Count() == 1) // Favorito já registrado
                {
                    return true;
                }

                if (!fileExists || !Directory.Exists(_repositoryFavoritePath)) // Caso o diretório não exista, cria.
                {
                    await Task.FromResult(Directory.CreateDirectory(_repositoryFavoritePath)).ConfigureAwait(false);
                }

                string favoritesStr = await Task.FromResult(JsonSerializer.Serialize(favorites)).ConfigureAwait(false);

                await Task.Factory.StartNew(() => File.WriteAllText(pathFile, favoritesStr)).ConfigureAwait(false);

                return File.Exists(pathFile);
            }
            else
            {
                throw new ArgumentNullException("Informe a entidade Favorita corretamente");
            }
        }

        public async Task<FavoriteRepositoryVM[]> GetFavoriteRepositories()
        {
            var pathFile = _repositoryFavoritePath + "favorites.json";
            var fileExists = await Task.FromResult(File.Exists(pathFile)).ConfigureAwait(false);

            IList<FavoriteRepositoryVM> favorites = fileExists
                ? await Task.FromResult(JsonSerializer.Deserialize<FavoriteRepositoryVM[]>(File.ReadAllText(pathFile))).ConfigureAwait(false)
                : null;

            return favorites?.ToArray();
        }
    }
}
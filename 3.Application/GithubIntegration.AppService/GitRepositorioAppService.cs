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
        private readonly string _uriGetRepositoryByUri;
        private readonly string _uriGetContributorsRepository;

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
            _uriGetRepositoryByUri = _configuration.GetSection("GithubIntegration")["RepositoryByUri"].ThrowIfNull("Uri Repositories By User Github").ToString();
            _uriGetContributorsRepository = _configuration.GetSection("GithubIntegration")["ContributorRepository"].ThrowIfNull("Uri Repositories By User Github").ToString();
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
                    var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_uriGetRepositoryByUri, requestRepository.loginOwner, requestRepository.nameRepository));
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

        public async Task<bool> Add(FavoriteRepositoryVM vw)
        {
            IList<FavoriteRepositoryVM> favorites;

            if (File.Exists(@"C:\Users\Public\Documents\GitIntegration\favorites.json"))
            {
                var favoritesStr = await Task.FromResult(File.re(@"C:\Users\Public\Documents\GitIntegration\favorites.json"));
                if (!favoritesStr.IsNull())
                {
                    favorites = await JsonSerializer.DeserializeAsync<FavoriteRepositoryVM[]>(favoritesStr);
                }
            }
        }
    }
}
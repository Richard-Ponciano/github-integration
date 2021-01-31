﻿using GithubIntegration.AppService.Contract.Request;
using GithubIntegration.AppService.Contract.View;
using System.Threading.Tasks;

namespace GithubIntegration.AppService.Contract.Interface
{
    public interface IGitRepositorioAppService
    {
        public Task<GitRepositoryView[]> GetRepositoriesByUser(string user);
        public Task<GitRepositoriesView> GetRepositoriesByName(string name);
        public Task<GitRepositoryView> GetRepositoriesByOwnerAndRepository(RepositoryRequest requestRepository);
        public Task<GitContributor[]> GetContributorsRepository(RepositoryRequest requestRepository);
    }
}
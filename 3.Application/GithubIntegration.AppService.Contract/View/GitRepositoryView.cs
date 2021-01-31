using System;

namespace GithubIntegration.AppService.Contract.View
{
    public class GitRepositoryView
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public DateTime updated_at { get; set; }
        public GitOwnerView owner { get; set; }
    }
}
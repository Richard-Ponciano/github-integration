namespace GithubIntegration.AppService.Contract.View
{
    public class GitContributor
        : GitOwnerView
    {
        public int contributions { get; set; }
    }
}
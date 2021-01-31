namespace GithubIntegration.AppService.Contract.View
{
    public class GitRepositoriesView
    {
        public int total_count { get; set; }

        public GitRepositoryView[] items { get; set; }
    }
}
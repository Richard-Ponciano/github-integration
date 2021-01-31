namespace GithubIntegration.Infra.CrossCutting.Helper.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Valida se a string é nula ou vazia
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
    }
}
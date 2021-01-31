using System;

namespace GithubIntegration.Infra.CrossCutting.Helper
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Valida se o objeto é nulo e dispara ArgumentNullException caso positivo
        /// </summary>
        /// <param name="o"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static object ThrowIfNull(this object o, string objName = null) => o ?? throw new ArgumentNullException(objName ?? nameof(o));

        /// <summary>
        /// Valida se o objeto é nulo
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNull(this object o) => o == null;
    }
}
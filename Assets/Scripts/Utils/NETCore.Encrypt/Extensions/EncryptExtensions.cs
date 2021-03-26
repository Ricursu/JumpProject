using System;
using System.Collections.Generic;
using System.Text;
using NETCore.Encrypt;
using NETCore.Encrypt.Shared;

namespace NETCore.Encrypt.Extensions
{
    public static class EncryptExtensions
    {
        /// <summary>
        /// String MD5 extension
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string MD5(this string srcString)
        {
            Check.Argument.IsNotEmpty(srcString, nameof(srcString));
            return EncryptProvider.Md5(srcString);
        }

        /// <summary>
        /// String HMACMD5 extensions
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string HMACMD5(this string srcString, string key)
        {
            Check.Argument.IsNotEmpty(srcString, nameof(srcString));
            return EncryptProvider.HMACMD5(srcString, key);
        }
    }
}

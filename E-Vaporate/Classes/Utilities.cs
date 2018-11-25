using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace E_Vaporate.Classes
{
    class Utilities
    {
        /// <summary>
        /// Generates a hashed and salted password for storage in the DB
        /// </summary>
        /// <param name="password">The Password to be hashed</param>
        /// <param name="_salt">The Salt for the hash, Use username for this</param>
        /// <returns></returns>
        public static byte[] GeneratePasswordSalt(string password, string _salt)
        {
            byte[] plainText = password.ToCharArray().Select(c => (byte)c).ToArray();
            byte[] salt = _salt.ToCharArray().Select(c => (byte)c).ToArray();
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        /// <summary>
        /// Simplifies logging in or getting users in general using only the username and password
        /// </summary>
        /// <param name="username">Username as a string</param>
        /// <param name="password">Password as a string</param>
        /// <returns>User object if one can be found, null if nothing can be found</returns>
        public static Model.User GetUser(string username, string password)
        {
            var context = new Model.EVaporateModel();
            if (username == string.Empty)
            {
                return null;
            }
            if (password == string.Empty)
            {
                return null;
            }
            using (context)
            {
                Model.User temp = context.Users.Where(u => u.Username.ToLower().Equals(username.ToLower(), StringComparison.CurrentCultureIgnoreCase)).First();
                if (temp != null)
                {
                    if (temp.HashedPassword.Sum(a=> int.Parse(a.ToString())) == GeneratePasswordSalt(password, username.ToLower()).Sum(a => int.Parse(a.ToString())))
                    {
                        return temp;
                    }
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool IsPublisher(Model.User user)
        {
            using (var context = new Model.EVaporateModel())
            {
                if (context.Publishers.Where(p => p.PublisherID == user.UserID).FirstOrDefault() != null)
                {
                    return true;
                }
                else return false;
            }
        }
    }
}

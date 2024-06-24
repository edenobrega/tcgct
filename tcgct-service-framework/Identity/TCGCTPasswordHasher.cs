using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Identity
{
	public class TCGCTPasswordHasher : IPasswordHasher<TCGCTUser>
	{
		public TCGCTPasswordHasher()
		{

		}

		private string createHash(string password, string salt, int n, int r, int p)
		{
			byte[] secret = Encoding.UTF8.GetBytes(password);
			byte[] salt_bytes = Encoding.UTF8.GetBytes(salt);

			byte[] hash_bytes = SCrypt.Generate(
				secret,
				salt_bytes,
				n,
				r,
				p,
				64
			);

			string hash = Convert.ToHexString(hash_bytes, 0, 64).ToLower();


			return $"scrypt:{n}:{r}:{p}${salt}${hash}";
		}

		public string HashPassword(TCGCTUser user, string password)
		{
			char[] SALT_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
			ReadOnlySpan<char> salt_chars = new ReadOnlySpan<char>(SALT_CHARS);
			string salt = new string(RandomNumberGenerator.GetItems<char>(salt_chars, 16));

			int n = 32768;
			int r = 8;
			int p = 1;

			return createHash(password, salt, n, r, p);
		}

		public PasswordVerificationResult VerifyHashedPassword(TCGCTUser user, string hashedPassword, string providedPassword)
		{
			string[] pwParts = hashedPassword.Split('$');
			string[] settings = pwParts[0].Split(':');

			if (!(int.TryParse(settings[1], out int n) && int.TryParse(settings[2], out int r) && int.TryParse(settings[3], out int p)))
			{
				return PasswordVerificationResult.Failed;
			}

			string hash = createHash(
				providedPassword,
				pwParts[1],
				n,
				r,
				p
			);

			if (hash == hashedPassword)
			{
				return PasswordVerificationResult.Success;
			}

			return PasswordVerificationResult.Failed;
		}
	}
}

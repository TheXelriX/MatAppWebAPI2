using MatAppWebAPI2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MatAppWebAPI2
{
	public class AuthService
	{
		public TokenModel GetToken(User user)
		{
			IEnumerable<Claim> claims = CollectUserClaimsAsync(user);
			JwtSecurityToken token = CreateToken(claims);
			string refreshToken = GenerateRefreshToken();
			return new TokenModel()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				//Implementera refresh
				TokenExpirationInMinutes = 60 * 24 * 10
			};
		}

		private static IEnumerable<Claim> CollectUserClaimsAsync(User user)
		{
			yield return new Claim("username", user.Name);
			yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			yield return new("userid", user.Id.ToString());
			yield return new("usertype", user.Type.ToString());
		}

		private JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims)
		{
			string key = "a6689811-ba8b-419f-b8db-2f4ecf88c490";
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentNullException(nameof(key));

			SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(key));

			JwtSecurityToken token = new(
					issuer: "matapp",
					audience: "matapp",
					expires: DateTime.Now.AddMinutes(60 * 24 * 10),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return token;
		}

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}
	}

	public class TokenModel
	{
		public string? Token { get; set; }
		public int TokenExpirationInMinutes { get; set; }
	}
}

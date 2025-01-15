using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenHelper
{
    private readonly byte[] _key;

    public JwtTokenHelper(string secretKeyBase64)
    {
        _key = Convert.FromBase64String(secretKeyBase64); // Decodifica la clave
    }

    public string GenerateToken(int userId, string role)
    {
    var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Asegúrate de incluir este claim
    new Claim(ClaimTypes.Role, role),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

        var key = new SymmetricSecurityKey(_key);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "EventsApi",
            audience: "EventsApi",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

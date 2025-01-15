using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using EventsApi.Domain.Enums;
using EventsApi.Models.Enums;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenHelper
{
    private readonly byte[] _key;

    public JwtTokenHelper(string secretKeyBase64)
    {
        _key = Convert.FromBase64String(secretKeyBase64);
    }

    public string GenerateToken(int userId, Rol role)
    {
        // Usamos el m�todo de extensi�n para obtener el valor string del enum.
        string roleValue = GetEnumMemberValue(role);

        Claim[] claims = new Claim[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(ClaimTypes.Role, roleValue),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(_key);
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "EventsApi",
            audience: "EventsApi",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private string GetEnumMemberValue(Enum enumValue)
    {
        var type = enumValue.GetType();
        var memberInfo = type.GetMember(enumValue.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
        return (attributes.Length > 0) ? ((EnumMemberAttribute)attributes[0]).Value : enumValue.ToString();
    }
}


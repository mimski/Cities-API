using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CityInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentException(nameof(configuration));
    }

    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(this.configuration["Authentication:SecretForKey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
        claimsForToken.Add(new Claim("given_name", user.FirstName));
        claimsForToken.Add(new Claim("family_name", user.LastName));
        claimsForToken.Add(new Claim("city", user.City));

        var jwtSecurityToken = new JwtSecurityToken(this.configuration["Authentication:Issuer"],
            this.configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private CityInfoUser ValidateUserCredentials(string? userName, string? password)
    {
        // we do not have a user DB or table. For demo purpose, we asume the creds are valid
        return new CityInfoUser(1, userName ?? "", "Joe", "Doe", "UK");
    }
}

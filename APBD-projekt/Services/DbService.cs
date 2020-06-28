using APBD_projekt.DTOs.Requests;
using APBD_projekt.DTOs.Responses;
using APBD_projekt.Exceptions;
using APBD_projekt.Generators;
using APBD_projekt.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace APBD_projekt.Services
{
    public class DbService : IDbService
    {
        private readonly s17878DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICalculatorService _calculatorService;

        public DbService(s17878DBContext context, IConfiguration configuration, ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
            _context = context;
            _configuration = configuration;
        }
        public CreateNewCampaignResponse CreateCampaign(CreateNewCampaignRequest request)
        {
            var fromBuilding = _context.Building.FirstOrDefault(b => b.IdBuilding == request.FromIdBuilding);
            var toBuilding = _context.Building.FirstOrDefault(b => b.IdBuilding == request.ToIdBuilding);
            var client = _context.Client.SingleOrDefault(p => p.IdClient == request.IdClient);
            if (client == null)
            {
                throw new UserDoesntExistExcetion($"User with id = {request.IdClient} doesn't exist");
            }
            if (fromBuilding == null)
            {
                throw new BuildingDoesntExistException($"Building with id {request.FromIdBuilding} doesn't exist!");
            }
            if (toBuilding == null)
            {
                throw new BuildingDoesntExistException($"Building with id {request.ToIdBuilding} doesn't exist!");
            }
            if (request.StartDate > request.EndDate)
            {
                throw new WrongDateException($"Start date={request.StartDate} can't be grater than {request.EndDate}");
            }
            if (!fromBuilding.Street.Equals(toBuilding.Street) || !fromBuilding.City.Equals(toBuilding.City))
            {
                throw new BuildingsOnDifferentStreetsException("Buldings are on different streets!");
            }

            var newCampaign = new Campaign()
            {
                IdClient = request.IdClient,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PricePerSquareMeter = request.PricePerSquareMeter,
                FromIdBuilding = request.FromIdBuilding,
                ToIdBuilding = request.ToIdBuilding
            };

            var bannerBuildings = _context.Building
                        .Where(b => b.StreetNumber >= fromBuilding.StreetNumber && b.StreetNumber <= toBuilding.StreetNumber)
                        .OrderByDescending(b => b.Height)
                        .ToList();

            var banerAreas = _calculatorService.CalculateMinBanerArea(bannerBuildings, fromBuilding, toBuilding);
            var newBanner1 = new Banner()
            {
                Name = 1,
                Price = banerAreas[0] * request.PricePerSquareMeter,
                Campaign = newCampaign,
                Area = banerAreas[0]
            };

            var newBanner2 = new Banner()
            {
                Name = 2,
                Price = banerAreas[1] * request.PricePerSquareMeter,
                Campaign = newCampaign,
                Area = banerAreas[1]
            };

            _context.Campaign.Add(newCampaign);
            _context.Banner.AddRange(newBanner1, newBanner2);
            _context.SaveChanges();

            return new CreateNewCampaignResponse()
            {
                IdClient = request.IdClient,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PricePerSquareMeter = request.PricePerSquareMeter,
                FromIdBuilding = request.FromIdBuilding,
                ToIdBuilding = request.ToIdBuilding,
                Banners = new List<Banner>() { newBanner1, newBanner2 },
                Price = (newBanner1.Area + newBanner2.Area) * request.PricePerSquareMeter
            };
        }

        public List<ListCampaignsResponse> ListCampains()
        {
            var result = from campaign in _context.Set<Campaign>()
                         join client in _context.Set<Client>()
                         on campaign.IdClient equals client.IdClient
                         orderby campaign.StartDate descending
                         select new ListCampaignsResponse()
                         {
                             IdCampaign = campaign.IdCampaign,
                             Client = new ClientResponse()
                             {
                                 FirstName = client.FirstName,
                                 LastName = client.LastName,
                                 Email = client.Email,
                                 Phone = client.Phone,
                                 Login = client.Login,
                             },
                             Banners = _context.Banner.Where(b => b.IdCampaign == campaign.IdCampaign).ToList(),
                             StartDate = campaign.StartDate,
                             EndDate = campaign.StartDate,
                             PricePerSquareMeter = campaign.PricePerSquareMeter,
                             FromIdBuilding = campaign.FromIdBuilding,
                             ToIdBuilding = campaign.ToIdBuilding
                         };
            return result.ToList();
        }
        public LoginResponse Login(LoginRequest request)
        {
            var client = _context.Client.SingleOrDefault(p => p.Login == request.Login);
            if (client == null)
            {
                throw new UserDoesntExistExcetion($"User with {request.Login} login doesn't exist");
            }
            if(!client.Password.Equals(HashGenerator.CreateHashForPassword(request.Password, client.Salt)))
            {
                throw new WrongPasswordException("Wrong Password!");
            }
            var clientClaims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(client.IdClient)),
                new Claim(ClaimTypes.Name, client.Login),
                new Claim(ClaimTypes.Role, "Client")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "s17878@pjwstk.edu.pl",
                audience: "Clients",
                claims: clientClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );
            client.RefreshToken = RefreshTokenGenerator.CreateRefreshToken();
            client.TokenExpirationDate = DateTime.Now.AddDays(7);
            _context.SaveChanges();

            return new LoginResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = client.RefreshToken
            };
        }

        public LoginResponse RefreshJwtToken(string refreshToken)
        {
            var client = _context.Client.SingleOrDefault(p => p.RefreshToken == refreshToken);
            if (client == null)
            {
                throw new UserDoesntExistExcetion("Couldn't find user with this refresh token");
            }
            if (client.TokenExpirationDate < DateTime.Now)
            {
                throw new RefreshTokenExpiredException("Refresh token has expired");
            }

            var clientClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(client.IdClient)),
                new Claim(ClaimTypes.Name, client.Login),
                new Claim(ClaimTypes.Role, "Client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "s17878@pjwstk.edu.pl",
                audience: "Clients",
                claims: clientClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
              );
            client.RefreshToken = RefreshTokenGenerator.CreateRefreshToken();
            client.TokenExpirationDate = DateTime.Now.AddDays(7);
            _context.SaveChanges();

            return new LoginResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = client.RefreshToken
            };

        }

        public RegisterResponse Register(RegisterRequest request)
        {
            var userExists = _context.Client.Any(c => c.Login.Equals(request.Login));
            if (userExists) throw new UserAlreadyExistsException("User already Exists!");

            var salt = SaltGenerator.CreateSalt();

            var newClient = new Client()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Login = request.Login,
                Password = HashGenerator.CreateHashForPassword(request.Password, salt),
                Salt = salt,
                RefreshToken = RefreshTokenGenerator.CreateRefreshToken(),
                TokenExpirationDate = DateTime.Now.AddDays(7)
            };

            var newClientClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(newClient.IdClient)),
                new Claim(ClaimTypes.Name, newClient.Login),
                new Claim(ClaimTypes.Role, "Client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "s17878@pjwstk.edu.pl",
                audience: "Clients",
                claims: newClientClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );


            _context.Client.Add(newClient);
            _context.SaveChanges();

            return new RegisterResponse() { 
                        FirstName = newClient.FirstName,
                        LastName = newClient.LastName,
                        Email = newClient.Email,
                        Phone = newClient.Phone,
                        Login = newClient.Login,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = newClient.RefreshToken
            };
        }
    }
}

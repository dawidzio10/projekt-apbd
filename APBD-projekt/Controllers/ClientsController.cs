using APBD_projekt.DTOs.Requests;
using APBD_projekt.Exceptions;
using APBD_projekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_projekt.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ClientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var response = _dbService.Login(request);
                return Ok(response);
            }
            catch (UserDoesntExistExcetion e)
            {
                return NotFound(e.Message);
            }
            catch (WrongPasswordException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            try
            {
                var response = _dbService.Register(request);
                return CreatedAtAction("Register",response);
            }
            catch (UserAlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("refreshToken/{refreshToken}")]
        public IActionResult RefreshJwtToken(string refreshToken)
        {
            try
            {
                var response = _dbService.RefreshJwtToken(refreshToken);
                return Ok(response);
            }
            catch (UserDoesntExistExcetion e)
            {
                return NotFound(e.Message);
            }
            catch (RefreshTokenExpiredException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult ListCampaigns()
        {
            var response = _dbService.ListCampains();
            return Ok(response);
        }
        [HttpPost]
        [Route("createcampaign")]
        public IActionResult CreateCampaign(CreateNewCampaignRequest request)
        {
            try
            {
                var response = _dbService.CreateCampaign(request);
                return CreatedAtAction("CreateCampaign", response);
            }
            catch (BuildingDoesntExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (BuildingsOnDifferentStreetsException e)
            {
                return BadRequest(e.Message);
            }
            catch (UserDoesntExistExcetion e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
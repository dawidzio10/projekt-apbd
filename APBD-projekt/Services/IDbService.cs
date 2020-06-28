using APBD_projekt.DTOs.Requests;
using APBD_projekt.DTOs.Responses;
using System.Collections.Generic;

namespace APBD_projekt.Services
{
    public interface IDbService
    {
        public RegisterResponse Register(RegisterRequest request);
        public LoginResponse RefreshJwtToken(string refreshToken);
        public LoginResponse Login(LoginRequest request);
        public List<ListCampaignsResponse> ListCampains();
        public CreateNewCampaignResponse CreateCampaign(CreateNewCampaignRequest request);
    }
}

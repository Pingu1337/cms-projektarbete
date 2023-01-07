namespace frontend.Services
{
    public interface IAuthService
    {
        Task<bool> IsSignedIn();
        Task<string> GetUsernameAsync();
        Task<string> GetJwt();
    }
    public class AuthService : IAuthService
    {
        private ILocalStorageService _localStorageService;
        public AuthService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<bool> IsSignedIn()
        {
            var Token = await _localStorageService.GetCookie<string>("JWT");
            if (Token != null)
            {
                if (Token.StartsWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GetUsernameAsync()
        {
            var username = await _localStorageService.GetCookie<string>("Username");
            if (username == null)
            {
                throw new Exception("Username is null");
            }
            else
            {
                return username;
            }
        }
        public async Task<string> GetJwt()
        {   
            var Token = await _localStorageService.GetCookie<string>("JWT");
            if (Token == null)
            {
                return string.Empty;
            }
            else
            {
                return Token;
            }
        }
    } 
}

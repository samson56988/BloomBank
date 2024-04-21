using CustomerAPI.Dtos;

namespace CustomerAPI.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<Tuple<object, string>> CreateLoginDetails(CreateLoginDetails auth);

        Task<Tuple<object, string>> Login(CreateLoginDetails auth);
    }
}

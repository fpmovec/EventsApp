using Application.Services;
using Castle.Core.Logging;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Web.Controllers;
using Web.ViewModels;

namespace Tests.AuthenticationTests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;


        public AuthenticationControllerTests()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                _userStoreMock.Object, null, null, null, null, null, null, null, null);
        }


        [Theory]
        [InlineData("Test name", "Test password", "e@e.com", "InvalidPhone")]
        [InlineData("Test name", "Test password", "e@e", "+375331111111")]
        public async Task Register_User_Returns_Bad_Request(string name, string password, string email, string phone)
        {
            var authController = new AuthenticationController(
                _userManagerMock.Object, _jwtServiceMock.Object, Mock.Of<ILogger<AuthenticationController>>()
                );

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());

            IActionResult? result = await authController.RegisterUser(new RegisterViewModel(name, password, email, phone));

            result = result as BadRequestObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Register_User_Returns_Ok()
        {
            var authController = new AuthenticationController(
                _userManagerMock.Object, _jwtServiceMock.Object, Mock.Of<ILogger<AuthenticationController>>()
                );

            IdentityUser newUser = new() { Email = "e@e.com", UserName = "Username", PhoneNumber = "80331111111" };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<IdentityUser>(null));
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            IActionResult result = await authController.RegisterUser(new("Username", "Password", "Email", "80331111111"));

            _jwtServiceMock.Verify(
                j => j.GenerateJwtTokensAsync(It.Is<IdentityUser>(
                    s => s.Email == newUser.Email || s.UserName == newUser.UserName), default), Times.Once());

            Assert.NotNull(result as OkObjectResult);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore.Users;
using ZCYX.FRMSCore.Users.Dto;

namespace ZCYX.FRMSCore.Tests.Users
{
    public class UserAppService_Tests : FRMSCoreTestBase
    {
        private readonly IUserAppService _userAppService;
        private readonly Authorization.IAbpPermissionBaseAppService _abpPermissionBaseAppService;

        public UserAppService_Tests()
        {

            _userAppService = Resolve<IUserAppService>();
            _abpPermissionBaseAppService = Resolve<Authorization.IAbpPermissionBaseAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            LoginAsHostAdmin();

            // Act
            var output = await _userAppService.GetAll(new PagedResultRequestDto { MaxResultCount = 20, SkipCount = 0 });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        //[Fact]
        [MultiTenantFact]
        public async Task CreateUser_Test()
        {
            LoginAsHostAdmin();

            // Act
            await _userAppService.Create(
                new CreateUserDto
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = "john.nash",
                    WorkNumber = "12345678",
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}

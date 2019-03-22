using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore.Users;
using ZCYX.FRMSCore.Users.Dto;
using Project;

namespace ZCYX.FRMSCore.Tests.ReplyUnit
{
    public class ReplyUnitAppService_Test : FRMSCoreTestBase
    {
        private readonly IReplyUnitAppService _replyUnitAppService;

        public ReplyUnitAppService_Test()
        {

            _replyUnitAppService = Resolve<IReplyUnitAppService>();
        }



        [Fact]
        public async Task Create_Test()
        {
            LoginAsHostAdmin();
            AbpSession.UserId.Value.ShouldBe(1);
            // Act
            await _replyUnitAppService.CreateOrUpdate(new ReplyUnitDto() { Name = "Test123", Sort = 11 });
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.ReplyUnit.FirstOrDefaultAsync(u => u.Name == "Test123");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}

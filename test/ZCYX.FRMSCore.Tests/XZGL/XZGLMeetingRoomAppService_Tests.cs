using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using MeetingGL;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XZGL;
using ZCYX.FRMSCore.Roles;

namespace ZCYX.FRMSCore.Tests.XZGL
{
   public  class XZGLMeetingRoomAppService_Tests: FRMSCoreTestBase
    {
        private readonly Authorization.IAbpPermissionBaseAppService _abpPermissionBaseAppService;

        private readonly IXZGLMeetingRoomAppService _meetingRoomAppService;
        public XZGLMeetingRoomAppService_Tests( )
        {
            _meetingRoomAppService = Resolve<IXZGLMeetingRoomAppService>();
            _abpPermissionBaseAppService = Resolve<Authorization.IAbpPermissionBaseAppService>();
        }

        [Fact]
        public async Task GetXZGLMeetingRooms_Test()
        {
            LoginAsHostAdmin();
            //// Act
            var output = await _meetingRoomAppService.GetList(new GetXZGLMeetingRoomListInput { MaxResultCount = 20, SkipCount = 0 });

            //// Assert
            output.Items.Count.ShouldBeGreaterThan(-1);
        }
        [MultiTenantFact]
        public async Task CreateXZGLMeetingRoom_Test()
        {
            LoginAsHostAdmin();

            // Act
            await _meetingRoomAppService.Create(
                new CreateXZGLMeetingRoomInput
                {
                    Name = "test",
                    Address = "test",
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.XZGLMeetingRoom.FirstOrDefaultAsync(u => u.Name == "test");
                johnNashUser.ShouldNotBeNull();
            });
        }
        [Fact]
        public async void DeleteXZGLMeetingRoom_Test()
        {
            LoginAsHostAdmin();

            //Arrange
            await _meetingRoomAppService.Create(
               new CreateXZGLMeetingRoomInput
               {
                   Name = "test",
                   Address = "test",
               });
            var defaultTask = UsingDbContext(ctx => ctx.XZGLMeetingRoom.FirstOrDefault());

            //Act
            var d = new EntityDto<Guid>(defaultTask.Id);
            _meetingRoomAppService.Delete(d);

            //Assert
            var task = UsingDbContext(ctx => ctx.XZGLMeetingRoom.FirstOrDefault(t => t.Id == defaultTask.Id && !t.IsDeleted));
            task.ShouldBeNull();
        }
    }
}

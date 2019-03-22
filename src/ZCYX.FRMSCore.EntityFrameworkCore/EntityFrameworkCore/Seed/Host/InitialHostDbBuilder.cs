using ZCYX.FRMSCore.Authorization;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly FRMSCoreDbContext _context;
        private readonly IAbpPermissionBaseAppService _abpPermissionBaseAppService;


        public InitialHostDbBuilder(FRMSCoreDbContext context, IAbpPermissionBaseAppService abpPermissionBaseAppService)
        {
            _context = context;
            _abpPermissionBaseAppService = abpPermissionBaseAppService;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context, _abpPermissionBaseAppService).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}

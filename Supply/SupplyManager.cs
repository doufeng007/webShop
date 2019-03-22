using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Reflection.Extensions;
using Castle.Windsor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.UI;
using Supply.Entity;
using Abp.WorkFlow;
using Abp.File;

namespace Supply
{
    [RemoteService(IsEnabled = false)]
    public class SupplyManager : ApplicationService
    {

        private readonly IRepository<SupplyApplyMain, Guid> _supplyApplyMainRepository;
        private readonly IRepository<SupplyApplySub, Guid> _supplyApplySubRepository;
        private readonly IRepository<SupplyApplyResult, Guid> _supplyApplyResultRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IRepository<SupplyPurchaseMain, Guid> _supplyPurchaseMainRepository;
        private readonly IRepository<SupplyPurchasePlan, Guid> _supplyPurchasePlaneRepository;
        private readonly IRepository<SupplyPurchaseResult, Guid> _supplyPurchaseResultRepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;

        public SupplyManager(IRepository<SupplyApplyMain, Guid> supplyApplyMainRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<User, long> userRepository, 
            IRepository<SupplyApplySub, Guid> supplyApplySubRepository, ISupplyBaseRepository supplyBaseRepository
    , IRepository<SupplyPurchaseMain, Guid> supplyPurchaseMainRepository, IRepository<SupplyPurchasePlan, Guid> supplyPurchasePlaneRepository
    , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
    , IRepository<SupplyApplyResult, Guid> supplyApplyResultRepository, IRepository<UserSupply, Guid> userSupplyRepository
    , IRepository<SupplyPurchaseResult, Guid> supplyPurchaseResultRepository)
        {
            _supplyApplyMainRepository = supplyApplyMainRepository;
            _supplyApplySubRepository = supplyApplySubRepository;
            _supplyBaseRepository = supplyBaseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _userRepository = userRepository;
            _supplyPurchaseMainRepository = supplyPurchaseMainRepository;
            _supplyPurchasePlaneRepository = supplyPurchasePlaneRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _supplyApplyResultRepository = supplyApplyResultRepository;
            _userSupplyRepository = userSupplyRepository;
            _supplyPurchaseResultRepository = supplyPurchaseResultRepository;
        }

        

    }
}

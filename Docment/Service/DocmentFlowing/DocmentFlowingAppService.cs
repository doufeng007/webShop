using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore.Application;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;
using Abp.WorkFlowDictionary;
using Abp.AutoMapper;

namespace Docment
{
    public class DocmentFlowingAppService : IDocmentFlowingAppService
    {
        private readonly IRepository<DocmentFlowing, Guid> _docmentFlowingRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<DocmentList, Guid> _docmentRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitsRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _orgRepository;
        private readonly IRepository<DocmentBorrowSub, Guid> _docmentBorrowSubRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        public DocmentFlowingAppService(IRepository<DocmentFlowing, Guid> docmentFlowingRepository, IRepository<User, long> userRepository, IRepository<WorkFlowOrganizationUnits, long> orgRepository,
            IRepository<DocmentList, Guid> docmentRepository, IRepository<DocmentBorrowSub, Guid> docmentBorrowSubRepository, IRepository<AbpDictionary, Guid> abpDictionaryRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository, OrganizationUnitManager organizationUnitManager) {
            _docmentFlowingRepository = docmentFlowingRepository;
            _userRepository = userRepository;
            _docmentRepository = docmentRepository;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _orgRepository = orgRepository;
            _organizationUnitManager = organizationUnitManager;
            _docmentBorrowSubRepository = docmentBorrowSubRepository;
            _abpDictionaryRepository = abpDictionaryRepository;
        }
        /// <summary>
        /// 创建外部流转记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOutFlowing(DocmentFlowingInput input)
        {
            if (input.DocmentIds == null || input.DocmentIds.Count == 0) {
                throw new Abp.UI.UserFriendlyException("请至少选择一个档案。");
            }
            foreach (var docid in input.DocmentIds)
            {
                var doc = _docmentRepository.Get(docid);
                doc.IsOut = true;//外部借阅，档案状态改为外部
                await _docmentFlowingRepository.InsertAsync(new DocmentFlowing()
                {
                    Des = input.Des,
                    IsOut = true,
                    DocmentId = docid,
                    UserName = input.UserName
                });
            }
        }
        /// <summary>
        /// 获取档案流转记录
        /// </summary>
        /// <param name="docmentId"></param>
        /// <returns></returns>
        public List<DocmentFlowingDto> GetAll(Guid docmentId)
        {
            var ret = from a in _docmentFlowingRepository.GetAll()
                      join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                      where a.DocmentId == docmentId
                      select new DocmentFlowingDto()
                      {
                          Des = a.Des,
                          DocmentId = a.DocmentId,
                          Id = a.Id,
                          IsOut = a.IsOut,
                          CreatorUserId = a.CreatorUserId.Value,
                          UserName = b.Name,
                          CreationTime = a.CreationTime
                      };
            return ret.ToList();
        }
        /// <summary>
        /// 获取统计的详情列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocmentListDto>> GetDocmentList(SearchInput input)
        {
            var userids = new List<long>();
            if (input.Type == Type.用户) {
                userids.Add(input.Id);
            } else {
                var orgids = new List<long>();
                orgids.Add(input.Id);
               var child=  _organizationUnitManager.FindChildren(input.Id).Select(ite=>ite.Id).ToList();
                if (child != null && child.Count > 0) {
                    orgids.AddRange(child);
                }
                var orguser = _userOrganizationUnitsRepository.GetAll().Where(ite =>orgids.Contains( ite.OrganizationUnitId)&&ite.IsMain).Select(ite => ite.UserId).Distinct().ToList();
                userids.AddRange(orguser);
            }
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        join c in _userRepository.GetAll() on a.UserId equals c.Id
                        where userids.Contains(a.UserId.Value)
                        select new DocmentListDto()
                        {
                            CreationTime = a.CreationTime,
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Id = a.Id,
                            QrCodeId = a.QrCodeId,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            Type = a.Type,
                            IsOld = a.IsOld,
                            IsOut = a.IsOut,
                            IsProject = a.IsProject,
                            ArchiveId = a.ArchiveId,
                            Type_Name = b.Title,
                            UserId = a.UserId,
                            Des = a.Des,
                            Status = (int)a.Status,
                            NeedBack = a.NeedBack,
                            UserId_Name=c.Name,
                            StatusTitle = ((DocmentStatus)a.Status).ToString()
                        };
            switch (input.ListType) {
                case ListType.资料总数:
                    break;
                case ListType.发起归档:
                    query = query.Where(ite => ite.Status > 0);
                    break;
                case ListType.在外流转:
                    query = query.Where(ite => ite.IsOut);
                    break;
                case ListType.需归还档案:
                    query = query.Where(ite => ite.NeedBack);
                    break;
            }
            var count = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<DocmentListDto>(count, ret);
        }

        /// <summary>
        /// 档案统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DocmentStaticDto>> GetStatic(DocmentStaticSearshDto input)
        {
            var query = from a in _docmentRepository.GetAll()
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        join c in _userOrganizationUnitsRepository.GetAll() on new { UserId = a.UserId.GetValueOrDefault(), IsMain = true } equals new { UserId = c.UserId, IsMain = c.IsMain }
                        join d in _orgRepository.GetAll() on c.OrganizationUnitId equals d.Id
                        group a by new { a.UserId, b.Name, c.OrganizationUnitId, d.Code, d.DisplayName, d.ParentId } into e
                        select new DocmentStaticDto()
                        {
                            OrgCode = e.Key.Code,
                            OrgId = e.Key.OrganizationUnitId,
                            OrgName = e.Key.DisplayName,
                            ParentId=e.Key.ParentId,
                            UserId = e.Key.UserId,
                            UserName = e.Key.Name,
                            DocmentCount = e.Count(g => g.Id != null),
                            WaitDocmentCount = e.Count(g => g.Status > 0),
                            OutDocmentCount=e.Count(g=>g.IsOut),
                            WaitBackDocmentCount=e.Count(g=>g.NeedBack)
                            //WaitBackDocmentCount=_docmentBorrowSubRepository.GetAll().
                            //Where(ite=>ite.CreatorUserId==e.Key.UserId&&ite.Status== BorrowSubStatus.使用中).Select(ite=>ite.DocmentId).Distinct().Count()
                        };
            if (input.StaticType == StaticType.Org) {
                var orgs = _orgRepository.GetAll().Select(ite => new DocmentStaticDto()
                {
                    OrgId = ite.Id,
                    ParentId = ite.ParentId,
                    OrgCode = ite.Code,
                    OrgName = ite.DisplayName
                }).ToList();
                var docs = query.ToList();
                foreach (var doc in docs) {
                    var o = orgs.FirstOrDefault(ite => ite.OrgId == doc.OrgId);
                    if (o != null) {
                        o.DocmentCount += doc.DocmentCount;
                        o.WaitDocmentCount += doc.WaitDocmentCount;
                        o.WaitBackDocmentCount += doc.WaitBackDocmentCount;
                        o.OutDocmentCount += doc.OutDocmentCount;
                    }
                    orgs.Add(new DocmentStaticDto {
                        ParentId = doc.OrgId,
                        OrgCode = doc.OrgCode,
                        OrgName = doc.UserName,
                        DocmentCount = doc.DocmentCount,
                        OutDocmentCount = doc.OutDocmentCount,
                        WaitBackDocmentCount = doc.WaitBackDocmentCount,
                        WaitDocmentCount = doc.WaitDocmentCount,
                        UserId=doc.UserId,
                        UserName=doc.UserName
                    });
                    while (o.ParentId.HasValue) {
                        o= orgs.FirstOrDefault(ite => ite.OrgId == o.ParentId);
                        o.DocmentCount += doc.DocmentCount;
                        o.WaitDocmentCount += doc.WaitDocmentCount;
                        o.WaitBackDocmentCount += doc.WaitBackDocmentCount;
                        o.OutDocmentCount += doc.OutDocmentCount;
                    }
                }
                return new PagedResultDto<DocmentStaticDto>(orgs.Count,orgs);
            } else {
                var count = query.Count();
                var ret = await query.OrderByDescending(r => r.UserName).PageBy(input).ToListAsync();
                return new PagedResultDto<DocmentStaticDto>(count,ret);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using HR.Service.EmployeeRequire.Dto;
using ZCYX.FRMSCore;
using System.Linq;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Application;
using Abp.Organizations;
using Abp.UI;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Abp.IdentityFramework;
using ZCYX.FRMSCore.Model;

namespace HR.Service
{
    [AbpAuthorize]
    public class EmployeeAppService : FRMSCoreAppServiceBase, IEmployeeAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly AbpOrganizationUnitsManager _organizationUnitManager;
        private readonly IRepository<EducationExperience, Guid> _educationExperienceRepository;
        private readonly IRepository<WorkExperience, Guid> _workExperienceRepository;
        private readonly IRepository<EmployeeFamily, Guid> _employeeFamilyRepository;
        private readonly IRepository<EmployeeSkill, Guid> _employeeSkillRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly UserManager _userManager;
        public EmployeeAppService(IRepository<Employee, Guid> employeeRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IRepository<EducationExperience, Guid> educationExperienceRepository,
            IRepository<WorkExperience, Guid> workExperienceRepository,
           IRepository<EmployeeFamily, Guid> employeeFamilyRepository,
           AbpOrganizationUnitsManager organizationUnitManager,
           IRepository<PostInfo, Guid> postsRepository, IRepository<UserPosts, Guid> userPostsRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository,
          IRepository<EmployeeSkill, Guid> employeeSkillRepository, IRepository<User, long> userRepository,
            UserManager userManager)
        {
            _employeeRepository = employeeRepository;
            _educationExperienceRepository = educationExperienceRepository;
            _employeeFamilyRepository = employeeFamilyRepository;
            _employeeSkillRepository = employeeSkillRepository;
            _workExperienceRepository = workExperienceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _postsRepository = postsRepository;
            _userPostsRepository = userPostsRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        /// <summary>
        /// 员工个人信息初始化或编辑
        /// </summary>
        public async Task CreateOrUpdate(EmployeeInput input)
        {
            //var eid = Guid.Empty;
            Employee model = null;
            
            if (input.Id.HasValue)
            {

                model = _employeeRepository.Get(input.Id.Value);
                ObjectMapper.Map(input, model);
                model.Birthday2 = this.SolarToChineseLunisolarDate(model.Birthday);
                var user = await UserManager.GetUserByIdAsync(model.UserId.Value);
                user.EnterTime = input.EnterTime;
                model.Email = user.EmailAddress;
                model.Phone = user.PhoneNumber;
                model.Sex =user.Sex.Value?1:0;
                model.IdCard = user.IdCard;
                model.EnterTime = user.EnterTime;
               CheckErrors(await _userManager.UpdateAsync(user));
                _employeeRepository.Update(model);
            }
            else
            {
                model = _employeeRepository.FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
                var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
                if (model != null)
                {
                    user.EnterTime = input.EnterTime;
                    //user.PhoneNumber = input.Phone;
                    //user.EmailAddress = input.Email;
                    //user.Sex = input.Sex == 0 ? false : true;
                    //user.IdCard = input.IdCard;
                    //user.EnterTime = input.EnterTime;
                    CheckErrors(await _userManager.UpdateAsync(user));

                    ObjectMapper.Map(input, model);
                    model.Name = user.Name;
                    model.Code = user.WorkNumber;
                    model.Email = user.EmailAddress;
                    model.Phone = user.PhoneNumber;
                    model.Sex = user.Sex.Value ? 1 : 0;
                    model.IdCard = user.IdCard;
                    model.EnterTime = user.EnterTime;
                    model.Birthday2 = this.SolarToChineseLunisolarDate(model.Birthday);
                    await _employeeRepository.UpdateAsync(model);                   
                }
                else
                {
                    model = input.MapTo<Employee>();
                    model.UserId = AbpSession.UserId.Value;
                    model.Name = user.Name;
                    model.Phone = user.PhoneNumber;
                    model.Email = user.EmailAddress;
                    model.Birthday2 = this.SolarToChineseLunisolarDate(model.Birthday);
                    model.Code = user.WorkNumber;
                    _employeeRepository.Insert(model);
                }

            }
           
            if (input.WorkExperience != null && input.WorkExperience.Count > 0)
            {
                var ids = input.WorkExperience.Select(ite => ite.Id).ToList();
                _workExperienceRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                foreach (var w in input.WorkExperience)
                {
                    if (w.Id.HasValue)
                    {
                        var wmodel = _workExperienceRepository.Get(w.Id.Value);

                        ObjectMapper.Map(w, wmodel);
                        _workExperienceRepository.Update(wmodel);
                    }
                    else
                    {
                        var wmodel = w.MapTo<WorkExperience>();
                        wmodel.EmployeeId = model.Id;
                        _workExperienceRepository.Insert(wmodel);
                    }
                }
            }
            if (input.EducationExperience != null && input.EducationExperience.Count > 0)
            {
                var ids = input.EducationExperience.Select(ite => ite.Id).ToList();
                _educationExperienceRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                foreach (var e in input.EducationExperience)
                {
                    if (e.Id.HasValue)
                    {
                        var edumodel = _educationExperienceRepository.Get(e.Id.Value);
                        ObjectMapper.Map(e, edumodel);
                        _educationExperienceRepository.Update(edumodel);
                    }
                    else
                    {
                        var edumodel = e.MapTo<EducationExperience>();
                        edumodel.EmployeeId = model.Id;
                        _educationExperienceRepository.Insert(edumodel);
                    }
                }
            }
            if (input.EmployeeFamily != null && input.EmployeeFamily.Count > 0)
            {
                var ids = input.EmployeeFamily.Select(ite => ite.Id).ToList();
                _employeeFamilyRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                foreach (var e in input.EmployeeFamily)
                {
                    if (e.Id.HasValue)
                    {
                        var fmodel = _employeeFamilyRepository.Get(e.Id.Value);

                        ObjectMapper.Map(e, fmodel);
                        _employeeFamilyRepository.Update(fmodel);
                    }
                    else
                    {
                        var fmodel = e.MapTo<EmployeeFamily>();
                        fmodel.EmployeeId = model.Id;
                        _employeeFamilyRepository.Insert(fmodel);
                    }
                }
            }
            if (input.EmployeeSkill != null && input.EmployeeSkill.Count > 0)
            {
                var ids = input.EmployeeSkill.Select(ite => ite.Id).ToList();
                _employeeSkillRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                foreach (var e in input.EmployeeSkill)
                {
                    if (e.Id.HasValue)
                    {
                        var smodel = _employeeSkillRepository.Get(e.Id.Value);
                        ObjectMapper.Map(e, smodel);
                        _employeeSkillRepository.Update(smodel);
                    }
                    else
                    {
                        var fmodel = e.MapTo<EmployeeSkill>();
                        fmodel.EmployeeId = model.Id;
                        _employeeSkillRepository.Insert(fmodel);
                    }
                }
            }
        }
        /// <summary>
        /// 获取员工个人信息
        /// </summary>
        /// <param name="input">员工id，不传表示获取当前登陆用户的员工信息</param>
        /// <returns></returns>
        public async Task<EmployeeDto> Get(Guid? input)
        {
            var ret = new EmployeeDto();
            Employee model = null;
            if (input.HasValue)
            {
                model = await _employeeRepository.GetAsync(input.Value);
                ret = model.MapTo<EmployeeDto>(); 
            }
            else
            {
                //获取当前用户员工信息
                model = _employeeRepository.GetAll().FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
                if (model == null)
                {
                    return null;
                }
                else
                {
                    ret = model.MapTo<EmployeeDto>();
                    
                }
            }
            var u =await UserManager.GetUserByIdAsync(ret.UserId.Value);
            ret.Name = u.Name;
            ret.Phone = u.PhoneNumber;
            ret.Email = u.EmailAddress;
            var edu = await _educationExperienceRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (edu != null && edu.Count > 0)
            {
                ret.EducationExperience = edu.MapTo<List<EducationExperienceDto>>();
            }
            var work = await _workExperienceRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (work != null && work.Count > 0)
            {
                ret.WorkExperience = work.MapTo<List<WorkExperienceDto>>();
            }
            var family = await _employeeFamilyRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (family != null && family.Count > 0)
            {
                ret.EmployeeFamily = family.MapTo<List<EmployeeFamilyDto>>();
            }
            var skill = await _employeeSkillRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (skill != null && skill.Count > 0)
            {
                ret.EmployeeSkill = skill.MapTo<List<EmployeeSkillDto>>();
            }
            var org = await (from a in _userOrganizationUnitRepository.GetAll()
                             join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                             where a.UserId == model.UserId
                             select new SimpleOrganizationDto()
                             {
                                 Code = b.Code,
                                 Id = a.OrganizationUnitId,
                                 IsMain = a.IsMain,
                                 Title = b.DisplayName
                             }).ToListAsync();
            ret.Organization = org;

            var post = from a in _postsRepository.GetAll()
                       join b in _userPostsRepository.GetAll() on a.Id equals b.PostId
                       join c in _organizationUnitPostsRepository.GetAll() on b.OrgPostId equals c.Id
                       join d in _organizationUnitRepository.GetAll() on c.OrganizationUnitId equals d.Id
                       where b.UserId == model.UserId
                       select new UserPostDto { Id = b.Id, OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, OrgName = d.DisplayName };
            ret.Posts = post.ToList();
            return ret;
        }
        /// <summary>
        /// 人事部获取员工信息列表
        /// </summary>
        public async Task<PagedResultDto<EmployeeListDto>> GetList(EmployeeSearchInput input)
        {
            var query = from a in _employeeRepository.GetAll()
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        orderby a.CreationTime descending
                        select new EmployeeListDto()
                        {
                            Alipay = a.Alipay,
                            BankName = a.BankName,
                            BankNo = a.BankNo,
                            Birthday = a.Birthday,
                            Code = a.Code,
                            Email = b.EmailAddress,
                            EnterTime = a.EnterTime,
                            Id = a.Id,
                            IsTemp = a.IsTemp,
                            IsResign=!b.IsActive,
                            Name = a.Name,
                            Phone = b.PhoneNumber,
                            Sex = a.Sex,
                            UserId = a.UserId,
                            WXchat = a.WXchat
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.Phone.Contains(input.SearchKey) || ite.Code.Contains(input.SearchKey));
            }
            if (input.OrgId.HasValue)
            {
                var orgu = _userOrganizationUnitRepository.GetAll().Where(ite => ite.OrganizationUnitId == input.OrgId.Value).Select(ite => ite.UserId).ToList();
                query = query.Where(ite => orgu.Contains(ite.UserId.Value));
            }
            if (input.IsTemp.HasValue && input.IsTemp.Value)
            {
                query = query.Where(ite => ite.IsTemp == true);
            }
            var totalCount = query.Count();

            var ret = (await query.PageBy(input).ToListAsync()).MapTo<List<EmployeeListDto>>();
            
            foreach (var r in ret)
            {
                if (r.UserId.HasValue)
                {
                    var org = await (from a in _userOrganizationUnitRepository.GetAll()
                                     join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                                     where a.UserId == r.UserId.Value
                                     select new SimpleOrganizationDto()
                                     {
                                         Code = b.Code,
                                         Id = a.OrganizationUnitId,
                                         IsMain = a.IsMain,
                                         Title = b.DisplayName
                                     }).ToListAsync();
                    var post = from a in _postsRepository.GetAll()
                               join b in _userPostsRepository.GetAll() on a.Id equals b.PostId
                               join c in _organizationUnitPostsRepository.GetAll() on b.OrgPostId equals c.Id
                               join d in _organizationUnitRepository.GetAll() on c.OrganizationUnitId equals d.Id
                               where b.UserId == r.UserId.Value
                               select new UserPostDto { Id = b.Id, OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, OrgName = d.DisplayName };
                    r.Organization = org.ToList();
                    r.Posts = post.ToList();
                }
            }
            return new PagedResultDto<EmployeeListDto>(totalCount, ret);
        }

        /// 公历转为农历的函数
        /// </summary>
        /// <remarks>作者：DeltaCat</remarks>
        /// <example>网址：http://www.zu14.cn</example>
        /// <param name="solarDateTime">公历日期</param>
        /// <returns>农历的日期</returns>
        public string SolarToChineseLunisolarDate(DateTime solarDateTime)
        {
            System.Globalization.ChineseLunisolarCalendar cal = new System.Globalization.ChineseLunisolarCalendar();

            int year = cal.GetYear(solarDateTime);
            int month = cal.GetMonth(solarDateTime);
            int day = cal.GetDayOfMonth(solarDateTime);
            int leapMonth = cal.GetLeapMonth(year);
            return string.Format("农历{0}{1}（{2}）年{3}{4}月{5}{6}"
                                , "甲乙丙丁戊己庚辛壬癸"[(year - 4) % 10]
                                , "子丑寅卯辰巳午未申酉戌亥"[(year - 4) % 12]
                                , "鼠牛虎兔龙蛇马羊猴鸡狗猪"[(year - 4) % 12]
                                , month == leapMonth ? "闰" : ""
                                , "无正二三四五六七八九十冬腊"[leapMonth > 0 && leapMonth <= month ? month - 1 : month]
                                , "初十廿三"[day / 10]
                                , "日一二三四五六七八九"[day % 10]
                                );
        }

        public async Task<EmployeeDto> GetByUserId(long? input)
        {
            var ret = new EmployeeDto();
            Employee model = null;
            if (input.HasValue)
            {
                model = await _employeeRepository.FirstOrDefaultAsync(ite=>ite.UserId==input.Value);
                if (model == null) {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到用户。");
                }
                ret = model.MapTo<EmployeeDto>();
            }
            else
            {
                //获取当前用户员工信息
                model = _employeeRepository.GetAll().FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
                if (model == null)
                {
                    return null;
                }
                else
                {
                    ret = model.MapTo<EmployeeDto>();

                }
            }
            var u = await UserManager.GetUserByIdAsync(ret.UserId.Value);
            ret.Name = u.Name;
            ret.Phone = u.PhoneNumber;
            ret.Email = u.EmailAddress;
            var edu = await _educationExperienceRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (edu != null && edu.Count > 0)
            {
                ret.EducationExperience = edu.MapTo<List<EducationExperienceDto>>();
            }
            var work = await _workExperienceRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (work != null && work.Count > 0)
            {
                ret.WorkExperience = work.MapTo<List<WorkExperienceDto>>();
            }
            var family = await _employeeFamilyRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (family != null && family.Count > 0)
            {
                ret.EmployeeFamily = family.MapTo<List<EmployeeFamilyDto>>();
            }
            var skill = await _employeeSkillRepository.GetAll().Where(ite => ite.EmployeeId == ret.Id).ToListAsync();
            if (skill != null && skill.Count > 0)
            {
                ret.EmployeeSkill = skill.MapTo<List<EmployeeSkillDto>>();
            }
            var org = await(from a in _userOrganizationUnitRepository.GetAll()
                            join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                            where a.UserId == model.UserId
                            select new SimpleOrganizationDto()
                            {
                                Code = b.Code,
                                Id = a.OrganizationUnitId,
                                IsMain = a.IsMain,
                                Title = b.DisplayName
                            }).ToListAsync();
            ret.Organization = org;

            var post = from a in _postsRepository.GetAll()
                       join b in _userPostsRepository.GetAll() on a.Id equals b.PostId
                       join c in _organizationUnitPostsRepository.GetAll() on b.OrgPostId equals c.Id
                       join d in _organizationUnitRepository.GetAll() on c.OrganizationUnitId equals d.Id
                       where b.UserId == model.UserId
                       select new UserPostDto { Id = b.Id, OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, OrgName = d.DisplayName };
            ret.Posts = post.ToList();
            return ret;
        }
    }
}

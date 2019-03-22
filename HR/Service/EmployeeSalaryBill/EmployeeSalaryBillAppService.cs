using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.UI;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Application;
using Abp.Configuration;
using ZCYX.FRMSCore.Model;

namespace HR.Service
{
    public class EmployeeSalaryBillAppService : FRMSCoreAppServiceBase, IEmployeeSalaryBillAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<EmployeeSalaryBill, Guid> _employeeSalaryBillRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;

        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;

        private readonly IRepository<Setting,long> _setting;

        public EmployeeSalaryBillAppService(IRepository<Employee, Guid> employeeRepository,
            IRepository<PostInfo, Guid> postsRepository,
            IRepository<UserPosts, Guid> userPostsRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<Setting, long> setting,
            IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository,
            IRepository<EmployeeSalaryBill, Guid> employeeSalaryBillRepository,IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository)
        {
            _postsRepository = postsRepository;
            _userPostsRepository = userPostsRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _employeeRepository = employeeRepository;
            _employeeSalaryBillRepository = employeeSalaryBillRepository;
            _setting = setting;
        }
        /// <summary>
        /// 人事填报工资条
        /// </summary>
        /// <param name="input">默认每月5日前编辑上月工资条；5日后编辑本月工资条</param>
        public void CreateOrUpdate(List< EmployeeSalaryBillInput> input)
        {
            var set =  _setting.FirstOrDefault(ite => ite.Name == "HR.Salary.Setting");
            if (set == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请先设置工资发放日期。");
            }
            var setting = Newtonsoft.Json.JsonConvert.DeserializeObject<SalarySettingInput>(set.Value);
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, setting.Day + setting.Length);
            foreach (var item in input)
            {


                if (DateTime.Now.Day <= date.Day)
                {
                    if (item.Month != DateTime.Now.Month - 1)
                    {
                        //throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"工资结算之日（每月{date.Day}号）前只能编辑上一月工资条哦。");
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"每月1号-{date.Day}号才能编辑工资条哦。");
                    }
                }
                else
                {
                    if (item.Month != DateTime.Now.Month)
                    {
                        //throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"工资结算之日（每月{date.Day}号）后只能编辑本月工资条哦。");
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"每月1号-{date.Day}号才能编辑工资条哦。");
                    }
                }
                if (item.Id.HasValue)
                {
                    //编辑工资条
                    var bill = _employeeSalaryBillRepository.Get(item.Id.Value);
                    item.MapTo(bill);
                    _employeeSalaryBillRepository.Update(bill);
                }
                else
                {
                    var bill = item.MapTo<EmployeeSalaryBill>();
                    _employeeSalaryBillRepository.Insert(bill);
                }

            }

            //var financeSalaryBill = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IEmployeeFinanceSalaryBillAppService>();
            //financeSalaryBill.Create(null);
        }
        /// <summary>
        /// 个人获取本年度月份奖金明细
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeSalaryBillBonusForMe>> GetBonusForMe()
        {
            var e = _employeeRepository.FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
            if (e == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr,"你还未填写员工基本信息，无法获取工资条。");
            }
            var ret = await _employeeSalaryBillRepository.GetAll().Where(ite => ite.EmployeeId == e.Id && ite.Year == DateTime.Now.Year).ToListAsync();
            return ret.MapTo<List<EmployeeSalaryBillBonusForMe>>();
        }
        /// <summary>
        /// 人事获取本年以前月份的工资条
        /// </summary>
        /// <param name="input">员工id</param>
        /// <returns></returns>
        public async Task<List<EmployeeSalaryBillDto>> GetList(Guid? input)
        {
            if (input.HasValue == false) {
                var self = _employeeRepository.FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
                if (self == null) {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请您先完善个人信息。");
                }
                input = self.Id;
            }
            var query =await (from a in _employeeSalaryBillRepository.GetAll()
                         join b in _employeeRepository.GetAll() on a.EmployeeId equals b.Id
                         where a.Year == DateTime.Now.Year && a.Month <= DateTime.Now.Month && a.EmployeeId == input
                         select new EmployeeSalaryBillDto()
                         {
                             Id=a.Id,
                             AccumulationFund = a.AccumulationFund,
                             BaseSalary = a.BaseSalary,
                             Deduction = a.Deduction,
                             DeductionReason = a.DeductionReason,
                             EmployeeId = a.EmployeeId,
                             EndowmentInsurance = a.EndowmentInsurance,
                             InjuryInsurance = a.InjuryInsurance,
                             MaternityInsurance = a.MaternityInsurance,
                             MedicalInsurance = a.MedicalInsurance,
                             Month = a.Month,
                             MonthBonus = a.MonthBonus,
                             Name = b.Name,
                             PreSalary = a.PreSalary,
                             QuarterBonus = a.QuarterBonus,
                             Salary = a.Salary,
                             Tax = a.Tax,
                             UnworkInsurance = a.UnworkInsurance,
                             WorkBonus = a.WorkBonus,
                             WorkSalary = a.WorkSalary,
                             Year = a.Year,
                             YearBonus = a.YearBonus
                             
                         }).ToListAsync();


            var ret = query.MapTo<List<EmployeeSalaryBillDto>>();
            if (ret.FirstOrDefault(ite => ite.Month == DateTime.Now.Month) == null&&DateTime.Now.Day>5)
            {
                var e = _employeeRepository.FirstOrDefault(ite=>ite.Id==input);
                if (e == null) {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前员工信息");
                }
                ret.Add(new EmployeeSalaryBillDto() { Month = DateTime.Now.Month, Year = DateTime.Now.Year, EmployeeId = input.Value,Name= e.Name});

            }
            ret = ret.OrderByDescending(ite => ite.Month).ToList();
            return ret;
        }
        /// <summary>
        ///  个人获取本年月份工资明细
        /// </summary>
        /// <returns></returns>
        public async Task<List< EmployeeSalaryBillForMe>> GetSalaryForMe()
        {
            var e = _employeeRepository.FirstOrDefault(ite => ite.UserId == AbpSession.UserId.Value);
            if (e == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "你还未填写员工基本信息，无法获取工资条。");
            }
            var ret =await _employeeSalaryBillRepository.GetAll().Where(ite => ite.EmployeeId == e.Id && ite.Year == DateTime.Now.Year).ToListAsync();
            return ret.MapTo<List<EmployeeSalaryBillForMe>>();
        }
        /// <summary>
        /// 人事填报工资条-获取人员列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EmployeeSalaryDto> GetAllEmployee(EmployeeSearchInput input) {

            var model = new EmployeeSalaryDto();
            if (input.Year.HasValue == false) {
                input.Year = DateTime.Now.Year;
            }
            if (input.Month.HasValue == false) {
                input.Month = DateTime.Now.Month-1;
                var set = _setting.FirstOrDefault(ite => ite.Name == "HR.Salary.Setting");
                if (set == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请先设置工资发放日期。");
                }
                var setting = Newtonsoft.Json.JsonConvert.DeserializeObject<SalarySettingInput>(set.Value);
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, setting.Day);
                if (DateTime.Now.Day <= date.Day)
                {
                    input.Month = input.Month - 2;
                }
            }
            var query = _employeeRepository.GetAll();
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
            
            var ret = query.OrderByDescending(r => r.LastModificationTime).PageBy(input).Select(ite => new EmployeeSalaryBillDto() {
                Name = ite.Name,
                EmployeeId = ite.Id,
                Month =  input.Month.Value ,
                Year = input.Year.Value,
                UserId =ite.UserId
            }).ToList();
            foreach (var r in ret)
            {
                var s = _employeeSalaryBillRepository.FirstOrDefault(ite => ite.Year == DateTime.Now.Year && ite.Month == input.Month.Value && ite.EmployeeId == r.EmployeeId);
                if (s != null) {
                    r.AccumulationFund = s.AccumulationFund;
                    r.BaseSalary = s.BaseSalary;
                    r.Deduction = s.Deduction;
                    r.DeductionReason = s.DeductionReason;
                    r.Id = s.Id;
                    r.EndowmentInsurance = s.EndowmentInsurance;
                    r.InjuryInsurance = s.InjuryInsurance;
                    r.MaternityInsurance = s.MaternityInsurance;
                    r.MedicalInsurance = s.MedicalInsurance;
                    r.Month = s.Month;
                    r.MonthBonus = s.MonthBonus;
                    r.PreSalary = s.PreSalary;
                    r.QuarterBonus = s.QuarterBonus;
                    r.Salary = s.Salary;
                    r.Tax = s.Tax;
                    r.UnworkInsurance = s.UnworkInsurance;
                    r.WorkBonus = s.WorkBonus;
                    r.WorkSalary = s.WorkSalary;
                    r.Year = s.Year;
                    r.YearBonus = s.YearBonus;
                }
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
            model.Items = new PagedResultDto<EmployeeSalaryBillDto>(totalCount, ret);
            model.Year = input.Year.Value;
            model.Month = input.Month.Value;
            return model;
        }

        /// <summary>
        /// 设置工资发放配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetSetting(SalarySettingInput input)
        {
            if (input.Day > 23 || input.Day <= 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeLenErr, "工资条截至时间只能在1-23日之间。");
            }
            if (input.Length > 5 || input.Length <= 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeLenErr, "工资条展示时间只能在5日之内。");
            }
            var s = new SalarySettingDto();
           
            var set = await _setting.FirstOrDefaultAsync(ite => ite.Name == "HR.Salary.Setting");
          
            if (set == null)
            {
                set = new Setting();
                set.Name = "HR.Salary.Setting";
                s.HangFilreId = Guid.NewGuid();
            } else
            {
                var ret=   Newtonsoft.Json.JsonConvert.DeserializeObject<SalarySettingDto>(set.Value);
                s.HangFilreId = ret.HangFilreId;
            }
            s.Day = input.Day;
            s.Length = input.Length;
            set.Value = Newtonsoft.Json.JsonConvert.SerializeObject(s);
            await _setting.InsertOrUpdateAsync(set);
        }
        /// <summary>
        /// 获取工资发放配置
        /// </summary>
        /// <returns></returns>
        public async Task<SalarySettingInput> GetSetting()
        {
            var set = await _setting.FirstOrDefaultAsync(ite => ite.Name == "HR.Salary.Setting");
            if (set == null) {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请先设置工资发放日期。");
            }
            var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<SalarySettingInput>(set.Value);
            return ret;
        }
    }
}

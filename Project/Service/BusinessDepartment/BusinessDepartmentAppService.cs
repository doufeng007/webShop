using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

using Abp.UI;
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
namespace Project
{
    public class BusinessDepartmentAppService : FRMSCoreAppServiceBase, IBusinessDepartmentAppService
    {
        private readonly IRepository<BusinessDepartment> _businessDepartmentRepository;
        public BusinessDepartmentAppService(IRepository<BusinessDepartment> businessDepartmentRepository)
        {
            _businessDepartmentRepository = businessDepartmentRepository;
        }

        public async Task CreateOrUpdate(BusinessDepartmentDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateAsync(input);
            }
            else
            {
                await CreateAsync(input);
            }
        }

        private async Task UpdateAsync(BusinessDepartmentDto input)
        {
            if (_businessDepartmentRepository.GetAll().Where(p => p.Name == input.Name && p.Id != input.Id.Value).Count() > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = await _businessDepartmentRepository.GetAsync(input.Id.Value);
            model.Name = input.Name;
            model.Code = input.Code;
            model.ContactUser = input.ContactUser;
            model.ContactTel = input.ContactTel;
            model.Address = input.Address;
            model.Email = input.Email;
            model.Phone = input.Phone;
            model.CreationTime = input.CreationTime;
            model.InputCode1 = model.Name.ToChineseSpell().ToLower();
            model.InputCode2 = model.Name.ToChineseSpell().ToLower();
            await _businessDepartmentRepository.UpdateAsync(model);
        }

        private async Task CreateAsync(BusinessDepartmentDto input)
        {
            if (_businessDepartmentRepository.GetAll().Where(p => p.Name == input.Name).Count() > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = new BusinessDepartment();
            model.Name = input.Name;
            model.Code = input.Code;
            model.ContactUser = input.ContactUser;
            model.ContactTel = input.ContactTel;
            model.Address = input.Address;
            model.Email = input.Email;
            model.Phone = input.Phone;
            model.CreationTime = DateTime.Now;
            model.InputCode1 = model.Name.ToChineseSpell().ToLower();
            model.InputCode2 = model.Name.ToChineseSpell().ToLower();
            await _businessDepartmentRepository.InsertAsync(model);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        public async Task<GetBusinessDepartmentForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            if (!input.Id.HasValue)
                return new GetBusinessDepartmentForEditOutput();
            var model = await _businessDepartmentRepository.GetAsync(input.Id.Value);
            var output = model.MapTo<GetBusinessDepartmentForEditOutput>();
            return output;
        }

        public async Task<PagedResultDto<BusinessDepartmentList>> GetPage(GetBusinessDepartmentListInput input)
        {
            var query = _businessDepartmentRepository.GetAll();
            var count = await query.CountAsync();
            var model = await query.OrderByDescending(p=>p.CreationTime).OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var dto = model.MapTo<List<BusinessDepartmentList>>();
            return new PagedResultDto<BusinessDepartmentList>(count, dto);
        }

        public async Task Delete(EntityDto<int> input)
        {
            var model = await _businessDepartmentRepository.GetAsync(input.Id);
            await _businessDepartmentRepository.DeleteAsync(model);
        }

        public List<BusinessDepartmentList> GetAll()
        {
            var model = _businessDepartmentRepository.GetAll().OrderByDescending(p=>p.CreationTime).ToList();
            var dto = model.MapTo<List<BusinessDepartmentList>>();
            return dto;
        }

        public string GetName(EntityDto input)
        {
            var model = _businessDepartmentRepository.GetAll().FirstOrDefault(r => r.Id == input.Id);
            if (model == null) return "";
            return model.Name;
        }

    }
}
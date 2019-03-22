using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System;
using Abp.UI;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using ZCYX.FRMSCore.Application;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ChargeOrganizationsAppService : ApplicationService, IChargeOrganizationsAppService
    {
        private readonly IRepository<ChargeOrganizations> _chargeOrganizationsRepository;
        public ChargeOrganizationsAppService(IRepository<ChargeOrganizations> chargeOrganizationsRepository)
        {
            _chargeOrganizationsRepository = chargeOrganizationsRepository;
        }

        public async Task CreateOrUpdate(ChargeOrganizationsDto input)
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

        private async Task UpdateAsync(ChargeOrganizationsDto input)
        {
            if (_chargeOrganizationsRepository.GetAll().Where(p => p.Name == input.Name && p.Id != input.Id.Value).Count() > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = await _chargeOrganizationsRepository.GetAsync(input.Id.Value);
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
            await _chargeOrganizationsRepository.UpdateAsync(model);
        }

        private async Task CreateAsync(ChargeOrganizationsDto input)
        {
            if (_chargeOrganizationsRepository.GetAll().Where(p => p.Name == input.Name).Count() > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该名称已经存在");
            }
            var model = new ChargeOrganizations();
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
            await _chargeOrganizationsRepository.InsertAsync(model);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        public async Task<GetChargeOrganizationsForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            if (!input.Id.HasValue)
                return new GetChargeOrganizationsForEditOutput();
            var model = await _chargeOrganizationsRepository.GetAsync(input.Id.Value);
            var output = model.MapTo<GetChargeOrganizationsForEditOutput>();
            return output;
        }

        public async Task<PagedResultDto<ChargeOrganizationsList>> GetPage(GetChargeOrganizationsListInput input)
        {
            var query = _chargeOrganizationsRepository.GetAll();
            var count = await query.CountAsync();
            var model = await query.OrderByDescending(p => p.CreationTime).OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var dto = model.MapTo<List<ChargeOrganizationsList>>();
            return new PagedResultDto<ChargeOrganizationsList>(count, dto);
        }

        public async Task Delete(EntityDto<int> input)
        {
            var model = await _chargeOrganizationsRepository.GetAsync(input.Id);
            await _chargeOrganizationsRepository.DeleteAsync(model);
        }

        public List<ChargeOrganizationsList> GetAll()
        {
            var model = _chargeOrganizationsRepository.GetAll().OrderByDescending(p => p.CreationTime).ToList();
            var dto = model.MapTo<List<ChargeOrganizationsList>>();
            return dto;
        }

        public string GetName(EntityDto input)
        {
            var model = _chargeOrganizationsRepository.GetAll().FirstOrDefault(r => r.Id == input.Id);
            if (model == null) return "";
            return model.Name;
        }


        public string COGetPy(string name)
        {
            string account = name.ToChineseSpell();
            var ret = (account.IsNullOrEmpty() ? "" : account).ToUpper();
            var newCode = "";
            if (!ret.IsNullOrWhiteSpace())
            {
                for (var i = 0; i < 1000; i++)
                {
                    if (i == 0)
                        newCode = ret;
                    else
                        newCode = $"{ret}{i}";
                    if (_chargeOrganizationsRepository.GetAll().Any(r => r.Code == newCode))
                    {
                        continue;
                    }
                    else
                        break;
                }
            }

            return newCode;
        }


    }
}
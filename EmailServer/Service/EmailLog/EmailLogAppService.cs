using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.File;
using ZCYX.FRMSCore.Authorization.Users;
using System.Net.Mail;
using System.IO;

namespace EmailServer
{
    public class EmailLogAppService : FRMSCoreAppServiceBase, IEmailLogAppService
    {
        private readonly IRepository<EmailLog, Guid> _repository;
        private readonly IRepository<User, long> _usersRepository;
       private readonly IAbpFileRelationAppService _abpFileRelationAppService;
       private readonly IRepository<AbpFile, Guid> _abpFilerepository;
        private readonly IEmailAppService _emailAppServiceRepository;
        public EmailLogAppService(IRepository<EmailLog, Guid> repository, IRepository<User, long> usersRepository, IEmailAppService emailAppServiceRepository,IAbpFileRelationAppService abpFileRelationAppService, IRepository<AbpFile, Guid> abpFilerepository)
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _usersRepository = usersRepository;
            _abpFilerepository = abpFilerepository;
            _emailAppServiceRepository = emailAppServiceRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmailLogListOutputDto>> GetList(GetEmailLogListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.CreatorUserId == AbpSession.UserId.Value)

                        select new EmailLogListOutputDto()
                        {
                            Id = a.Id,
                            To = a.To,
                            CC = a.CC,
                            Subject = a.Subject,
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.Subject.Contains(input.SearchKey) || x.To.Contains(input.SearchKey) || x.CC.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<EmailLogListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<EmailLogOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var tmp = model.MapTo<EmailLogOutputDto>();
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.邮件附件
            });
            return tmp;
		}
        /// <summary>
        /// 添加一个EmailLog
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateEmailLogInput input)
        {
            if (input.To.Count() == 0 && input.ToEmail.Count()==0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择收件人.");
            var fromUser = _usersRepository.GetAll().FirstOrDefault(x => x.Id == AbpSession.UserId.Value);
            if (fromUser == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请先登录.");
            MailMessage mail = new MailMessage();
            var to = "";
            foreach (var item in input.To)
            {
                var user = _usersRepository.GetAll().FirstOrDefault(x => x.Id == item);
                if (user != null)
                {
                    to += user.Name + $"<{user.EmailAddress}>;";
                    mail.To.Add(new MailAddress(user.EmailAddress, user.Name, Encoding.UTF8)); ;
                }
            }
            foreach (var item in input.ToEmail)
            {
                to += item + $"<{item}>;";
                mail.To.Add(new MailAddress(item, item, Encoding.UTF8)); ;
            }
            var cc = "";
            foreach (var item in input.CC)
            {
                var user = _usersRepository.GetAll().FirstOrDefault(x => x.Id == item);
                if (user != null)
                {
                    cc += user.Name + $"<{user.EmailAddress}>;";
                    mail.CC.Add(new MailAddress(user.EmailAddress, user.Name, Encoding.UTF8)); 
                }
            }
            foreach (var item in input.CCEmail)
            {
                cc += item + $"<{item}>;";
                mail.CC.Add(new MailAddress(item, item, Encoding.UTF8)); ;
            }
            var id = Guid.NewGuid();
            var newmodel = new EmailLog()
            {
                To = to,
                CC = cc,
                From = fromUser.Name + $"<{fromUser.EmailAddress}>;",
                Subject = input.Subject,
                Body = input.Body
            };
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    var file = _abpFilerepository.Get(item.Id);
                    Stream fileStream = File.OpenRead(file.FilePath);
                    mail.Attachments.Add(new Attachment(fileStream, file.FileName));
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.邮件附件,
                    Files = fileList
                });
            }
            mail.Subject = input.Subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = input.Body;
            mail.IsBodyHtml = true;
            _emailAppServiceRepository.SendMailMessage(mail);
            await _repository.InsertAsync(newmodel);

        }
		
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}
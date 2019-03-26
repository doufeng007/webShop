using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.File
{
    public interface IAbpFileRelationAppService : IApplicationService
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>


        Task<List<GetAbpFilesOutput>> GetListAsync(GetAbpFilesInput input);

        List<GetAbpFilesOutput> GetList(GetAbpFilesInput input);


        Task<List<GetMultiAbpFilesOutput>> GetMultiListAsync(GetMultiAbpFilesInput input);


        List<GetMultiAbpFilesOutput> GetMultiList(GetMultiAbpFilesInput input);


        Task<GetAbpFilesOutput> GetAsync(GetAbpFilesInput input);

        GetAbpFilesOutput Get(GetAbpFilesInput input);

        Task CreateAsync(CreateFileRelationsInput input);

        void Create(CreateFileRelationsInput input);


        Task UpdateAsync(CreateFileRelationsInput input);

        void Update(CreateFileRelationsInput input);

        /// <summary>
        /// 批量上传文件关联
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="businessType"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        Task CreateListAsync(string businessId, AbpFileBusinessType businessType, List<GetAbpFilesOutput> files);
        Task UpdateListAsync(string businessId, AbpFileBusinessType businessType, List<GetAbpFilesOutput> files);



    }




}

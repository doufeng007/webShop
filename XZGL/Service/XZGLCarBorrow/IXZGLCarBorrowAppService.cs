using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace XZGL
{
    public interface IXZGLCarBorrowAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<XZGLCarBorrowListOutputDto>> GetList(GetXZGLCarBorrowListInput input);
        Task<PagedResultDto<XZGLCarBorrowListOutputDto>> GetCarList(GetXZGLCarBorrowByCarListInput input);
        Task<PagedResultDto<XZGLWorkOutCarOutputDto>> GetWorkOutList(GetXZGLCarBorrowByWorkOutListInput input);
        Task DeleteWorkOutRelation(EntityDto<Guid> input);
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<XZGLCarBorrowOutputDto> Get(GetWorkFlowTaskCommentInput input);
        Task<XZGLCarBorrowCarInfoOutputDto> GetCarInfo(GetWorkFlowTaskCommentInput input);
        Task UpdateRelation(RelationInput input);
        /// <summary>
        /// 添加一个XZGLCarBorrow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        InitWorkFlowOutput Create(CreateXZGLCarBorrowInput input);

        /// <summary>
        /// 修改一个XZGLCarBorrow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateXZGLCarBorrowInput input);
        Task UpdateCarInfo(UpdateXZGLCarInfoInput input);
        [RemoteService(IsEnabled = false)]
        XZGLCarBorrow GetCarBorrow(string InstanceID);
        [RemoteService(IsEnabled = false)]
        string GetCarDriver(string InstanceID);
        [RemoteService(IsEnabled = false)]
        string GetUserId(string InstanceID);
    }
}
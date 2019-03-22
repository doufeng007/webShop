using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.Configuration;

namespace Train
{
    public interface ITrainAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<TrainListOutputDto>> GetList(GetTrainListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<TrainOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个Train
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateTrainInput input);

		/// <summary>
        /// 修改一个Train
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateTrainInput input);
        void SendMessageForJoinUser(Guid guid);
        void SendMessageByUser(Guid guid);
        string GetNeedExperienceUsers(string guid);
        string GetTrainNeedCopyForUsers(string guid);
        void AddJoinScore(string guid);
        string GetExperienceLeader(string guid);
        Task UpdatePersonnel(UpdateTrainCopyForInput input);
        Task UpdateDocumentId(UpdateTrainDocumentInput input);
        Task<List<CommentListOutput>> GetComment(GetWorkFlowTaskCommentUserInput input);

        Task<List<CommentOutput>> GetCommentList(GetWorkFlowTaskCommentInput input);
    }
}
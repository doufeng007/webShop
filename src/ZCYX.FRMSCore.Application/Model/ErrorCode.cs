using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZCYX.FRMSCore.Model
{
    public enum ErrorCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Succ = -1,

        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("系统错误")]
        SystemErr = 0,

        /// <summary>
        /// 必要字段值为空
        /// </summary>
        [Description("必要字段值为空")]
        NullCodeErr = 101,

        /// <summary>
        /// 字段长度不正确
        /// </summary>
        [Description("字段长度不正确")]
        CodeLenErr = 102,

        /// <summary>
        /// 参数值不正确
        /// </summary>
        [Description("参数值不正确")]
        CodeValErr = 103,

        /// <summary>
        /// 参数多余
        /// </summary>
        [Description("参数多余")]
        CodeOutRange = 104,

        /// <summary>
        /// http接口错误
        /// </summary>
        [Description("http接口错误")]
        HttpPortErr = 105,

        /// <summary>
        /// 数据库访问错误
        /// </summary>
        [Description("数据库访问错误")]
        DataAccessErr = 106,

        /// <summary>
        /// 时间戳超时
        /// </summary>
        [Description("时间戳超时")]
        TimeExpire = 107,

        /// <summary>
        /// 签名错误
        /// </summary>
        [Description("签名错误")]
        SignCodeErr = 108,

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        [Description("用户名或密码错误")]
        UnameOrPwdErr = 109,

        /// <summary>
        /// 验证码错误
        /// </summary>
        [Description("验证码错误")]
        CheckCodeErr = 110,

        /// <summary>
        /// 必要参数不存在
        /// </summary>
        [Description("必要参数不存在")]
        NullPropertyCodeErr = 111,

        /// <summary>
        /// 授权信息验证失效
        /// </summary>
        [Description("授权信息验证失效")]
        UserOauthInvalid = 112,
        /// <summary>
        /// 业务逻辑数据异常
        /// </summary>
        [Description("业务逻辑数据异常")]
        BussinessDataException = 113,


        /// <summary>
        /// 与其他系统交互错误
        /// </summary>
        [Description("与其他系统交互错误")]
        OtherSystemConetion = 114,


        /// <summary>
        /// 授权信息验证失效
        /// </summary>
        [Description("数据重复")]
        DataDuplication = 115,

        /// <summary>
        /// 其他错误
        /// </summary>
        [Description("其他错误")]
        OtherErr = 999,

    }

}

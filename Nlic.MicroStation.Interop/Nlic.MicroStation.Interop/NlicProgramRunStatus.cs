/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   程序执行状态
 * Author:      Michael Hou
 * CDT:         2011-05-24
 * Version:     1.0.0.0
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using System;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 程序执行状态
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum NlicProgramRunStatus {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 4,
        /// <summary>
        /// 没有元素被处理，通常是因为模型或dgn文件为空
        /// </summary>
        NoElements = 3,
        /// <summary>
        /// 存在没有处理的元素。
        /// </summary>
        /// <remarks>通常是因为某些元素太特殊所致</remarks>
        HasUnOperationElement = 2,
        /// <summary>
        /// 不能处理。
        /// </summary>
        /// <remarks>通常是因为存在某些特殊元素导致程序运行的前提条件检查没有通过</remarks>
        CantOperation = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 有异常发生
        /// </summary>
        Exception = -1
    }
}

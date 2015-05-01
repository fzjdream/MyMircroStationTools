/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   获取元素容器的方式
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
    /// 获取元素容器的方式
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum NlicGetElementEnumeratorType {
        /// <summary>
        /// 从已定义围栅获取
        /// </summary>
        ByExistFence = 0,
        /// <summary>
        /// 从选择的元素中获取
        /// </summary>
        BySelected = 1,
        /// <summary>
        /// 最大化视口1，自动定义围栅获取
        /// </summary>
        ByAutoDefineFence = 2,
        /// <summary>
        /// 扫描模型的所有Graphical
        /// </summary>
        ScanAllGraphical = 3,
        /// <summary>
        /// 用户定义扫描规则扫描获取
        /// </summary>
        UserDefineScanCriteria = 4,
        /// <summary>
        /// 从保存的围栅获取
        /// </summary>
        BySavedFence = 5,
        /// <summary>
        /// 从保存的视图获取
        /// </summary>
        BySavedView = 6,
        /// <summary>
        /// 从命名组获取
        /// </summary>
        ByNamedGroup = 7
    }
}

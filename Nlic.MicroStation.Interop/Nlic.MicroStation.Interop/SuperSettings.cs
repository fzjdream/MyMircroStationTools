/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   更改当前激活dgn文件设置的一些操作。
 * Author:      侯祥意
 * CDT:         2011-06-03
 * Version:     1.0.0.0
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 扩展的设置。
    /// </summary>
    public class SuperSettings {
        /// <summary>
        /// 将围栅定义模式改为搭界.
        /// </summary>
        /// <remarks></remarks>
        public static void SettingFenceDefineModelOverlay() {
            Application app = Utilities.ComApp;
            app.ActiveSettings.FenceVoid = false;
            app.ActiveSettings.FenceClip = false;
            app.ActiveSettings.FenceOverlap = true;
        }
    }
}

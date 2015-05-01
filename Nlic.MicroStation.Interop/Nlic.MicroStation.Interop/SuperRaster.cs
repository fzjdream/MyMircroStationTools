/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn栅格文件的一些操作。
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
    /// 扩展的光栅。
    /// </summary>
    public class SuperRaster {
        /// <summary>
        /// 卸载所有光栅文件.
        /// </summary>
        /// <remarks></remarks>
        public static void DetachAllRasters() {
            Application app = Utilities.ComApp;
            if (app.RasterManager.Rasters.Count > 0) {
                app.RasterManager.Rasters.Clear();
            }
        }
    }
}

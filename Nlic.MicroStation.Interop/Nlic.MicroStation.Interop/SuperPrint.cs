/************************************************************************
 * * Copyright © 2014 南宁市国土资源信息中心 All rights reserved.
 * *
 * CLR:         4.0.30319.18408
 * Machine:     XINXI-FENGZJ
 * File:        SuperPrint
 * GUID:        5c32d012-37cf-4e88-97fd-e242ed96c394
 * Domain:      NLIS
 * User:        fengzhenjian 
 * ----------------------------------------------------------------------
 * Depiction:   
 * Author:      Von_dream
 * CDT:         2014/5/23 16:15:36
 * Version:     1.0.0.1
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
using System.Xml.Schema;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 扩展输出打印
    /// </summary>
    public class SuperPrint {
        private static readonly Application App = Utilities.ComApp;

        private static readonly string DriverPltcfgFullName =
            @"C:\ProgramData\Bentley\MapEnterprise V8i\08.11.09\WorkSpace\System\pltcfg\jpeg.pltcfg";
        /// <summary>
        /// 将围栅内容导出Jpg
        /// </summary>
        public static void OutputJpgByFence() {
            App.CadInputQueue.SendCommand(
                @"vba load Z:\WorkSpace\Bentley\V8i\BaseResources\vba\printJPG.mvba;vba run PrintModule.Main");
        }
    }
}

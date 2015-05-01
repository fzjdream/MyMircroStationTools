/************************************************************************
 * * Copyright © 2014 南宁市国土资源信息中心 All rights reserved.
 * *
 * CLR:         4.0.30319.18408
 * Machine:     XINXI-FENGZJ
 * File:        SuperCadInputQueue
 * GUID:        4d0a2ec9-052a-4d96-bff4-90f3c71e8815
 * Domain:      NLIS
 * User:        fengzhenjian 
 * ----------------------------------------------------------------------
 * Depiction:   
 * Author:      Von_dream
 * CDT:         2014/3/31 15:43:31
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
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    public static class SuperCadInputQueue {
        private static readonly Application App = Utilities.ComApp;

        public static void OutputJPGImage(){
            App.CadInputQueue.SendCommand(@"MDL COMMAND MGDSHOOK,fileList_setDirectoryCmd C:\Users\fengzhenjian.NLIS\Desktop\",true);
            App.CadInputQueue.SendCommand(@"MDL COMMAND MGDSHOOK,fileList_setFileNameCmd 11.jpg",true);
            var message = App.CadInputQueue.GetInput(MsdCadInputType.Command);
            var saveJPG = App.ActiveDesignFile.FindSavedView(@"C:\Users\fengzhenjian.NLIS\Desktop\1.jpg");
            saveJPG.GetPicture(100,100);
        }
    }
}

/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn参考的一些操作。
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
    /// 扩展的参考。
    /// </summary>
    public class SuperAttachment {
        private static readonly Application _app = Utilities.ComApp;
        /// <summary>
        /// 卸载所有参考文件.
        /// </summary>
        /// <remarks></remarks>
        public static void DetachAllReferences() {
            if (_app.ActiveModelReference.Attachments.Count > 0) {
                foreach (Attachment att in _app.ActiveModelReference.Attachments) {
                    _app.ActiveModelReference.Attachments.Remove(att);
                }
            }
        }

        public static void AttachFile(string fileFullName) {
            
        }

    }
}

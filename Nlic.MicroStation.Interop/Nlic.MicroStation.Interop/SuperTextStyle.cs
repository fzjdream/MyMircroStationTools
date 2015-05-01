/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn文本样式的一些操作
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
using System.Runtime.InteropServices;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using BMI = Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    public class SuperTextStyle {
        public static TextStyle ActiveTextStyle(string name) {
            Application app = BMI.Utilities.ComApp;

            try {
                TextStyle tStyle =
                    app.ActiveDesignFile.TextStyles.Cast<TextStyle>().FirstOrDefault(style => style.Name == name);

                if (tStyle != null) {
                    app.ActiveSettings.TextStyle = tStyle;
                    app.ActiveSettings.TextStyle.Font = tStyle.Font;
                    app.ActiveSettings.TextStyle.Height = tStyle.Height;
                    app.ActiveSettings.TextStyle.Width = tStyle.Width;
                }
                else {
                    var stFont = app.ActiveDesignFile.Fonts.Find(
                        MsdFontType.WindowsTrueType, "宋体");
                    if (stFont != null) {
                        app.ActiveSettings.TextStyle.Font = stFont;
                    }
                }

                return tStyle;
            }
            catch (Exception ex) {
                return ActiveTextStyle2(name);
            }
        }

        public static TextStyle ActiveTextStyle2(string name) {
            Application app = BMI.Utilities.ComApp;
            try {
                app.CadInputQueue.SendCommand("TEXTSTYLE DESELECT ALL");
                app.CadInputQueue.SendCommand("TEXTSTYLE SELECT \"" + name + "\"");
                app.CadInputQueue.SendCommand("TEXTSTYLE Active");

                return app.ActiveDesignFile.TextStyles.Find(name);
            }
            catch (Exception ex) {
                SuperMessage.RecordErrorMsgInMsgCenter(ex);
                //throw ex;
            }

            return null;
        }
    }
}

/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn围栅的一些操作。
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
using Nlic.MicroStation.Interop;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 扩展的围栅。
    /// </summary>
    public class SuperFence {
        private static readonly Application App = Utilities.ComApp;
        /// <summary>
        /// 根据当前视口的显示区域定义围栅.
        /// </summary>
        /// <param name="model">模型.</param>
        /// <remarks>1、默认的视口为视口1；
        /// 2、如果没有任何打开的视口，则抛出ViewNotOpenException的异常；
        /// 3、围栅定义模式设为搭界（Overlay）。</remarks>
        public static void DefineFenceByView(ModelReference model) {
            View view = App.ActiveDesignFile.Views[1];
            if (view == null) {
                view = App.CommandState.LastView();
            }
            if (view.IsOpen == false) {
                ViewNotOpenException ex = new ViewNotOpenException("视图没有打开，不能定义围栅");

                ex.Data.Add("Model Name", model.Name);
                ex.Data.Add("View ScreenIndex", view.ScreenIndex);
                throw ex;
            }
            Point3d viewSize = view.get_Extents();
            Point2d[] viewArea = new Point2d[4];
            viewArea[1].Y = viewSize.Y;
            viewArea[2].X = viewSize.X;
            viewArea[2].Y = viewSize.Y;
            viewArea[3].X = viewSize.X;

            SuperSettings.SettingFenceDefineModelOverlay();

            App.ActiveDesignFile.Fence.DefineFromViewPoints(view, ref viewArea);
            App.ActiveDesignFile.Fence.Draw();
        }
        /// <summary>
        /// 获取围栅
        /// </summary>
        /// <returns></returns>
        public static Fence GetActiveDesignFileFence() {
            return App.ActiveDesignFile.Fence;
        }
    }
}

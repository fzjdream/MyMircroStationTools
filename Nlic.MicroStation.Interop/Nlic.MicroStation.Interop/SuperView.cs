/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   超级视图——dgn视图的一些操作。
 * Author:      侯祥意
 * CDT:         2011-06-03
 * Version:     1.3.0.2
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    Michael Hou
 * UDT:         2013-12-18
 * Desc:        新增ZoomIn函数
 ************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 扩展的视图。
    /// </summary>
    public class SuperView {
        /// <summary>
        /// 全幅显示所有打开的视口.
        /// </summary>
        /// <remarks>欲全幅显示的视口必须打开</remarks>
        public static void FitView() {
            Application app = Utilities.ComApp;
            try {
                foreach (View vw in app.ActiveDesignFile.Views) {
                    if (vw.IsOpen) {
                        vw.Fit(false);
                        vw.Redraw();
                    }
                }
            }
            catch (Exception e) {
                return;
                //throw e;
            }
        }

        /// <summary>
        /// 打开视口1，并最大化显示.
        /// </summary>
        /// <remarks></remarks>
        public static void OpenView1() {
            Application app = Utilities.ComApp;
            if (app.ActiveDesignFile.Views[1].IsOpen == false) {
                app.ActiveDesignFile.Views[1].IsOpen = true;
                app.ActiveDesignFile.Views[1].Maximize();
            }
        }

        /// <summary>
        /// 将视口1最大化.
        /// </summary>
        /// <remarks></remarks>
        public static void MaximizeView1() {
            Application app = Utilities.ComApp;
            if (app.ActiveDesignFile.Views[1].IsMaximized == false) {
                app.ActiveDesignFile.Views[1].Maximize();
            }
        }

        /// <summary>
        /// 将视图中心定位到元素位置并且缩放到合适大小
        /// </summary>
        /// <param name="element">定位的元素</param>
        /// <param name="view">指定的视图</param>
        public static void ZoomInElement(Element element, View view = null) {
            var rng = element.Range;
            if (rng.High.X > rng.Low.X) {
                var eleCenter = SuperElement.GetElementRangeCenter(element);
                var extend = Utilities.ComApp.Point3dSubtractPoint3dVector3d(ref rng.High, Utilities.ComApp.Vector3dFromPoint3d(ref rng.Low));
                view = view ?? Utilities.ComApp.ActiveDesignFile.Views[1];
                view.set_Origin(rng.Low);
                view.set_Extents(extend);
                var vCenter = view.get_Center();
                var moveV = new Point3d() { X = eleCenter.X - vCenter.X, Y = eleCenter.Y - vCenter.Y };
                var newOrgin = Utilities.ComApp.Point3dAdd(rng.Low, moveV);
                view.set_Origin(ref newOrgin);
                //view.set_Center(ref newOrgin);
                view.Zoom(2);
                view.Redraw();
            }
        }

        /// <summary>
        /// 将以元素为中心定位视图1，并且将视图大小设置为元素相当
        /// </summary>
        /// <param name="range"></param>
        /// <remarks>
        /// Author:周俊晖
        /// CDT:2013-12-16
        /// UDT:2013-12-19
        /// </remarks>
        public static void ZoomInElement(Range3d range) {
            var view = Utilities.ComApp.ActiveDesignFile.Views[1];
            var centerPoint = new Point3d();
            var rangeHigh = range.High;
            var rangeLow = range.Low;

            centerPoint.X = (rangeHigh.X + rangeLow.X) / 2;
            centerPoint.Y = (rangeHigh.Y + rangeLow.Y) / 2;

            view.Zoom(Math.Abs(rangeHigh.X - rangeLow.X) * 1.5 / view.get_Extents().X);
            view.Redraw();
            view.set_Center(centerPoint);
            view.Redraw();
        }

        public static void ZoomInPoint(Point3d center, View view = null) {
            view = view ?? Utilities.ComApp.ActiveDesignFile.Views[1];
            view.set_Center(ref center);
            //view.set_Origin(ref center);
            var zoomFactor = 5 * 1.5 / view.get_Extents().X;
            view.Zoom(zoomFactor); // (2);
            view.Redraw();
        }

    }
}

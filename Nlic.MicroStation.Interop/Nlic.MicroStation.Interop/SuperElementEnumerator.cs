/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn元素容器的一些操作。
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
    /// 扩展的元素容器。
    /// </summary>
    public class SuperElementEnumerator {
        /// <summary>
        /// 获取围栅内的元素容器.
        /// </summary>
        /// <param name="includeLockedElement">是否包含被锁定的元素.</param>
        /// <param name="cloneElementsIfClipping"></param>
        /// <returns></returns>
        /// <remarks>与围栅的定义模式有关</remarks>
        public static ElementEnumerator GetFenceElementEnumerator(bool includeLockedElement = false,
            bool cloneElementsIfClipping = false) {
            Application app = Utilities.ComApp;

            if (app.ActiveDesignFile.Fence.IsDefined) {
                return app.ActiveDesignFile.Fence.GetContents(cloneElementsIfClipping, includeLockedElement);
            }

            return null;
        }

        /// <summary>
        /// 获取被选择的元素的元素容器.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ElementEnumerator GetSelectedElementEnumerator() {
            Application app = Utilities.ComApp;

            if (app.ActiveModelReference.AnyElementsSelected) {
                return app.ActiveModelReference.GetSelectedElements();
            }

            return null;
        }

        /// <summary>
        /// 获取元素容器，根据围栅及选择的元素获取.
        /// </summary>
        /// <returns></returns>
        /// <remarks>先扫描围栅；
        /// 如果围栅未定义，再扫描选择集；
        /// 如果没有选择元素，返回null。</remarks>
        public static ElementEnumerator GetElementEnumerator() {
            ElementEnumerator ee = GetFenceElementEnumerator();
            if (ee == null) {
                ee = GetSelectedElementEnumerator();
                return ee;
            }

            return ee;
        }

        /// <summary>
        /// 计算元素容器内的元素个数.
        /// </summary>
        /// <param name="ee">元素容器.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetElementEnumeratorElementCount(ElementEnumerator ee) {
            int elemCount = 0;

            Element[] elems = ee.BuildArrayFromContents();
            elemCount = elems.Length;

            return elemCount;
        }

        /// <summary>
        /// 用扫描方式获取模型中的所有元素.
        /// </summary>
        /// <param name="model">模型.</param>
        /// <returns>元素容器</returns>
        /// <remarks></remarks>
        public static ElementEnumerator GetGraphicalElementEnumeratorByScan(
            ModelReference model) {
            ElementScanCriteria scanCriteria = new ElementScanCriteriaClass();
            scanCriteria.ExcludeNonGraphical();
            return model.Scan(scanCriteria);
        }

        /// <summary>
        /// 根据图层获取元素
        /// </summary>
        /// <param name="level"> </param>
        /// <returns> </returns>
        public static ElementEnumerator GetElementEnumeratorByLevel(Level level) {
            ElementScanCriteria scan = new ElementScanCriteriaClass();
            scan.ExcludeAllLevels();
            scan.IncludeLevel(level);

            var comApp = Utilities.ComApp;

            return comApp.ActiveModelReference.Scan(scan);
        }

        /// <summary>
        /// 获取点附近的元素
        /// </summary>
        /// <param name="point">获取范围的中心点</param>
        /// <param name="searchRadius">获取范围的半径</param>
        /// <returns></returns>
        public static ElementEnumerator GetElementEnumeratorByPoint(Point3d point, double searchRadius) {
            var fncEle = Utilities.ComApp.ActiveDesignFile.Fence.IsDefined
                                 ? Utilities.ComApp.ActiveDesignFile.Fence.CreateElement()
                                 : null;
            Utilities.ComApp.ActiveDesignFile.Fence.Undefine();
            var circle = Utilities.ComApp.CreateEllipseElement2(null, ref point, searchRadius, searchRadius,
                                                                Utilities.ComApp.Matrix3dIdentity());
            Utilities.ComApp.ActiveDesignFile.Fence.DefineFromElement(Utilities.ComApp.ActiveDesignFile.Views[1], circle);
            var eleEnum = Utilities.ComApp.ActiveDesignFile.Fence.GetContents();
            Utilities.ComApp.ActiveDesignFile.Fence.Undefine();
            if (fncEle != null) {
                Utilities.ComApp.ActiveDesignFile.Fence.DefineFromElement(Utilities.ComApp.ActiveDesignFile.Views[1], fncEle);
            }
            return eleEnum;
        }

        /// <summary>
        /// 是否放置了围栅或选择了多边形元素
        /// </summary>
        /// <returns></returns>
        public static bool IsDefineQueryElement() {
            var comApp = Utilities.ComApp;
            if (comApp.ActiveDesignFile.Fence.IsDefined) {
                return true;
            }

            if (comApp.ActiveModelReference.AnyElementsSelected) {
                var ee = comApp.ActiveModelReference.GetSelectedElements();
                // 只取第一个
                ee.MoveNext();
                if (SuperElement.IsValidParcel(ee.Current)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取定义查询的元素
        /// </summary>
        /// <returns></returns>
        /// <remarks>如果没有放置fence，也没有选择多边形元素，会报错。建议先进行IsDefineQueryElement()检查。</remarks>
        public static Element GetQueryElement() {
            var comApp = Utilities.ComApp;

            if (comApp.ActiveDesignFile.Fence.IsDefined) {
                return comApp.ActiveDesignFile.Fence.CreateElement();
            }

            try {
                var ee = comApp.ActiveModelReference.GetSelectedElements();
                // 只取第一个
                ee.MoveNext();
                return ee.Current;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 获取定义查询的元素
        /// </summary>
        /// <returns></returns>
        /// <remarks>如果没有放置fence，也没有选择多边形元素，则以当前视图的范围作为查询条件。</remarks>
        public static Element GetQueryElementEx() {
            var comApp = Utilities.ComApp;

            if (comApp.ActiveDesignFile.Fence.IsDefined) {
                return comApp.ActiveDesignFile.Fence.CreateElement();
            }

            if (comApp.ActiveModelReference.AnyElementsSelected) {
                var ee = comApp.ActiveModelReference.GetSelectedElements();
                // 只取第一个
                ee.MoveNext();
                return ee.Current;
            }

            var view = comApp.ActiveDesignFile.Views[1];

            var origin = view.get_Origin();
            var extend = view.get_Extents();
            var point = new Point3d();
            var verties = new List<Point3d>();

            point.X = origin.X;
            point.Y = origin.Y;
            verties.Add(point);

            point.X = origin.X + extend.X;
            verties.Add(point);

            point.Y = origin.Y + extend.Y;
            verties.Add(point);

            point.X = origin.X;
            point.Y = origin.Y + extend.Y;
            verties.Add(point);

            point.Y = origin.Y;
            verties.Add(point);

            return comApp.CreateShapeElement1(null, verties.ToArray());

            //try {
            //    var ee = comApp.ActiveModelReference.GetSelectedElements();
            //    // 只取第一个
            //    ee.MoveNext();
            //    return ee.Current;
            //}
            //catch (Exception ex) {
            //    throw ex;
            //}
        }



    }
}

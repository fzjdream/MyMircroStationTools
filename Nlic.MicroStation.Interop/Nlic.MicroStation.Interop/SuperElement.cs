/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn元素的一些操作。
 * Author:      侯祥意
 * CDT:         2011-05-12
 * Version:     1.0.0.2
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using BMI = Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 扩展的元素。
    /// </summary>
    public class SuperElement {
        public static Application ComApp = Utilities.ComApp;
        private const int ROUND_DIGITS = 2;    // 坐标值四舍五入保留的小数位数


        /// <summary>
        /// 根据精度判断两个点是否相等.
        /// </summary>
        /// <param name="point1">点1.</param>
        /// <param name="point2">点2.</param>
        /// <param name="digits">精度，小数点位数.</param>
        /// <returns>判断结果</returns>
        /// <remarks></remarks>
        public static bool IsPoint3dEqual(Point3d point1, Point3d point2, int digits = 4) {
            if ((Math.Round(point1.X, digits) == Math.Round(point2.X, digits)) &&
                (Math.Round(point1.Y, digits) == Math.Round(point2.Y, digits)) &&
                (Math.Round(point1.Z, digits) == Math.Round(point2.Z, digits))) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 复制元素属性.
        /// </summary>
        /// <param name="templateElement">源模板元素.</param>
        /// <param name="targetElement">目标元素.</param>
        /// <param name="targetElemAlreadyAddToModel">目标元素是否已添加到模型中。</param>
        /// <param name="is2DModel">当前激活模型是否为2D模型。</param>
        /// <remarks></remarks>
        public static void CloneAttribute(Element templateElement, ref Element targetElement,
            bool targetElemAlreadyAddToModel = false,
            bool is2DModel = true) {

            var comApp = BMI.Utilities.ComApp;

            targetElement.Color = templateElement.Color;
            targetElement.LineWeight = templateElement.LineWeight;

            try {
                //if (templateElement.Level != null) {
                targetElement.let_Level(templateElement.Level);
                //}
            }
            catch (Exception ex) {
                targetElement.let_Level(comApp.ActiveSettings.Level);
            }

            try {
                // templateElement.LineStyle如果为“Not Found”，该句代码会出错。
                targetElement.let_LineStyle(templateElement.LineStyle);
            }
            catch (Exception ex) {
                targetElement.let_LineStyle(comApp.ActiveDesignFile.LineStyles.Find("0"));
            }

            targetElement.Class = templateElement.Class;

            // For an element, getting or setting the value of GraphicGroup raises an error if the element is not graphical.
            targetElement.GraphicGroup = templateElement.GraphicGroup;

            targetElement.IsHidden = templateElement.IsHidden;
            targetElement.IsLocked = templateElement.IsLocked;
            targetElement.IsSnappable = templateElement.IsSnappable;
            targetElement.Transparency = templateElement.Transparency;

            if (is2DModel) {
                // Display priority is valid on in 2D files.
                // Attachment.DisplayPriority must be between -15 and 15, inclusive.
                targetElement.DisplayPriority = templateElement.DisplayPriority;
            }

            if (targetElemAlreadyAddToModel) {
                // Setting InDisplaySet raises an error if the element is not already in a model.
                // Changes to the display set are not shown in a view until the next time the view is redrawn.
                targetElement.InDisplaySet = templateElement.InDisplaySet;
                // IsHighlighted raises an exception if the element is not contained in a model. 
                targetElement.IsHighlighted = templateElement.IsHighlighted;
            }

            if (templateElement.IsClosedElement()) {
                targetElement.AsClosedElement().IsFilled = templateElement.AsClosedElement().IsFilled;
                targetElement.AsClosedElement().FillMode = templateElement.AsClosedElement().FillMode;
                targetElement.AsClosedElement().FillColor = templateElement.AsClosedElement().FillColor;
            }

            // copy xdata
            if (templateElement.HasAnyXData()) {
                var appNames = templateElement.GetXDataApplicationNames();
                foreach (var appName in appNames) {
                    var xDataObj = templateElement.GetXData1(appName);
                    targetElement.SetXData1(appName, xDataObj);
                }
            }

            // copy databaselink
            if (templateElement.HasAnyDatabaseLinks()) {
                var dbLinks = templateElement.GetDatabaseLinks();
                foreach (var dbLink in dbLinks) {
                    targetElement.AddDatabaseLink(dbLink);
                }
            }

            // copy tags

            // copy userAttribute


            // todo 添加对tag userAttribute的clone
        }

        /// <summary>
        /// 获取图形元素信息，id、图层、范围等.
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetGraphicalElementInfo(Element elem) {
            const string lineStart = "\r\n  ";

            string elemInfo = GetNonGraphicalElementInfo(elem);
            if (elem.IsGraphical) {
                elemInfo += "\r\n元素范围：";

                Range3d range = elem.Range;

                string elemLowPtStr = "Low: xy = " + range.Low.X + " , " + range.Low.Y;
                string elemHighPtStr = "High: xy = " + range.High.X + " , " + range.High.Y;

                Application app = Utilities.ComApp;
                Point3d center = app.Point3dFromXY((range.Low.X + range.High.X) / 2,
                    (range.Low.Y + range.High.Y) / 2);
                string elemCenterPtStr = "Center: xy = " + center.X + " , " + center.Y;

                string rangeDesc = lineStart + elemLowPtStr + lineStart + elemHighPtStr + lineStart + elemCenterPtStr;

                elemInfo += rangeDesc;
            }

            return elemInfo;
        }

        /// <summary>
        /// 获取非图形元素信息，id、图层、范围等.
        /// </summary>
        /// <param name="elem">.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetNonGraphicalElementInfo(Element elem) {
            const string lineStart = "\r\n  ";

            string elemInfo = "\n元素基本信息：";
            elemInfo += lineStart + "type: " + elem.Type;

            try {
                if (elem.Level != null) {
                    elemInfo += lineStart + "level: " + elem.Level.Name;
                }
            }
            catch (Exception ex) {
                throw new ApplicationException(ex.Message);
            }

            elemInfo += lineStart + "id: " + elem.ID.ToString();
            elemInfo += lineStart + "file position: " + elem.FilePosition.ToString();
            return elemInfo;
        }

        /// <summary>
        /// 移动元素到一个新图层.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="existLevelName">新图层名.</param>
        /// <remarks></remarks>
        private static void MoveToLevel(ref Element elem, string existLevelName) {
            Application app = Utilities.ComApp;
            Level level = app.ActiveDesignFile.Levels.Find(existLevelName);

            elem.let_Level(level);
            elem.Rewrite();
        }

        /// <summary>
        /// 更改元素图层属性.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="levelName">新图层名.</param>
        /// <param name="isNewLevelIfNotExist">如果图层不存在，是否新建</param>
        /// <remarks>如果图层不存在，则新建</remarks>
        public static bool ChangeLevel(ref Element elem, string levelName, bool isNewLevelIfNotExist = false) {
            bool lvlExist = SuperLevel.HasExistLevel(levelName);
            if (lvlExist == false) {
                if (isNewLevelIfNotExist) {
                    SuperLevel.AddNewLevelToActiveDesignFile(levelName);
                }
                else {
                    return false;
                }
            }

            MoveToLevel(ref elem, levelName);
            return true;
        }

        public static bool ChangeColor(ref Element elem, int colorIndex, bool isReWrite = false) {
            try {
                if (elem.IsCellElement()) {
                    elem.Color = colorIndex;
                    var ee = elem.AsComplexElement().GetSubElements();
                    while (ee.MoveNext()) {
                        ee.Current.Color = colorIndex;
                        ee.Current.Rewrite();

                        //var subElem = ee.Current;
                        //ChangeColor(ref subElem, colorIndex, true);
                    }
                }
                else if (elem.IsSharedCellElement()) {
                    var cellDefine = elem.AsSharedCellElement().GetSharedCellDefinition();
                    var ee = cellDefine.GetSubElements();
                    while (ee.MoveNext()) {
                        ee.Current.Color = colorIndex;
                        ee.Current.Rewrite();

                        //var subElem = ee.Current;
                        //ChangeColor(ref subElem, colorIndex, true);
                    }
                }
                else {
                    elem.Color = colorIndex;
                }

                if (isReWrite) {
                    elem.Rewrite();
                }
            }
            catch (Exception ex) {
                SuperMessage.RecordErrorMsgInMsgCenter(ex);
                return false;
            }

            return true;
        }

        public static bool ChangeDisplayPriority(ref Element elem, int priority, bool isReWrite = false) {
            try {
                if (elem.IsCellElement()) {
                    elem.DisplayPriority = priority;
                    var ee = elem.AsComplexElement().GetSubElements();
                    while (ee.MoveNext()) {
                        ee.Current.DisplayPriority = priority;
                        ee.Current.Rewrite();

                        //var subElem = ee.Current;
                        //ChangeDisplayPriority(ref subElem, priority, true);
                    }
                }
                else if (elem.IsSharedCellElement()) {
                    var cellDefine = elem.AsSharedCellElement().GetSharedCellDefinition();
                    var ee = cellDefine.GetSubElements();
                    while (ee.MoveNext()) {
                        ee.Current.DisplayPriority = priority;
                        ee.Current.Rewrite();

                        //var subElem = ee.Current;
                        //ChangeDisplayPriority(ref subElem, priority, true);
                    }
                }
                else {
                    elem.DisplayPriority = priority;
                }

                if (isReWrite) {
                    elem.Rewrite();
                }
            }
            catch (Exception ex) {
                SuperMessage.RecordErrorMsgInMsgCenter(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ArcElement实际上是否为Circle。在MS中，有的圆弧首尾点均相同，SweepAngle为360度，这种圆弧实际上是圆。
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsArcActuallyIsCircle(ArcElement arc) {
            if (IsPoint3dEqual(arc.StartPoint, arc.EndPoint, ROUND_DIGITS)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ArcElement实际上是否为Line。在MS中，有的圆弧几乎是一条直线。
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="toleranceDigits">比较距离容限的小数点精确位数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsArcActuallyIsLine(ArcElement arc, int toleranceDigits = 6) {
            Application app = Utilities.ComApp;
            double lineDistance = app.Point3dDistance(arc.StartPoint, arc.EndPoint);    //圆弧首尾点直线距离

            //const int lenDigits = 2;    //圆弧首尾点直线距离与圆弧长度比较的小数位数

            return Math.Round(lineDistance, toleranceDigits) == Math.Round(arc.Length, toleranceDigits);  // Math.Round(lineDistance, lenDigits) == Math.Round(arc.Length, lenDigits);
        }

        /// <summary>
        /// CurveElement实际上是否为一个点。在MS中，有的curve只有两个点，且首尾点均相同.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns><c>true</c> if [is curve actually is point] [the specified curve]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsCurveActuallyIsPoint(CurveElement curve) {
            if (IsPoint3dEqual(curve.StartPoint, curve.EndPoint, ROUND_DIGITS)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取CurveElement的中点.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Point3d GetCurveElementMidPoint(CurveElement curve) {
            double x = (curve.StartPoint.X + curve.EndPoint.X) / 2;
            double y = (curve.StartPoint.Y + curve.EndPoint.Y) / 2;
            double z = (curve.StartPoint.Z + curve.EndPoint.Z) / 2;

            var pt = new Point3d { X = x, Y = y, Z = z };

            return pt;
        }

        /// <summary>
        /// VertexList是否Valid。有的VertexList(如line,curve,shape)实际上为一个点，其特点为：只有两个点，且这两个点坐标相同；这类元素为bad element.
        /// </summary>
        /// <param name="vertexList"></param>
        /// <returns><c>true</c> if [is valid vertex list] [the specified vertex list]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsValidVertexList(VertexList vertexList) {
            if (vertexList.VerticesCount == 1) {
                return false;
            }
            if (vertexList.VerticesCount == 2) {
                Point3d[] vertices = vertexList.GetVertices();
                if (IsPoint3dEqual(vertices[0], vertices[1], ROUND_DIGITS)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 打散复杂元素.
        /// </summary>
        /// <param name="compElem"></param>
        /// <remarks></remarks>
        public static void DropComplexElement(Element compElem) {
            if (compElem.IsComplexElement() == false) {
                return;
            }

            Application app = Utilities.ComApp;

            ElementEnumerator ee = compElem.AsComplexElement().Drop();
            app.ActiveModelReference.RemoveElement(compElem);

            app.ActiveModelReference.AddElements(ee.BuildArrayFromContents());
        }

        /// <summary>
        /// 写入XData属性。
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="appName">XData的标题。</param>
        /// <param name="xValues">所有属性值。</param>
        /// <param name="isDeleteAllExistXData">是否删除所有已存在的XData</param>
        /// <param name="isDeleteExistSameAppNameXData">对已存在的，具有相同ApplicationName的XData是否先删除，再重新写入</param>
        /// <param name="isAddXDataToSubElements">对于复杂元素的每一个子元素，是否写入XData</param>
        /// <remarks >
        /// 1、SetXData方法默认会删除已存在的、同名的ApplicationName的XData；
        /// 2、对于复杂元素，SetXData不会将XData值写入到每个子元素中，当打散复杂元素，其XData值会丢失。
        /// </remarks>
        public static void WriteXData(ref Element elem,
            string appName, XDatum[] xValues,
            bool isDeleteAllExistXData = false,
            bool isDeleteExistSameAppNameXData = false,
            bool isAddXDataToSubElements = true) {
            // 清空已有XData
            if (isDeleteAllExistXData) {
                if (elem.HasAnyXData()) {
                    elem.DeleteAllXData();
                }
            }

            WriteXDataProcess(ref elem, appName, xValues, isDeleteExistSameAppNameXData);

            if (!isAddXDataToSubElements) {
                return;
            }

            if (elem.IsComplexElement()) {
                var subEe = elem.AsComplexElement().GetSubElements();

                while (subEe.MoveNext()) {
                    var subElem = subEe.Current;
                    WriteXData(ref subElem, appName, xValues, isDeleteAllExistXData, isDeleteExistSameAppNameXData);
                }
            }
        }

        public static void WriteXData(ref Element elem,
            string appName, string[] xValues,
            bool isDeleteAllExistXData = false,
            bool isDeleteExistSameAppNameXData = false,
            bool isAddXDataToSubElements = true) {
            var xDatums = new XDatum[0];

            foreach (var xData in xValues) {
                Utilities.ComApp.AppendXDatum(ref xDatums, MsdXDatumType.String, xData);
            }

            WriteXData(ref elem, appName, xDatums,
                isDeleteAllExistXData,
                isDeleteExistSameAppNameXData,
                isAddXDataToSubElements);
        }

        private static void WriteXDataProcess(ref Element elem,
            string appName, XDatum[] xValues,
            bool isDeleteExistSameAppNameXData) {
            var xDataObj = elem.GetXData1(appName);

            if (isDeleteExistSameAppNameXData) {
                while (xDataObj.Count > 0) {
                    xDataObj.DeleteEntry(0);
                }
            }

            foreach (var xData in xValues) {
                xDataObj.AppendXDatum(xData.Type, xData.Value);
            }

            elem.SetXData1(appName, xDataObj);
            elem.Rewrite();
        }

        /// <summary>
        /// 获取单元的中心原点。
        /// </summary>
        /// <param name="cell">单元元素。</param>
        /// <returns>和单元图形相吻合的中心原点。</returns>
        /// <remarks>在v8中，有一类为Associative Region的单元，没有真正意义上的中心原点，其Origin为0。</remarks>
        public static Point3d GetCellElementOrigin(CellElement cell) {
            Point3d origin = cell.Origin;
            Point3d rangeCenter = GetElementRangeCenter(cell);

            if (IsPoint3dEqual(origin, rangeCenter, ROUND_DIGITS)) {
                return origin;
            }

            return rangeCenter;
        }

        /// <summary>
        /// 获取共享单元的中心原点。
        /// </summary>
        /// <param name="sharedCell">共享单元。</param>
        /// <returns>和共享单元图形相吻合的中心原点。</returns>
        /// <remarks>在v8中，有一些共享单元元素有问题，中心原点偏的太离谱。</remarks>
        public static Point3d GetSharedCellOrigin(SharedCellElement sharedCell) {
            Point3d origin = sharedCell.Origin;
            Point3d rangeCenter = GetElementRangeCenter(sharedCell);

            if (rangeCenter.X < 0 || rangeCenter.Y < 0) {
                return origin;
            }

            if (IsPoint3dEqual(origin, rangeCenter, ROUND_DIGITS)) {
                return origin;
            }

            return rangeCenter;
        }

        /// <summary>
        /// 获取元素范围的中心点。
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static Point3d GetElementRangeCenter(Element elem) {
            return new Point3d {
                X = (elem.Range.Low.X + elem.Range.High.X) / 2,
                Y = (elem.Range.Low.Y + elem.Range.High.Y) / 2,
                Z = (elem.Range.Low.Z + elem.Range.High.Z) / 2
            };
        }

        /// <summary>
        /// 获取元素范围的中心点。
        /// </summary>
        /// <param name="elems"></param>
        /// <returns></returns>
        public static Point3d GetElementsRangeCenter(List<Element> elems) {
            if (elems == null) {
                return new Point3d();
            }

            //先获取整个数组内的全部元素的最大最小XYZ
            var maxX = elems[0].Range.High.X;
            var minX = elems[0].Range.Low.X;
            var maxY = elems[0].Range.High.Y;
            var minY = elems[0].Range.Low.Y;
            var maxZ = elems[0].Range.High.Z;
            var minZ = elems[0].Range.Low.Z;

            var elemCount = elems.Count;

            for (var i = 1; i < elemCount; i++) {
                var range = elems[i].Range;
                if (range.High.X > maxX) {
                    maxX = range.High.X;
                }

                if (range.High.Y > maxY) {
                    maxY = range.High.Y;
                }

                if (range.High.Z > maxZ) {
                    maxZ = range.High.Z;
                }

                if (range.Low.X < minX) {
                    minX = elems[i].Range.Low.X;
                }

                if (range.Low.Y < minY) {
                    minY = elems[i].Range.Low.Y;
                }

                if (range.Low.Z < minZ) {
                    minZ = elems[i].Range.Low.Z;
                }
            }

            return new Point3d {
                X = (minX + maxX) / 2,
                Y = (minY + maxY) / 2,
                Z = (minZ + maxZ) / 2
            };

        }

        /// <summary>
        /// 圆弧元素是否为标准圆弧。
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static bool IsStandardArcElement(ArcElement arc) {
            return Math.Round(arc.PrimaryRadius, ROUND_DIGITS - 1) == Math.Round(arc.SecondaryRadius, ROUND_DIGITS - 1);
        }

        /// <summary>
        /// 圆弧是否为椭圆弧（非标准圆弧）。
        /// </summary>
        /// <param name="arc">The arc.</param>
        /// <returns>
        ///   <c>true</c> if [is arc is ellipse arc] [the specified arc]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>在很多应用中，不支持椭圆弧。</remarks>
        // todo 测试未通过
        public static bool IsArcIsEllipseArc(ArcElement arc) {
            return !IsStandardArcElement(arc);
        }

        /// <summary>
        /// 圆弧是否太小，近似于一个点
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        /// <remarks>有一些圆弧太小，长度不到0.1，半径小于厘米级。</remarks>
        public static bool IsArcLikePoint(ArcElement arc) {
            const double lenLimit = 0.5;
            return arc.PrimaryRadius <= lenLimit;
        }

        /// <summary>
        /// Determines whether [is elliptical element is circle] [the specified elliptical element].
        /// </summary>
        /// <param name="ellipticalElement">The elliptical element.</param>
        /// <returns>
        ///   <c>true</c> if [is elliptical element is circle] [the specified elliptical element]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEllipticalElementIsCircle(EllipticalElement ellipticalElement) {
            return ellipticalElement.IsCircular;
        }

        /// <summary>
        /// 为文本节点元素添加下划线（或上划线）。
        /// </summary>
        /// <param name="textNodeElement">文本节点元素。</param>
        /// <param name="lineOffsetFactor">下划线或上划线偏移的距离（间距的比例因子）。</param>
        public static void AddUnderlineForTextNode(TextNodeElement textNodeElement, double lineOffsetFactor = 0.5) {
            var ee = textNodeElement.GetSubElements();

            for (var i = 1; i <= textNodeElement.TextLinesCount - 1; i++) {
                var lineSpacing = textNodeElement.LineSpacing;

                ee.MoveNext();
                var textElem = ee.Current as TextElement;

                if (textNodeElement.TextLine[i].Length >= textNodeElement.TextLine[i + 1].Length) {
                    if (textElem != null) {
                        textElem.TextStyle.IsOverlined = false;
                        textElem.TextStyle.IsUnderlined = true;
                        textElem.TextStyle.UnderlineOffset = (lineSpacing * lineOffsetFactor) /
                                                             textElem.AnnotationScaleFactor;
                        textElem.Rewrite();
                    }
                }
                else {
                    if (textElem != null) {
                        textElem.TextStyle.IsUnderlined = false;
                        textElem.TextStyle.IsOverlined = false;
                        textElem.Rewrite();
                    }

                    ee.MoveNext();
                    textElem = ee.Current as TextElement;

                    if (textElem != null) {
                        textElem.TextStyle.IsUnderlined = false;
                        textElem.TextStyle.IsOverlined = true;
                        textElem.TextStyle.OverlineOffset = (lineSpacing * lineOffsetFactor) /
                                                            textElem.AnnotationScaleFactor;
                        textElem.Rewrite();
                    }
                }
            }
        }

        /// <summary>
        /// 获取圆弧上的中点。
        /// </summary>
        /// <param name="arcElement"></param>
        /// <returns></returns>
        public static Point3d GetArcMidpoint(ArcElement arcElement) {
            return arcElement.PointAtDistance(arcElement.Length / 2);
        }

        [DllImport("stdmdlbltin.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern long mdlElmdscr_isGroupedHole(long groupEdP);

        [DllImport("stdmdlbltin.dll")]
        private static extern long mdlElmdscr_createShapeWithHoles(ref long outEdPP, long solidEdP, long holeEdP);
        // Declare Function mdlElmdscr_createShapeWithHoles Lib "stdmdlbltin.dll" ( ByRef outEdPP As Long , ByVal solidEdP As Long , ByVal holeEdP As Long ) As Long 

        [DllImport("stdmdlbltin.dll")]
        public static extern void mdlElmdscr_setProperties(int el, ref int level, ref int ggNum,
                                                           ref int elementClass, ref int locked, ref int newElm,
                                                           ref int modified, ref int viewIndepend, ref int solidHole);

        /// <summary>
        /// Determines whether [is grouped hole element] [the specified elem].
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <returns>
        ///   <c>true</c> if [is grouped hole element] [the specified elem]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGroupedHoleElement(Element elem) {
#if NODE_DEBUG
            System.Windows.Forms.MessageBox.Show("mdlElmdscr_isGroupedHole(elem.MdlElementDescrP()) = " + mdlElmdscr_isGroupedHole(elem.MdlElementDescrP()));
#endif

            if (!elem.IsCellElement()) {
                return false;
            }

            if (mdlElmdscr_isGroupedHole(elem.MdlElementDescrP()) == 1) {
                return true;
            }

            var subEE = elem.AsCellElement().GetSubElements();
            while (subEE.MoveNext()) {
                if (subEE.Current.IsClosedElement() == false) {
                    return false;
                }
                if (subEE.Current.AsClosedElement().IsHole) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 将面的IsHole属性设置为True。
        /// </summary>
        /// <param name="closedElement">The closed element.</param>
        /// <returns></returns>
        /// <remarks>v8中没有创建带洞多边形的方法，但是将内多边形的IsHole属性设置为True，创建出的多边形就是带洞多边形。</remarks>
        public static ClosedElement SetShapeIsHolePropertyToTrue(ClosedElement closedElement) {
            var NULLPtr = 0;
            var isHoleMask = 1;
            if (closedElement != null) {
                var element = closedElement as Element;
                if (element != null) {
                    var eleDscrP = element.MdlElementDescrP();
                    mdlElmdscr_setProperties(eleDscrP, ref NULLPtr, ref NULLPtr, ref NULLPtr, ref NULLPtr, ref NULLPtr, ref NULLPtr, ref NULLPtr, ref isHoleMask);
                    var newElem = Utilities.ComApp.MdlCreateElementFromElementDescrP(eleDscrP);
                    if (newElem.AsClosedElement().IsHole) {
                        return newElem.AsClosedElement();
                    }
                }
            }
            return closedElement;
        }

        ///// <summary>
        ///// 创建带洞的多边形。
        ///// </summary>
        ///// <param name="closedElems">所有子多边形。</param>
        ///// <returns></returns>
        //public static Element CreateGroupedHoleElement(IList<ClosedElement> closedElems) {
        //    var elemLst = closedElems as List<ClosedElement>;
        //    //取面积最大的闭合多边形为外多边形，其余的为洞多边形
        //    var maxArea = closedElems.Max(x => x.Area());
        //    var newList = new List<Element>();
        //    if (elemLst != null)
        //        foreach (var element in elemLst) {
        //            if (element.Area() != maxArea) {
        //                var holeElem = SetShapeIsHolePropertyToTrue(element) as Element;
        //                newList.Add(holeElem);
        //            }
        //            else {
        //                newList.Add(element as Element);
        //            }
        //        }
        //    var subElems = newList.ToArray();
        //    var range = newList.First().Range;
        //    //计算洞或者岛多边形的集合中心)
        //    range = newList.Aggregate(range, (current, element) => Utilities.ComApp.Range3dUnion(current, element.Range));
        //    var cellOrigin = new Point3d { X = range.High.X / 2 + range.Low.X / 2, Y = range.High.Y / 2 + range.Low.Y / 2 };

        //    var gholeElem = Utilities.ComApp.CreateCellElement1(string.Empty, ref subElems, cellOrigin);
        //    return gholeElem;
        //}
        /// <summary>
        /// 创建带洞的多边形。
        /// </summary>
        /// <param name="closedElems">所有子多边形。</param>
        /// <returns></returns>
        public static Element CreateGroupedHoleElement(IList<ClosedElement> closedElems) {
            //var elemLst = closedElems as List<ClosedElement>;
            //取面积最大的闭合多边形为外多边形，其余的为洞多边形
            var maxArea = closedElems.Max(x => x.Area());
            var newList = new List<Element>();
            if (closedElems.Count != 0)
                foreach (var element in closedElems) {
                    if (!element.Area().Equals(maxArea)) {
                        var holeElem = SetShapeIsHolePropertyToTrue(element) as Element;
                        newList.Add(holeElem);
                    }
                    else {
                        newList.Add(element as Element);
                    }
                }
            var subElems = newList.ToArray();
            var range = newList.First().Range;
            //计算洞或者岛多边形的集合中心)
            range = newList.Aggregate(range, (current, element) => Utilities.ComApp.Range3dUnion(current, element.Range));
            var cellOrigin = new Point3d { X = range.High.X / 2 + range.Low.X / 2, Y = range.High.Y / 2 + range.Low.Y / 2 };

            var gholeElem = Utilities.ComApp.CreateCellElement1(string.Empty, ref subElems, cellOrigin);
            return gholeElem;
        }
        ///// <summary>
        ///// 创建带洞的多边形。
        ///// </summary>
        ///// <param name="cellElem"></param>
        ///// <returns></returns>
        //public static Element CreateGroupedHoleElement(CellElement cellElem) {
        //    // todo
        //    return null;
        //}

        /// <summary>
        /// 检查一个element是否为一个地块，地块只能是Polygon或者Compound Polygon。
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid parcel] [the specified elem]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidParcel(Element elem) {
            // 地块一定是一个Polygon或者Compound Polygon
            if (elem.IsClosedElement()) {
                return true;
            }
            if (elem.IsCellElement()) {
                var cellEe = elem.AsCellElement().GetSubElements();

                while (cellEe.MoveNext()) {
                    if (IsValidParcel(cellEe.Current) == false) {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// 获取多边形的面积。
        /// </summary>
        /// <returns></returns>
        // todo 测试未通过
        [Obsolete("已废弃的函数，推荐使用GetArea()", false)]
        public static double GetShapeArea(Element elem) {
            double area = 0d;

            if (elem.IsClosedElement()) {
                area = elem.AsClosedElement().Area();
            }
            else if (IsGroupedHoleElement(elem) || IsValidParcel(elem)) {
                var cellEe = elem.AsCellElement().GetSubElements();
                while (cellEe.MoveNext()) {
                    double subArea = 0d;
                    if (cellEe.Current.AsClosedElement().IsHole == false) {
                        subArea = cellEe.Current.AsClosedElement().Area();
                    }
                    else if (cellEe.Current.AsClosedElement().IsHole) {
                        subArea = -cellEe.Current.AsClosedElement().Area();
                    }

                    area += subArea;
                }
            }

            return area;
        }

        /// <summary>
        /// 多边形是否是一个矩形。
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns>
        ///   <c>true</c> if [is shape is rectangle] [the specified shape]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>v8中没有矩形这一数据类型。</remarks>
        // todo 未测试
        public static bool IsShapeIsRectangle(ShapeElement shape) {
            // todo 未实现
            return false;
        }

        /// <summary>
        /// 线是否为一个点。
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>
        ///   <c>true</c> if [line is point] [the specified line]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>v8中没有点这一数据类型，长度为0的线即为点。</remarks>
        public static bool IsLineIsPoint(LineElement line) {
            return line.Length == 0;
        }

        /// <summary>
        /// Gets the point string.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static string GetPointString(Point3d point) {
            var sb = new StringBuilder();

            sb.Append(point.X);
            sb.Append(",");
            sb.Append(point.Y);

            if (point.Z != 0) {
                sb.Append(",");
                sb.Append(point.Z);

                sb.Insert(0, "xyz = ");
            }
            else {
                sb.Insert(0, "xy = ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取线的中点坐标。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <remarks>该点一定位于线上，且距离线的首尾点距离相等</remarks>
        public static Point3d GetLineMidPoint(LineElement line) {
            return line.PointAtDistance(line.Length / 2);
        }

        /// <summary>
        /// 获取线的中间点坐标。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Point3d GetLineMidPositionPoint(LineElement line) {
            if (line.VerticesCount == 2) {
                return GetLineMidPoint(line);
            }

            int midIndex = Convert.ToInt32(line.VerticesCount / 2);
            return line.get_Vertex(midIndex);
        }

        /// <summary>
        /// 获取文本节点的值。
        /// </summary>
        /// <param name="textNode"></param>
        /// <returns></returns>
        public static List<string> GetTextNodeValue(TextNodeElement textNode) {
            var strList = new List<string>();
            for (int i = 1; i <= textNode.TextLinesCount; i++) {
                if (string.IsNullOrEmpty(textNode.TextLine[i]) == false) {
                    strList.Add(textNode.TextLine[i]);
                }
            }

            return strList;
        }

        /// <summary>
        /// Determines whether [is real complex shape elem] [the specified complex shape].
        /// </summary>
        /// <param name="complexShape">The complex shape.</param>
        /// <returns>
        ///   <c>false</c>复杂多边形的子元素为一个多边形; otherwise, <c>true</c>.
        /// </returns>
        public static bool IsRealComplexShapeElem(ComplexShapeElement complexShape) {
            ElementEnumerator ee = complexShape.GetSubElements();
            while (ee.MoveNext()) {
                if (ee.Current.IsClosedElement() || ee.Current.IsComplexElement()) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取关键点捕捉到的所有元素
        /// </summary>
        /// <param name="locatePoint"></param>
        /// <param name="locateView"></param>
        /// <returns></returns>
        /// <remarks>
        /// Author:Michael Hou
        /// CDT:2013-10-22
        /// UDT:
        /// </remarks>
        public static List<Element> GetLocateElements(Point3d locatePoint, View locateView) {
            var elems = new List<Element>();
            Element elem = Utilities.ComApp.CommandState.LocateElement(locatePoint, locateView, true);

            while (elem != null) {
                if (elem.Level != null) {
                    elems.Add(elem);
                }
                elem = Utilities.ComApp.CommandState.LocateElement(locatePoint, locateView, false);
            }

            return elems;
        }
        /// <summary>
        /// 生成点数超过4000个点的复杂多边形
        /// </summary>
        /// <returns></returns>
        public static ComplexShapeElement CreateShapeElementByEnormousPoints(List<Point3d> verties) {
            if (verties == null || verties.Count == 0) {
                return null;
            }

            var len = verties.Count;
            var hazz = len / 4000;//取整
            var remainder = len % 4000;//取余 
            var chainableElems = new List<ChainableElement>();
            var subVerties = new List<Point3d>();

            for (var i = 0; i < hazz; i++) {
                subVerties.Clear();
                subVerties = verties.GetRange(4000 * i, 4000);
                chainableElems.Add(Utilities.ComApp.CreateLineElement1(null, subVerties.ToArray()));
            }

            //整除不尽的部分
            if (remainder != 0) {
                subVerties.Clear();
                subVerties = verties.GetRange(4000 * hazz, remainder);
                chainableElems.Add(Utilities.ComApp.CreateLineElement1(null, subVerties.ToArray()));
            }


            return Utilities.ComApp.CreateComplexShapeElement1(chainableElems.ToArray());
        }
        /// <summary>
        /// 生成点数超过4000个点的复杂多边形
        /// </summary>
        /// <returns></returns>
        public static ComplexShapeElement CreateShapeElementByEnormousPoints(Point3d[] verties) {
            if (verties == null || verties.Length == 0) {
                return null;
            }

            var vertiesList = verties.ToList();
            var len = vertiesList.Count;
            var hazz = len / 4000;//取整
            var remainder = len % 4000;//取余 
            var chainableElems = new List<ChainableElement>();
            var subVerties = new List<Point3d>();

            for (var i = 0; i < hazz; i++) {
                subVerties.Clear();
                subVerties = vertiesList.GetRange(4000 * i, 4000);
                chainableElems.Add(Utilities.ComApp.CreateLineElement1(null, subVerties.ToArray()));
            }

            //整除不尽的部分
            if (remainder != 0) {
                subVerties.Clear();
                subVerties = vertiesList.GetRange(4000 * hazz, remainder);
                chainableElems.Add(Utilities.ComApp.CreateLineElement1(null, subVerties.ToArray()));
            }


            return Utilities.ComApp.CreateComplexShapeElement1(chainableElems.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closedElements"></param>
        /// <returns></returns>
        public static List<Element> GetElementListByClosedElementList(List<ClosedElement> closedElements) {
            if (closedElements == null || closedElements.Count <= 0) {
                return null;
            }

            return closedElements.Cast<Element>().ToList();
        }

        /// <summary>
        /// 根据图层名，获取图层上元素集
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static List<Element> GetElementListByLevel(Level level) {
            var elementList = new List<Element>();
            Application app = Utilities.ComApp;
            ElementScanCriteria oScanCriteria = new ElementScanCriteriaClass();
            oScanCriteria.ExcludeAllLevels();
            oScanCriteria.IncludeLevel(level);
            ElementEnumerator oScanEnumerator = app.ActiveModelReference.Scan(oScanCriteria);
            while (oScanEnumerator.MoveNext()) {
                elementList.Add(oScanEnumerator.Current);
            }
            return elementList;
        }
        /// <summary>
        /// 删除图层上的元素
        /// </summary>
        /// <param name="level"></param>
        public static void RemoveElementByLevel(Level level) {
            var elements = GetElementListByLevel(level);
            Application app = Utilities.ComApp;
            foreach (var element in elements) {
                app.ActiveModelReference.RemoveElement(element);
            }
        }

        /// <summary>
        ///删除元素 
        /// </summary>
        /// <param name="element"></param>
        public static void RemoveElement(Element element) {
            Utilities.ComApp.ActiveModelReference.RemoveElement(element);
        }

        /// <summary>
        /// 获取元素的面积
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetArea(Element element) {
            double area = 0;

            //try {
            if (element.IsClosedElement()) {
                area = element.AsClosedElement().Area();
            }
            else if (element.IsCellElement()) {
                var ee = element.AsCellElement().GetSubElements();
                ee.Reset();
                while (ee.MoveNext()) {
                    if (ee.Current.IsClosedElement()) {
                        double dbtemp = ee.Current.AsClosedElement().Area();
                        if (ee.Current.AsClosedElement().IsHole) {
                            area -= dbtemp;
                        }
                        else {
                            area += dbtemp;
                        }
                    }
                    else if (ee.Current.IsCellElement()) {
                        area += GetArea(ee.Current);
                    }
                    else {
                        throw new ApplicationException("单元中的子元素不是多边形，不能计算面积！");
                    }
                }
            }
            //}
            //catch (Exception ex) {
            //    area = 0;
            //    SuperMessage.RecordErrorMsgInMsgCenter(ex, element);
            //}

            return area;
        }

        /// <summary>
        /// 获取元素中心点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Point3d GetElementCentriod(Element element) {
            var centriod = new Point3d();

            //try {
            bool hasCentriod = false;

            if (element.IsClosedElement()) {
                //简单闭合多边形
                centriod = element.AsClosedElement().Centroid();
                hasCentriod = true;
            }
            else if (element.IsCellElement()) {
                var subElems = element.AsCellElement().GetSubElements();
                while (subElems.MoveNext()) {
                    if (subElems.Current.IsClosedElement()) {
                        centriod = subElems.Current.AsClosedElement().Centroid();
                        hasCentriod = true;
                        break;
                    }
                }
            }


            if (!hasCentriod) {
                centriod = GetElementRangeCenter(element);
            }

            //else if (IsGroupedHoleElement(element)) {//带洞多边形
            //    var subElement = element.AsCellElement().GetSubElements();
            //    while (subElement.MoveNext()) {
            //        if (subElement.Current.AsClosedElement().IsHole == false) {
            //            centriod = subElement.Current.AsClosedElement().Centroid();
            //            break;
            //        }
            //    }
            //}

            //}
            //catch (Exception ex) {
            //    SuperMessage.RecordErrorMsgInMsgCenter(ex, element);
            //}

            return centriod;
        }

        /// <summary>
        /// 通过围栅获取单行文本元素
        /// </summary>
        /// <returns></returns>
        public static List<TextElement> GetTextElementByFance() {
            var fence = Utilities.ComApp.ActiveDesignFile.Fence;
            var textElementList = new List<TextElement>();
            if (fence.IsDefined) {
                var elements = fence.GetContents();
                while (elements.MoveNext()) {
                    if (elements.Current.IsTextElement()) {
                        textElementList.Add(elements.Current.AsTextElement());
                    }
                }
                return textElementList;
            }
            return textElementList;
        }
        /// <summary>
        /// 通过围栅获取多行文本元素
        /// </summary>
        /// <returns></returns>
        public static List<TextNodeElement> GetTextNodexElementByFance() {
            var fence = Utilities.ComApp.ActiveDesignFile.Fence;
            var textElementList = new List<TextNodeElement>();
            if (fence.IsDefined) {
                var elements = fence.GetContents();
                while (elements.MoveNext()) {
                    if (elements.Current.IsTextNodeElement()) {
                        textElementList.Add(elements.Current.AsTextNodeElement());
                    }
                }
                return textElementList;
            }
            return textElementList;
        }
        /// <summary>
        /// 打散多行文本，并放到不同的图层
        /// </summary>
        public static void SplitMutilTextElementToDifferentToLayer() {
            var txtNodeElementList = GetTextNodexElementByFance();
            var txtElementList = GetTextElementByFance();
            var elementList = new List<Element>();
            elementList.AddRange(txtNodeElementList.Cast<Element>());
            elementList.AddRange(txtElementList.Cast<Element>());
            Matrix3d matrix3dIdentity = Utilities.ComApp.Matrix3dIdentity();
            foreach (var element in elementList) {
                if (element.IsTextElement()) {
                    var textElement = element.AsTextElement();
                    Point3d point = textElement.get_Origin();
                    //                    textElement.TextStyle.Font = Utilities.ComApp.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "新宋体");
                    var text = textElement.Text;
                    if (string.IsNullOrEmpty(text.Trim())) {
                        continue;
                    }
                    var newTxtElement = Utilities.ComApp.CreateTextElement1(null, text, ref point, ref matrix3dIdentity);
                    newTxtElement.TextStyle = textElement.TextStyle;
                    newTxtElement.TextStyle.Width = 0.1;
                    newTxtElement.TextStyle.Height = 0.1;
                    newTxtElement.TextStyle.Justification = MsdTextJustification.LeftCenter;
                    var level = SuperLevel.GetLevelFromActiveDesignFile("分割文本层" + 0);
                    newTxtElement.Level = level;
                    Utilities.ComApp.ActiveModelReference.AddElement(newTxtElement);
                }
                else if (element.IsTextNodeElement()) {
                    Point3d point = element.AsTextNodeElement().Origin;
                    var subTextElements = element.AsTextNodeElement().GetSubElements();
                    int i = 0;
                    while (subTextElements.MoveNext()) {
                        var txtElement = subTextElements.Current.AsTextElement();
                        //                        txtElement.TextStyle.Font = Utilities.ComApp.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "新宋体");
                        var text = txtElement.Text;
                        if (string.IsNullOrEmpty(text.Trim())) {
                            continue;
                        }
                        var level = SuperLevel.GetLevelFromActiveDesignFile("分割文本层" + i);
                        var newTxtElement = Utilities.ComApp.CreateTextElement1(null, text, ref point, ref matrix3dIdentity);
                        newTxtElement.Level = level;
                        newTxtElement.TextStyle = txtElement.TextStyle;
                        newTxtElement.TextStyle.Width = 0.1;
                        newTxtElement.TextStyle.Height = 0.1;
                        newTxtElement.TextStyle.Justification = MsdTextJustification.LeftCenter;
                        Utilities.ComApp.ActiveModelReference.AddElement(newTxtElement);
                        //                        if (Utilities.ComApp.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "宋体") == null) {
                        //                            newTxtElement.TextStyle.Font =
                        //                                Utilities.ComApp.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "宋体");
                        //                        }
                        //                        else {
                        //                            newTxtElement.TextStyle.Font =
                        //                                Utilities.ComApp.ActiveDesignFile.Fonts.Find(MsdFontType.WindowsTrueType, "新宋体");
                        //                        }
                        i++;
                    }
                }

            }
        }
        /// <summary>
        /// 以平移中心的的方式转换元素坐标
        /// </summary>
        /// <param name="elem">待转换的元素</param>
        /// <param name="origin">待转换元素的中心原点</param>
        /// <param name="newOrigin">新的中心原点</param>
        /// <param name="isRewrite"></param>
        public static void MoveElement(ref  Element elem, Point3d origin, Point3d newOrigin, bool isRewrite = true) {
            var ptMove = new Point3d {
                X = newOrigin.X - origin.X,
                Y = newOrigin.Y - origin.Y,
                Z = newOrigin.Z - origin.Z
            };
            elem.Move(ptMove);
            if (isRewrite) {
                elem.Rewrite();
            }
        }
        /// <summary>
        /// 取消选择全部元素
        /// </summary>
        public static void UnselectAllElements() {
            ComApp.ActiveModelReference.UnselectAllElements();
        }
        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="element"></param>
        public static void SelectElement(Element element) {
            ComApp.ActiveModelReference.SelectElement(element);
        }
        /// <summary>
        /// 通过Id获取元素
        /// </summary>
        /// <param name="id"></param>
        public static Element GetElementById(long id) {
            return ComApp.ActiveModelReference.GetElementByID(id);
        }
        /// <summary>
        /// 设置选择的文本居中对齐
        /// </summary>
        public static void SetTextElementJustification(){
            var pointSelectEven = new PointSelectEventHandler();
            ComApp.CommandState.StartPrimitive(pointSelectEven);
        }
    }
}

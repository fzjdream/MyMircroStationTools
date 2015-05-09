/************************************************************************
 * * Copyright © 2015 南宁市国土资源信息中心 All rights reserved.
 * *
 * CLR:         4.0.30319.18408
 * Machine:     XINXI-FENGZJ
 * File:        ElementOperation
 * GUID:        bc266ecf-a467-4abc-b4e3-856d977142ef
 * Domain:      NLIS
 * User:        fengzhenjian 
 * ----------------------------------------------------------------------
 * Depiction:   
 * Author:      Von_dream
 * CDT:         2015/4/8 15:47:31
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
using System.Windows.Forms;
using Bentley.Internal.MicroStation.Elements;
using Bentley.Interop.MicroStationDGN;
using Nlic.MicroStation.Interop;
using Bentley.MicroStation.InteropServices;
using Application = Bentley.Interop.MicroStationDGN.Application;
using Element = Bentley.Interop.MicroStationDGN.Element;

namespace MyMicroStationTools {
    public class ElementOperation {
        private static Application _comApp = SuperElement.ComApp;

        /// <summary>
        /// 曲线折线化
        /// </summary>
        public ElementCollection StrokeCurveElement() {
            var failElemCollection = new ElementCollection {
                CurveElementList = new List<Element>(),
                BsplineCurveElementList = new List<Element>()
            };
            failElemCollection.CurveElementList = new List<Element>();
            failElemCollection.BsplineCurveElementList = new List<Element>();
            const double tolerance = 0.0005;
            int curveCount = 0;
            int bsplineCount = 0;
            ElementScanCriteria esc = new ElementScanCriteriaClass();
            //打散复杂链元素；
            //            DropComplexElement();
            esc.IncludeType(MsdElementType.BsplineCurve);
            esc.IncludeType(MsdElementType.Curve);
            var elemEnumerator = _comApp.ActiveModelReference.Scan(esc);
            while (elemEnumerator != null && elemEnumerator.MoveNext()) {
                var elem = elemEnumerator.Current;
                if (elem.IsBsplineCurveElement()) {
                    try {
                        var points = elem.ConstructVertexList(tolerance);
                        var lineElem = _comApp.CreateLineElement1(elem, ref points);
                        _comApp.ActiveModelReference.AddElement(lineElem);
                        lineElem.Redraw();
                        _comApp.ActiveModelReference.RemoveElement(elem);
                        bsplineCount += 1;
                    }
                    catch (Exception e) {
                        SuperMessage.RecordErrorMsgInMsgCenter("B样条折线化异常", e.ToString());
                        //                        failElemCollection.BsplineCurveElementList.Add(elem.AsBsplineCurveElement());
                        //删除0长度的线
                        try {
                            if (elem.AsBsplineCurveElement().Length == 0) {
                                _comApp.ActiveModelReference.RemoveElement(elem);
                            }
                        }
                        catch (Exception exception) {
                            //                            Console.WriteLine(exception);
                        }
                    }
                }
                else if (elem.IsCurveElement()) {
                    try {
                        var points = elem.ConstructVertexList(tolerance);
                        var lineElem = _comApp.CreateLineElement1(elem, ref points);
                        _comApp.ActiveModelReference.AddElement(lineElem);
                        lineElem.Redraw();
                        _comApp.ActiveModelReference.RemoveElement(elem);
                        bsplineCount += 1;
                    }
                    catch (Exception e) {
                        SuperMessage.RecordErrorMsgInMsgCenter("曲线折线化异常", e.ToString());
                        //                        failElemCollection.CurveElementList.Add(elem.AsCurveElement());
                        //删除0长度的线
                        try {
                            if (elem.AsCurveElement().Length == 0) {
                                _comApp.ActiveModelReference.RemoveElement(elem);
                            }
                        }
                        catch (Exception exception) {
                            //                            Console.WriteLine(exception);
                        }
                    }
                }
            }
            MessageBox.Show(string.Format("成功转换{0}条曲线{1}条B样条曲线{2}失败{3}条曲线{4}条B样条曲线",
                curveCount, bsplineCount, Environment.NewLine,
                failElemCollection.CurveElementList.Count,
                failElemCollection.BsplineCurveElementList.Count));
            return failElemCollection;
        }
        /// <summary>
        /// 添加选择的元素到选择集
        /// </summary>
        public ElementCollection AddSelecedElements() {
            var elemCollection = new ElementCollection {
                CurveElementList = new List<Element>(),
                BsplineCurveElementList = new List<Element>(),
                LineElementList = new List<Element>(),
                ArcElementList = new List<Element>(),
                DimensionElementList = new List<Element>(),
                EllipseElementList = new List<Element>(),
                TextElementList = new List<Element>(),
                TextNodeElementList = new List<Element>(),
            };
            var elemEnumer = SuperElementEnumerator.GetElementEnumerator();
            if (_comApp.ActiveModelReference.AnyElementsSelected) {
                while (elemEnumer.MoveNext()) {
                    var elem = elemEnumer.Current;
                    if (elem.IsCurveElement()) {
                        elemCollection.CurveElementList.Add(elem.AsCurveElement());
                    }
                    else if (elem.IsLineElement()) {
                        elemCollection.LineElementList.Add(elem.AsLineElement());
                    }
                    else if (elem.IsBsplineCurveElement()) {
                        elemCollection.BsplineCurveElementList.Add(elem.AsBsplineCurveElement());
                    }
                    else if (elem.IsDimensionElement()) {
                        elemCollection.DimensionElementList.Add(elem.AsDimensionElement());
                    }
                    else if (elem.IsArcElement()) {
                        elemCollection.ArcElementList.Add(elem.AsArcElement());
                    }
                    else if (elem.IsEllipseElement()) {
                        elemCollection.EllipseElementList.Add(elem.AsEllipseElement());
                    }
                    else if (elem.IsTextNodeElement()) {
                        elemCollection.TextNodeElementList.Add(elem.AsTextNodeElement());
                    }
                    else if (elem.IsTextElement()) {
                        elemCollection.TextElementList.Add(elem.AsTextElement());
                    }
                }
            }
            return elemCollection;
        }

        public void DeletePoint() {
            var elemeEnum = SuperElementEnumerator.GetElementEnumerator();
            int deleteCount = 0;
            while (elemeEnum.MoveNext()) {
                var element = elemeEnum.Current;
                if (element.IsLineElement()) {
                    if (element.AsLineElement().Length < 0.00001) {
                        deleteCount++;
                        SuperElement.ComApp.ActiveModelReference.RemoveElement(element);
                    }
                }
                else if (element.IsArcElement()) {
                    if (element.AsArcElement().Length < 0.00001) {
                        deleteCount++;
                        SuperElement.ComApp.ActiveModelReference.RemoveElement(element);
                    }
                }
            }
            MessageBox.Show(String.Format("共删除{0}个点", deleteCount));
        }
        /// <summary>
        /// 打散复杂元素
        /// </summary>
        private void DropComplexElement() {
            ElementScanCriteria esc = new ElementScanCriteriaClass();
            esc.IncludeType(MsdElementType.ComplexShape);
            esc.IncludeType(MsdElementType.ComplexString);
            //            var elemEnumerator = _comApp.ActiveModelReference.Scan(esc);
            var elemEnumerator = SuperElementEnumerator.GetElementEnumerator();
            while (elemEnumerator != null && elemEnumerator.MoveNext()) {
                var elem = elemEnumerator.Current;
                if (elem.IsComplexElement()) {
                    elem.AsComplexElement().Drop();
                }
                else if (elem.IsComponentElement) {
                    elem.AsComplexShapeElement().Drop();
                }
                else if (elem.IsComplexStringElement()) {
                    elem.AsComplexStringElement().Drop();
                }
            }
        }
        public void MoveDimensionText() {
            var elemEnum = SuperElementEnumerator.GetSelectedElementEnumerator();
            if (elemEnum == null) {
                MessageBox.Show("请选择元素");
            }
            else {
                var textElements = new List<TextElement>();
                var dimensionElements = new List<DimensionElement>();
                var targPoint = new Point3d();

                while (elemEnum.MoveNext()) {
                    var element = elemEnum.Current;
                    if (element.IsDimensionElement()) {
                        dimensionElements.Add(element.AsDimensionElement());
                    }
                    else if (element.IsTextElement()) {
                        var textElem = element.AsTextElement();
                        textElem.TextStyle.Justification = MsdTextJustification.CenterCenter;
                        textElements.Add(textElem);
                    }
                }
                if (dimensionElements.Count == 0 || textElements.Count == 0) {
                    MessageBox.Show("请选择标注元素和文本元素");
                }
                else if (dimensionElements.Count > 1) {
                    MessageBox.Show("只能选择一个标注元素");
                }
                else {
                    targPoint = dimensionElements[0].Point[1];
                    foreach (TextElement textElement in textElements) {
                        Element element = textElement;
                        var origin = textElement.get_Origin();
                        SuperElement.MoveElement(ref element, origin, targPoint);
                    }

                }
            }

        }

    }
}

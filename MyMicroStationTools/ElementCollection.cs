/************************************************************************
 * * Copyright © 2015 南宁市国土资源信息中心 All rights reserved.
 * *
 * CLR:         4.0.30319.18408
 * Machine:     XINXI-FENGZJ
 * File:        ElementCollections
 * GUID:        b780a011-4817-4eb4-94da-8c0c8fdd1d55
 * Domain:      NLIS
 * User:        fengzhenjian 
 * ----------------------------------------------------------------------
 * Depiction:   
 * Author:      Von_dream
 * CDT:         2015/4/9 15:26:23
 * Version:     1.0.0.1
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bentley.Interop.MicroStationDGN;

namespace MyMicroStationTools {
    public class ElementCollection {
        /// <summary>
        /// B样条曲线元素列表
        /// </summary>
        public List<Element> BsplineCurveElementList { get; set; }
        /// <summary>
        /// 曲线元素列表
        /// </summary>
        public List<Element> CurveElementList { get; set; }
        /// <summary>
        /// 折线元素列表
        /// </summary>
        public List<Element> LineElementList { get; set; }
        /// <summary>
        /// 尺寸标注元素列表
        /// </summary>
        public List<Element> DimensionElementList { get; set; }
        /// <summary>
        /// 圆元素列表
        /// </summary>
        public List<Element> EllipseElementList { get; set; }
        /// <summary>
        /// 弧元素列表
        /// </summary>
        public List<Element> ArcElementList { get; set; }
        /// <summary>
        /// 文本节点元素列表
        /// </summary>
        public List<Element> TextNodeElementList { get; set; }
        /// <summary>
        /// 文本元素列表
        /// </summary>
        public List<Element> TextElementList { get; set; } 
    }
}

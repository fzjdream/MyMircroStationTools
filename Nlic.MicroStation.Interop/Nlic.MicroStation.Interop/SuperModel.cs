/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn模型的一些操作
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
using System.Runtime.InteropServices;
using System.Text;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    public static class SuperModel {

        private static readonly Application App = Utilities.ComApp;
        /// <summary>
        /// 根据模型名和图层名删除对应模型对应图层的elem
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="levelName"></param>
        public static void RemoveQueryModelElemByLevel(string modelName, string levelName) {
            var models = App.ActiveDesignFile.Models;
            ElementScanCriteria scan = new ElementScanCriteriaClass();
            scan.ExcludeNonGraphical();
            foreach (var model in models) {
                if (((ModelReference)model).Name.Equals(modelName)) {
                    var queryModel = (ModelReference)model;
                    var level = queryModel.Levels.Find(levelName);
                    if (level == null) {
                        break;
                    }
                    var ee = queryModel.Scan(scan);
                    while (ee.MoveNext()) {
                        var elem = ee.Current;
                        if (elem.Level != null && elem.Level.Name.Equals(level.Name)) {
                            queryModel.RemoveElement(ee.Current);
                        }
                        else {
                            if (elem.IsCellElement()) {
                                var cellElem = elem.AsCellElement();
                                var subEe = cellElem.GetSubElements();
                                while (subEe.MoveNext()) {
                                    var subElem = subEe.Current;
                                    if (subElem.Level != null && subElem.Level.Name.Equals(level.Name)) {
                                        queryModel.RemoveElement(cellElem);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 建立查询模型
        /// </summary>
        /// <returns></returns>
        public static ModelReference CreateModel(string modelName) {
            var models = App.ActiveDesignFile.Models;
            var modelsCount = models.Count;

            for (var i = 1; i <= modelsCount; i++) {
                if (models[i].Name.Equals("Query")) {
                    return models[i];
                }

            }

            //查询不到该模型，则生成一个该模型，并参考到default模型中
            var template = App.ActiveDesignFile.DefaultModelReference;

            var queryModel = App.ActiveDesignFile.Models.Add(template, modelName, "专供查询用的图形", template.Type,
                                                                 template.Is3D);
            //queryModel.Activate();
            //ComApp.ActiveDesignFile.DefaultModelReference.Activate();
            //ComApp.SaveSettings();
            return queryModel;
        }
        /// <summary>
        /// 参考模式
        /// </summary>
        /// <param name="modelName"></param>
        public static void AttachModel(string modelName) {
            var attachments = App.ActiveModelReference.Attachments;
            foreach (var attachment in attachments) {
                if (((Attachment)attachment).AttachModelName.Equals(modelName)) {
                    return;
                }
            }
            var queryAttach = attachments.AddCoincident1(string.Empty, modelName, "QueryAttachment", "查询参考",
                                       MsdAddAttachmentFlags.TrueScale);
            if (queryAttach != null) {
                queryAttach.ElementsVisible = true;
                queryAttach.DisplayFlag = true;
                queryAttach.Redraw();
                queryAttach.Rewrite();
            }
        }
    }
}

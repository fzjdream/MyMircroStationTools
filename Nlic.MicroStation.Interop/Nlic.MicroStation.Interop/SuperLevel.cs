/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   dgn图层的一些操作。
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
    /// 扩展的图层。
    /// </summary>
    public class SuperLevel {
        private static readonly Application App = Utilities.ComApp;
        /// <summary>
        /// 判断图层是否存在.
        /// </summary>
        /// <param name="levelName">图层名.</param>
        /// <returns><c>true</c> if [has exist level] [the specified level name]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool HasExistLevel(string levelName) {
            bool existLevel = true;
            if (App.ActiveDesignFile.Levels.Find(levelName) == null) {
                existLevel = false;
            }
            return existLevel;
        }

        /// <summary>
        /// 添加一个新图层到当前激活dgn文件.
        /// </summary>
        /// <param name="levelName">图层名.</param>
        /// <returns>新添加的图层</returns>
        /// <remarks></remarks>
        public static Level AddNewLevelToActiveDesignFile(string levelName) {
            Level lvl = App.ActiveDesignFile.AddNewLevel(levelName);
            App.ActiveDesignFile.Levels.Rewrite();
            return lvl;
        }

        /// <summary>
        /// 从当前激活dgn文件获取图层，如果没有则创建.
        /// </summary>
        /// <param name="levelName">图层名.</param>
        /// <returns>找到、或新添加的图层</returns>
        /// <remarks></remarks>
        public static Level GetLevelFromActiveDesignFile(string levelName) {
            if (HasExistLevel(levelName) == false) {
                return AddNewLevelToActiveDesignFile(levelName);
            }
            return App.ActiveDesignFile.Levels.Find(levelName);
        }
        /// <summary>
        /// 从当前激活dgn文件获取图层，如果没有返回Null
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public static Level GetLevelByLevelName(string levelName) {
            Application app = Utilities.ComApp;
            if (HasExistLevel(levelName)) {
                return app.ActiveDesignFile.Levels.Find(levelName);
            }
            return null;
        }
        /// <summary>
        /// 获取特定模型下的特定图层
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public static Level GetLevelByModeNameAndLevelName(string modelName, string levelName) {
            var models = Utilities.ComApp.ActiveDesignFile.Models;
            foreach (var model in models) {
                if (((ModelReference)model).Name.Equals(modelName)) {
                    var queryModel = (ModelReference)model;
                    var level = queryModel.Levels.Find(levelName);
                    return level;
                }
            }
            return null;
        }


        /// <summary>
        /// 显示所有图层.
        /// </summary>
        /// <remarks></remarks>
        public static void DisplayAllLevel() {
            bool isNeedRewrite = false;
            Levels lvls = App.ActiveModelReference.Levels;
            foreach (Level lvl in lvls) {
                if (lvl.IsInUse && lvl.IsDisplayed == false) {
                    lvl.IsDisplayed = true;
                    isNeedRewrite = true;
                }
            }

            if (isNeedRewrite) {
                lvls.Rewrite();
            }
        }
        /// <summary>
        /// 关闭所有图层
        /// </summary>
        public static void HideAllLevel() {
            bool isNeedRewrite = false;
            Levels lvls = App.ActiveModelReference.Levels;
            foreach (Level lvl in lvls) {
                if (lvl.IsDisplayed) {
                    lvl.IsDisplayed = false;
                    isNeedRewrite = true;
                }
            }
            if (isNeedRewrite) {
                lvls.Rewrite();
            }
        }

        /// <summary>
        /// 隐藏图层
        /// </summary>
        /// <param name="level"></param>

        public static void HideLevel(Level level) {
            var views = App.ActiveDesignFile.Views;
            foreach (View view in views) {
                if (view.IsOpen && view.IsLevelShown(level)) {
                    view.HideLevel(level);
                    view.Redraw();
                }
            }
        }

        /// <summary>
        /// 显示图层
        /// </summary>
        /// <param name="level"></param>
        public static void ShowLevel(Level level) {
            var views = App.ActiveDesignFile.Views;
            foreach (View view in views) {
                if (view.IsOpen) {
                    view.ShowLevel(level);
                    view.Redraw();
                }
            }
        }

    }
}

/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   MicroStation消息提示。
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
using System.Windows.Forms;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.WinForms;
using MessageCenter = Bentley.MicroStation.Application.MessageCenter;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// MicroStation消息提示。
    /// </summary>
    public class SuperMessage {
        private const string NEWLINE = "\n";

        /// <summary>
        /// 向用户弹出警告消息对话框.
        /// </summary>
        /// <param name="msgDetail">详细消息.</param>
        /// <param name="msgCaption">消息标题.</param>
        /// <returns>用户选择结果选项</returns>
        /// <remarks></remarks>
        public static DialogResult ShowWarningMsgToUser(string msgDetail, string msgCaption) {
            DialogResult choose = MessageBox.Show(new MicroStationWin32(),
                msgDetail,
                msgCaption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            return choose;
        }

        /// <summary>
        /// 向用户弹出出错消息对话框.
        /// </summary>
        /// <param name="msgDetail">详细消息.</param>
        /// <param name="msgCaption">消息标题.</param>
        /// <remarks></remarks>
        public static void ShowErrorMsgToUser(string msgDetail, string msgCaption) {
            MessageBox.Show(new MicroStationWin32(),
                msgDetail,
                msgCaption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// 向用户弹出消息提示对话框.
        /// </summary>
        /// <param name="msgDetail">详细消息.</param>
        /// <param name="msgCaption">消息标题.</param>
        /// <remarks></remarks>
        public static void ShowMsgToUser(string msgDetail, string msgCaption) {
            MessageBox.Show(new MicroStationWin32(),
                msgDetail,
                msgCaption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// 在MicroStation的消息中心记录错误信息.
        /// </summary>
        /// <param name="err">异常.</param>
        /// <param name="elem">导致异常发生的元素.</param>
        /// <returns>错误详细信息</returns>
        /// <remarks></remarks>
        public static string RecordErrorMsgInMsgCenter(Exception err, Element elem = null) {
            string errDetail = "";
            // 获取出错元素信息
            if (elem != null) {
                errDetail = SuperElement.GetGraphicalElementInfo(elem);
                errDetail += NEWLINE;
            }
            // 记录错误详细信息
            errDetail += NEWLINE + "Message: " + err.Message;
            errDetail += NEWLINE + "StackTrace:" + NEWLINE;
            errDetail += err.StackTrace;
            MessageCenter.ShowErrorMessage(err.Message, errDetail, false);

            return "错误详情：" + NEWLINE + err.Message + ";" + NEWLINE + errDetail;
        }

        /// <summary>
        /// 在MicroStation的消息中心记录错误信息.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="detail"></param>
        /// <param name="openAlertBox"></param>
        /// <returns></returns>
        public static void RecordErrorMsgInMsgCenter(string caption, string detail, bool openAlertBox = false) {
            MessageCenter.ShowErrorMessage(caption, detail, openAlertBox);
        }

        /// <summary>
        /// 在MicroStation的消息中心记录错误信息.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="detail">The detail.</param>
        /// <param name="openAlertBox">if set to <c>true</c> [open alert box].</param>
        /// <remarks></remarks>
        public static void RecordMsgInMsgCenter(string caption, string detail, bool openAlertBox = false) {
            MessageCenter.ShowInfoMessage(caption, detail, openAlertBox);
        }

        /// <summary>
        /// 显示错误消息.
        /// </summary>
        /// <param name="err">异常.</param>
        /// <param name="toUserMsgBoxDetail">面向用户弹出的对话框的消息细节.</param>
        /// <param name="toUserMsgBoxCaption">面向用户弹出的对话框的消息标题.</param>
        /// <param name="elem">导致异常发生的元素.</param>
        /// <param name="showErrorMsgToUser">是否向用户弹出提示对话框.</param>
        /// <returns>错误信息描述</returns>
        /// <remarks></remarks>
        public static string DisplayErrorMsg(Exception err,
            string toUserMsgBoxDetail, string toUserMsgBoxCaption,
            Element elem = null,
            bool showErrorMsgToUser = true) {
            string msg = toUserMsgBoxDetail;
            msg += NEWLINE + RecordErrorMsgInMsgCenter(err, elem);
            if (showErrorMsgToUser) {
                ShowErrorMsgToUser(toUserMsgBoxDetail, toUserMsgBoxCaption);
            }

            return msg;
        }
    }
}

/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   对dgn元素进行操作的最底层的抽象类
 * Author:      Michael Hou
 * CDT:         2011-05-23
 * Version:     1.0.0.0
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;

namespace Nlic.MicroStation.Interop {
    public abstract class DgnTransaction {
        protected DgnTransaction() {
            _comApp = Utilities.ComApp;
            _msg = "";
            IsShowMessageBoxToUser = true;
            _progRunStatus = NlicProgramRunStatus.Success;
        }

        protected Application _comApp;

        /// <summary>
        /// 事务执行的消息
        /// </summary>
        protected string _msg;
        /// <summary>
        /// 获取事务执行的消息.
        /// </summary>
        /// <remarks></remarks>
        public string Message {
            get { return _msg; }
        }

        /// <summary>
        /// 是否要向用户弹出提示对话框.
        /// </summary>
        /// <value></value>
        /// <remarks>缺省值为true，当事务执行结束或者程序发生错误，都会弹出提示对话框</remarks>
        public bool IsShowMessageBoxToUser { get; set; }

        /// <summary>
        /// 程序运行状态
        /// </summary>
        protected NlicProgramRunStatus _progRunStatus;
        /// <summary>
        /// 元素容器
        /// </summary>
        protected ElementEnumerator _ee;

        public abstract NlicProgramRunStatus Execute(ElementEnumerator elemEnum);
    }
}

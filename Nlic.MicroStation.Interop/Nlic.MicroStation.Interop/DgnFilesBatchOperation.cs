/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   对dgn文件进行批量处理的抽象类
 * Author:      Michael Hou
 * CDT:         2011-05-13
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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Application = Bentley.Interop.MicroStationDGN.Application;

namespace Nlic.MicroStation.Interop {
    /// <summary>
    /// 对dgn文件进行批量处理的抽象类
    /// </summary>
    /// <remarks></remarks>
    public abstract class DgnFilesBatchOperation {
        protected DgnFilesBatchOperation() {
            IsNeedPreConditionCheck = true;
            OperatedFileMark = "";
            DgnFileScanFilter = "*.dgn";
            IsCopyNewDgnFile = false;
        }
        #region 属性
        /// <summary>
        /// 对dgn文件的处理方法
        /// </summary>
        protected DgnTransaction _operTransaction;

        /// <summary>
        /// 在处理每一个dgn文件之前，是否要进行条件检查.
        /// </summary>
        /// <value><c>true</c> if this instance is need pre condition check; otherwise, <c>false</c>.</value>
        /// <remarks>通常，在处理dgn文件之前，都要进行一些条件检查。
        /// 比如，检查模型是否为只读，是否有锁定的元素、图层等。</remarks>
        public bool IsNeedPreConditionCheck { get; set; }
        /// <summary>
        /// 处理dgn文件前的条件检查
        /// </summary>
        protected DgnTransaction _preConditionCheckTransaction;
        /// <summary>
        /// 设置对每一个dgn文件进行条件检查的事务。
        /// </summary>
        /// <param name="preConditionCheckTransaction">The pre condition check transaction.</param>
        /// <remarks></remarks>
        public void SetPreConditionCheckTransaction(DgnTransaction preConditionCheckTransaction) {
            _preConditionCheckTransaction = preConditionCheckTransaction;
        }

        /// <summary>
        /// 获取dgn文件每个模型的元素容器的方式
        /// </summary>
        protected NlicGetElementEnumeratorType _getEEType;
        /// <summary>
        /// 用户定义的扫描规则。只有当_getEEType的值为UserDefineScanCriteria时，才使用用户定义的规则进行扫描。
        /// </summary>
        protected ElementScanCriteria _elemScanCriteria;
        /// <summary>
        /// 设置获取元素容器的元素扫描规则.
        /// </summary>
        /// <param name="elementScanCriteria">元素扫描规则.</param>
        /// <remarks>只有当_getEEType的值为UserDefineScanCriteria时，才使用用户定义的规则进行扫描。</remarks>
        public void SetScanCriteria(ElementScanCriteria elementScanCriteria) {
            _elemScanCriteria = elementScanCriteria;
        }

        /// <summary>
        /// 已处理的文件加的标识.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string OperatedFileMark { get; set; }

        /// <summary>
        /// 扫描的dgn文件的过滤名称，必须以.dgn结尾.
        /// </summary>
        /// <value></value>
        /// <remarks>不能为空，缺省值为“*.dgn”</remarks>
        public string DgnFileScanFilter {
            get;
            set;
            //set {
            //    if (value.ToString().ToLower().EndsWith(".dgn")) {
            //        _dgnFileScanFilter = value;
            //    }
            //    else {
            //        MessageBox.Show("值必须以“.dgn”结尾！");
            //    }
            //}
        }

        /// <summary>
        /// 记录操作的log信息文件后缀名
        /// </summary>
        protected const string LOGFILE_EXTENTION = ".txt";
        /// <summary>
        /// 新的换行符
        /// </summary>
        protected const string NEWLINE = "\r\n";

        /// <summary>
        /// 处理的dgn文件的文件夹
        /// </summary>
        protected string _dgnFolder;
        /// <summary>
        /// 存放统计信息的文件夹，完整路径
        /// </summary>
        protected string _statLogFolder;
        /// <summary>
        /// 存放统计信息的文件夹，在处理的dgn文件的文件夹下自动创建
        /// </summary>
        protected const string STATFOLDERNAME = "stat";

        /// <summary>
        /// 待处理的所有dgn文件的名称
        /// </summary>
        protected IList<string> _filesName;
        /// <summary>
        /// 当前处理的dgn文件
        /// </summary>
        protected FileInfo _file;

        /// <summary>
        /// 是否复制一个dgn文件进行处理.
        /// </summary>
        /// <value></value>
        /// <remarks>缺省值为false</remarks>
        public bool IsCopyNewDgnFile { get; set; }

        protected Application _comApp;
        /// <summary>
        /// 当前在MicroStation中打开的dgn文件
        /// </summary>
        protected DesignFile _dgnFile;
        /// <summary>
        /// 从当前打开的dgn文件的激活模型获取的所有元素容器
        /// </summary>
        protected ElementEnumerator _modelEe;

        /// <summary>
        /// 操作过程的消息提示
        /// </summary>
        protected string _operMsg;

        /// <summary>
        /// 处理合计用时，单位：秒
        /// </summary>
        protected double _sumOperTime;
        /// <summary>
        /// 处理的dgn文件总数
        /// </summary>
        protected int _sumOperDgnFiles;
        /// <summary>
        /// 处理成功的dgn文件总数
        /// </summary>
        protected int _sumOperSuccessDgnFiles;
        /// <summary>
        /// 处理失败的dgn文件总数
        /// </summary>
        protected int _sumOperFailDgnFiles;
        /// <summary>
        /// 处理中遇到的模型或文件为空的dgn文件总数
        /// </summary>
        protected int _sumOperNullDgnFiles;
        /// <summary>
        /// 没有通过前提条件检查的dgn文件总数
        /// </summary>
        protected int _sumCantOperDgnFiles;

        /// <summary>
        /// 记录处理成功的dgn文件文件名的日志文件
        /// </summary>
        protected StreamWriter _statOperSuccessLogFile;
        /// <summary>
        /// 记录处理失败的dgn文件文件名的日志文件
        /// </summary>
        protected StreamWriter _statOperFailLogFile;
        /// <summary>
        /// 记录为空的dgn文件文件名的日志文件
        /// </summary>
        protected StreamWriter _statOperNullLogFile;
        /// <summary>
        /// 日志文件，记录没有通过前提条件检查的dgn文件文件名
        /// </summary>
        protected StreamWriter _statCantOperLogFile;

        /// <summary>
        /// 记录程序出错的错误信息日志文件
        /// </summary>
        protected StreamWriter _errInfoLogFile;
        /// <summary>
        /// 对于导致程序出错的dgn文件，在日志文件前加的前缀
        /// </summary>
        protected const string OPERERR_LOGFILE_PREFIX = "err_";

        /// <summary>
        /// 记录对每一个dgn文件处理信息的日志文件，一个处理成功的dgn文件对应一个日志文件。
        /// </summary>
        protected StreamWriter _operLogFile;
        /// <summary>
        /// 处理成功的dgn文件，在日志文件前加的前缀
        /// </summary>
        protected const string OPEROK_LOGFILE_PREFIX = "ok_";
        /// <summary>
        /// 处理的日志文件名称
        /// </summary>
        protected string _operLogFileName;

        /// <summary>
        /// 每一个模型的处理状态
        /// </summary>
        protected NlicProgramRunStatus[] _operModelsStatus;
        /// <summary>
        /// 当前的dgn文件的处理结果状态
        /// </summary>
        protected NlicProgramRunStatus _curDgnFileOperStatus;
        #endregion
        #region 方法
        public virtual void Execute(string dgnFolder,
            DgnTransaction operationMethod,
            NlicGetElementEnumeratorType getElementEnumeratorType = NlicGetElementEnumeratorType.ByAutoDefineFence
            ) {
            var timeWatch = new Stopwatch();
            timeWatch.Start();

            _dgnFolder = dgnFolder;
            _operTransaction = operationMethod;
            _getEEType = getElementEnumeratorType;

            InitializeStatEnvironment();

            GetDgnFilesFullNameByFilter();

            DealDgnFilesTransaction();

            timeWatch.Stop();
            _sumOperTime = Math.Round(timeWatch.Elapsed.TotalSeconds, 3);
            timeWatch.Reset();

            CloseStatLogFile();
            WriteOperationSumaryToLogFile();
            PromptTheEnd();
        }
        #region 处理前的初始化操作，初始化统计环境参数
        /// <summary>
        /// 初始化统计环境.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void InitializeStatEnvironment() {
            _comApp = Utilities.ComApp;

            InitializeStatCountPara();
            CreateStatLogFolder();
            CreateStatLogFile();
        }
        /// <summary>
        /// 初始化统计计数参数.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void InitializeStatCountPara() {
            _sumOperDgnFiles = 0;
            _sumOperSuccessDgnFiles = 0;
            _sumOperFailDgnFiles = 0;
            _sumOperNullDgnFiles = 0;
            _sumCantOperDgnFiles = 0;
        }
        /// <summary>
        /// 创建存放统计日志记录的文件夹.
        /// </summary>
        /// <remarks></remarks>
        private void CreateStatLogFolder() {
            _statLogFolder = _dgnFolder + "\\" + STATFOLDERNAME;
            Directory.CreateDirectory(_statLogFolder);
        }
        /// <summary>
        /// 创建统计记录日志文件.
        /// </summary>
        /// <remarks>调用该方法前必须先调用CreateStatLogFolder方法创建统计信息文件夹</remarks>
        protected virtual void CreateStatLogFile() {
            _statOperSuccessLogFile = File.CreateText(Path.Combine(_statLogFolder, "已成功处理的dgn文件" + LOGFILE_EXTENTION));
            _statOperSuccessLogFile.AutoFlush = true;

            _statOperFailLogFile = File.CreateText(Path.Combine(_statLogFolder, "处理失败的dgn文件" + LOGFILE_EXTENTION));
            _statOperFailLogFile.AutoFlush = true;

            _statOperNullLogFile = File.CreateText(Path.Combine(_statLogFolder, "没有元素的空dgn文件" + LOGFILE_EXTENTION));
            _statOperNullLogFile.AutoFlush = true;

            _errInfoLogFile = File.CreateText(Path.Combine(_statLogFolder, "导致程序运行出错的dgn文件" + LOGFILE_EXTENTION));
            _errInfoLogFile.AutoFlush = true;

            _statCantOperLogFile = File.CreateText(Path.Combine(_statLogFolder, "不能进行处理的dgn文件" + LOGFILE_EXTENTION));
            _statCantOperLogFile.AutoFlush = true;
        }
        #endregion
        #region 扫描文件夹，获取处理的dgn文件名
        /// <summary>
        /// 按照查找dgn文件的筛选规则获取dgn文件夹中所有的dgn文件名.
        /// </summary>
        /// <remarks></remarks>
        private void GetDgnFilesFullNameByFilter() {
            var dir = new DirectoryInfo(_dgnFolder);
            _filesName = new List<string>();

            AddDgnFileFullNameToList(dir);
        }
        /// <summary>
        /// 将查找到的dgn文件的文件名添加到列表中.
        /// </summary>
        /// <param name="dir">文件夹.</param>
        /// <remarks></remarks>
        private void AddDgnFileFullNameToList(DirectoryInfo dir) {
            foreach (var file in dir.GetFiles(DgnFileScanFilter)) {
                _filesName.Add(file.FullName);
            }

            foreach (var subDir in dir.GetDirectories()) {
                AddDgnFileFullNameToList(subDir);
            }
        }
        #endregion

        /// <summary>
        /// 处理所有dgn文件的事务.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void DealDgnFilesTransaction() {
            foreach (var dgnFileFullName in _filesName) {
                try {
                    SetCurrentDealDgnFile(dgnFileFullName);

                    _dgnFile = _comApp.OpenDesignFile(_file.FullName, false, MsdV7Action.UpgradeToV8);

                    CreateOperationLogFile();
                    WriteOperationLogFileTitle();

                    _operMsg = "";

                    OperateDgnFileTransaction();

                    _sumOperDgnFiles++;

                    RecordOperationInfo();
                }
                catch (Exception err) {
                    RecordProgramError(err);
                    CloseStatLogFile();
                    WriteOperationSumaryToLogFile();

                    _curDgnFileOperStatus = NlicProgramRunStatus.Exception;
                }
                finally {
                    // 全屏显示图形
                    SuperView.FitView();
                    _dgnFile.Save(); //保存文件
                    _comApp.SaveSettings(); //保存设置

                    // 关闭记录转换信息的txt文件
                    _operLogFile.Close();

                    if (_curDgnFileOperStatus == NlicProgramRunStatus.CantOperation ||
                        _curDgnFileOperStatus == NlicProgramRunStatus.Fail ||
                        _curDgnFileOperStatus == NlicProgramRunStatus.Exception) {
                        RenameOperationLogFile();
                    }
                }
            }
        }

        /// <summary>
        /// 设置当前待处理的dgn文件.
        /// </summary>
        /// <param name="dgnFileFullName">Full name of the DGN file.</param>
        /// <remarks></remarks>
        private void SetCurrentDealDgnFile(string dgnFileFullName) {
            if (IsCopyNewDgnFile == false) {
                _file = new FileInfo(dgnFileFullName);
            }
            else {
                FileInfo sourceFile = new FileInfo(dgnFileFullName);
                _file =
                    sourceFile.CopyTo(
                        Path.Combine(sourceFile.DirectoryName,
                                     Path.GetFileNameWithoutExtension(sourceFile.Name) + OperatedFileMark
                                     + sourceFile.Extension), true);
            }
        }

        /// <summary>
        /// 在当前处理的dgn文件的同目录下创建一个记录处理信息的日志文件.
        /// </summary>
        /// <remarks></remarks>
        private void CreateOperationLogFile() {
            SetOperationLogFileName();
            _operLogFile = File.CreateText(_operLogFileName);
            _operLogFile.AutoFlush = true;
        }
        /// <summary>
        /// 设置记录处理信息的日志文件的文件名.
        /// </summary>
        /// <remarks></remarks>
        private void SetOperationLogFileName() {
            _operLogFileName = Path.Combine(_file.DirectoryName,
                                            OPEROK_LOGFILE_PREFIX + Path.GetFileNameWithoutExtension(_file.Name) +
                                            LOGFILE_EXTENTION);
        }
        /// <summary>
        /// 为记录处理信息的日志文件添加标题信息.
        /// </summary>
        /// <remarks>调用该方法前，_operLogFile必须已创建</remarks>
        private void WriteOperationLogFileTitle() {
            _operLogFile.WriteLine(_file.FullName);
            _operLogFile.WriteLine();
            _operLogFile.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
        }
        /// <summary>
        /// 将对当前dgn文件的处理消息添加到消息日志文件
        /// </summary>
        /// <remarks></remarks>
        protected virtual void AppendOperateInfoToLogFile() {
            _operLogFile.WriteLine("模型——" + _comApp.ActiveModelReference.Name);
            _operLogFile.WriteLine(_operMsg);
            _operLogFile.WriteLine();
        }

        /// <summary>
        /// 处理每一个dgn文件的事务.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void OperateDgnFileTransaction() {
            _operModelsStatus = new NlicProgramRunStatus[_dgnFile.Models.Count];

            int modelCount = 0;
            foreach (ModelReference model in _dgnFile.Models) {
                model.Activate();
                BeforeOperateModelSetting();
                GetModelElementEnumerator(model);

                if (IsPreConditionCheckOK()) {
                    GetModelElementEnumerator(model);    //重新获取元素，这步非常重要！

                    _operModelsStatus[modelCount] = OperateActiveModelElementsTransaction();
                }
                else {
                    // 只要有一个模型不能处理，则将该文件标识为不能处理
                    _curDgnFileOperStatus = NlicProgramRunStatus.CantOperation;
                    AppendOperateInfoToLogFile();
                    return;
                }

                AppendOperateInfoToLogFile();
                modelCount++;

                JudgeCurrentDgnFileOperationStatus();
            }
        }

        /// <summary>
        /// 处理模型中的元素前的设置.
        /// </summary>
        /// <remarks></remarks>
        protected virtual void BeforeOperateModelSetting() {
            SuperView.OpenView1();
            SuperView.MaximizeView1();
            // 卸载所有参考
            SuperAttachment.DetachAllReferences();
            // 卸载所有光栅文件
            SuperRaster.DetachAllRasters();
            // 将所有图层设为显示状态
            SuperLevel.DisplayAllLevel();
            // 全屏显示图形
            SuperView.FitView();
        }

        /// <summary>
        /// 获取待处理的模型的元素索引器.
        /// </summary>
        /// <remarks></remarks>
        private void GetModelElementEnumerator(ModelReference model) {
            switch (_getEEType) {
                case NlicGetElementEnumeratorType.ByAutoDefineFence:
                    SuperFence.DefineFenceByView(model);
                    _modelEe = SuperElementEnumerator.GetFenceElementEnumerator();
                    break;
                case NlicGetElementEnumeratorType.ScanAllGraphical:
                    _modelEe = SuperElementEnumerator.GetGraphicalElementEnumeratorByScan(model);
                    break;
                case NlicGetElementEnumeratorType.UserDefineScanCriteria:
                    //todo 添加扫描规则为空的异常 
                    _modelEe = model.Scan(_elemScanCriteria);
                    break;
            }
        }

        /// <summary>
        /// 待处理的dgn文件的激活模型是否通过前提条件检查
        /// </summary>
        /// <returns><c>true</c> if [is pre condition check OK]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        protected virtual bool IsPreConditionCheckOK() {
            if (!IsNeedPreConditionCheck) {
                return true;
            }

            if (_preConditionCheckTransaction == null) {
                // todo 抛出_preConditionCheckTransaction未设置的异常 
                return false;
            }

            if (_preConditionCheckTransaction.Execute(_modelEe) == NlicProgramRunStatus.Success) {
                return true;
            }
            else {
                _operMsg = _preConditionCheckTransaction.Message;
            }

            return false;
        }

        /// <summary>
        /// Operates the active model elements transaction.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual NlicProgramRunStatus OperateActiveModelElementsTransaction() {
            NlicProgramRunStatus runStatus = _operTransaction.Execute(_modelEe);
            _operMsg += NEWLINE + _operTransaction.Message;

            return runStatus;
        }

        /// <summary>
        /// 判断当前处理的dgn文件的最终处理结果。
        /// </summary>
        /// <remarks>根据所有模型的处理结果进行判断。只要有一个模型处理失败，则认为该dgn文件处理失败。</remarks>
        protected virtual void JudgeCurrentDgnFileOperationStatus() {
            var statusNo = (int)_operModelsStatus.Min();
            _curDgnFileOperStatus = (NlicProgramRunStatus)statusNo;
        }

        /// <summary>
        /// 根据每个文件的处理结果，进行记录（记录文件名）。
        /// </summary>
        /// <remarks></remarks>
        protected virtual void RecordOperationInfo() {
            switch (_curDgnFileOperStatus) {
                case NlicProgramRunStatus.Success:
                    _sumOperSuccessDgnFiles++;
                    WriteDgnFileFullNameToLogFile(_statOperSuccessLogFile);
                    break;
                case NlicProgramRunStatus.NoElements:
                    _sumOperNullDgnFiles++;
                    WriteDgnFileFullNameToLogFile(_statOperNullLogFile);

                    // 存在为空的dgn文件实际上也是处理成功的
                    _sumOperSuccessDgnFiles++;
                    WriteDgnFileFullNameToLogFile(_statOperSuccessLogFile);
                    break;
                case NlicProgramRunStatus.CantOperation:
                    _sumCantOperDgnFiles++;
                    WriteDgnFileFullNameToLogFile(_statCantOperLogFile);
                    break;
                case NlicProgramRunStatus.Fail:
                    _sumOperFailDgnFiles++;
                    WriteDgnFileFullNameToLogFile(_statOperFailLogFile);
                    break;
                //case NlicProgramRunStatus.Exception:
                //    break;
                //default:
                //    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteDgnFileFullNameToLogFile(StreamWriter logFile) {
            logFile.WriteLine(_file.FullName);
        }

        /// <summary>
        /// 记录程序的错误信息。
        /// </summary>
        /// <param name="err"></param>
        /// <remarks></remarks>
        private void RecordProgramError(Exception err) {
            _errInfoLogFile.WriteLine(_file.FullName);
            _errInfoLogFile.WriteLine(err.Message);
            _errInfoLogFile.WriteLine(err.StackTrace);
            _errInfoLogFile.WriteLine();
        }

        /// <summary>
        /// 关闭统计信息日志文件
        /// </summary>
        /// <remarks></remarks>
        protected virtual void CloseStatLogFile() {
            _statCantOperLogFile.Close();
            _statOperFailLogFile.Close();
            _statOperNullLogFile.Close();
            _statOperSuccessLogFile.Close();

            _errInfoLogFile.Close();
        }

        /// <summary>
        /// 写入统计摘要信息到日志文件。
        /// </summary>
        /// <remarks></remarks>
        protected virtual void WriteOperationSumaryToLogFile() {
            StreamWriter sw = File.CreateText(Path.Combine(_statLogFolder, "统计" + LOGFILE_EXTENTION));

            sw.WriteLine("处理总计用时： " + _sumOperTime + "秒");
            sw.WriteLine("文件总数： " + _sumOperDgnFiles);
            sw.WriteLine();
            sw.WriteLine("成功处理的文件总数： " + _sumOperSuccessDgnFiles);
            sw.WriteLine("不能处理的文件总数： " + _sumCantOperDgnFiles);
            sw.WriteLine("处理失败的文件总数： " + _sumOperFailDgnFiles);
            sw.WriteLine();
            sw.WriteLine("没有元素的空文件总数： " + _sumOperNullDgnFiles);

            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// 重命名记录处理信息的日志文件，原因是该文件处理失败。
        /// </summary>
        /// <remarks></remarks>
        protected virtual void RenameOperationLogFile() {
            string newName = Path.Combine(_file.DirectoryName,
                                                       OPERERR_LOGFILE_PREFIX + Path.GetFileNameWithoutExtension(_file.Name) +
                                                       LOGFILE_EXTENTION);

            File.Copy(_operLogFileName, newName, true);
            File.Delete(_operLogFileName);
        }

        protected virtual void PromptTheEnd() {
            MessageBox.Show("Oh Yeah! 所有文件处理完成，共用时：" + _sumOperTime + "秒");
        }
        #endregion
    }
}

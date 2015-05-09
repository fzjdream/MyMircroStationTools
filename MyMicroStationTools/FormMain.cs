using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bentley.Interop.MicroStationDGN;
using DevComponents.DotNetBar;
using DevComponents.Editors;
using BMW = Bentley.MicroStation.WinForms;
using Nlic.MicroStation.Interop;

namespace MyMicroStationTools {
//    public partial class FormMain : Form {
                    public partial class FormMain : BMW.Adapter {
        public FormMain() {
            InitializeComponent();
        }
        private readonly ElementOperation elementOperation = new ElementOperation();
        private volatile static FormMain _frmMain;
        private static readonly object SyncRoot = new object();
        public static FormMain GetInstance() {
            if (_frmMain == null || _frmMain.IsDisposed) {
                lock (SyncRoot) {
                    if (_frmMain == null || _frmMain.IsDisposed) {
                        _frmMain = new FormMain();
                    }
                }
            }
            return _frmMain;
        }

        #region RibbonControl添加事件

        private void RibbionControlAddBtnClikcEvent() {
            foreach (var ribbonSubControl in ribbonCtrlMain.Controls) {
                var ribbonPanel = ribbonSubControl as RibbonPanel;
                if (ribbonPanel != null)
                    foreach (var ribbonPanelSubControl in ribbonPanel.Controls) {
                        var ribbonBar = ribbonPanelSubControl as RibbonBar;
                        if (ribbonBar != null) {
                            RibbonBarAddItemEvent(ribbonBar);
                        }
                    }
            }
        }

        private void RibbonBarAddItemEvent(RibbonBar ribbonBar) {
            foreach (var item in ribbonBar.Items) {
                var btnItem = item as ButtonItem;
                if (btnItem != null) {
                    ButtonItemAddClickEvent(btnItem);
                }
                var itemContainer = item as ItemContainer;
                if (itemContainer != null) {
                    ItemContainerAddBtnItemClickEvent(itemContainer);
                }
            }
        }

        private void ItemContainerAddBtnItemClickEvent(ItemContainer itemContainer) {
            foreach (var subItem in itemContainer.SubItems) {
                var subItemContainer = subItem as ItemContainer;
                if (subItemContainer != null) {
                    ItemContainerAddBtnItemClickEvent(subItemContainer);
                }
                var subBtnItem = subItem as ButtonItem;
                if (subBtnItem != null) {
                    ButtonItemAddClickEvent(subBtnItem);
                }

            }
        }

        private void ButtonItemAddClickEvent(ButtonItem btnItem) {
            btnItem.Click += ButtonItem_ClickEvent;
            if (btnItem.SubItems.Count > 0) {
                foreach (var subItem in btnItem.SubItems) {
                    var subBtnItem = subItem as ButtonItem;
                    if (subBtnItem != null) {
                        ButtonItemAddClickEvent(subBtnItem);
                    }
                }
            }

        }

        private void ButtonItem_ClickEvent(object sender, EventArgs e) {
            var btnItem = sender as ButtonItem;
            //曲线折线化
            if (btnItem == btnStrokeCurveElem) {
                var fialElemColl = elementOperation.StrokeCurveElement();
                AddElementCollecionToCombo(fialElemColl);
                MessageBox.Show("Test");
            }
            //添加选择集
            else if (btnItem == btnAddSelectElement) {
                var eleColl = elementOperation.AddSelecedElements();
                AddElementCollecionToCombo(eleColl);
            }
            //上一个
            else if (btnItem == btnPrevious) {
                if (integerElemCount.Value > 1) {
                    integerElemCount.Value -= 1;
                }
                ZoomToElement();
            }
            //下一个
            else if (btnItem == btnNext) {
                if (integerElemCount.Value < integerElemCount.MaxValue + 1) {
                    integerElemCount.Value += 1;
                }
                ZoomToElement();
            }
            //  文本居中对齐
            else if (btnItem == btnTxtCenterAlignment) {
                SuperElement.SetTextElementJustification();
            }
             //删除点
            else if (btnItem==btnDeletePoint) {
                elementOperation.DeletePoint();
            }
            else if (btnItem==btnMoveTextElement) {
                elementOperation.MoveDimensionText();
            }
        }

        #endregion

        private void FormMain_Load(object sender, EventArgs e) {
            RibbionControlAddBtnClikcEvent();
        }
        /// <summary>
        /// 添加选择集
        /// </summary>
        private void AddElementCollecionToCombo(ElementCollection elemCollection){
            comboSelectedElem.Items.Clear();
            if (elemCollection.CurveElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "曲线（" + elemCollection.CurveElementList.Count + ")",
                    Value = elemCollection.CurveElementList
                });
            }
            if (elemCollection.BsplineCurveElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "B样条曲线（" + elemCollection.BsplineCurveElementList.Count + ")",
                    Value = elemCollection.BsplineCurveElementList
                });
            }
            if (elemCollection.ArcElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "弧（" + elemCollection.ArcElementList.Count + ")",
                    Value = elemCollection.ArcElementList
                });
            }
            if (elemCollection.EllipseElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "圆（" + elemCollection.EllipseElementList.Count + ")",
                    Value = elemCollection.EllipseElementList
                });
            }
            if (elemCollection.DimensionElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "标注尺寸（" + elemCollection.DimensionElementList.Count + ")",
                    Value = elemCollection.DimensionElementList
                });
            }
            if (elemCollection.LineElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "折线（" + elemCollection.LineElementList.Count + ")",
                    Value = elemCollection.LineElementList
                });
            }
            if (elemCollection.TextElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "文本（" + elemCollection.TextElementList.Count + ")",
                    Value = elemCollection.TextElementList
                });
            }
            if (elemCollection.TextNodeElementList.Count > 0) {
                comboSelectedElem.Items.Add(new ComboItem {
                    Text = "文本节点（" + elemCollection.TextNodeElementList.Count + ")",
                    Value = elemCollection.TextNodeElementList
                });
            }

            if (comboSelectedElem.Items.Count > 0) {
                comboSelectedElem.SelectedItem = comboSelectedElem.Items[0];
            }

        }
        /// <summary>
        /// 缩放至元素
        /// </summary>
        private void ZoomToElement() {
            SuperElement.UnselectAllElements();
            var selectedItem = comboSelectedElem.SelectedItem as ComboItem;
            if (selectedItem != null) {
                var elements = selectedItem.Value as List<Element>;
                if (elements != null) {
                    var index = integerElemCount.Value;
                    var elem = elements[index - 1];
                    SuperView.ZoomInElement(elem.Range);
                    SuperElement.SelectElement(elem);
                }
            }
        }

        private void comboSelectedElem_SelectedIndexChanged(object sender, EventArgs e) {
            var selectedItem = comboSelectedElem.SelectedItem as ComboItem;
            if (selectedItem != null) {
                integerElemCount.Value = 1;
                var elements = selectedItem.Value as List<Element>;
                if (elements != null) {
                    integerElemCount.MaxValue = elements.Count;
                }
            }
        }

        private void txtElemId_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                long elemId;
                long.TryParse(txtElemId.Text, out elemId);
                var elem = SuperElement.GetElementById(elemId);
                if (elem != null) {
                    SuperView.ZoomInElement(elem.Range);
                    SuperElement.SelectElement(elem);
                }
            }
        }

    }
}

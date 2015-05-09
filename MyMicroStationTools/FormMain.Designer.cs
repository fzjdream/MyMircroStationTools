namespace MyMicroStationTools {
    partial class FormMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.ribbonCtrlMain = new DevComponents.DotNetBar.RibbonControl();
            this.ribbonPanel1 = new DevComponents.DotNetBar.RibbonPanel();
            this.ribbonBar2 = new DevComponents.DotNetBar.RibbonBar();
            this.integerElemCount = new DevComponents.Editors.IntegerInput();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.comboSelectedElem = new DevComponents.DotNetBar.ComboBoxItem();
            this.itemContainer3 = new DevComponents.DotNetBar.ItemContainer();
            this.btnPrevious = new DevComponents.DotNetBar.ButtonItem();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.btnNext = new DevComponents.DotNetBar.ButtonItem();
            this.btnAddSelectElement = new DevComponents.DotNetBar.ButtonItem();
            this.itemContainer4 = new DevComponents.DotNetBar.ItemContainer();
            this.txtElemId = new DevComponents.DotNetBar.TextBoxItem();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.btnStrokeCurveElem = new DevComponents.DotNetBar.ButtonItem();
            this.btnTxtCenterAlignment = new DevComponents.DotNetBar.ButtonItem();
            this.btnDeletePoint = new DevComponents.DotNetBar.ButtonItem();
            this.ribbonTabItem1 = new DevComponents.DotNetBar.RibbonTabItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.qatCustomizeItem1 = new DevComponents.DotNetBar.QatCustomizeItem();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.itemContainer5 = new DevComponents.DotNetBar.ItemContainer();
            this.btnMoveTextElement = new DevComponents.DotNetBar.ButtonItem();
            this.ribbonCtrlMain.SuspendLayout();
            this.ribbonPanel1.SuspendLayout();
            this.ribbonBar2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.integerElemCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonCtrlMain
            // 
            // 
            // 
            // 
            this.ribbonCtrlMain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonCtrlMain.Controls.Add(this.ribbonPanel1);
            this.ribbonCtrlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonCtrlMain.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ribbonTabItem1});
            this.ribbonCtrlMain.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.ribbonCtrlMain.Location = new System.Drawing.Point(0, 0);
            this.ribbonCtrlMain.Name = "ribbonCtrlMain";
            this.ribbonCtrlMain.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.ribbonCtrlMain.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.qatCustomizeItem1});
            this.ribbonCtrlMain.Size = new System.Drawing.Size(678, 121);
            this.ribbonCtrlMain.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonCtrlMain.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.ribbonCtrlMain.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.ribbonCtrlMain.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.ribbonCtrlMain.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.ribbonCtrlMain.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.ribbonCtrlMain.SystemText.QatDialogAddButton = "&Add >>";
            this.ribbonCtrlMain.SystemText.QatDialogCancelButton = "Cancel";
            this.ribbonCtrlMain.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.ribbonCtrlMain.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.ribbonCtrlMain.SystemText.QatDialogOkButton = "OK";
            this.ribbonCtrlMain.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonCtrlMain.SystemText.QatDialogRemoveButton = "&Remove";
            this.ribbonCtrlMain.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.ribbonCtrlMain.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonCtrlMain.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.ribbonCtrlMain.TabGroupHeight = 14;
            this.ribbonCtrlMain.TabIndex = 0;
            this.ribbonCtrlMain.Text = "ribbonControl1";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonPanel1.Controls.Add(this.ribbonBar2);
            this.ribbonPanel1.Controls.Add(this.ribbonBar1);
            this.ribbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonPanel1.Location = new System.Drawing.Point(0, 25);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.ribbonPanel1.Size = new System.Drawing.Size(678, 93);
            // 
            // 
            // 
            this.ribbonPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonPanel1.TabIndex = 1;
            // 
            // ribbonBar2
            // 
            this.ribbonBar2.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar2.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar2.ContainerControlProcessDialogKey = true;
            this.ribbonBar2.Controls.Add(this.integerElemCount);
            this.ribbonBar2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar2.DragDropSupport = true;
            this.ribbonBar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer2,
            this.itemContainer4});
            this.ribbonBar2.Location = new System.Drawing.Point(183, 0);
            this.ribbonBar2.Name = "ribbonBar2";
            this.ribbonBar2.Size = new System.Drawing.Size(285, 90);
            this.ribbonBar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar2.TabIndex = 1;
            this.ribbonBar2.Text = "元素导航";
            // 
            // 
            // 
            this.ribbonBar2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar2.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // integerElemCount
            // 
            // 
            // 
            // 
            this.integerElemCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.integerElemCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.integerElemCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.integerElemCount.Location = new System.Drawing.Point(28, 28);
            this.integerElemCount.Name = "integerElemCount";
            this.integerElemCount.Size = new System.Drawing.Size(43, 21);
            this.integerElemCount.TabIndex = 3;
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer2.Name = "itemContainer2";
            this.itemContainer2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.comboSelectedElem,
            this.itemContainer3});
            // 
            // 
            // 
            this.itemContainer2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // comboSelectedElem
            // 
            this.comboSelectedElem.ComboWidth = 200;
            this.comboSelectedElem.DropDownHeight = 106;
            this.comboSelectedElem.ItemHeight = 16;
            this.comboSelectedElem.Name = "comboSelectedElem";
            this.comboSelectedElem.Text = "comboBoxItem1";
            this.comboSelectedElem.SelectedIndexChanged += new System.EventHandler(this.comboSelectedElem_SelectedIndexChanged);
            // 
            // itemContainer3
            // 
            // 
            // 
            // 
            this.itemContainer3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer3.Name = "itemContainer3";
            this.itemContainer3.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnPrevious,
            this.controlContainerItem1,
            this.btnNext,
            this.btnAddSelectElement});
            // 
            // 
            // 
            this.itemContainer3.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Text = "<-";
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = true;
            this.controlContainerItem1.Control = this.integerElemCount;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            this.controlContainerItem1.Text = "con";
            // 
            // btnNext
            // 
            this.btnNext.Name = "btnNext";
            this.btnNext.Text = "->";
            // 
            // btnAddSelectElement
            // 
            this.btnAddSelectElement.Name = "btnAddSelectElement";
            this.btnAddSelectElement.Text = "添加选择集";
            // 
            // itemContainer4
            // 
            // 
            // 
            // 
            this.itemContainer4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer4.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer4.Name = "itemContainer4";
            this.itemContainer4.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.txtElemId});
            // 
            // 
            // 
            this.itemContainer4.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // txtElemId
            // 
            this.txtElemId.Name = "txtElemId";
            this.txtElemId.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.txtElemId.WatermarkText = "元素标识";
            this.txtElemId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtElemId_KeyDown);
            // 
            // ribbonBar1
            // 
            this.ribbonBar1.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar1.ContainerControlProcessDialogKey = true;
            this.ribbonBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar1.DragDropSupport = true;
            this.ribbonBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1,
            this.itemContainer5});
            this.ribbonBar1.Location = new System.Drawing.Point(3, 0);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.Size = new System.Drawing.Size(180, 90);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 0;
            this.ribbonBar1.Text = "常用工具";
            // 
            // 
            // 
            this.ribbonBar1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnStrokeCurveElem,
            this.btnTxtCenterAlignment,
            this.btnDeletePoint});
            // 
            // 
            // 
            this.itemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnStrokeCurveElem
            // 
            this.btnStrokeCurveElem.Name = "btnStrokeCurveElem";
            this.btnStrokeCurveElem.Text = "曲线折线化";
            // 
            // btnTxtCenterAlignment
            // 
            this.btnTxtCenterAlignment.Name = "btnTxtCenterAlignment";
            this.btnTxtCenterAlignment.Text = "文本居中";
            // 
            // btnDeletePoint
            // 
            this.btnDeletePoint.Name = "btnDeletePoint";
            this.btnDeletePoint.Text = "删除点";
            // 
            // ribbonTabItem1
            // 
            this.ribbonTabItem1.Checked = true;
            this.ribbonTabItem1.Name = "ribbonTabItem1";
            this.ribbonTabItem1.Panel = this.ribbonPanel1;
            this.ribbonTabItem1.Text = "常用工具";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "buttonItem1";
            // 
            // qatCustomizeItem1
            // 
            this.qatCustomizeItem1.Name = "qatCustomizeItem1";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Blue;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 121);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(678, 296);
            this.superTabControl1.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.superTabControl1.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControl1.TabIndex = 1;
            // 
            // itemContainer5
            // 
            // 
            // 
            // 
            this.itemContainer5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer5.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer5.Name = "itemContainer5";
            this.itemContainer5.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnMoveTextElement});
            // 
            // 
            // 
            this.itemContainer5.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnMoveTextElement
            // 
            this.btnMoveTextElement.Name = "btnMoveTextElement";
            this.btnMoveTextElement.Text = "移动标注";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 417);
            this.Controls.Add(this.superTabControl1);
            this.Controls.Add(this.ribbonCtrlMain);
            this.Name = "FormMain";
            this.Text = "DGN整理工具  by Von_dream";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ribbonCtrlMain.ResumeLayout(false);
            this.ribbonCtrlMain.PerformLayout();
            this.ribbonPanel1.ResumeLayout(false);
            this.ribbonBar2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.integerElemCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.RibbonControl ribbonCtrlMain;
        private DevComponents.DotNetBar.RibbonPanel ribbonPanel1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private DevComponents.DotNetBar.RibbonTabItem ribbonTabItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.QatCustomizeItem qatCustomizeItem1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ButtonItem btnStrokeCurveElem;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar2;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        private DevComponents.DotNetBar.ComboBoxItem comboSelectedElem;
        private DevComponents.DotNetBar.ItemContainer itemContainer3;
        private DevComponents.DotNetBar.ButtonItem btnPrevious;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem1;
        private DevComponents.DotNetBar.ButtonItem btnNext;
        private DevComponents.Editors.IntegerInput integerElemCount;
        private DevComponents.DotNetBar.ButtonItem btnAddSelectElement;
        private DevComponents.DotNetBar.ItemContainer itemContainer4;
        private DevComponents.DotNetBar.TextBoxItem txtElemId;
        private DevComponents.DotNetBar.ButtonItem btnTxtCenterAlignment;
        private DevComponents.DotNetBar.ButtonItem btnDeletePoint;
        private DevComponents.DotNetBar.ItemContainer itemContainer5;
        private DevComponents.DotNetBar.ButtonItem btnMoveTextElement;
    }
}
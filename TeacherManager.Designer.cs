
namespace SchoolManagement
{
    partial class TeacherManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherManager));
            this.txtID = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbTeacherID = new System.Windows.Forms.Label();
            this.lbDepName = new System.Windows.Forms.Label();
            this.cbDepartment = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.lbExport = new System.Windows.Forms.Label();
            this.lbSave = new System.Windows.Forms.Label();
            this.lbDeleteTeacher = new System.Windows.Forms.Label();
            this.lbEditTeacher = new System.Windows.Forms.Label();
            this.lbAddTeacher = new System.Windows.Forms.Label();
            this.lbNewPass = new System.Windows.Forms.Label();
            this.txtPassword = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbGender = new System.Windows.Forms.Label();
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.rbFemale = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbMale = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbAdress = new System.Windows.Forms.Label();
            this.lbBirth = new System.Windows.Forms.Label();
            this.dtpBirth = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.txtName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbFullName = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtSearch = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.dgvTeachers = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbPrev = new System.Windows.Forms.PictureBox();
            this.pbNext = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbSave = new System.Windows.Forms.PictureBox();
            this.pbDelete = new System.Windows.Forms.PictureBox();
            this.pbEdit = new System.Windows.Forms.PictureBox();
            this.pbTeachers = new System.Windows.Forms.PictureBox();
            this.pbReload = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.cbDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeachers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReload)).BeginInit();
            this.SuspendLayout();
            // 
            // txtID
            // 
            resources.ApplyResources(this.txtID, "txtID");
            this.txtID.Name = "txtID";
            this.txtID.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtID.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtID.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtID.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtID.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtID.StateCommon.Border.Rounding = 20;
            this.txtID.StateCommon.Border.Width = 1;
            this.txtID.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtID.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            // 
            // lbTeacherID
            // 
            resources.ApplyResources(this.lbTeacherID, "lbTeacherID");
            this.lbTeacherID.Name = "lbTeacherID";
            // 
            // lbDepName
            // 
            resources.ApplyResources(this.lbDepName, "lbDepName");
            this.lbDepName.Name = "lbDepName";
            // 
            // cbDepartment
            // 
            resources.ApplyResources(this.cbDepartment, "cbDepartment");
            this.cbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDepartment.DropDownWidth = 285;
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.ComboBox.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.ComboBox.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.ComboBox.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.cbDepartment.StateCommon.ComboBox.Border.Rounding = 20;
            this.cbDepartment.StateCommon.ComboBox.Border.Width = 1;
            this.cbDepartment.StateCommon.ComboBox.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDepartment.StateCommon.DropBack.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.DropBack.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.Item.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.Item.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.Item.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.Item.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.Item.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.cbDepartment.StateCommon.Item.Border.Width = 1;
            this.cbDepartment.StateCommon.Item.Content.ShortText.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lbExport
            // 
            resources.ApplyResources(this.lbExport, "lbExport");
            this.lbExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbExport.Name = "lbExport";
            // 
            // lbSave
            // 
            resources.ApplyResources(this.lbSave, "lbSave");
            this.lbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSave.Name = "lbSave";
            this.lbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // lbDeleteTeacher
            // 
            resources.ApplyResources(this.lbDeleteTeacher, "lbDeleteTeacher");
            this.lbDeleteTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbDeleteTeacher.Name = "lbDeleteTeacher";
            // 
            // lbEditTeacher
            // 
            resources.ApplyResources(this.lbEditTeacher, "lbEditTeacher");
            this.lbEditTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbEditTeacher.Name = "lbEditTeacher";
            this.lbEditTeacher.Click += new System.EventHandler(this.pbEdit_Click);
            // 
            // lbAddTeacher
            // 
            resources.ApplyResources(this.lbAddTeacher, "lbAddTeacher");
            this.lbAddTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAddTeacher.Name = "lbAddTeacher";
            // 
            // lbNewPass
            // 
            resources.ApplyResources(this.lbNewPass, "lbNewPass");
            this.lbNewPass.Name = "lbNewPass";
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtPassword.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtPassword.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtPassword.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtPassword.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtPassword.StateCommon.Border.Rounding = 20;
            this.txtPassword.StateCommon.Border.Width = 1;
            this.txtPassword.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtPassword.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            // 
            // lbGender
            // 
            resources.ApplyResources(this.lbGender, "lbGender");
            this.lbGender.Name = "lbGender";
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.ButtonSpecs.FormClose.AllowInheritExtraText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.AllowInheritExtraText")));
            this.kryptonPalette1.ButtonSpecs.FormClose.AllowInheritImage = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.AllowInheritImage")));
            this.kryptonPalette1.ButtonSpecs.FormClose.AllowInheritText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.AllowInheritText")));
            this.kryptonPalette1.ButtonSpecs.FormClose.AllowInheritToolTipTitle = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.AllowInheritToolTipTitle")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ColorMap = ((System.Drawing.Color)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ColorMap")));
            this.kryptonPalette1.ButtonSpecs.FormClose.Edge = ((ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.Edge")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ExtraText = resources.GetString("kryptonPalette1.ButtonSpecs.FormClose.ExtraText");
            this.kryptonPalette1.ButtonSpecs.FormClose.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.Image")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedNormal")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedPressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedPressed")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageCheckedTracking")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageDisabled = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageDisabled")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageNormal")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageTracking")));
            this.kryptonPalette1.ButtonSpecs.FormClose.Orientation = ((ComponentFactory.Krypton.Toolkit.PaletteButtonOrientation)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.Orientation")));
            this.kryptonPalette1.ButtonSpecs.FormClose.Style = ((ComponentFactory.Krypton.Toolkit.PaletteButtonStyle)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.Style")));
            this.kryptonPalette1.ButtonSpecs.FormClose.Text = resources.GetString("kryptonPalette1.ButtonSpecs.FormClose.Text");
            this.kryptonPalette1.ButtonSpecs.FormClose.ToolTipTitle = resources.GetString("kryptonPalette1.ButtonSpecs.FormClose.ToolTipTitle");
            this.kryptonPalette1.ButtonSpecs.FormMax.AllowInheritExtraText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.AllowInheritExtraText")));
            this.kryptonPalette1.ButtonSpecs.FormMax.AllowInheritImage = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.AllowInheritImage")));
            this.kryptonPalette1.ButtonSpecs.FormMax.AllowInheritText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.AllowInheritText")));
            this.kryptonPalette1.ButtonSpecs.FormMax.AllowInheritToolTipTitle = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.AllowInheritToolTipTitle")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ColorMap = ((System.Drawing.Color)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ColorMap")));
            this.kryptonPalette1.ButtonSpecs.FormMax.Edge = ((ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.Edge")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ExtraText = resources.GetString("kryptonPalette1.ButtonSpecs.FormMax.ExtraText");
            this.kryptonPalette1.ButtonSpecs.FormMax.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.Image")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedNormal")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedPressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedPressed")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageCheckedTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageDisabled = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageDisabled")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageNormal")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMax.Orientation = ((ComponentFactory.Krypton.Toolkit.PaletteButtonOrientation)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.Orientation")));
            this.kryptonPalette1.ButtonSpecs.FormMax.Style = ((ComponentFactory.Krypton.Toolkit.PaletteButtonStyle)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.Style")));
            this.kryptonPalette1.ButtonSpecs.FormMax.Text = resources.GetString("kryptonPalette1.ButtonSpecs.FormMax.Text");
            this.kryptonPalette1.ButtonSpecs.FormMax.ToolTipTitle = resources.GetString("kryptonPalette1.ButtonSpecs.FormMax.ToolTipTitle");
            this.kryptonPalette1.ButtonSpecs.FormMin.AllowInheritExtraText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.AllowInheritExtraText")));
            this.kryptonPalette1.ButtonSpecs.FormMin.AllowInheritImage = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.AllowInheritImage")));
            this.kryptonPalette1.ButtonSpecs.FormMin.AllowInheritText = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.AllowInheritText")));
            this.kryptonPalette1.ButtonSpecs.FormMin.AllowInheritToolTipTitle = ((bool)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.AllowInheritToolTipTitle")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ColorMap = ((System.Drawing.Color)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ColorMap")));
            this.kryptonPalette1.ButtonSpecs.FormMin.Edge = ((ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.Edge")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ExtraText = resources.GetString("kryptonPalette1.ButtonSpecs.FormMin.ExtraText");
            this.kryptonPalette1.ButtonSpecs.FormMin.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.Image")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedNormal")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedPressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedPressed")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageCheckedTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageDisabled = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageDisabled")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageNormal = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageNormal")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMin.Orientation = ((ComponentFactory.Krypton.Toolkit.PaletteButtonOrientation)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.Orientation")));
            this.kryptonPalette1.ButtonSpecs.FormMin.Style = ((ComponentFactory.Krypton.Toolkit.PaletteButtonStyle)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.Style")));
            this.kryptonPalette1.ButtonSpecs.FormMin.Text = resources.GetString("kryptonPalette1.ButtonSpecs.FormMin.Text");
            this.kryptonPalette1.ButtonSpecs.FormMin.ToolTipTitle = resources.GetString("kryptonPalette1.ButtonSpecs.FormMin.ToolTipTitle");
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StatePressed.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonFormClose.StateTracking.Border.Width = 0;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.None;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.Rounding = 12;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.ButtonEdgeInset = 10;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, -1, -1, -1);
            // 
            // rbFemale
            // 
            resources.ApplyResources(this.rbFemale, "rbFemale");
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.StateCommon.ShortText.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFemale.StateDisabled.ShortText.Color1 = System.Drawing.Color.Black;
            this.rbFemale.StateDisabled.ShortText.Color2 = System.Drawing.Color.Black;
            this.rbFemale.Values.ExtraText = resources.GetString("rbFemale.Values.ExtraText");
            this.rbFemale.Values.ImageTransparentColor = ((System.Drawing.Color)(resources.GetObject("rbFemale.Values.ImageTransparentColor")));
            this.rbFemale.Values.Text = resources.GetString("rbFemale.Values.Text");
            // 
            // rbMale
            // 
            resources.ApplyResources(this.rbMale, "rbMale");
            this.rbMale.Name = "rbMale";
            this.rbMale.StateCommon.ShortText.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMale.StateDisabled.ShortText.Color1 = System.Drawing.Color.Black;
            this.rbMale.StateDisabled.ShortText.Color2 = System.Drawing.Color.Black;
            this.rbMale.Values.ExtraText = resources.GetString("rbMale.Values.ExtraText");
            this.rbMale.Values.ImageTransparentColor = ((System.Drawing.Color)(resources.GetObject("rbMale.Values.ImageTransparentColor")));
            this.rbMale.Values.Text = resources.GetString("rbMale.Values.Text");
            // 
            // txtAddress
            // 
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtAddress.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtAddress.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtAddress.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtAddress.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtAddress.StateCommon.Border.Rounding = 20;
            this.txtAddress.StateCommon.Border.Width = 1;
            this.txtAddress.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtAddress.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            // 
            // lbAdress
            // 
            resources.ApplyResources(this.lbAdress, "lbAdress");
            this.lbAdress.Name = "lbAdress";
            // 
            // lbBirth
            // 
            resources.ApplyResources(this.lbBirth, "lbBirth");
            this.lbBirth.Name = "lbBirth";
            // 
            // dtpBirth
            // 
            resources.ApplyResources(this.dtpBirth, "dtpBirth");
            this.dtpBirth.Name = "dtpBirth";
            this.dtpBirth.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.dtpBirth.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.dtpBirth.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.dtpBirth.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dtpBirth.StateCommon.Border.Rounding = 20;
            this.dtpBirth.StateCommon.Border.Width = 1;
            this.dtpBirth.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBirth.StateDisabled.Content.Color1 = System.Drawing.Color.Black;
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtName.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtName.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtName.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtName.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtName.StateCommon.Border.Rounding = 20;
            this.txtName.StateCommon.Border.Width = 1;
            this.txtName.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtName.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            // 
            // lbFullName
            // 
            resources.ApplyResources(this.lbFullName, "lbFullName");
            this.lbFullName.Name = "lbFullName";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.OverrideDefault.Back.ColorAngle = 62F;
            this.btnSearch.OverrideDefault.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.OverrideDefault.Border.ColorAngle = 62F;
            this.btnSearch.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSearch.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSearch.OverrideDefault.Border.Rounding = 20;
            this.btnSearch.OverrideDefault.Border.Width = 1;
            this.btnSearch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StateCommon.Back.ColorAngle = 62F;
            this.btnSearch.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StateCommon.Border.ColorAngle = 62F;
            this.btnSearch.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSearch.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSearch.StateCommon.Border.Rounding = 20;
            this.btnSearch.StateCommon.Border.Width = 1;
            this.btnSearch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Black;
            this.btnSearch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.Black;
            this.btnSearch.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StatePressed.Back.ColorAngle = 62F;
            this.btnSearch.StatePressed.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StatePressed.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StatePressed.Border.ColorAngle = 62F;
            this.btnSearch.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSearch.StatePressed.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSearch.StatePressed.Border.Rounding = 20;
            this.btnSearch.StatePressed.Border.Width = 1;
            this.btnSearch.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StateTracking.Back.ColorAngle = 62F;
            this.btnSearch.StateTracking.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSearch.StateTracking.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSearch.StateTracking.Border.ColorAngle = 62F;
            this.btnSearch.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSearch.StateTracking.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSearch.StateTracking.Border.Rounding = 20;
            this.btnSearch.StateTracking.Border.Width = 1;
            this.btnSearch.Values.ExtraText = resources.GetString("btnSearch.Values.ExtraText");
            this.btnSearch.Values.ImageTransparentColor = ((System.Drawing.Color)(resources.GetObject("btnSearch.Values.ImageTransparentColor")));
            this.btnSearch.Values.Text = resources.GetString("btnSearch.Values.Text");
            this.btnSearch.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtSearch.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtSearch.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtSearch.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtSearch.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtSearch.StateCommon.Border.Rounding = 20;
            this.txtSearch.StateCommon.Border.Width = 1;
            this.txtSearch.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtSearch.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            // 
            // dgvTeachers
            // 
            resources.ApplyResources(this.dgvTeachers, "dgvTeachers");
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            this.dgvTeachers.AllowUserToOrderColumns = true;
            this.dgvTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTeachers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.ReadOnly = true;
            this.dgvTeachers.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.dgvTeachers.StateCommon.Background.Color2 = System.Drawing.Color.White;
            this.dgvTeachers.StateCommon.Background.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.dgvTeachers.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvTeachers.StateCommon.DataCell.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.dgvTeachers.StateCommon.DataCell.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.dgvTeachers.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.DataCell.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvTeachers.StateCommon.DataCell.Border.Width = 1;
            this.dgvTeachers.StateCommon.DataCell.Content.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTeachers.StateCommon.HeaderColumn.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderColumn.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderColumn.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderColumn.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderColumn.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvTeachers.StateCommon.HeaderColumn.Border.Width = 1;
            this.dgvTeachers.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.Color.White;
            this.dgvTeachers.StateCommon.HeaderColumn.Content.Color2 = System.Drawing.Color.White;
            this.dgvTeachers.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTeachers.StateCommon.HeaderRow.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderRow.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvTeachers.StateCommon.HeaderRow.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvTeachers.StateCommon.HeaderRow.Border.Width = 1;
            this.dgvTeachers.StateSelected.DataCell.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.dgvTeachers.StateSelected.DataCell.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.dgvTeachers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTeachers_CellMouseClick);
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pbNext_Click);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pbPrev_Click);
            // 
            // pbPrev
            // 
            resources.ApplyResources(this.pbPrev, "pbPrev");
            this.pbPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbPrev.Name = "pbPrev";
            this.pbPrev.TabStop = false;
            this.pbPrev.Click += new System.EventHandler(this.pbPrev_Click);
            // 
            // pbNext
            // 
            resources.ApplyResources(this.pbNext, "pbNext");
            this.pbNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbNext.Name = "pbNext";
            this.pbNext.TabStop = false;
            this.pbNext.Click += new System.EventHandler(this.pbNext_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pbSave
            // 
            resources.ApplyResources(this.pbSave, "pbSave");
            this.pbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSave.Name = "pbSave";
            this.pbSave.TabStop = false;
            this.pbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // pbDelete
            // 
            resources.ApplyResources(this.pbDelete, "pbDelete");
            this.pbDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.TabStop = false;
            this.pbDelete.Click += new System.EventHandler(this.pbDelete_Click);
            // 
            // pbEdit
            // 
            resources.ApplyResources(this.pbEdit, "pbEdit");
            this.pbEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEdit.Name = "pbEdit";
            this.pbEdit.TabStop = false;
            this.pbEdit.Click += new System.EventHandler(this.pbEdit_Click);
            // 
            // pbTeachers
            // 
            resources.ApplyResources(this.pbTeachers, "pbTeachers");
            this.pbTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbTeachers.Name = "pbTeachers";
            this.pbTeachers.TabStop = false;
            this.pbTeachers.Click += new System.EventHandler(this.pbTeachers_Click);
            // 
            // pbReload
            // 
            resources.ApplyResources(this.pbReload, "pbReload");
            this.pbReload.Name = "pbReload";
            this.pbReload.TabStop = false;
            this.pbReload.Click += new System.EventHandler(this.pbReload_Click);
            // 
            // TeacherManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pbPrev);
            this.Controls.Add(this.pbNext);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.lbTeacherID);
            this.Controls.Add(this.lbDepName);
            this.Controls.Add(this.cbDepartment);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbSave);
            this.Controls.Add(this.pbSave);
            this.Controls.Add(this.lbDeleteTeacher);
            this.Controls.Add(this.pbDelete);
            this.Controls.Add(this.lbEditTeacher);
            this.Controls.Add(this.pbEdit);
            this.Controls.Add(this.lbAddTeacher);
            this.Controls.Add(this.pbTeachers);
            this.Controls.Add(this.lbNewPass);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.pbReload);
            this.Controls.Add(this.lbGender);
            this.Controls.Add(this.rbFemale);
            this.Controls.Add(this.rbMale);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lbAdress);
            this.Controls.Add(this.lbBirth);
            this.Controls.Add(this.dtpBirth);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lbFullName);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvTeachers);
            this.Controls.Add(this.lbExport);
            this.Name = "TeacherManager";
            this.Palette = this.kryptonPalette1;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.Load += new System.EventHandler(this.TeacherManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cbDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeachers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPrev;
        private System.Windows.Forms.PictureBox pbNext;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtID;
        private System.Windows.Forms.Label lbTeacherID;
        private System.Windows.Forms.Label lbDepName;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cbDepartment;
        private System.Windows.Forms.Label lbExport;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbSave;
        private System.Windows.Forms.PictureBox pbSave;
        private System.Windows.Forms.Label lbDeleteTeacher;
        private System.Windows.Forms.PictureBox pbDelete;
        private System.Windows.Forms.Label lbEditTeacher;
        private System.Windows.Forms.PictureBox pbEdit;
        private System.Windows.Forms.Label lbAddTeacher;
        private System.Windows.Forms.PictureBox pbTeachers;
        private System.Windows.Forms.Label lbNewPass;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPassword;
        private System.Windows.Forms.PictureBox pbReload;
        private System.Windows.Forms.Label lbGender;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbFemale;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbMale;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;
        private System.Windows.Forms.Label lbAdress;
        private System.Windows.Forms.Label lbBirth;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpBirth;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtName;
        private System.Windows.Forms.Label lbFullName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvTeachers;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}
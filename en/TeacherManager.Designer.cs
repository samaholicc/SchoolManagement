
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
            this.pbPrev = new System.Windows.Forms.PictureBox();
            this.pbNext = new System.Windows.Forms.PictureBox();
            this.txtID = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbTeacherID = new System.Windows.Forms.Label();
            this.lbDepName = new System.Windows.Forms.Label();
            this.cbDepartment = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.lbExport = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbSave = new System.Windows.Forms.Label();
            this.pbSave = new System.Windows.Forms.PictureBox();
            this.lbDeleteTeacher = new System.Windows.Forms.Label();
            this.pbDelete = new System.Windows.Forms.PictureBox();
            this.lbEditTeacher = new System.Windows.Forms.Label();
            this.pbEdit = new System.Windows.Forms.PictureBox();
            this.lbAddTeacher = new System.Windows.Forms.Label();
            this.pbTeachers = new System.Windows.Forms.PictureBox();
            this.lbNewPass = new System.Windows.Forms.Label();
            this.txtPassword = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.pbReload = new System.Windows.Forms.PictureBox();
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeachers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPrev
            // 
            this.pbPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbPrev.Image = ((System.Drawing.Image)(resources.GetObject("pbPrev.Image")));
            this.pbPrev.Location = new System.Drawing.Point(939, 753);
            this.pbPrev.Name = "pbPrev";
            this.pbPrev.Size = new System.Drawing.Size(39, 33);
            this.pbPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPrev.TabIndex = 108;
            this.pbPrev.TabStop = false;
            this.pbPrev.Click += new System.EventHandler(this.pbPrev_Click);
            // 
            // pbNext
            // 
            this.pbNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbNext.Image = ((System.Drawing.Image)(resources.GetObject("pbNext.Image")));
            this.pbNext.Location = new System.Drawing.Point(988, 753);
            this.pbNext.Name = "pbNext";
            this.pbNext.Size = new System.Drawing.Size(39, 33);
            this.pbNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNext.TabIndex = 107;
            this.pbNext.TabStop = false;
            this.pbNext.Click += new System.EventHandler(this.pbNext_Click);
            // 
            // txtID
            // 
            this.txtID.Enabled = false;
            this.txtID.Location = new System.Drawing.Point(211, 6);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(315, 33);
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
            this.txtID.TabIndex = 106;
            // 
            // lbTeacherID
            // 
            this.lbTeacherID.AutoSize = true;
            this.lbTeacherID.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTeacherID.Location = new System.Drawing.Point(12, 13);
            this.lbTeacherID.Name = "lbTeacherID";
            this.lbTeacherID.Size = new System.Drawing.Size(92, 20);
            this.lbTeacherID.TabIndex = 105;
            this.lbTeacherID.Text = "Teacher ID:";
            // 
            // lbDepName
            // 
            this.lbDepName.AutoSize = true;
            this.lbDepName.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDepName.Location = new System.Drawing.Point(12, 111);
            this.lbDepName.Name = "lbDepName";
            this.lbDepName.Size = new System.Drawing.Size(101, 20);
            this.lbDepName.TabIndex = 104;
            this.lbDepName.Text = "Department:";
            // 
            // cbDepartment
            // 
            this.cbDepartment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDepartment.DropDownWidth = 285;
            this.cbDepartment.Enabled = false;
            this.cbDepartment.Location = new System.Drawing.Point(211, 95);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(315, 36);
            this.cbDepartment.StateCommon.ComboBox.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.cbDepartment.StateCommon.ComboBox.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.ComboBox.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.cbDepartment.StateCommon.ComboBox.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.cbDepartment.StateCommon.ComboBox.Border.Rounding = 20;
            this.cbDepartment.StateCommon.ComboBox.Border.Width = 1;
            this.cbDepartment.StateCommon.ComboBox.Content.Font = new System.Drawing.Font("_Lingobit_Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cbDepartment.TabIndex = 103;
            this.cbDepartment.SelectedIndexChanged += new System.EventHandler(this.cbDepartment_SelectedIndexChanged);
            // 
            // lbExport
            // 
            this.lbExport.AutoSize = true;
            this.lbExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbExport.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.lbExport.Location = new System.Drawing.Point(865, 374);
            this.lbExport.Name = "lbExport";
            this.lbExport.Size = new System.Drawing.Size(48, 17);
            this.lbExport.TabIndex = 102;
            this.lbExport.Text = "Export";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(919, 358);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 101;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lbSave
            // 
            this.lbSave.AutoSize = true;
            this.lbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbSave.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSave.Location = new System.Drawing.Point(832, 304);
            this.lbSave.Name = "lbSave";
            this.lbSave.Size = new System.Drawing.Size(46, 20);
            this.lbSave.TabIndex = 100;
            this.lbSave.Text = "SAVE";
            this.lbSave.Visible = false;
            this.lbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // pbSave
            // 
            this.pbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSave.Image = ((System.Drawing.Image)(resources.GetObject("pbSave.Image")));
            this.pbSave.Location = new System.Drawing.Point(825, 176);
            this.pbSave.Name = "pbSave";
            this.pbSave.Size = new System.Drawing.Size(114, 125);
            this.pbSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSave.TabIndex = 99;
            this.pbSave.TabStop = false;
            this.pbSave.Visible = false;
            this.pbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // lbDeleteTeacher
            // 
            this.lbDeleteTeacher.AutoSize = true;
            this.lbDeleteTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbDeleteTeacher.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDeleteTeacher.Location = new System.Drawing.Point(638, 305);
            this.lbDeleteTeacher.Name = "lbDeleteTeacher";
            this.lbDeleteTeacher.Size = new System.Drawing.Size(123, 20);
            this.lbDeleteTeacher.TabIndex = 98;
            this.lbDeleteTeacher.Text = "DELETE TEACHER";
            // 
            // pbDelete
            // 
            this.pbDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDelete.Image = ((System.Drawing.Image)(resources.GetObject("pbDelete.Image")));
            this.pbDelete.Location = new System.Drawing.Point(642, 176);
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.Size = new System.Drawing.Size(114, 125);
            this.pbDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDelete.TabIndex = 97;
            this.pbDelete.TabStop = false;
            this.pbDelete.Click += new System.EventHandler(this.pbDelete_Click);
            // 
            // lbEditTeacher
            // 
            this.lbEditTeacher.AutoSize = true;
            this.lbEditTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbEditTeacher.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEditTeacher.Location = new System.Drawing.Point(832, 134);
            this.lbEditTeacher.Name = "lbEditTeacher";
            this.lbEditTeacher.Size = new System.Drawing.Size(105, 20);
            this.lbEditTeacher.TabIndex = 96;
            this.lbEditTeacher.Text = "EDIT TEACHER";
            this.lbEditTeacher.Click += new System.EventHandler(this.pbEdit_Click);
            // 
            // pbEdit
            // 
            this.pbEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEdit.Image = ((System.Drawing.Image)(resources.GetObject("pbEdit.Image")));
            this.pbEdit.Location = new System.Drawing.Point(825, 6);
            this.pbEdit.Name = "pbEdit";
            this.pbEdit.Size = new System.Drawing.Size(114, 125);
            this.pbEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEdit.TabIndex = 95;
            this.pbEdit.TabStop = false;
            this.pbEdit.Click += new System.EventHandler(this.pbEdit_Click);
            // 
            // lbAddTeacher
            // 
            this.lbAddTeacher.AutoSize = true;
            this.lbAddTeacher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAddTeacher.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAddTeacher.Location = new System.Drawing.Point(646, 134);
            this.lbAddTeacher.Name = "_Lingobit_lbAddTeacher";
            this.lbAddTeacher.Size = new System.Drawing.Size(110, 20);
            this.lbAddTeacher.TabIndex = 94;
            this.lbAddTeacher.Text = "NEW TEACHER";
            // 
            // pbTeachers
            // 
            this.pbTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbTeachers.Image = ((System.Drawing.Image)(resources.GetObject("pbTeachers.Image")));
            this.pbTeachers.Location = new System.Drawing.Point(642, 6);
            this.pbTeachers.Name = "pbTeachers";
            this.pbTeachers.Size = new System.Drawing.Size(114, 125);
            this.pbTeachers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTeachers.TabIndex = 93;
            this.pbTeachers.TabStop = false;
            this.pbTeachers.Click += new System.EventHandler(this.pbTeachers_Click);
            // 
            // lbNewPass
            // 
            this.lbNewPass.AutoSize = true;
            this.lbNewPass.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNewPass.Location = new System.Drawing.Point(13, 281);
            this.lbNewPass.Name = "lbNewPass";
            this.lbNewPass.Size = new System.Drawing.Size(122, 20);
            this.lbNewPass.TabIndex = 92;
            this.lbNewPass.Text = "New Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(211, 267);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(315, 34);
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
            this.txtPassword.TabIndex = 91;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // pbReload
            // 
            this.pbReload.Image = ((System.Drawing.Image)(resources.GetObject("pbReload.Image")));
            this.pbReload.Location = new System.Drawing.Point(427, 358);
            this.pbReload.Name = "pbReload";
            this.pbReload.Size = new System.Drawing.Size(39, 33);
            this.pbReload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbReload.TabIndex = 90;
            this.pbReload.TabStop = false;
            this.pbReload.Click += new System.EventHandler(this.pbReload_Click);
            // 
            // lbGender
            // 
            this.lbGender.AutoSize = true;
            this.lbGender.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGender.Location = new System.Drawing.Point(13, 236);
            this.lbGender.Name = "_Lingobit_lbGender";
            this.lbGender.Size = new System.Drawing.Size(70, 20);
            this.lbGender.TabIndex = 89;
            this.lbGender.Text = "Gender:";
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.ButtonSpecs.FormClose.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.Image")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormClose.ImageStates.ImageTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMax.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.Image")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMax.ImageStates.ImageTracking")));
            this.kryptonPalette1.ButtonSpecs.FormMin.Image = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.Image")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImagePressed = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImagePressed")));
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageTracking")));
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
            this.rbFemale.Enabled = false;
            this.rbFemale.Location = new System.Drawing.Point(315, 233);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(79, 22);
            this.rbFemale.StateCommon.ShortText.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFemale.StateDisabled.ShortText.Color1 = System.Drawing.Color.Black;
            this.rbFemale.StateDisabled.ShortText.Color2 = System.Drawing.Color.Black;
            this.rbFemale.TabIndex = 88;
            this.rbFemale.Values.Text = "Female";
            // 
            // rbMale
            // 
            this.rbMale.Enabled = false;
            this.rbMale.Location = new System.Drawing.Point(211, 234);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(61, 22);
            this.rbMale.StateCommon.ShortText.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMale.StateDisabled.ShortText.Color1 = System.Drawing.Color.Black;
            this.rbMale.StateDisabled.ShortText.Color2 = System.Drawing.Color.Black;
            this.rbMale.TabIndex = 87;
            this.rbMale.Values.Text = "Male";
            // 
            // txtAddress
            // 
            this.txtAddress.Enabled = false;
            this.txtAddress.Location = new System.Drawing.Point(211, 179);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(315, 33);
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
            this.txtAddress.StateCommon.Content.Font = new System.Drawing.Font("_Lingobit_Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.StateCommon.Content.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtAddress.TabIndex = 86;
            // 
            // lbAdress
            // 
            this.lbAdress.AutoSize = true;
            this.lbAdress.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAdress.Location = new System.Drawing.Point(12, 192);
            this.lbAdress.Name = "lbAdress";
            this.lbAdress.Size = new System.Drawing.Size(71, 20);
            this.lbAdress.TabIndex = 85;
            this.lbAdress.Text = "Address:";
            // 
            // lbBirth
            // 
            this.lbBirth.AutoSize = true;
            this.lbBirth.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBirth.Location = new System.Drawing.Point(12, 154);
            this.lbBirth.Name = "lbBirth";
            this.lbBirth.Size = new System.Drawing.Size(44, 20);
            this.lbBirth.TabIndex = 82;
            this.lbBirth.Text = "Birth:";
            // 
            // dtpBirth
            // 
            this.dtpBirth.Enabled = false;
            this.dtpBirth.Location = new System.Drawing.Point(211, 136);
            this.dtpBirth.Name = "dtpBirth";
            this.dtpBirth.Size = new System.Drawing.Size(315, 37);
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
            this.dtpBirth.TabIndex = 81;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(211, 56);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(315, 33);
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
            this.txtName.TabIndex = 78;
            // 
            // lbFullName
            // 
            this.lbFullName.AutoSize = true;
            this.lbFullName.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFullName.Location = new System.Drawing.Point(12, 65);
            this.lbFullName.Name = "lbFullName";
            this.lbFullName.Size = new System.Drawing.Size(83, 20);
            this.lbFullName.TabIndex = 77;
            this.lbFullName.Text = "Full Name:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(325, 358);
            this.btnSearch.Name = "_Lingobit_btnSearch";
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
            this.btnSearch.Size = new System.Drawing.Size(96, 33);
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
            this.btnSearch.TabIndex = 76;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 358);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(307, 33);
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
            this.txtSearch.TabIndex = 75;
            // 
            // dgvTeachers
            // 
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            this.dgvTeachers.AllowUserToOrderColumns = true;
            this.dgvTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTeachers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Location = new System.Drawing.Point(1, 397);
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.ReadOnly = true;
            this.dgvTeachers.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTeachers.Size = new System.Drawing.Size(957, 248);
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
            this.dgvTeachers.TabIndex = 74;
            this.dgvTeachers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTeachers_CellContentClick);
            this.dgvTeachers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTeachers_CellMouseClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(853, 651);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 33);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 109;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pbPrev_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("_Lingobit_pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(898, 651);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(39, 33);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 110;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pbNext_Click);
            // 
            // TeacherManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(970, 688);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEACHER MANAGER";
            this.Load += new System.EventHandler(this.TeacherManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPrev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTeachers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
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
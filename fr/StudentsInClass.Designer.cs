
namespace SchoolManagement
{
    partial class StudentsInClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentsInClass));
            this.lbDeleteStudent = new System.Windows.Forms.Label();
            this.lbAddStudents = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lbStudentGrade = new System.Windows.Forms.Label();
            this.txtAver = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbMoyenne = new System.Windows.Forms.Label();
            this.txtFinal = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbNote2 = new System.Windows.Forms.Label();
            this.txtMid = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lbNote1 = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.Name2 = new System.Windows.Forms.Label();
            this.lbMSSV = new System.Windows.Forms.Label();
            this.lbInfoStudent = new System.Windows.Forms.Label();
            this.lbStudentID = new System.Windows.Forms.Label();
            this.count = new System.Windows.Forms.Label();
            this.lbExport = new System.Windows.Forms.Label();
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.lbStudentlist = new System.Windows.Forms.Label();
            this.dgvStudents = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.pbDeleteStudent = new System.Windows.Forms.PictureBox();
            this.pbStudents = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteStudent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStudents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbDeleteStudent
            // 
            this.lbDeleteStudent.AutoSize = true;
            this.lbDeleteStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbDeleteStudent.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDeleteStudent.Location = new System.Drawing.Point(536, 480);
            this.lbDeleteStudent.Name = "lbDeleteStudent";
            this.lbDeleteStudent.Size = new System.Drawing.Size(118, 20);
            this.lbDeleteStudent.TabIndex = 171;
            this.lbDeleteStudent.Text = "DELETE STUDENT";
            this.lbDeleteStudent.Click += new System.EventHandler(this.pbDelete_Click);
            // 
            // lbAddStudents
            // 
            this.lbAddStudents.AutoSize = true;
            this.lbAddStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbAddStudents.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAddStudents.Location = new System.Drawing.Point(386, 480);
            this.lbAddStudents.Name = "lbAddStudents";
            this.lbAddStudents.Size = new System.Drawing.Size(104, 20);
            this.lbAddStudents.TabIndex = 169;
            this.lbAddStudents.Text = "ADD STUDENT";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(381, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(273, 20);
            this.label10.TabIndex = 167;
            this.label10.Text = "--------------------ACTIONS--------------------";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(426, 273);
            this.btnSave.Name = "btnSave";
            this.btnSave.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.OverrideDefault.Back.ColorAngle = 62F;
            this.btnSave.OverrideDefault.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.OverrideDefault.Border.ColorAngle = 62F;
            this.btnSave.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSave.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSave.OverrideDefault.Border.Rounding = 20;
            this.btnSave.OverrideDefault.Border.Width = 1;
            this.btnSave.Size = new System.Drawing.Size(170, 44);
            this.btnSave.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StateCommon.Back.ColorAngle = 62F;
            this.btnSave.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StateCommon.Border.ColorAngle = 62F;
            this.btnSave.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSave.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSave.StateCommon.Border.Rounding = 20;
            this.btnSave.StateCommon.Border.Width = 1;
            this.btnSave.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Black;
            this.btnSave.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.Black;
            this.btnSave.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StatePressed.Back.ColorAngle = 62F;
            this.btnSave.StatePressed.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StatePressed.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StatePressed.Border.ColorAngle = 62F;
            this.btnSave.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSave.StatePressed.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSave.StatePressed.Border.Rounding = 20;
            this.btnSave.StatePressed.Border.Width = 1;
            this.btnSave.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StateTracking.Back.ColorAngle = 62F;
            this.btnSave.StateTracking.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(197)))), ((int)(((byte)(252)))));
            this.btnSave.StateTracking.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(195)))), ((int)(((byte)(252)))));
            this.btnSave.StateTracking.Border.ColorAngle = 62F;
            this.btnSave.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSave.StateTracking.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnSave.StateTracking.Border.Rounding = 20;
            this.btnSave.StateTracking.Border.Width = 1;
            this.btnSave.TabIndex = 166;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbStudentGrade
            // 
            this.lbStudentGrade.AutoSize = true;
            this.lbStudentGrade.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStudentGrade.Location = new System.Drawing.Point(415, 127);
            this.lbStudentGrade.Name = "lbStudentGrade";
            this.lbStudentGrade.Size = new System.Drawing.Size(223, 20);
            this.lbStudentGrade.TabIndex = 165;
            this.lbStudentGrade.Text = "----------STUDENT GRADE----------";
            this.lbStudentGrade.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbStudentGrade.Visible = false;
            // 
            // txtAver
            // 
            this.txtAver.Enabled = false;
            this.txtAver.Location = new System.Drawing.Point(575, 222);
            this.txtAver.Name = "txtAver";
            this.txtAver.Size = new System.Drawing.Size(54, 33);
            this.txtAver.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtAver.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtAver.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtAver.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtAver.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtAver.StateCommon.Border.Rounding = 20;
            this.txtAver.StateCommon.Border.Width = 1;
            this.txtAver.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtAver.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAver.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtAver.TabIndex = 164;
            // 
            // lbMoyenne
            // 
            this.lbMoyenne.AutoSize = true;
            this.lbMoyenne.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMoyenne.Location = new System.Drawing.Point(566, 183);
            this.lbMoyenne.Name = "lbMoyenne";
            this.lbMoyenne.Size = new System.Drawing.Size(72, 18);
            this.lbMoyenne.TabIndex = 163;
            this.lbMoyenne.Text = "Average";
            this.lbMoyenne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFinal
            // 
            this.txtFinal.Location = new System.Drawing.Point(480, 222);
            this.txtFinal.Name = "txtFinal";
            this.txtFinal.Size = new System.Drawing.Size(54, 33);
            this.txtFinal.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtFinal.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtFinal.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtFinal.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtFinal.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtFinal.StateCommon.Border.Rounding = 20;
            this.txtFinal.StateCommon.Border.Width = 1;
            this.txtFinal.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtFinal.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFinal.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtFinal.TabIndex = 162;
            // 
            // lbNote2
            // 
            this.lbNote2.AutoSize = true;
            this.lbNote2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNote2.Location = new System.Drawing.Point(487, 165);
            this.lbNote2.Name = "lbNote2";
            this.lbNote2.Size = new System.Drawing.Size(42, 54);
            this.lbNote2.TabIndex = 161;
            this.lbNote2.Text = "Final\r\n|\r\nterm";
            this.lbNote2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMid
            // 
            this.txtMid.Location = new System.Drawing.Point(385, 222);
            this.txtMid.Name = "txtMid";
            this.txtMid.Size = new System.Drawing.Size(54, 33);
            this.txtMid.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtMid.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtMid.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.txtMid.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.txtMid.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.txtMid.StateCommon.Border.Rounding = 20;
            this.txtMid.StateCommon.Border.Width = 1;
            this.txtMid.StateCommon.Content.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMid.StateCommon.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMid.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtMid.TabIndex = 160;
            // 
            // lbNote1
            // 
            this.lbNote1.AutoSize = true;
            this.lbNote1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNote1.Location = new System.Drawing.Point(393, 165);
            this.lbNote1.Name = "lbNote1";
            this.lbNote1.Size = new System.Drawing.Size(41, 54);
            this.lbNote1.TabIndex = 159;
            this.lbNote1.Text = "Mid\r\n|\r\nterm";
            this.lbNote1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbNote1.Click += new System.EventHandler(this.label3_Click);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(528, 73);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(121, 20);
            this.lbName.TabIndex = 158;
            this.lbName.Text = "______________";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Name2
            // 
            this.Name2.AutoSize = true;
            this.Name2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name2.Location = new System.Drawing.Point(387, 73);
            this.Name2.Name = "Name2";
            this.Name2.Size = new System.Drawing.Size(57, 18);
            this.Name2.TabIndex = 157;
            this.Name2.Text = "Name:";
            this.Name2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMSSV
            // 
            this.lbMSSV.AutoSize = true;
            this.lbMSSV.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMSSV.Location = new System.Drawing.Point(528, 41);
            this.lbMSSV.Name = "lbMSSV";
            this.lbMSSV.Size = new System.Drawing.Size(121, 20);
            this.lbMSSV.TabIndex = 156;
            this.lbMSSV.Text = "______________";
            this.lbMSSV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbInfoStudent
            // 
            this.lbInfoStudent.AutoSize = true;
            this.lbInfoStudent.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoStudent.Location = new System.Drawing.Point(422, 10);
            this.lbInfoStudent.Name = "lbInfoStudent";
            this.lbInfoStudent.Size = new System.Drawing.Size(207, 20);
            this.lbInfoStudent.TabIndex = 155;
            this.lbInfoStudent.Text = "----------STUDENT INFO----------";
            this.lbInfoStudent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStudentID
            // 
            this.lbStudentID.AutoSize = true;
            this.lbStudentID.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStudentID.Location = new System.Drawing.Point(387, 43);
            this.lbStudentID.Name = "lbStudentID";
            this.lbStudentID.Size = new System.Drawing.Size(85, 18);
            this.lbStudentID.TabIndex = 154;
            this.lbStudentID.Text = "Student ID:";
            this.lbStudentID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // count
            // 
            this.count.AutoSize = true;
            this.count.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.count.Location = new System.Drawing.Point(20, 523);
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(32, 20);
            this.count.TabIndex = 153;
            this.count.Text = "0/0";
            this.count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbExport
            // 
            this.lbExport.AutoSize = true;
            this.lbExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbExport.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExport.Location = new System.Drawing.Point(270, 523);
            this.lbExport.Name = "lbExport";
            this.lbExport.Size = new System.Drawing.Size(54, 20);
            this.lbExport.TabIndex = 152;
            this.lbExport.Text = "Export";
            this.lbExport.Click += new System.EventHandler(this.pictureBox1_Click);
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
            this.kryptonPalette1.ButtonSpecs.FormMin.ImageStates.ImageTracking = ((System.Drawing.Image)(resources.GetObject("")));
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
            // lbStudentlist
            // 
            this.lbStudentlist.AutoSize = true;
            this.lbStudentlist.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStudentlist.Location = new System.Drawing.Point(91, 10);
            this.lbStudentlist.Name = "lbStudentlist";
            this.lbStudentlist.Size = new System.Drawing.Size(195, 20);
            this.lbStudentlist.TabIndex = 150;
            this.lbStudentlist.Text = "----------STUDENT LIST----------";
            this.lbStudentlist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AllowUserToOrderColumns = true;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Location = new System.Drawing.Point(20, 42);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.Size = new System.Drawing.Size(342, 458);
            this.dgvStudents.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.dgvStudents.StateCommon.Background.Color2 = System.Drawing.Color.White;
            this.dgvStudents.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvStudents.StateCommon.DataCell.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.dgvStudents.StateCommon.DataCell.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.dgvStudents.StateCommon.DataCell.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.DataCell.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.DataCell.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvStudents.StateCommon.DataCell.Border.Width = 1;
            this.dgvStudents.StateCommon.DataCell.Content.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.StateCommon.HeaderColumn.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderColumn.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderColumn.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderColumn.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderColumn.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvStudents.StateCommon.HeaderColumn.Border.Width = 1;
            this.dgvStudents.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.Color.White;
            this.dgvStudents.StateCommon.HeaderColumn.Content.Color2 = System.Drawing.Color.White;
            this.dgvStudents.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.StateCommon.HeaderRow.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderRow.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvStudents.StateCommon.HeaderRow.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.dgvStudents.StateCommon.HeaderRow.Border.Width = 1;
            this.dgvStudents.StateSelected.DataCell.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.dgvStudents.StateSelected.DataCell.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.dgvStudents.TabIndex = 149;
            this.dgvStudents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudents_CellContentClick);
            this.dgvStudents.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStudents_CellMouseClick);
            // 
            // pbDeleteStudent
            // 
            this.pbDeleteStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbDeleteStudent.Image = ((System.Drawing.Image)(resources.GetObject("pbDeleteStudent.Image")));
            this.pbDeleteStudent.Location = new System.Drawing.Point(545, 378);
            this.pbDeleteStudent.Name = "pbDeleteStudent";
            this.pbDeleteStudent.Size = new System.Drawing.Size(97, 110);
            this.pbDeleteStudent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDeleteStudent.TabIndex = 170;
            this.pbDeleteStudent.TabStop = false;
            this.pbDeleteStudent.Click += new System.EventHandler(this.pbDelete_Click);
            // 
            // pbStudents
            // 
            this.pbStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbStudents.Image = ((System.Drawing.Image)(resources.GetObject("pbStudents.Image")));
            this.pbStudents.Location = new System.Drawing.Point(393, 381);
            this.pbStudents.Name = "pbStudents";
            this.pbStudents.Size = new System.Drawing.Size(90, 96);
            this.pbStudents.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbStudents.TabIndex = 168;
            this.pbStudents.TabStop = false;
            this.pbStudents.Click += new System.EventHandler(this.pbStudents_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(323, 515);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 151;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // StudentsInClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(675, 559);
            this.Controls.Add(this.lbDeleteStudent);
            this.Controls.Add(this.pbDeleteStudent);
            this.Controls.Add(this.lbAddStudents);
            this.Controls.Add(this.pbStudents);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbStudentGrade);
            this.Controls.Add(this.txtAver);
            this.Controls.Add(this.lbMoyenne);
            this.Controls.Add(this.txtFinal);
            this.Controls.Add(this.lbNote2);
            this.Controls.Add(this.txtMid);
            this.Controls.Add(this.lbNote1);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.Name2);
            this.Controls.Add(this.lbMSSV);
            this.Controls.Add(this.lbInfoStudent);
            this.Controls.Add(this.lbStudentID);
            this.Controls.Add(this.count);
            this.Controls.Add(this.lbExport);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbStudentlist);
            this.Controls.Add(this.dgvStudents);
            this.Name = "StudentsInClass";
            this.Palette = this.kryptonPalette1;
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STUDENTS";
            this.Load += new System.EventHandler(this.StudensInClass_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteStudent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStudents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDeleteStudent;
        private System.Windows.Forms.PictureBox pbDeleteStudent;
        private System.Windows.Forms.Label lbAddStudents;
        private System.Windows.Forms.PictureBox pbStudents;
        private System.Windows.Forms.Label label10;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private System.Windows.Forms.Label lbStudentGrade;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAver;
        private System.Windows.Forms.Label lbMoyenne;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFinal;
        private System.Windows.Forms.Label lbNote2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMid;
        private System.Windows.Forms.Label lbNote1;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label Name2;
        private System.Windows.Forms.Label lbMSSV;
        private System.Windows.Forms.Label lbInfoStudent;
        private System.Windows.Forms.Label lbStudentID;
        private System.Windows.Forms.Label count;
        private System.Windows.Forms.Label lbExport;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private System.Windows.Forms.Label lbStudentlist;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvStudents;
    }
}
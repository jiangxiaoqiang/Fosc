namespace Fosc.Dolphin.UI.Dev
{
    partial class FrmMain
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Encrypt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_CodeGenerator = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Sqlite = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(986, 24);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(51, 20);
            this.toolStripMenuItemFile.Text = "File(&F)";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.openToolStripMenuItem.Text = "Open(&O)";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.exitToolStripMenuItem.Text = "Exit(&E)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Encrypt,
            this.toolStripMenuItem_CodeGenerator,
            this.toolStripMenuItem_Sqlite,
            this.clearToolStripMenuItem,
            this.testTToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(76, 20);
            this.toolStripMenuItem2.Text = "Widget(&W)";
            // 
            // toolStripMenuItem_Encrypt
            // 
            this.toolStripMenuItem_Encrypt.Name = "toolStripMenuItem_Encrypt";
            this.toolStripMenuItem_Encrypt.Size = new System.Drawing.Size(170, 22);
            this.toolStripMenuItem_Encrypt.Text = "Encrypt(&E)";
            this.toolStripMenuItem_Encrypt.Click += new System.EventHandler(this.toolStripMenuItem_Encrypt_Click);
            // 
            // toolStripMenuItem_CodeGenerator
            // 
            this.toolStripMenuItem_CodeGenerator.Name = "toolStripMenuItem_CodeGenerator";
            this.toolStripMenuItem_CodeGenerator.Size = new System.Drawing.Size(170, 22);
            this.toolStripMenuItem_CodeGenerator.Text = "CodeGenerator(&G)";
            this.toolStripMenuItem_CodeGenerator.Click += new System.EventHandler(this.toolStripMenuItem_CodeGenerator_Click);
            // 
            // toolStripMenuItem_Sqlite
            // 
            this.toolStripMenuItem_Sqlite.Name = "toolStripMenuItem_Sqlite";
            this.toolStripMenuItem_Sqlite.Size = new System.Drawing.Size(170, 22);
            this.toolStripMenuItem_Sqlite.Text = "Sqlite(&S)";
            this.toolStripMenuItem_Sqlite.Click += new System.EventHandler(this.toolStripMenuItem_Sqlite_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.clearToolStripMenuItem.Text = "Clear(&C)";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // testTToolStripMenuItem
            // 
            this.testTToolStripMenuItem.Name = "testTToolStripMenuItem";
            this.testTToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.testTToolStripMenuItem.Text = "Test(&T)";
            this.testTToolStripMenuItem.Click += new System.EventHandler(this.testTToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(63, 20);
            this.toolStripMenuItem1.Text = "Tools(&T)";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.optionsToolStripMenuItem.Text = "Options(&O)";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.documentToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.helpToolStripMenuItem.Text = "Help(&H)";
            // 
            // documentToolStripMenuItem
            // 
            this.documentToolStripMenuItem.Name = "documentToolStripMenuItem";
            this.documentToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.documentToolStripMenuItem.Text = "Document(&D)";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 533);
            this.Controls.Add(this.menuStripMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FrmMain";
            this.Text = "Dolphin";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_CodeGenerator;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Encrypt;
        private System.Windows.Forms.ToolStripMenuItem testTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Sqlite;
    }
}


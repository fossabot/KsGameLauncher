﻿namespace KonaStaGameLauncher
{
    partial class AboutForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label_Application = new System.Windows.Forms.Label();
            this.button_Ok = new System.Windows.Forms.Button();
            this.label_Version = new System.Windows.Forms.Label();
            this.label_Authors = new System.Windows.Forms.Label();
            this.linkLabel_Support = new System.Windows.Forms.LinkLabel();
            this.label_SpecialThanks = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_Application
            // 
            resources.ApplyResources(this.label_Application, "label_Application");
            this.label_Application.Name = "label_Application";
            // 
            // button_Ok
            // 
            resources.ApplyResources(this.button_Ok, "button_Ok");
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // label_Version
            // 
            resources.ApplyResources(this.label_Version, "label_Version");
            this.label_Version.Name = "label_Version";
            // 
            // label_Authors
            // 
            resources.ApplyResources(this.label_Authors, "label_Authors");
            this.label_Authors.Name = "label_Authors";
            // 
            // linkLabel_Support
            // 
            resources.ApplyResources(this.linkLabel_Support, "linkLabel_Support");
            this.linkLabel_Support.Name = "linkLabel_Support";
            this.linkLabel_Support.TabStop = true;
            // 
            // label_SpecialThanks
            // 
            resources.ApplyResources(this.label_SpecialThanks, "label_SpecialThanks");
            this.label_SpecialThanks.Name = "label_SpecialThanks";
            // 
            // textBox1
            // 
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.TabStop = false;
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label_SpecialThanks);
            this.Controls.Add(this.linkLabel_Support);
            this.Controls.Add(this.label_Authors);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.label_Application);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Application;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Label label_Authors;
        private System.Windows.Forms.LinkLabel linkLabel_Support;
        private System.Windows.Forms.Label label_SpecialThanks;
        private System.Windows.Forms.TextBox textBox1;
    }
}
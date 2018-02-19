using Microsoft.Win32;
using System;
using System.Threading;
namespace Backup
{
    partial class Form1
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
            if (disposing)
            {
                WriteRegistry();
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
            this.folderBrowserDialogSource = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonSourceSelect = new System.Windows.Forms.Button();
            this.buttonTargetSelect = new System.Windows.Forms.Button();
            this.folderBrowserDialogTarget = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.RichTextBox();
            this.textBoxTargetDir = new System.Windows.Forms.TextBox();
            this.listBoxSourceDir = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSourceRemove = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialogSource
            // 
            this.folderBrowserDialogSource.SelectedPath = "5";
            this.folderBrowserDialogSource.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // buttonSourceSelect
            // 
            this.buttonSourceSelect.Location = new System.Drawing.Point(782, 94);
            this.buttonSourceSelect.Name = "buttonSourceSelect";
            this.buttonSourceSelect.Size = new System.Drawing.Size(164, 23);
            this.buttonSourceSelect.TabIndex = 0;
            this.buttonSourceSelect.Text = "Hinzufügen";
            this.buttonSourceSelect.UseVisualStyleBackColor = true;
            this.buttonSourceSelect.Click += new System.EventHandler(this.buttonSourceSelect_Click);
            // 
            // buttonTargetSelect
            // 
            this.buttonTargetSelect.Location = new System.Drawing.Point(979, 186);
            this.buttonTargetSelect.Name = "buttonTargetSelect";
            this.buttonTargetSelect.Size = new System.Drawing.Size(164, 23);
            this.buttonTargetSelect.TabIndex = 1;
            this.buttonTargetSelect.Text = "Auswahl";
            this.buttonTargetSelect.UseVisualStyleBackColor = true;
            this.buttonTargetSelect.Click += new System.EventHandler(this.buttonTargetSelect_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(992, 580);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(164, 48);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Beenden";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(822, 230);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(164, 55);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "Start >>";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(28, 341);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(1115, 218);
            this.textBoxOutput.TabIndex = 8;
            this.textBoxOutput.Text = "";
            // 
            // textBoxTargetDir
            // 
            this.textBoxTargetDir.Location = new System.Drawing.Point(28, 160);
            this.textBoxTargetDir.Name = "textBoxTargetDir";
            this.textBoxTargetDir.Size = new System.Drawing.Size(1115, 20);
            this.textBoxTargetDir.TabIndex = 3;
            // 
            // listBoxSourceDir
            // 
            this.listBoxSourceDir.FormattingEnabled = true;
            this.listBoxSourceDir.Location = new System.Drawing.Point(16, 19);
            this.listBoxSourceDir.Name = "listBoxSourceDir";
            this.listBoxSourceDir.Size = new System.Drawing.Size(1115, 69);
            this.listBoxSourceDir.TabIndex = 9;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(992, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(164, 55);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSourceRemove);
            this.groupBox1.Controls.Add(this.buttonSourceSelect);
            this.groupBox1.Controls.Add(this.listBoxSourceDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1144, 124);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zu kopierende Verzeichnisse:";
            // 
            // buttonSourceRemove
            // 
            this.buttonSourceRemove.Location = new System.Drawing.Point(967, 94);
            this.buttonSourceRemove.Name = "buttonSourceRemove";
            this.buttonSourceRemove.Size = new System.Drawing.Size(163, 23);
            this.buttonSourceRemove.TabIndex = 10;
            this.buttonSourceRemove.Text = "Entfernen";
            this.buttonSourceRemove.UseVisualStyleBackColor = true;
            this.buttonSourceRemove.Click += new System.EventHandler(this.buttonSourceRemove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonClear);
            this.groupBox2.Location = new System.Drawing.Point(12, 296);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1144, 278);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meldungen";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(967, 16);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(163, 23);
            this.buttonClear.TabIndex = 0;
            this.buttonClear.Text = "Meldungen Löschen";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(12, 136);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1144, 79);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Kopieren nach:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 639);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxTargetDir);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonTargetSelect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "Speedy File Backup";
            this.MaximizeBox = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSource;
        private System.Windows.Forms.Button buttonSourceSelect;
        private System.Windows.Forms.Button buttonTargetSelect;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogTarget;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.RichTextBox textBoxOutput;
        private System.Windows.Forms.TextBox textBoxTargetDir;
        private System.Windows.Forms.ListBox listBoxSourceDir;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSourceRemove;

    }
}


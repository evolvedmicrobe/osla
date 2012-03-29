using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMlrCtls;
using System.Resources;
using Microsoft.VisualBasic.Compatibility;
using MlrServ;
using System.Threading;
using System.IO;

namespace VictorRemoteServer
{
    partial class VictorForm : Form
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
                InternalDispose(disposing);
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VictorForm));
            this.tabProtView = new System.Windows.Forms.TabControl();
            this.tabBackup = new System.Windows.Forms.TabPage();
            this.ProtocolTree = new AxMlrCtls.AxProtocolTree();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProtInfo = new System.Windows.Forms.TextBox();
            this.lblErrors = new System.Windows.Forms.Label();
            this.txtErrors = new System.Windows.Forms.RichTextBox();
            this.btnDisplayProt = new System.Windows.Forms.Button();
            this.tabCenter = new System.Windows.Forms.TabPage();
            this.btnRefreshProtocols = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.tabProtView.SuspendLayout();
            this.tabBackup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProtocolTree)).BeginInit();
            this.tabCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabProtView
            // 
            this.tabProtView.Controls.Add(this.tabBackup);
            this.tabProtView.Controls.Add(this.tabCenter);
            this.tabProtView.Location = new System.Drawing.Point(12, 12);
            this.tabProtView.Name = "tabProtView";
            this.tabProtView.SelectedIndex = 0;
            this.tabProtView.Size = new System.Drawing.Size(786, 498);
            this.tabProtView.TabIndex = 11;
            this.tabProtView.TabStop = false;
            // 
            // tabBackup
            // 
            this.tabBackup.Controls.Add(this.button1);
            this.tabBackup.Controls.Add(this.ProtocolTree);
            this.tabBackup.Controls.Add(this.label2);
            this.tabBackup.Controls.Add(this.lblStatus);
            this.tabBackup.Controls.Add(this.label1);
            this.tabBackup.Controls.Add(this.txtProtInfo);
            this.tabBackup.Controls.Add(this.lblErrors);
            this.tabBackup.Controls.Add(this.txtErrors);
            this.tabBackup.Controls.Add(this.btnDisplayProt);
            this.tabBackup.Location = new System.Drawing.Point(4, 22);
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Padding = new System.Windows.Forms.Padding(3);
            this.tabBackup.Size = new System.Drawing.Size(778, 472);
            this.tabBackup.TabIndex = 1;
            this.tabBackup.Text = "Main Page";
            this.tabBackup.UseVisualStyleBackColor = true;
            this.tabBackup.Click += new System.EventHandler(this.tabBackup_Click);
            // 
            // ProtocolTree
            // 
            this.ProtocolTree.Location = new System.Drawing.Point(215, 21);
            this.ProtocolTree.Name = "ProtocolTree";
            this.ProtocolTree.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ProtocolTree.OcxState")));
            this.ProtocolTree.Size = new System.Drawing.Size(259, 392);
            this.ProtocolTree.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 125);
            this.label2.TabIndex = 17;
            this.label2.Text = "THIS PROGRAM\r\nRUNS THE PLATE\r\nREADER FOR \r\nCLARITY NEVER \r\nCLOSE IT";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(33, 415);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(134, 25);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Instrument Is: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, -11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Available Protocols";
            // 
            // txtProtInfo
            // 
            this.txtProtInfo.Location = new System.Drawing.Point(19, 50);
            this.txtProtInfo.Name = "txtProtInfo";
            this.txtProtInfo.Size = new System.Drawing.Size(100, 20);
            this.txtProtInfo.TabIndex = 15;
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(437, -11);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(65, 13);
            this.lblErrors.TabIndex = 14;
            this.lblErrors.Text = "List of Errors";
            // 
            // txtErrors
            // 
            this.txtErrors.Location = new System.Drawing.Point(495, 9);
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.Size = new System.Drawing.Size(222, 389);
            this.txtErrors.TabIndex = 13;
            this.txtErrors.Text = "";
            // 
            // btnDisplayProt
            // 
            this.btnDisplayProt.Location = new System.Drawing.Point(19, 21);
            this.btnDisplayProt.Name = "btnDisplayProt";
            this.btnDisplayProt.Size = new System.Drawing.Size(135, 23);
            this.btnDisplayProt.TabIndex = 12;
            this.btnDisplayProt.Text = "View Protocol Information";
            this.btnDisplayProt.Click += new System.EventHandler(this.btnDisplayProt_Click);
            // 
            // tabCenter
            // 
            this.tabCenter.Controls.Add(this.btnRefreshProtocols);
            this.tabCenter.Controls.Add(this.dataGridView1);
            this.tabCenter.Location = new System.Drawing.Point(4, 22);
            this.tabCenter.Name = "tabCenter";
            this.tabCenter.Padding = new System.Windows.Forms.Padding(3);
            this.tabCenter.Size = new System.Drawing.Size(778, 472);
            this.tabCenter.TabIndex = 0;
            this.tabCenter.Text = "View Protocol IDs";
            this.tabCenter.UseVisualStyleBackColor = true;
            // 
            // btnRefreshProtocols
            // 
            this.btnRefreshProtocols.Location = new System.Drawing.Point(16, 64);
            this.btnRefreshProtocols.Name = "btnRefreshProtocols";
            this.btnRefreshProtocols.Size = new System.Drawing.Size(156, 30);
            this.btnRefreshProtocols.TabIndex = 1;
            this.btnRefreshProtocols.Text = "View Protocol IDs";
            this.btnRefreshProtocols.UseVisualStyleBackColor = true;
            this.btnRefreshProtocols.Click += new System.EventHandler(this.btnRefreshProtocols_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(191, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(555, 393);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(360, 440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Test Plate Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // VictorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 529);
            this.ControlBox = false;
            this.Controls.Add(this.tabProtView);
            this.Name = "VictorForm";
            this.Text = "Victor Instrument Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabProtView.ResumeLayout(false);
            this.tabBackup.ResumeLayout(false);
            this.tabBackup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProtocolTree)).EndInit();
            this.tabCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabProtView;
        private TabPage tabBackup;
        private Label label2;
        private Label lblStatus;
        private Label label1;
        private TextBox txtProtInfo;
        private Label lblErrors;
        private RichTextBox txtErrors;
        private Button btnDisplayProt;
        private TabPage tabCenter;
        private Button btnRefreshProtocols;
        private DataGridView dataGridView1;
        private AxProtocolTree ProtocolTree;
        private Button button1;

    }
}


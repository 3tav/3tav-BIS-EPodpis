﻿namespace mSignAgent
{
    partial class Form2
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
            this.btnPripraviPodatke = new System.Windows.Forms.Button();
            this.inputID = new System.Windows.Forms.TextBox();
            this.btnPosljiVPodpis = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputVrstaObrazca = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.inputTemplate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputProcedura = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerirajPDF = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.inputFileName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.inputCMD = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCMD = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.inputFilePath = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnPosljiVPodpisFile = new System.Windows.Forms.Button();
            this.btnTestEmail1 = new System.Windows.Forms.Button();
            this.btnTestEmail2 = new System.Windows.Forms.Button();
            this.btnSignPro = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPripraviPodatke
            // 
            this.btnPripraviPodatke.Location = new System.Drawing.Point(267, 22);
            this.btnPripraviPodatke.Name = "btnPripraviPodatke";
            this.btnPripraviPodatke.Size = new System.Drawing.Size(142, 23);
            this.btnPripraviPodatke.TabIndex = 0;
            this.btnPripraviPodatke.Text = "Pripravi podatke";
            this.btnPripraviPodatke.UseVisualStyleBackColor = true;
            this.btnPripraviPodatke.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputID
            // 
            this.inputID.Location = new System.Drawing.Point(80, 24);
            this.inputID.Name = "inputID";
            this.inputID.Size = new System.Drawing.Size(121, 20);
            this.inputID.TabIndex = 1;
            this.inputID.Text = "5549";
            // 
            // btnPosljiVPodpis
            // 
            this.btnPosljiVPodpis.Location = new System.Drawing.Point(267, 153);
            this.btnPosljiVPodpis.Name = "btnPosljiVPodpis";
            this.btnPosljiVPodpis.Size = new System.Drawing.Size(121, 23);
            this.btnPosljiVPodpis.TabIndex = 2;
            this.btnPosljiVPodpis.Text = "Pošlji v podpis";
            this.btnPosljiVPodpis.UseVisualStyleBackColor = true;
            this.btnPosljiVPodpis.Click += new System.EventHandler(this.btnPosljiVPodpis_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(424, 138);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Osveži status";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(551, 138);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Prenesi podpisano";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ID dok:";
            // 
            // inputVrstaObrazca
            // 
            this.inputVrstaObrazca.FormattingEnabled = true;
            this.inputVrstaObrazca.Location = new System.Drawing.Point(80, 61);
            this.inputVrstaObrazca.Name = "inputVrstaObrazca";
            this.inputVrstaObrazca.Size = new System.Drawing.Size(121, 21);
            this.inputVrstaObrazca.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Obrazec:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(77, 177);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(80, 92);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(110, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Podpis na daljavo";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(80, 247);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(597, 278);
            this.dataGridView1.TabIndex = 10;
            // 
            // inputTemplate
            // 
            this.inputTemplate.Location = new System.Drawing.Point(267, 61);
            this.inputTemplate.Name = "inputTemplate";
            this.inputTemplate.Size = new System.Drawing.Size(405, 20);
            this.inputTemplate.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Template:";
            // 
            // inputProcedura
            // 
            this.inputProcedura.Location = new System.Drawing.Point(267, 89);
            this.inputProcedura.Name = "inputProcedura";
            this.inputProcedura.Size = new System.Drawing.Size(142, 20);
            this.inputProcedura.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Procedura:";
            // 
            // btnGenerirajPDF
            // 
            this.btnGenerirajPDF.Location = new System.Drawing.Point(80, 138);
            this.btnGenerirajPDF.Name = "btnGenerirajPDF";
            this.btnGenerirajPDF.Size = new System.Drawing.Size(121, 23);
            this.btnGenerirajPDF.TabIndex = 15;
            this.btnGenerirajPDF.Text = "Generiraj PDF";
            this.btnGenerirajPDF.UseVisualStyleBackColor = true;
            this.btnGenerirajPDF.Click += new System.EventHandler(this.btnGenerirajPDF_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(511, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(161, 20);
            this.textBox1.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(441, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Output DIR:";
            // 
            // inputFileName
            // 
            this.inputFileName.Location = new System.Drawing.Point(511, 87);
            this.inputFileName.Name = "inputFileName";
            this.inputFileName.Size = new System.Drawing.Size(161, 20);
            this.inputFileName.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(450, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "File name:";
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(267, 124);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(121, 23);
            this.btnPreview.TabIndex = 20;
            this.btnPreview.Text = "Predogled";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // inputCMD
            // 
            this.inputCMD.Location = new System.Drawing.Point(424, 176);
            this.inputCMD.Name = "inputCMD";
            this.inputCMD.Size = new System.Drawing.Size(248, 20);
            this.inputCMD.TabIndex = 21;
            this.inputCMD.Text = "\"PDF-CREATE\" \"738525;true;-28\"";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(384, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "CMD:";
            // 
            // btnCMD
            // 
            this.btnCMD.Location = new System.Drawing.Point(678, 174);
            this.btnCMD.Name = "btnCMD";
            this.btnCMD.Size = new System.Drawing.Size(75, 23);
            this.btnCMD.TabIndex = 23;
            this.btnCMD.Text = "EXEC";
            this.btnCMD.UseVisualStyleBackColor = true;
            this.btnCMD.Click += new System.EventHandler(this.btnCMD_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(511, 113);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 17);
            this.checkBox2.TabIndex = 24;
            this.checkBox2.Text = "Omogoči urejanje";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // inputFilePath
            // 
            this.inputFilePath.Location = new System.Drawing.Point(147, 221);
            this.inputFilePath.Name = "inputFilePath";
            this.inputFilePath.Size = new System.Drawing.Size(419, 20);
            this.inputFilePath.TabIndex = 25;
            this.inputFilePath.Text = "ece-903200-predogled.pdf";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(77, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Custom File:";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(572, 221);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(24, 20);
            this.btnChooseFile.TabIndex = 27;
            this.btnChooseFile.Text = "...";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // btnPosljiVPodpisFile
            // 
            this.btnPosljiVPodpisFile.Location = new System.Drawing.Point(602, 220);
            this.btnPosljiVPodpisFile.Name = "btnPosljiVPodpisFile";
            this.btnPosljiVPodpisFile.Size = new System.Drawing.Size(75, 23);
            this.btnPosljiVPodpisFile.TabIndex = 28;
            this.btnPosljiVPodpisFile.Text = "V podpis";
            this.btnPosljiVPodpisFile.UseVisualStyleBackColor = true;
            this.btnPosljiVPodpisFile.Click += new System.EventHandler(this.btnPosljiVPodpisFile_Click);
            // 
            // btnTestEmail1
            // 
            this.btnTestEmail1.Location = new System.Drawing.Point(147, 192);
            this.btnTestEmail1.Name = "btnTestEmail1";
            this.btnTestEmail1.Size = new System.Drawing.Size(84, 23);
            this.btnTestEmail1.TabIndex = 29;
            this.btnTestEmail1.Text = "Test Mail 1";
            this.btnTestEmail1.UseVisualStyleBackColor = true;
            this.btnTestEmail1.Click += new System.EventHandler(this.btnTestEmail1_Click);
            // 
            // btnTestEmail2
            // 
            this.btnTestEmail2.Location = new System.Drawing.Point(237, 192);
            this.btnTestEmail2.Name = "btnTestEmail2";
            this.btnTestEmail2.Size = new System.Drawing.Size(84, 23);
            this.btnTestEmail2.TabIndex = 30;
            this.btnTestEmail2.Text = "Test Mail 2";
            this.btnTestEmail2.UseVisualStyleBackColor = true;
            this.btnTestEmail2.Click += new System.EventHandler(this.btnTestEmail2_Click);
            // 
            // btnSignPro
            // 
            this.btnSignPro.Location = new System.Drawing.Point(683, 219);
            this.btnSignPro.Name = "btnSignPro";
            this.btnSignPro.Size = new System.Drawing.Size(75, 23);
            this.btnSignPro.TabIndex = 31;
            this.btnSignPro.Text = "SignPro";
            this.btnSignPro.UseVisualStyleBackColor = true;
            this.btnSignPro.Click += new System.EventHandler(this.btnSignPro_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(678, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "Prenos Custom";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 537);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSignPro);
            this.Controls.Add(this.btnTestEmail2);
            this.Controls.Add(this.btnTestEmail1);
            this.Controls.Add(this.btnPosljiVPodpisFile);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.inputFilePath);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.btnCMD);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.inputCMD);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.inputFileName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnGenerirajPDF);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputProcedura);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputTemplate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputVrstaObrazca);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnPosljiVPodpis);
            this.Controls.Add(this.inputID);
            this.Controls.Add(this.btnPripraviPodatke);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPripraviPodatke;
        private System.Windows.Forms.TextBox inputID;
        private System.Windows.Forms.Button btnPosljiVPodpis;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox inputVrstaObrazca;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox inputTemplate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputProcedura;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGenerirajPDF;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox inputFileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.TextBox inputCMD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCMD;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox inputFilePath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Button btnPosljiVPodpisFile;
        private System.Windows.Forms.Button btnTestEmail1;
        private System.Windows.Forms.Button btnTestEmail2;
        private System.Windows.Forms.Button btnSignPro;
        private System.Windows.Forms.Button button1;
    }
}
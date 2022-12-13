namespace mSignTest
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetDocument = new System.Windows.Forms.Button();
            this.inputStatus = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.inputFilePath = new System.Windows.Forms.TextBox();
            this.inputEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inputGSM = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputDocID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnPosljiVPodpis = new System.Windows.Forms.Button();
            this.btnGetSignedPDF = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnZahteve = new System.Windows.Forms.Button();
            this.btnNovaZahteva = new System.Windows.Forms.Button();
            this.btnZahtevaStorno = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.inputRectX = new System.Windows.Forms.TextBox();
            this.inputRectY = new System.Windows.Forms.TextBox();
            this.inputRectUX = new System.Windows.Forms.TextBox();
            this.inputRectUY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox1.Location = new System.Drawing.Point(12, 310);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1006, 287);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(51, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test mSign";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetDocument
            // 
            this.btnGetDocument.Location = new System.Drawing.Point(219, 33);
            this.btnGetDocument.Name = "btnGetDocument";
            this.btnGetDocument.Size = new System.Drawing.Size(94, 23);
            this.btnGetDocument.TabIndex = 2;
            this.btnGetDocument.Text = "GetDocuments";
            this.btnGetDocument.UseVisualStyleBackColor = true;
            this.btnGetDocument.Click += new System.EventHandler(this.btnGetDocument_Click);
            // 
            // inputStatus
            // 
            this.inputStatus.Location = new System.Drawing.Point(152, 33);
            this.inputStatus.Name = "inputStatus";
            this.inputStatus.Size = new System.Drawing.Size(61, 20);
            this.inputStatus.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(319, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "GetDocument";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(492, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "CreateDocument";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(383, 101);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(103, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "CreateSharedDocument";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // inputFilePath
            // 
            this.inputFilePath.Location = new System.Drawing.Point(51, 65);
            this.inputFilePath.Name = "inputFilePath";
            this.inputFilePath.Size = new System.Drawing.Size(435, 20);
            this.inputFilePath.TabIndex = 7;
            this.inputFilePath.Text = "C:\\temp\\plmb\\plmb_pogodba_field.pdf";
            // 
            // inputEmail
            // 
            this.inputEmail.Location = new System.Drawing.Point(51, 101);
            this.inputEmail.Name = "inputEmail";
            this.inputEmail.Size = new System.Drawing.Size(216, 20);
            this.inputEmail.TabIndex = 8;
            this.inputEmail.Text = "simon@3tav.si";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Email:";
            // 
            // inputGSM
            // 
            this.inputGSM.Location = new System.Drawing.Point(51, 127);
            this.inputGSM.Name = "inputGSM";
            this.inputGSM.Size = new System.Drawing.Size(216, 20);
            this.inputGSM.TabIndex = 11;
            this.inputGSM.Text = "+38631402559";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "GSM:";
            // 
            // inputDocID
            // 
            this.inputDocID.Location = new System.Drawing.Point(318, 103);
            this.inputDocID.Name = "inputDocID";
            this.inputDocID.Size = new System.Drawing.Size(59, 20);
            this.inputDocID.TabIndex = 13;
            this.inputDocID.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(291, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "ID:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(943, 177);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "Parse";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(419, 33);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "Prenesi PDF";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnPosljiVPodpis
            // 
            this.btnPosljiVPodpis.Location = new System.Drawing.Point(51, 177);
            this.btnPosljiVPodpis.Name = "btnPosljiVPodpis";
            this.btnPosljiVPodpis.Size = new System.Drawing.Size(90, 23);
            this.btnPosljiVPodpis.TabIndex = 17;
            this.btnPosljiVPodpis.Text = "Pošlji v podpis";
            this.btnPosljiVPodpis.UseVisualStyleBackColor = true;
            this.btnPosljiVPodpis.Click += new System.EventHandler(this.btnPosljiVPodpis_Click);
            // 
            // btnGetSignedPDF
            // 
            this.btnGetSignedPDF.Location = new System.Drawing.Point(195, 177);
            this.btnGetSignedPDF.Name = "btnGetSignedPDF";
            this.btnGetSignedPDF.Size = new System.Drawing.Size(117, 23);
            this.btnGetSignedPDF.TabIndex = 18;
            this.btnGetSignedPDF.Text = "Prenesi podpisan dok";
            this.btnGetSignedPDF.UseVisualStyleBackColor = true;
            this.btnGetSignedPDF.Click += new System.EventHandler(this.btnGetSignedPDF_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 206);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1006, 98);
            this.dataGridView1.TabIndex = 19;
            // 
            // btnZahteve
            // 
            this.btnZahteve.Location = new System.Drawing.Point(596, 177);
            this.btnZahteve.Name = "btnZahteve";
            this.btnZahteve.Size = new System.Drawing.Size(111, 23);
            this.btnZahteve.TabIndex = 20;
            this.btnZahteve.Text = "Zahteve za podpis";
            this.btnZahteve.UseVisualStyleBackColor = true;
            this.btnZahteve.Click += new System.EventHandler(this.btnZahteve_Click);
            // 
            // btnNovaZahteva
            // 
            this.btnNovaZahteva.Location = new System.Drawing.Point(456, 177);
            this.btnNovaZahteva.Name = "btnNovaZahteva";
            this.btnNovaZahteva.Size = new System.Drawing.Size(111, 23);
            this.btnNovaZahteva.TabIndex = 21;
            this.btnNovaZahteva.Text = "Nova zahteva";
            this.btnNovaZahteva.UseVisualStyleBackColor = true;
            this.btnNovaZahteva.Click += new System.EventHandler(this.btnNovaZahteva_Click);
            // 
            // btnZahtevaStorno
            // 
            this.btnZahtevaStorno.Location = new System.Drawing.Point(730, 177);
            this.btnZahtevaStorno.Name = "btnZahtevaStorno";
            this.btnZahtevaStorno.Size = new System.Drawing.Size(111, 23);
            this.btnZahtevaStorno.TabIndex = 22;
            this.btnZahtevaStorno.Text = "Storno";
            this.btnZahtevaStorno.UseVisualStyleBackColor = true;
            this.btnZahtevaStorno.Click += new System.EventHandler(this.btnZahtevaStorno_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = global::mSignTest.Properties.Resources.logo_small;
            this.pictureBox1.Location = new System.Drawing.Point(787, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(231, 70);
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackgroundImage = global::mSignTest.Properties.Resources._3tav_logo;
            this.pictureBox2.Location = new System.Drawing.Point(777, 84);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(241, 50);
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(700, 38);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(75, 23);
            this.btnAddTag.TabIndex = 25;
            this.btnAddTag.Text = "Dodaj TAG";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // inputRectX
            // 
            this.inputRectX.Location = new System.Drawing.Point(620, 12);
            this.inputRectX.Name = "inputRectX";
            this.inputRectX.Size = new System.Drawing.Size(74, 20);
            this.inputRectX.TabIndex = 26;
            this.inputRectX.Text = "330";
            // 
            // inputRectY
            // 
            this.inputRectY.Location = new System.Drawing.Point(620, 38);
            this.inputRectY.Name = "inputRectY";
            this.inputRectY.Size = new System.Drawing.Size(74, 20);
            this.inputRectY.TabIndex = 27;
            this.inputRectY.Text = "300";
            // 
            // inputRectUX
            // 
            this.inputRectUX.Location = new System.Drawing.Point(620, 64);
            this.inputRectUX.Name = "inputRectUX";
            this.inputRectUX.Size = new System.Drawing.Size(74, 20);
            this.inputRectUX.TabIndex = 28;
            this.inputRectUX.Text = "570";
            // 
            // inputRectUY
            // 
            this.inputRectUY.Location = new System.Drawing.Point(620, 90);
            this.inputRectUY.Name = "inputRectUY";
            this.inputRectUY.Size = new System.Drawing.Size(74, 20);
            this.inputRectUY.TabIndex = 29;
            this.inputRectUY.Text = "350";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(570, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Lower X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(568, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Lower Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(602, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "uy";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(596, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "ux";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 609);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.inputRectUY);
            this.Controls.Add(this.inputRectUX);
            this.Controls.Add(this.inputRectY);
            this.Controls.Add(this.inputRectX);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnZahtevaStorno);
            this.Controls.Add(this.btnNovaZahteva);
            this.Controls.Add(this.btnZahteve);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnGetSignedPDF);
            this.Controls.Add(this.btnPosljiVPodpis);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputDocID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputGSM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputEmail);
            this.Controls.Add(this.inputFilePath);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.inputStatus);
            this.Controls.Add(this.btnGetDocument);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "MSign Test";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetDocument;
        private System.Windows.Forms.TextBox inputStatus;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox inputFilePath;
        private System.Windows.Forms.TextBox inputEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox inputGSM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputDocID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnPosljiVPodpis;
        private System.Windows.Forms.Button btnGetSignedPDF;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnZahteve;
        private System.Windows.Forms.Button btnNovaZahteva;
        private System.Windows.Forms.Button btnZahtevaStorno;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.TextBox inputRectX;
        private System.Windows.Forms.TextBox inputRectY;
        private System.Windows.Forms.TextBox inputRectUX;
        private System.Windows.Forms.TextBox inputRectUY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}


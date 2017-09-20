namespace Transfer
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
            this.bt_InputPathFind = new System.Windows.Forms.Button();
            this.txtCurInput = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.list_InputPaths = new System.Windows.Forms.ListBox();
            this.bt_ToJson_Localizing = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.bt_InputFolder = new System.Windows.Forms.Button();
            this.bt_InputExcel = new System.Windows.Forms.Button();
            this.comb_Sheets = new System.Windows.Forms.ComboBox();
            this.txtCurOutput = new System.Windows.Forms.TextBox();
            this.bt_OutputPathFind = new System.Windows.Forms.Button();
            this.list_OutputPaths = new System.Windows.Forms.ListBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_InputPathFind
            // 
            this.bt_InputPathFind.Location = new System.Drawing.Point(545, 12);
            this.bt_InputPathFind.Name = "bt_InputPathFind";
            this.bt_InputPathFind.Size = new System.Drawing.Size(76, 21);
            this.bt_InputPathFind.TabIndex = 4;
            this.bt_InputPathFind.Text = "...";
            this.bt_InputPathFind.UseVisualStyleBackColor = true;
            this.bt_InputPathFind.Click += new System.EventHandler(this.Click_InputPathFind);
            // 
            // txtCurInput
            // 
            this.txtCurInput.Location = new System.Drawing.Point(12, 12);
            this.txtCurInput.Name = "txtCurInput";
            this.txtCurInput.ReadOnly = true;
            this.txtCurInput.Size = new System.Drawing.Size(527, 21);
            this.txtCurInput.TabIndex = 3;
            this.txtCurInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(13, 43);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 14);
            this.textBox5.TabIndex = 10;
            this.textBox5.Text = "Files";
            // 
            // list_InputPaths
            // 
            this.list_InputPaths.FormattingEnabled = true;
            this.list_InputPaths.ItemHeight = 12;
            this.list_InputPaths.Location = new System.Drawing.Point(12, 62);
            this.list_InputPaths.Name = "list_InputPaths";
            this.list_InputPaths.ScrollAlwaysVisible = true;
            this.list_InputPaths.Size = new System.Drawing.Size(609, 64);
            this.list_InputPaths.TabIndex = 9;
            this.list_InputPaths.SelectedIndexChanged += new System.EventHandler(this.Changed_InputPaths);
            // 
            // bt_ToJson_Localizing
            // 
            this.bt_ToJson_Localizing.Location = new System.Drawing.Point(12, 539);
            this.bt_ToJson_Localizing.Name = "bt_ToJson_Localizing";
            this.bt_ToJson_Localizing.Size = new System.Drawing.Size(179, 31);
            this.bt_ToJson_Localizing.TabIndex = 4;
            this.bt_ToJson_Localizing.Text = "Localizing( To Json )";
            this.bt_ToJson_Localizing.UseVisualStyleBackColor = true;
            this.bt_ToJson_Localizing.Click += new System.EventHandler(this.Click_ExcelToXml);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 158);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(608, 267);
            this.dataGridView1.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(13, 138);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(41, 14);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "Sheet";
            // 
            // bt_InputFolder
            // 
            this.bt_InputFolder.Location = new System.Drawing.Point(464, 38);
            this.bt_InputFolder.Name = "bt_InputFolder";
            this.bt_InputFolder.Size = new System.Drawing.Size(75, 19);
            this.bt_InputFolder.TabIndex = 13;
            this.bt_InputFolder.Text = "Folder";
            this.bt_InputFolder.UseVisualStyleBackColor = true;
            this.bt_InputFolder.Click += new System.EventHandler(this.Click_InputFolder);
            // 
            // bt_InputExcel
            // 
            this.bt_InputExcel.Location = new System.Drawing.Point(545, 37);
            this.bt_InputExcel.Name = "bt_InputExcel";
            this.bt_InputExcel.Size = new System.Drawing.Size(76, 19);
            this.bt_InputExcel.TabIndex = 13;
            this.bt_InputExcel.Text = "Excel";
            this.bt_InputExcel.UseVisualStyleBackColor = true;
            this.bt_InputExcel.Click += new System.EventHandler(this.Click_InputExcel);
            // 
            // comb_Sheets
            // 
            this.comb_Sheets.FormattingEnabled = true;
            this.comb_Sheets.Location = new System.Drawing.Point(60, 132);
            this.comb_Sheets.Name = "comb_Sheets";
            this.comb_Sheets.Size = new System.Drawing.Size(121, 20);
            this.comb_Sheets.TabIndex = 15;
            // 
            // txtCurOutput
            // 
            this.txtCurOutput.Location = new System.Drawing.Point(12, 454);
            this.txtCurOutput.Name = "txtCurOutput";
            this.txtCurOutput.ReadOnly = true;
            this.txtCurOutput.Size = new System.Drawing.Size(527, 21);
            this.txtCurOutput.TabIndex = 3;
            this.txtCurOutput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // bt_OutputPathFind
            // 
            this.bt_OutputPathFind.Location = new System.Drawing.Point(545, 454);
            this.bt_OutputPathFind.Name = "bt_OutputPathFind";
            this.bt_OutputPathFind.Size = new System.Drawing.Size(75, 21);
            this.bt_OutputPathFind.TabIndex = 4;
            this.bt_OutputPathFind.Text = "...";
            this.bt_OutputPathFind.UseVisualStyleBackColor = true;
            this.bt_OutputPathFind.Click += new System.EventHandler(this.OutputPathFind_Click);
            // 
            // list_OutputPaths
            // 
            this.list_OutputPaths.FormattingEnabled = true;
            this.list_OutputPaths.ItemHeight = 12;
            this.list_OutputPaths.Location = new System.Drawing.Point(13, 481);
            this.list_OutputPaths.Name = "list_OutputPaths";
            this.list_OutputPaths.ScrollAlwaysVisible = true;
            this.list_OutputPaths.Size = new System.Drawing.Size(608, 52);
            this.list_OutputPaths.TabIndex = 9;
            this.list_OutputPaths.SelectedIndexChanged += new System.EventHandler(this.Changed_OutputPaths);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(12, 434);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 14);
            this.textBox4.TabIndex = 10;
            this.textBox4.Text = "Output";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(546, 539);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 19);
            this.button5.TabIndex = 13;
            this.button5.Text = "Folder";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Click_OutputFolder);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 581);
            this.Controls.Add(this.comb_Sheets);
            this.Controls.Add(this.bt_InputExcel);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.bt_InputFolder);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.list_OutputPaths);
            this.Controls.Add(this.list_InputPaths);
            this.Controls.Add(this.bt_ToJson_Localizing);
            this.Controls.Add(this.bt_OutputPathFind);
            this.Controls.Add(this.bt_InputPathFind);
            this.Controls.Add(this.txtCurOutput);
            this.Controls.Add(this.txtCurInput);
            this.Name = "Form1";
            this.Text = "DataTransfer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_InputPathFind;
        private System.Windows.Forms.TextBox txtCurInput;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.ListBox list_InputPaths;
        private System.Windows.Forms.Button bt_ToJson_Localizing;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button bt_InputFolder;
        private System.Windows.Forms.Button bt_InputExcel;
        private System.Windows.Forms.ComboBox comb_Sheets;
        private System.Windows.Forms.TextBox txtCurOutput;
        private System.Windows.Forms.Button bt_OutputPathFind;
        private System.Windows.Forms.ListBox list_OutputPaths;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button5;
    }
}


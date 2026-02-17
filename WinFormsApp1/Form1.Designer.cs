namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbxFilepath = new TextBox();
            btnImport = new Button();
            dgvDataList = new DataGridView();
            btnSearch = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDataList).BeginInit();
            SuspendLayout();
            // 
            // tbxFilepath
            // 
            tbxFilepath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tbxFilepath.Location = new Point(36, 21);
            tbxFilepath.Name = "tbxFilepath";
            tbxFilepath.PlaceholderText = "Enter path here !!!";
            tbxFilepath.Size = new Size(390, 31);
            tbxFilepath.TabIndex = 0;
            // 
            // btnImport
            // 
            btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnImport.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImport.Location = new Point(550, 21);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(220, 34);
            btnImport.TabIndex = 1;
            btnImport.Text = "Import File For Search";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // dgvDataList
            // 
            dgvDataList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDataList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDataList.Location = new Point(36, 61);
            dgvDataList.Name = "dgvDataList";
            dgvDataList.RowHeadersWidth = 62;
            dgvDataList.Size = new Size(734, 394);
            dgvDataList.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.Location = new Point(432, 21);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(112, 34);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(805, 494);
            Controls.Add(btnSearch);
            Controls.Add(dgvDataList);
            Controls.Add(btnImport);
            Controls.Add(tbxFilepath);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvDataList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbxFilepath;
        private Button btnImport;
        private DataGridView dgvDataList;
        private Button btnSearch;
    }
}

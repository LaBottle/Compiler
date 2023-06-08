namespace Compiler3 {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            FileButton = new Button();
            AnalysisButton = new Button();
            OutputRichTextBox = new RichTextBox();
            PathTextBox = new TextBox();
            InfoRichTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // FileButton
            // 
            FileButton.Location = new Point(858, 13);
            FileButton.Margin = new Padding(2, 2, 2, 2);
            FileButton.Name = "FileButton";
            FileButton.Size = new Size(75, 25);
            FileButton.TabIndex = 0;
            FileButton.Text = "选择文件";
            FileButton.UseVisualStyleBackColor = true;
            FileButton.Click += FileButton_Click;
            // 
            // AnalysisButton
            // 
            AnalysisButton.Location = new Point(937, 13);
            AnalysisButton.Margin = new Padding(2, 2, 2, 2);
            AnalysisButton.Name = "AnalysisButton";
            AnalysisButton.Size = new Size(75, 25);
            AnalysisButton.TabIndex = 1;
            AnalysisButton.Text = "分析";
            AnalysisButton.UseVisualStyleBackColor = true;
            AnalysisButton.Click += AnalysisButton_Click;
            // 
            // OutputRichTextBox
            // 
            OutputRichTextBox.Location = new Point(11, 54);
            OutputRichTextBox.Margin = new Padding(2, 2, 2, 2);
            OutputRichTextBox.Name = "OutputRichTextBox";
            OutputRichTextBox.ReadOnly = true;
            OutputRichTextBox.Size = new Size(500, 500);
            OutputRichTextBox.TabIndex = 2;
            OutputRichTextBox.Text = "";
            // 
            // PathTextBox
            // 
            PathTextBox.Location = new Point(9, 14);
            PathTextBox.Margin = new Padding(2, 2, 2, 2);
            PathTextBox.Name = "PathTextBox";
            PathTextBox.Size = new Size(833, 23);
            PathTextBox.TabIndex = 3;
            // 
            // InfoRichTextBox
            // 
            InfoRichTextBox.Location = new Point(526, 54);
            InfoRichTextBox.Margin = new Padding(2);
            InfoRichTextBox.Name = "InfoRichTextBox";
            InfoRichTextBox.ReadOnly = true;
            InfoRichTextBox.Size = new Size(500, 500);
            InfoRichTextBox.TabIndex = 4;
            InfoRichTextBox.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1037, 577);
            Controls.Add(InfoRichTextBox);
            Controls.Add(PathTextBox);
            Controls.Add(OutputRichTextBox);
            Controls.Add(AnalysisButton);
            Controls.Add(FileButton);
            Margin = new Padding(2, 2, 2, 2);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button FileButton;
        private Button AnalysisButton;
        private RichTextBox OutputRichTextBox;
        private TextBox PathTextBox;
        private RichTextBox InfoRichTextBox;
    }
}
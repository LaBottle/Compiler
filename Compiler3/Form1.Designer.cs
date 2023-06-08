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
            QuatRichTextBox = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // FileButton
            // 
            FileButton.Location = new Point(858, 13);
            FileButton.Margin = new Padding(2);
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
            AnalysisButton.Margin = new Padding(2);
            AnalysisButton.Name = "AnalysisButton";
            AnalysisButton.Size = new Size(75, 25);
            AnalysisButton.TabIndex = 1;
            AnalysisButton.Text = "分析";
            AnalysisButton.UseVisualStyleBackColor = true;
            AnalysisButton.Click += AnalysisButton_Click;
            // 
            // OutputRichTextBox
            // 
            OutputRichTextBox.Location = new Point(9, 66);
            OutputRichTextBox.Margin = new Padding(2);
            OutputRichTextBox.Name = "OutputRichTextBox";
            OutputRichTextBox.ReadOnly = true;
            OutputRichTextBox.Size = new Size(333, 500);
            OutputRichTextBox.TabIndex = 2;
            OutputRichTextBox.Text = "";
            // 
            // PathTextBox
            // 
            PathTextBox.Location = new Point(9, 14);
            PathTextBox.Margin = new Padding(2);
            PathTextBox.Name = "PathTextBox";
            PathTextBox.Size = new Size(833, 23);
            PathTextBox.TabIndex = 3;
            // 
            // InfoRichTextBox
            // 
            InfoRichTextBox.Location = new Point(348, 66);
            InfoRichTextBox.Margin = new Padding(2);
            InfoRichTextBox.Name = "InfoRichTextBox";
            InfoRichTextBox.ReadOnly = true;
            InfoRichTextBox.Size = new Size(341, 500);
            InfoRichTextBox.TabIndex = 4;
            InfoRichTextBox.Text = "";
            // 
            // QuatRichTextBox
            // 
            QuatRichTextBox.Location = new Point(693, 66);
            QuatRichTextBox.Margin = new Padding(2);
            QuatRichTextBox.Name = "QuatRichTextBox";
            QuatRichTextBox.ReadOnly = true;
            QuatRichTextBox.Size = new Size(333, 500);
            QuatRichTextBox.TabIndex = 5;
            QuatRichTextBox.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 47);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 6;
            label1.Text = "语法分析：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(348, 47);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 7;
            label2.Text = "语义检查：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(693, 47);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 8;
            label3.Text = "四元式：";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1037, 577);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(QuatRichTextBox);
            Controls.Add(InfoRichTextBox);
            Controls.Add(PathTextBox);
            Controls.Add(OutputRichTextBox);
            Controls.Add(AnalysisButton);
            Controls.Add(FileButton);
            Margin = new Padding(2);
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
        private RichTextBox QuatRichTextBox;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
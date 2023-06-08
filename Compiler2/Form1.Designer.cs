namespace Compiler2 {
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
            SuspendLayout();
            // 
            // FileButton
            // 
            FileButton.Location = new Point(384, 15);
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
            AnalysisButton.Location = new Point(463, 15);
            AnalysisButton.Margin = new Padding(2);
            AnalysisButton.Name = "AnalysisButton";
            AnalysisButton.Size = new Size(75, 25);
            AnalysisButton.TabIndex = 1;
            AnalysisButton.Text = "词法分析";
            AnalysisButton.UseVisualStyleBackColor = true;
            AnalysisButton.Click += AnalysisButton_Click;
            // 
            // OutputRichTextBox
            // 
            OutputRichTextBox.Location = new Point(11, 54);
            OutputRichTextBox.Margin = new Padding(2);
            OutputRichTextBox.Name = "OutputRichTextBox";
            OutputRichTextBox.ReadOnly = true;
            OutputRichTextBox.Size = new Size(527, 517);
            OutputRichTextBox.TabIndex = 2;
            OutputRichTextBox.Text = "";
            // 
            // PathTextBox
            // 
            PathTextBox.Location = new Point(11, 16);
            PathTextBox.Margin = new Padding(2);
            PathTextBox.Name = "PathTextBox";
            PathTextBox.Size = new Size(362, 23);
            PathTextBox.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 582);
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
    }
}
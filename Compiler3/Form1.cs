namespace Compiler3 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void FileButton_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Title = "请选择文件";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK) return;
            PathTextBox.Text = dialog.FileName;
        }

        private void AnalysisButton_Click(object sender, EventArgs e) {
            if (PathTextBox.Text == "") {
                MessageBox.Show("请选择文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OutputRichTextBox.Text = "正在分析...";
            InfoRichTextBox.Text = "";
            QuatRichTextBox.Text = "";
            AnalysisButton.Enabled = false;
            var s = File.ReadAllText(PathTextBox.Text);
            Task.Run(() => {
                try {
                    var ex = Compiler.Recursive(Compiler.Analysis(s), out var info, out var quats);
                    if (ex == null) {
                        Invoke(delegate {
                            OutputRichTextBox.Text = "没有语法错误。";
                        });

                    } else {
                        Invoke(delegate {
                            OutputRichTextBox.Text = "";
                            OutputRichTextBox.Text += ex.Message + "\n" + ex.StackTrace + "\n\n";
                            while (ex.InnerException != null) {
                                ex = ex.InnerException;
                                OutputRichTextBox.Text += ex.Message + "\n" + ex.StackTrace + "\n\n";
                            }
                        });
                    }
                    Invoke(delegate {
                        foreach (var i in info) {
                            InfoRichTextBox.Text += i.id + ": " + i.msg + "\n";
                        }
                    });
                    if (quats != null) {
                        Invoke(delegate {
                            for (int i = 0; i < quats.Count; i++) {
                                QuatRichTextBox.Text += $"{i}: ({quats[i].op}, {quats[i].arg1}, {quats[i].arg2}, {quats[i].res})\n";
                            }
                        });
                    } else {
                        Invoke(delegate {
                            QuatRichTextBox.Text = "有错误发生！四元式解析失败";
                        });
                    }
                    MessageBox.Show("分析完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Invoke(delegate {
                    AnalysisButton.Enabled = true;
                });
            });
        }
    }
}
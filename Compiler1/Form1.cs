namespace Compiler1 {
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
            try {
                var s = File.ReadAllText(PathTextBox.Text);
                var output = Compiler.Analysis(s);
                OutputRichTextBox.Text = "";
                foreach (var item in output) {
                    OutputRichTextBox.Text += item + "\n";
                }
                MessageBox.Show("分析完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
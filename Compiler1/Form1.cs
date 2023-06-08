namespace Compiler1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void FileButton_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Title = "��ѡ���ļ�";
            dialog.Filter = "�����ļ�(*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK) return;
            PathTextBox.Text = dialog.FileName;
        }

        private void AnalysisButton_Click(object sender, EventArgs e) {
            if (PathTextBox.Text == "") {
                MessageBox.Show("��ѡ���ļ���", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try {
                var s = File.ReadAllText(PathTextBox.Text);
                var output = Compiler.Analysis(s);
                OutputRichTextBox.Text = "";
                foreach (var item in output) {
                    OutputRichTextBox.Text += item + "\n";
                }
                MessageBox.Show("������ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
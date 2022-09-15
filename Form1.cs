namespace Pigeon
{
    public partial class Pigeon : Form
    {
        string path = Environment.CurrentDirectory + "\\files";
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        public Pigeon()
        {
            InitializeComponent();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            lookingFile();
        }

        private void lookingFile()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\files", "*.xlsx");
            foreach (var f in files)
            {
                var x = f.Split(Environment.CurrentDirectory + "\\files\\")[1].Split("_");
                var key = x[0];
                var value = x[1].Replace(".xlsx", "");
                if (key.StartsWith("B") && key.Length == 6)
                {
                    if (dict.Keys.Contains(key))
                    {
                        dict[key].Add(value);
                    }
                    else
                    {
                        dict.Add(key, new List<string> { value });
                    }
                }
            }
        }

        private void btnCheckFile_Click(object sender, EventArgs e)
        {
            CheckFiles checkFiles = new CheckFiles(dict);
            checkFiles.ShowDialog();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            Compare compare = new Compare(dict);
            compare.ShowDialog();
        }
    }
}
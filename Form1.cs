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
        }

        private void LookingFile()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\files", "*.*").Where(file => new string[] { ".xlsx", ".xls" }.Contains(Path.GetExtension(file))).ToArray();
            foreach (var f in files)
            {
                var x = f.Split(Environment.CurrentDirectory + "\\files\\")[1].Split("_");
                var key = x[0];
                var value = x[1].Split(".")[0];
                if (key.StartsWith("B") && key.Length == 6)
                {
                    if (dict.ContainsKey(key))
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
            LookingFile();
            CheckFiles checkFiles = new CheckFiles(dict);
            checkFiles.ShowDialog();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            LookingFile();
            Compare compare = new Compare(dict);
            compare.ShowDialog();
        }
    }
}
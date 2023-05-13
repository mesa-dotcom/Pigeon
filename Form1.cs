namespace Pigeon
{
    public partial class Pigeon : Form
    {
        string path_files = Environment.CurrentDirectory + "\\files";
        string path_results = Environment.CurrentDirectory + "\\results";
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        bool hasSAP = false;
        List<string> list = new List<string> { "Bank", "StoreSlip"};
        public Pigeon()
        {
            InitializeComponent();
            if (!Directory.Exists(path_files))
            {
                Directory.CreateDirectory(path_files);
            }
            if (!Directory.Exists(path_results))
            {
                Directory.CreateDirectory(path_results);
            }
        }

        private void LookingFile()
        {
            dict.Clear();
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\files", "*.*").Where(file => new string[] { ".xlsx", ".xls" }.Contains(Path.GetExtension(file))).ToArray();
            if (files.FirstOrDefault(f => f.Contains("\\SAP.xls") || f.Contains("\\SAP.xlsx")) != null)
            {
                hasSAP = true;
            } else
            {
                hasSAP = false;
            }
            foreach (var f in files)
            {
                var x = f.Split(Environment.CurrentDirectory + "\\files\\")[1].Split("_");
                var key = x[0];
                if (key.StartsWith("B") && key.Length == 6)
                {
                    var value = x[1].Split(".")[0];
                    if (dict.ContainsKey(key) && list.Contains(value))
                    {
                        dict[key].Add(value);
                    }
                    else if (list.Contains(value))
                    {
                        dict.Add(key, new List<string> { value });
                    }
                }
            }
        }

        private void btnCheckFile_Click(object sender, EventArgs e)
        {
            LookingFile();
            CheckFiles checkFiles = new CheckFiles(dict, hasSAP);
            checkFiles.ShowDialog();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            LookingFile();
            Compare compare = new Compare(dict, hasSAP);
            compare.ShowDialog();
        }
    }
}
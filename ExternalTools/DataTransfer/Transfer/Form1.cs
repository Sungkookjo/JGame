using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Common;
using System.IO;
using System.Web.Script.Serialization;
using Excel = Microsoft.Office.Interop.Excel;

namespace Transfer
{
    public partial class Form1 : Form
    {
        List<string> CachedPaths = new List<string>();
        string curpath = "";
        protected OleDbConnection OleDbCon = null;
        string[] country = { "eng", "kor" };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\";
                dlg.Filter = "엑셀 (*.xlsx)|*.xlsx|모든파일 (*.*)|*.*";
                dlg.FilterIndex = 1;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                fileName = dlg.FileName;

                if (fileName.Length > 2)
                {
                    string result = CachedPaths.Find(
                    delegate (string temp)
                    {
                        return temp == fileName;
                    }
                    );

                    if (result == null)
                    {
                        CachedPaths.Add(fileName);
                        listBox1.Items.Add(fileName);
                    }
                }
                textBox1.Text = fileName;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Path;

            if (listBox1.SelectedItem == null)
            {
                return;
            }

            Path = listBox1.SelectedItem.ToString();
            if (Path.Length > 2)
            {
                textBox1.Text = Path;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            curpath = textBox1.Text;
        }

        #region TODO_Excel
        public bool OpenExcel()
        {
            try
            {
                var conStr = string.Format(@"
                Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source={0};
                Extended Properties=""Excel 12.0 Xml;HDR=YES""
                ", curpath);

                if (OleDbCon != null)
                {
                    CloseExcel();
                }

                OleDbCon = new OleDbConnection(conStr);
                OleDbCon.Open();
            }
            catch
            {
                OleDbCon = null;
            }

            return true;
        }

        public void CloseExcel()
        {
            if (OleDbCon != null)
            {
                OleDbCon.Close();
                OleDbCon = null;
            }
        }

        public void OpenApp()
        {
            var excelApp = new Excel.Application();

            excelApp.Visible = true;
        }

        public void Localization_ToJson()
        {
            var sheetName = textBox3.Text;
            var Folder = System.IO.Path.GetDirectoryName(curpath);

            if (OleDbCon == null) return;
            
            // begin read
            var cmd = OleDbCon.CreateCommand();
            cmd.CommandText = String.Format(
                @"SELECT * FROM [{0}$]",
                sheetName
            );

            // read data
            for ( int i = 0; i < country.Length; ++i )
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    var query =
                        from DbDataRecord row in rdr
                        select new JGame.LocalizationItem( row[0].ToString() ,row[i+2].ToString() );

                    JGame.LocalizationData data = new JGame.LocalizationData();
                    data.items = query.ToArray<JGame.LocalizationItem>();

                    var json = new JavaScriptSerializer().Serialize(data);

                    File.WriteAllText(Folder + "\\" + country[i] +".json", json);
                }
            }
        }
        #endregion

        // Localization : excel to json
        private void button3_Click(object sender, EventArgs e)
        {
            OpenExcel();

            Localization_ToJson();
            
            CloseExcel();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Open_Click(object sender, EventArgs e)
        {
            if (curpath.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, " + curpath);
            }
        }
    }
}

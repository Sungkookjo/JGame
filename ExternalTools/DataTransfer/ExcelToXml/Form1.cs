using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Common;
using System.IO;
using System.Web.Script.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;

namespace Transfer
{
    // form 1
    public partial class Form1 : Form
    {
        // excel properties
        Excel.Application App = new Excel.Application();

        // input properties
        string curInputPath = null;

        // output properties
        string curOutputPath = null;
        
        // db connector
        protected OleDbConnection OleDbCon = null;
        string[] country = { "eng", "kor" };

        // key list
        const string key_CachedInputNum = "CachedInputNum";
        const string key_CachedOutputNum = "CachedOutputNum";
        const string key_format_InputPath = "InputPath_{0}";
        const string key_format_OutputPath = "OutputPath_{0}";
        const string key_Input_Excel = "input_excel_folder";
        const string key_Output_Xml = "output_xml_folder";

        // init
        public Form1()
        {
            InitializeComponent();
            
            // init cached input list
            int i;

            for (i = int.Parse(GetConfig(key_CachedInputNum, "0")); i > 0; --i)
            {
                list_InputPaths.Items.Add(GetConfig(string.Format(key_format_InputPath, i - 1), ""));
            }

            // init cached Output list

            for (i = int.Parse(GetConfig(key_CachedOutputNum, "0")); i > 0; --i)
            {
                list_OutputPaths.Items.Add(GetConfig(string.Format(key_format_OutputPath, i - 1), ""));
            }
        }

        private void Control1_HandleDestroyed(Object sender, EventArgs e)
        {
            App.Quit();
            App = null;

            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }

        #region Input func
        private void Click_InputPathFind(object sender, EventArgs e)
        {
            string fileName = string.Empty;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                var folderPath = GetConfig( key_Input_Excel , Application.StartupPath);

                if ( folderPath != null )
                {
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    if( !di.Exists )
                    {
                        folderPath = Application.StartupPath;
                    }
                }
                else
                {
                    folderPath = Application.StartupPath;
                }
                
                dlg.InitialDirectory = folderPath;
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
                    if (!list_InputPaths.Items.Contains(fileName))
                    {
                        list_InputPaths.Items.Add(fileName);
                        SetConfig(key_CachedInputNum, list_InputPaths.Items.Count.ToString());
                        SetConfig(string.Format(key_format_InputPath, list_InputPaths.Items.Count - 1), fileName);
                    }
                }
                txtCurInput.Text = fileName;
                SetConfig( key_Input_Excel, System.IO.Path.GetDirectoryName(fileName));
            }
        }
        private void Click_InputFolder(object sender, EventArgs e)
        {
            OpenFolder(curInputPath);
        }
        private void Click_InputExcel(object sender, EventArgs e)
        {
            if (curInputPath.Length > 0)
            {
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Open(curInputPath);
                excelApp.Visible = true;
            }
        }
        private void Changed_InputPaths(object sender, EventArgs e)
        {
            if (list_InputPaths.SelectedItem == null)
            {
                return;
            }

            curInputPath = list_InputPaths.SelectedItem.ToString();
            txtCurInput.Text = curInputPath;

            // update sheet
            try
            {
                Excel.Workbook excelBook = App.Workbooks.Open(curInputPath);

                String[] excelSheets = new String[excelBook.Worksheets.Count];

                comb_Sheets.Items.Clear();

                int i = 0;
                foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                {
                    comb_Sheets.Items.Add(wSheet.Name);
                    i++;
                }

                comb_Sheets.SelectedIndex = 0;

                excelBook.Close(0);
            }
            catch
            {

            }
        }
        #endregion

        #region Output func
        private void OutputPathFind_Click(object sender, EventArgs e)
        {
            string selectedFolder = string.Empty;
            
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                var folderPath = GetConfig( key_Output_Xml );

                if (folderPath != null)
                {
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    if (!di.Exists)
                    {
                        folderPath = Application.StartupPath;
                    }
                }
                else
                {
                    folderPath = Application.StartupPath;
                }

                dlg.SelectedPath = folderPath;

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                selectedFolder = dlg.SelectedPath;

                if (selectedFolder.Length > 2)
                {
                    if( !list_OutputPaths.Items.Contains(selectedFolder) )
                    {
                        list_OutputPaths.Items.Add(selectedFolder);
                        SetConfig(key_CachedOutputNum, list_OutputPaths.Items.Count.ToString());
                        SetConfig(string.Format(key_format_OutputPath, list_OutputPaths.Items.Count - 1), selectedFolder);
                    }
                }
                txtCurOutput.Text = selectedFolder;
                SetConfig(key_Output_Xml, System.IO.Path.GetDirectoryName(selectedFolder));
            }
        }
        private void Click_OutputFolder(object sender, EventArgs e)
        {
            OpenFolder(curOutputPath);
        }
        private void Changed_OutputPaths(object sender, EventArgs e)
        {
            if (list_OutputPaths.SelectedItem == null)
            {
                return;
            }

            curOutputPath = list_OutputPaths.SelectedItem.ToString();
            txtCurOutput.Text = curOutputPath;
        }
        #endregion

        // open folder.
        protected void OpenFolder(string path)
        {
            if (path.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, " + path);
            }
        }

        #region get/set/remove config
        private string GetConfig(string key, string defVal = null)
        {
            if (ConfigurationManager.AppSettings[key] == null && defVal != null)
            {
                SetConfig(key, defVal);
                return defVal;
            }

            return ConfigurationManager.AppSettings[key];
        }

        private void SetConfig(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void RemoveConfig(string key)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    return;
                }
                else
                {
                    settings.Remove(key);
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            curInputPath = txtCurInput.Text;
        }

        #region TODO_Excel
        // open
        public bool OpenExcel( string path )
        {
            try
            {
                var conStr = string.Format(@"
                Provider=Microsoft.ACE.OLEDB.12.0;
                Data Source={0};
                Extended Properties=""Excel 12.0 Xml;HDR=YES""
                ", path);

                if (OleDbCon != null)
                {
                    CloseExcel();
                }

                OleDbCon = new OleDbConnection(conStr);
                OleDbCon.Open();
            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                OleDbCon = null;
            }

            return true;
        }

        // close
        public void CloseExcel()
        {
            if (OleDbCon != null)
            {
                OleDbCon.Close();
                OleDbCon = null;
            }
        }
        
        // trans
        public bool Localization_ToJson()
        {
            var sheetName = comb_Sheets.SelectedItem;

            if (OleDbCon == null) return false;
            
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

                    File.WriteAllText(curOutputPath + "\\" + country[i] +".json", json);
                }
            }

            return true;
        }
        #endregion

        // Localization : excel to json
        private void Click_ExcelToXml(object sender, EventArgs e)
        {
            OpenExcel( curInputPath );

            if( Localization_ToJson() )
            {
                MessageBox.Show("성공", "결과", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("실패","결과",MessageBoxButtons.OK);
            }
            
            CloseExcel();
        }
    }
}

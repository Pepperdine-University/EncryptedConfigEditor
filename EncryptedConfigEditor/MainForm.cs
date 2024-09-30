using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EncryptedConfigEditor
{
    public partial class MainForm : Form
    {
        private Configuration _config;
        private string _configFilePath;
        private bool _isDirty;

        private bool IsDirty
        {
            set
            {
                _isDirty = value;
                Save.Enabled = _isDirty;
            }
            get => _isDirty;
        }

        public MainForm()
        {
            InitializeComponent();
            ConfigureLexer();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            PromptForConfigFile();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult? result = PromptIfUnsavedChanges();

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            DialogResult? result = PromptIfUnsavedChanges();

            if (result != DialogResult.Cancel)
            {
                PromptForConfigFile();
            }
        }

        private void PromptForConfigFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Config Files (*.config)|*.config",
                Title = "Select .config File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _configFilePath = openFileDialog.FileName;
                ConfigPath.Text = _configFilePath;
                LoadConfigFile(_configFilePath);
            }
        }

        private void LoadConfigFile(string path)
        {
            try
            {
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = path
                };
                _config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                PopulateSectionsCombo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading config file: {ex.Message}");
            }
        }

        private void PopulateSectionsCombo()
        {
            Sections.SelectedIndex = -1;
            Sections.Items.Clear();
            ConfigContents.Clear();
            List<string> sectionNames = new List<string>();
            foreach (string sectionName in _config.Sections.Keys)
            {
                try
                {
                    ConfigurationSection section = _config.GetSection(sectionName);
                    if (!string.IsNullOrWhiteSpace(section.SectionInformation.GetRawXml()))
                    {
                        sectionNames.Add(section.SectionInformation.Name);
                    }
                }
                catch (ConfigurationErrorsException ex)
                {
                    if (ex.InnerException is CryptographicException)
                    {
                        ShowErrorMessage($"The section '{sectionName}' could not be decrypted.\r\n\r\n{ex.InnerException.Message}");
                    }
                    // Other sections that can't be read can be ignored
                }
            }
            sectionNames.Sort(StringComparer.OrdinalIgnoreCase);

            foreach (string sectionName in sectionNames)
            {
                Sections.Items.Add(sectionName);
            }
        }

        private void Sections_SelectedIndexChanged(object sender, EventArgs e)
        {
            int previousSelection = (int)(Sections.Tag ?? -1);
            if (previousSelection == Sections.SelectedIndex) return;

            if (Sections.SelectedIndex != -1)
            {
                DialogResult? result = PromptIfUnsavedChanges();

                if (result == DialogResult.Cancel)
                {
                    Sections.SelectedIndex = previousSelection;
                    return;
                }

                string selectedSection = Sections.SelectedItem.ToString();
                LoadSectionContents(selectedSection);
            }

            Sections.Tag = Sections.SelectedIndex;
        }

        private void LoadSectionContents(string sectionName)
        {
            ConfigurationSection section = _config.GetSection(sectionName);

            if (section == null)
            {
                ConfigContents.Clear();
            }
            else
            {
                ConfigContents.Text = FormatXml(section.SectionInformation.GetRawXml());
            }

            IsDirty = false;
        }

        private void EncryptSection(string sectionName)
        {
            try
            {
                ConfigurationSection section = _config.GetSection(sectionName);

                if (section?.SectionInformation.IsProtected == false)
                {
                    section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                    section.SectionInformation.ForceSave = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error encrypting section: {ex.Message}");
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveConfigFile();
        }

        private void SaveConfigFile()
        {
            SaveSectionChanges();
            try
            {
                _config.Save(ConfigurationSaveMode.Modified);
                IsDirty = false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error saving config file: {ex.Message}");
            }
        }

        private void SaveSectionChanges()
        {
            string selectedSection = Sections.SelectedItem.ToString();

            ConfigurationSection section = _config.GetSection(selectedSection);

            if (section != null)
            {
                try
                {
                    section.SectionInformation.SetRawXml(FormatXml(ConfigContents.Text));
                    EncryptSection(selectedSection);

                    section.SectionInformation.ForceSave = true;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Error saving section: {ex.Message}");
                }
            }
        }

        private void ConfigContents_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception ex)
            {
                return $"Error formatting XML: {ex.Message}";
            }
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult? PromptIfUnsavedChanges()
        {
            if (!IsDirty)
            {
                return null;
            }

            DialogResult userChoice = MessageBox.Show("Do you want to save your changes before continuing?"
                , "Unsaved Changes"
                , MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Warning);

            if (userChoice == DialogResult.Yes)
            {
                SaveConfigFile();
            }

            return userChoice;
        }

        //TODO: deal with backwards horizontal scrolling
        /* https://stackoverflow.com/questions/7828121/shift-mouse-wheel-horizontal-scrolling
           protected override void OnMouseWheel(MouseEventArgs e)
           {
               if (this.VScroll && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
               {
                   this.VScroll = false;
                   base.OnMouseWheel(e);
                   this.VScroll = true;
               }
               else
               {
                   base.OnMouseWheel(e);
               }
           }
        */

        private void ConfigureLexer()
        {
            // based on https://gist.github.com/desjarlais/5e819f48c777d0b96a4f7fe2fb739a2e
            // Reset the styles
            ConfigContents.StyleResetDefault();
            ConfigContents.Styles[Style.Default].Font = "Consolas";
            ConfigContents.Styles[Style.Default].Size = 10;
            ConfigContents.StyleClearAll();

            // Set the XML Lexer
            ConfigContents.LexerName = "xml";

            // Show line numbers
            ConfigContents.Margins[0].Width = 20	;

            // Enable folding
            ConfigContents.SetProperty("fold", "1");
            ConfigContents.SetProperty("fold.compact", "1");
            ConfigContents.SetProperty("fold.html", "1");

            // Use Margin 2 for fold markers
            ConfigContents.Margins[2].Type = MarginType.Symbol;
            ConfigContents.Margins[2].Mask = Marker.MaskFolders;
            ConfigContents.Margins[2].Sensitive = true;
            ConfigContents.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                ConfigContents.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                ConfigContents.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Style the folder markers
            ConfigContents.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            ConfigContents.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            ConfigContents.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            ConfigContents.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            ConfigContents.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            ConfigContents.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            ConfigContents.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            ConfigContents.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            ConfigContents.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            ConfigContents.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

            // Set the Styles
            ConfigContents.StyleResetDefault();
            // I like fixed font for XML
            ConfigContents.Styles[Style.Default].Font = "Courier";
            ConfigContents.Styles[Style.Default].Size = 10;
            ConfigContents.StyleClearAll();
            ConfigContents.Styles[Style.Xml.Attribute].ForeColor = Color.Crimson;
            ConfigContents.Styles[Style.Xml.Entity].ForeColor = Color.Crimson;
            ConfigContents.Styles[Style.Xml.Comment].ForeColor = Color.Green;
            ConfigContents.Styles[Style.Xml.Tag].ForeColor = Color.BlueViolet;
            ConfigContents.Styles[Style.Xml.TagEnd].ForeColor = Color.BlueViolet;
            ConfigContents.Styles[Style.Xml.DoubleString].ForeColor = Color.MediumBlue;
            ConfigContents.Styles[Style.Xml.SingleString].ForeColor = Color.MediumBlue;
        }
    }
}

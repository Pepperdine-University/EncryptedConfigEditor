using System.Windows.Forms;

namespace EncryptedConfigEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.TextBox ConfigPath;
        private System.Windows.Forms.ComboBox Sections;
        private System.Windows.Forms.Label ConfigPathLabel;
        private System.Windows.Forms.Label SectionLabel;
        private ScintillaNET.Scintilla ConfigContents;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Browse = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.ConfigPath = new System.Windows.Forms.TextBox();
            this.Sections = new System.Windows.Forms.ComboBox();
            this.ConfigPathLabel = new System.Windows.Forms.Label();
            this.SectionLabel = new System.Windows.Forms.Label();
            this.ConfigContents = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(400, 25);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(100, 23);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Save
            // 
            this.Save.Enabled = false;
            this.Save.Location = new System.Drawing.Point(400, 73);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(100, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save Changes";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // ConfigPath
            // 
            this.ConfigPath.Location = new System.Drawing.Point(20, 25);
            this.ConfigPath.Name = "ConfigPath";
            this.ConfigPath.Size = new System.Drawing.Size(360, 20);
            this.ConfigPath.TabIndex = 4;
            // 
            // Sections
            // 
            this.Sections.FormattingEnabled = true;
            this.Sections.Location = new System.Drawing.Point(20, 75);
            this.Sections.Name = "Sections";
            this.Sections.Size = new System.Drawing.Size(360, 21);
            this.Sections.TabIndex = 5;
            this.Sections.SelectedIndexChanged += new System.EventHandler(this.Sections_SelectedIndexChanged);
            // 
            // ConfigPathLabel
            // 
            this.ConfigPathLabel.AutoSize = true;
            this.ConfigPathLabel.Location = new System.Drawing.Point(20, 10);
            this.ConfigPathLabel.Name = "ConfigPathLabel";
            this.ConfigPathLabel.Size = new System.Drawing.Size(84, 13);
            this.ConfigPathLabel.TabIndex = 6;
            this.ConfigPathLabel.Text = "Config File Path:";
            // 
            // SectionLabel
            // 
            this.SectionLabel.AutoSize = true;
            this.SectionLabel.Location = new System.Drawing.Point(20, 60);
            this.SectionLabel.Name = "SectionLabel";
            this.SectionLabel.Size = new System.Drawing.Size(88, 13);
            this.SectionLabel.TabIndex = 7;
            this.SectionLabel.Text = "Select a Section:";
            // 
            // ConfigContents
            // 
            this.ConfigContents.LexerName = null;
            this.ConfigContents.Location = new System.Drawing.Point(20, 111);
            this.ConfigContents.Name = "ConfigContents";
            this.ConfigContents.Size = new System.Drawing.Size(480, 157);
            this.ConfigContents.TabIndex = 8;
            this.ConfigContents.TextChanged += new System.EventHandler(this.ConfigContents_TextChanged);
            this.ConfigContents.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(521, 280);
            this.Controls.Add(this.ConfigContents);
            this.Controls.Add(this.SectionLabel);
            this.Controls.Add(this.ConfigPathLabel);
            this.Controls.Add(this.Sections);
            this.Controls.Add(this.ConfigPath);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Browse);
            this.Name = "MainForm";
            this.Text = "Encrypted Config Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}

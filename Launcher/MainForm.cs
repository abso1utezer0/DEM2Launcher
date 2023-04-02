// Decompiled with JetBrains decompiler
// Type: Launcher.MainForm
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using IniFiles;
using InputMapper;
using Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml.Serialization;

namespace Launcher
{
  public class MainForm : Form
  {
    public MainForm.RenderSettings m_kRenderSettings = new MainForm.RenderSettings();
    public InputContainer m_kInputMappings = new InputContainer();
    public Program.LauncherLanguage m_eCurrentLauncherLanguage;
    public Dictionary<string, string> m_kCurrentTranslations;
    private string m_kApplicationSettingsINIFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Disney Interactive Studios\\Epic Mickey 2\\AppSettings.ini";
    private string m_kInputConfigurationFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Disney Interactive Studios\\Epic Mickey 2\\InputMappings_User.xml";
    private bool m_bSaveConfigurationFiles = true;
    private IContainer components;
    private Button launcherLaunchButton;
    private Label launcherLanguageLabel;
    private ComboBox languageComboBox;
    private PictureBox logoPictureBox;
    private Button launcherOptionsButton;
    private Button launcherViewReadme;

    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileInt(
      string section,
      string key,
      int def,
      string filePath);

    public MainForm()
    {
      this.FormClosed += new FormClosedEventHandler(this.Form1_CloseEvent);
      this.InitializeComponent();
      this.ReadSettingsFromConfigurationFiles();
      this.InitialiseFormValuesFromSettings();
      this.WriteSettingsToConfigurationFiles();
    }

    private void Form1_CloseEvent(object sender, EventArgs e)
    {
      if (!this.m_bSaveConfigurationFiles)
        return;
      this.WriteSettingsToConfigurationFiles();
    }

    public string GetTranslation(string stringToTranslate, string returnValue)
    {
      string str;
      return this.m_kCurrentTranslations.TryGetValue(stringToTranslate, out str) ? str : returnValue;
    }

    private void ReadSettingsFromConfigurationFiles()
    {
      this.m_kRenderSettings.iAdapterIndex = 0;
      int num = 0;
      foreach (Program.ValidDisplayDevice validDisplayDevice in Program.ValidDisplayDevices)
      {
        if (validDisplayDevice.bPrimaryDevice)
        {
          this.m_kRenderSettings.iAdapterIndex = num;
          break;
        }
        ++num;
      }
      this.m_kRenderSettings.iWindowWidth = 1920;
      this.m_kRenderSettings.iWindowHeight = 1080;
      this.m_kRenderSettings.iWindowDisplayMode = 2;
      this.m_kRenderSettings.iDynamicShadowQuality = 3;
      this.m_kRenderSettings.iGroundingShadowQuality = 3;
      this.m_kRenderSettings.bVSync = true;
      this.m_kRenderSettings.bAntiAliasing = true;
      this.m_kRenderSettings.iWindowWidth = MainForm.GetPrivateProfileInt("Renderer", "WindowWidth", this.m_kRenderSettings.iWindowWidth, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.iWindowHeight = MainForm.GetPrivateProfileInt("Renderer", "WindowHeight", this.m_kRenderSettings.iWindowHeight, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.iAdapterIndex = MainForm.GetPrivateProfileInt("Renderer", "AdapterIndex", this.m_kRenderSettings.iAdapterIndex, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.iWindowDisplayMode = MainForm.GetPrivateProfileInt("Renderer", "WindowDisplayMode", this.m_kRenderSettings.iWindowDisplayMode, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.iDynamicShadowQuality = MainForm.GetPrivateProfileInt("Renderer", "DynamicShadowQuality", this.m_kRenderSettings.iDynamicShadowQuality, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.iGroundingShadowQuality = MainForm.GetPrivateProfileInt("Renderer", "GroundingShadowQuality", this.m_kRenderSettings.iGroundingShadowQuality, this.m_kApplicationSettingsINIFilename);
      this.m_kRenderSettings.bVSync = MainForm.GetPrivateProfileInt("Renderer", "VSync", this.m_kRenderSettings.bVSync ? 1 : 0, this.m_kApplicationSettingsINIFilename) != 0;
      this.m_kRenderSettings.bAntiAliasing = MainForm.GetPrivateProfileInt("Renderer", "AntiAliasing", this.m_kRenderSettings.bAntiAliasing ? 1 : 0, this.m_kApplicationSettingsINIFilename) != 0;
      if (this.m_kRenderSettings.iAdapterIndex < 0 || this.m_kRenderSettings.iAdapterIndex >= Program.ValidDisplayDevices.Count)
        this.m_kRenderSettings.iAdapterIndex = 0;
      if (this.m_kRenderSettings.iDynamicShadowQuality < 0 || this.m_kRenderSettings.iDynamicShadowQuality > 3)
        this.m_kRenderSettings.iDynamicShadowQuality = 3;
      if (this.m_kRenderSettings.iGroundingShadowQuality < 0 || this.m_kRenderSettings.iGroundingShadowQuality > 3)
        this.m_kRenderSettings.iGroundingShadowQuality = 3;
      this.SetCurrentLanguage((Program.LauncherLanguage) MainForm.GetPrivateProfileInt("Launcher", "Language", (int) Program.DefaultSystemLanguage, this.m_kApplicationSettingsINIFilename));
      string path = this.m_kInputConfigurationFilename;
      if (!File.Exists(path))
        path = "InputMappings.xml";
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (InputContainer));
      StreamReader streamReader = new StreamReader(path);
      this.m_kInputMappings = (InputContainer) xmlSerializer.Deserialize((TextReader) streamReader);
      this.m_kInputMappings.SetupOwners();
      this.m_kInputMappings.Sort();
      streamReader.Close();
    }

    private void WriteSettingsToConfigurationFiles()
    {
      string directoryName = Path.GetDirectoryName(this.m_kApplicationSettingsINIFilename);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      IniFile iniFile = IniFile.FromFile(this.m_kApplicationSettingsINIFilename);
      iniFile["Renderer"]["WindowWidth"] = this.m_kRenderSettings.iWindowWidth.ToString();
      iniFile["Renderer"]["WindowHeight"] = this.m_kRenderSettings.iWindowHeight.ToString();
      iniFile["Renderer"]["WindowDisplayMode"] = this.m_kRenderSettings.iWindowDisplayMode.ToString();
      iniFile["Renderer"]["AdapterIndex"] = this.m_kRenderSettings.iAdapterIndex.ToString();
      iniFile["Renderer"]["VSync"] = this.m_kRenderSettings.bVSync ? "1" : "0";
      iniFile["Renderer"]["AntiAliasing"] = this.m_kRenderSettings.bAntiAliasing ? "1" : "0";
      iniFile["Renderer"]["DynamicShadowQuality"] = this.m_kRenderSettings.iDynamicShadowQuality.ToString();
      iniFile["Renderer"]["GroundingShadowQuality"] = this.m_kRenderSettings.iGroundingShadowQuality.ToString();
      iniFile["Launcher"]["Language"] = ((int) this.m_eCurrentLauncherLanguage).ToString();
      iniFile["Renderer"].Comment = " Renderer Settings";
      iniFile["Renderer"].SetInlineComment("WindowWidth", " X resolution the game should use if not in maximised window mode");
      iniFile["Renderer"].SetInlineComment("WindowHeight", " Y resolution the game should use if not in maximised window mode");
      iniFile["Renderer"].SetInlineComment("WindowDisplayMode", " 0 for window, 1 for fullscreen, 2 for maximised window (removes window borders, and assumes maximum resolution)");
      iniFile["Renderer"].SetInlineComment("AdapterIndex", " Which graphics card to use - 0 for system default. Only useful for multi-card or multi-monitor systems");
      iniFile["Renderer"].SetInlineComment("VSync", " 1 to enable vertical sync, 0 to disable (may cause screen tearing on slow machines)");
      iniFile["Renderer"].SetInlineComment("AntiAliasing", " 1 to enable anti-aliasing, 0 to disable");
      iniFile["Renderer"].SetInlineComment("DynamicShadowQuality", " Quality of dynamic shadow maps. 0 disables dynamic shadows, 1 is lowest quality, 2 is medium, 3 is highest");
      iniFile["Renderer"].SetInlineComment("GroundingShadowQuality", " Quality of grounding shadow maps. 0 disables grounding shadows, 1 is lowest quality, 2 is medium, 3 is highest");
      iniFile["Launcher"].Comment = " Launcher Settings";
      iniFile["Launcher"].SetInlineComment("Language", " The index of the language to use for the Launcher. This is independant of the game language, which is controlled by the save-game");
      iniFile.Save(this.m_kApplicationSettingsINIFilename);
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (InputContainer));
      TextWriter textWriter = (TextWriter) new StreamWriter(this.m_kInputConfigurationFilename);
      xmlSerializer.Serialize(textWriter, (object) this.m_kInputMappings);
      textWriter.Close();
    }

    private void SetCurrentLanguage(Program.LauncherLanguage eNewLanguage)
    {
      if (!Program.m_akTranslations.TryGetValue(eNewLanguage, out this.m_kCurrentTranslations))
        Program.m_akTranslations.TryGetValue(Program.LauncherLanguage.English, out this.m_kCurrentTranslations);
      this.m_eCurrentLauncherLanguage = eNewLanguage;
      this.RecursiveTranslateControlLabels((Control) this, new Dictionary<string, string>());
      this.Text = this.GetTranslation("launcherMainTitle", "launcherMainTitle");
      Bitmap bitmap;
      switch (eNewLanguage)
      {
        case Program.LauncherLanguage.English:
          bitmap = Assets.DEM2_Black_ENG;
          break;
        case Program.LauncherLanguage.French:
          bitmap = Assets.DEM2_Black_FRA;
          break;
        case Program.LauncherLanguage.Spanish_PAL:
          bitmap = Assets.DEM2_Black_ESP;
          break;
        case Program.LauncherLanguage.Spanish_NTSC:
          bitmap = Assets.DEM2_Black_SPA;
          break;
        case Program.LauncherLanguage.German:
          bitmap = Assets.DEM2_Black_GER;
          break;
        case Program.LauncherLanguage.Italian:
          bitmap = Assets.DEM2_Black_ITA;
          break;
        case Program.LauncherLanguage.Portuguese:
          bitmap = Assets.DEM2_Black_POR;
          break;
        case Program.LauncherLanguage.Dutch:
          bitmap = Assets.DEM2_Black_DUT;
          break;
        case Program.LauncherLanguage.Swedish:
          bitmap = Assets.DEM2_Black_SWE;
          break;
        case Program.LauncherLanguage.Danish:
          bitmap = Assets.DEM2_Black_DAN;
          break;
        case Program.LauncherLanguage.Norwegian:
          bitmap = Assets.DEM2_Black_NOR;
          break;
        case Program.LauncherLanguage.Russian:
          bitmap = Assets.DEM2_Black_RUS;
          break;
        case Program.LauncherLanguage.Arabic:
          bitmap = Assets.DEM2_Black_ARA;
          break;
        case Program.LauncherLanguage.Czech:
          bitmap = Assets.DEM2_Black_CZH;
          break;
        case Program.LauncherLanguage.Hungarian:
          bitmap = Assets.DEM2_Logo_HUN;
          break;
        case Program.LauncherLanguage.BrazilianPortuguese:
          bitmap = Assets.DEM2_Black_PBR;
          break;
        case Program.LauncherLanguage.Polish:
          bitmap = Assets.DEM2_Black_POL;
          break;
        case Program.LauncherLanguage.Turkish:
          bitmap = Assets.DEM2_Black_TUR;
          break;
        case Program.LauncherLanguage.Greek:
          bitmap = Assets.DEM2_Black_ENG;
          break;
        default:
          bitmap = Assets.DEM2_Black_ENG;
          break;
      }
      this.logoPictureBox.Image = (Image) bitmap;
    }

    public void RecursiveTranslateControlLabels(
      Control parent,
      Dictionary<string, string> defaultTextDict)
    {
      foreach (Control control in (ArrangedElementCollection) parent.Controls)
      {
        string returnValue = defaultTextDict.ContainsKey(control.Name) ? defaultTextDict[control.Name] : control.Name;
        control.Text = this.GetTranslation(control.Name, returnValue);
        this.RecursiveTranslateControlLabels(control, defaultTextDict);
      }
    }

    private void InitialiseFormValuesFromSettings()
    {
      this.languageComboBox.Items.Clear();
      int num = 0;
      for (int index = 0; index < Program.m_kValidLanguageList.Count; ++index)
      {
        Program.LauncherLanguage kValidLanguage = Program.m_kValidLanguageList[index];
        Dictionary<string, string> dictionary;
        Program.m_akTranslations.TryGetValue(kValidLanguage, out dictionary);
        string str;
        if (dictionary.TryGetValue("launcherLanguageName", out str))
          this.languageComboBox.Items.Add((object) str);
        else
          this.languageComboBox.Items.Add((object) kValidLanguage.ToString());
        if (kValidLanguage == this.m_eCurrentLauncherLanguage)
          num = index;
      }
      this.languageComboBox.SelectedIndex = num;
    }

    private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.SetCurrentLanguage(Program.m_kValidLanguageList[this.languageComboBox.SelectedIndex]);

    private void launchButton_Click(object sender, EventArgs e)
    {
      this.WriteSettingsToConfigurationFiles();
      this.m_bSaveConfigurationFiles = false;
      string path = "DEM2.exe";
      if (!File.Exists(path))
        return;
      this.Hide();
      try
      {
        Mutex.OpenExisting("DEM2-85437943-954D-482a-AEBF-9DE13B3BB0BC");
      }
      catch (WaitHandleCannotBeOpenedException ex)
      {
        Process process = new Process();
        process.StartInfo.FileName = path;
        process.Start();
        process.WaitForExit();
      }
      this.Close();
    }

    private void optionsButton_Click(object sender, EventArgs e)
    {
      int num = (int) new OptionsForm(this).ShowDialog();
    }

    private void readmeButton_Click(object sender, EventArgs e)
    {
      string str = "readmeEN";
      switch (this.m_eCurrentLauncherLanguage)
      {
        case Program.LauncherLanguage.English:
          str = "readmeEN";
          break;
        case Program.LauncherLanguage.French:
          str = "readmeFR";
          break;
        case Program.LauncherLanguage.Spanish_PAL:
          str = "readmeES";
          break;
        case Program.LauncherLanguage.Spanish_NTSC:
          str = "readmeLAS";
          break;
        case Program.LauncherLanguage.German:
          str = "readmeDE";
          break;
        case Program.LauncherLanguage.Italian:
          str = "readmeIT";
          break;
        case Program.LauncherLanguage.Portuguese:
          str = "readmePT";
          break;
        case Program.LauncherLanguage.Dutch:
          str = "readmeNL";
          break;
        case Program.LauncherLanguage.Swedish:
          str = "readmeSV";
          break;
        case Program.LauncherLanguage.Danish:
          str = "readmeDA";
          break;
        case Program.LauncherLanguage.Norwegian:
          str = "readmeNO";
          break;
        case Program.LauncherLanguage.Russian:
          str = "readmeRU";
          break;
        case Program.LauncherLanguage.Arabic:
          str = "readmeAR";
          break;
        case Program.LauncherLanguage.Czech:
          str = "readmeCZ";
          break;
        case Program.LauncherLanguage.Hungarian:
          str = "readmeHU";
          break;
        case Program.LauncherLanguage.BrazilianPortuguese:
          str = "readmePT_BR";
          break;
        case Program.LauncherLanguage.Polish:
          str = "readmePL";
          break;
        case Program.LauncherLanguage.Turkish:
          str = "readmeTU";
          break;
        case Program.LauncherLanguage.Greek:
          str = "readmeEL";
          break;
      }
      Process.Start("notepad.exe", string.Format("Readme\\{0}.txt", (object) str));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.launcherLaunchButton = new Button();
      this.launcherLanguageLabel = new Label();
      this.languageComboBox = new ComboBox();
      this.logoPictureBox = new PictureBox();
      this.launcherOptionsButton = new Button();
      this.launcherViewReadme = new Button();
      ((ISupportInitialize) this.logoPictureBox).BeginInit();
      this.SuspendLayout();
      this.launcherLaunchButton.BackColor = Color.Transparent;
      this.launcherLaunchButton.Location = new Point(12, 253);
      this.launcherLaunchButton.Name = "launcherLaunchButton";
      this.launcherLaunchButton.Size = new Size(425, 64);
      this.launcherLaunchButton.TabIndex = 7;
      this.launcherLaunchButton.Text = "Play";
      this.launcherLaunchButton.UseVisualStyleBackColor = false;
      this.launcherLaunchButton.Click += new EventHandler(this.launchButton_Click);
      this.launcherLanguageLabel.BackColor = Color.Transparent;
      this.launcherLanguageLabel.Location = new Point(12, 204);
      this.launcherLanguageLabel.Name = "launcherLanguageLabel";
      this.launcherLanguageLabel.Size = new Size(200, 13);
      this.launcherLanguageLabel.TabIndex = 0;
      this.launcherLanguageLabel.Text = "Launcher Language";
      this.launcherLanguageLabel.TextAlign = ContentAlignment.TopRight;
      this.languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.languageComboBox.FormattingEnabled = true;
      this.languageComboBox.Location = new Point(218, 201);
      this.languageComboBox.Name = "languageComboBox";
      this.languageComboBox.Size = new Size(219, 21);
      this.languageComboBox.TabIndex = 0;
      this.languageComboBox.SelectedIndexChanged += new EventHandler(this.languageComboBox_SelectedIndexChanged);
      this.logoPictureBox.BackColor = Color.Transparent;
      this.logoPictureBox.Location = new Point(12, 12);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new Size(425, 182);
      this.logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
      this.logoPictureBox.TabIndex = 5;
      this.logoPictureBox.TabStop = false;
      this.launcherOptionsButton.Location = new Point(12, 322);
      this.launcherOptionsButton.Name = "launcherOptionsButton";
      this.launcherOptionsButton.Size = new Size(425, 38);
      this.launcherOptionsButton.TabIndex = 7;
      this.launcherOptionsButton.Text = "Display Options";
      this.launcherOptionsButton.UseVisualStyleBackColor = true;
      this.launcherOptionsButton.Click += new EventHandler(this.optionsButton_Click);
      this.launcherViewReadme.Location = new Point(12, 365);
      this.launcherViewReadme.Name = "launcherViewReadme";
      this.launcherViewReadme.Size = new Size(425, 27);
      this.launcherViewReadme.TabIndex = 7;
      this.launcherViewReadme.Text = "View Readme File";
      this.launcherViewReadme.UseVisualStyleBackColor = true;
      this.launcherViewReadme.Click += new EventHandler(this.readmeButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.BackColor = Color.White;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.ClientSize = new Size(447, 404);
      this.Controls.Add((Control) this.launcherLanguageLabel);
      this.Controls.Add((Control) this.languageComboBox);
      this.Controls.Add((Control) this.logoPictureBox);
      this.Controls.Add((Control) this.launcherViewReadme);
      this.Controls.Add((Control) this.launcherOptionsButton);
      this.Controls.Add((Control) this.launcherLaunchButton);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MainForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Disney Epic Mickey 2";
      ((ISupportInitialize) this.logoPictureBox).EndInit();
      this.ResumeLayout(false);
    }

    public struct RenderSettings
    {
      public int iWindowWidth;
      public int iWindowHeight;
      public int iAdapterIndex;
      public int iWindowDisplayMode;
      public int iDynamicShadowQuality;
      public int iGroundingShadowQuality;
      public bool bVSync;
      public bool bAntiAliasing;
    }
  }
}

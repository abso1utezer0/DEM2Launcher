// Decompiled with JetBrains decompiler
// Type: Launcher.OptionsForm
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
  public class OptionsForm : Form
  {
    private MainForm.RenderSettings m_kRenderSettings;
    private MainForm m_kParentForm;
    private IContainer components;
    private Label launcherDisplayModeLabel;
    private ComboBox displayModeComboBox;
    private ComboBox resolutionComboBox;
    private CheckBox launcherVSyncCheckBox;
    private Label launcherGroundingShadowResLabel;
    private ComboBox displayDeviceComboBox;
    private CheckBox launcherAntiAliasingCheckBox;
    private Label launcherDynamicShadowResLabel;
    private ComboBox dynamicShadowQualityComboBox;
    private Label launcherResolutionLabel;
    private Label launcherDisplayDeviceLabel;
    private ComboBox groundingShadowQualityComboBox;
    private Button launcherAcceptButton;
    private Button launcherCancelButton;

    public OptionsForm(MainForm kParentForm)
    {
      this.InitializeComponent();
      this.m_kParentForm = kParentForm;
      this.m_kRenderSettings = this.m_kParentForm.m_kRenderSettings;
      this.InitialiseFormValuesFromSettings();
      this.m_kParentForm.RecursiveTranslateControlLabels((Control) this, new Dictionary<string, string>());
    }

    private void InitialiseFormValuesFromSettings()
    {
      this.displayDeviceComboBox.Items.Clear();
      foreach (Program.ValidDisplayDevice validDisplayDevice in Program.ValidDisplayDevices)
      {
        string str = validDisplayDevice.kDisplayName;
        if (validDisplayDevice.bPrimaryDevice)
          str = str + " " + this.m_kParentForm.GetTranslation("launcherPrimaryDisplay", "launcherPrimaryDisplay");
        this.displayDeviceComboBox.Items.Add((object) str);
      }
      this.displayDeviceComboBox.SelectedIndex = this.m_kRenderSettings.iAdapterIndex;
      this.displayModeComboBox.Items.Clear();
      string[] strArray1 = new string[3]
      {
        "launcherDisplayModeWindow",
        "launcherDisplayModeFull",
        "launcherDisplayModeMaxWindow"
      };
      foreach (string str in strArray1)
        this.displayModeComboBox.Items.Add((object) this.m_kParentForm.GetTranslation(str, str));
      this.displayModeComboBox.SelectedIndex = this.m_kRenderSettings.iWindowDisplayMode;
      this.launcherVSyncCheckBox.Checked = this.m_kRenderSettings.bVSync;
      this.launcherAntiAliasingCheckBox.Checked = this.m_kRenderSettings.bAntiAliasing;
      this.dynamicShadowQualityComboBox.Items.Clear();
      this.groundingShadowQualityComboBox.Items.Clear();
      string[] strArray2 = new string[4]
      {
        "launcherQualityNone",
        "launcherQualityLow",
        "launcherQualityMedium",
        "launcherQualityHigh"
      };
      foreach (string str in strArray2)
      {
        this.dynamicShadowQualityComboBox.Items.Add((object) this.m_kParentForm.GetTranslation(str, str));
        this.groundingShadowQualityComboBox.Items.Add((object) this.m_kParentForm.GetTranslation(str, str));
      }
      this.dynamicShadowQualityComboBox.SelectedIndex = this.m_kRenderSettings.iDynamicShadowQuality;
      this.groundingShadowQualityComboBox.SelectedIndex = this.m_kRenderSettings.iGroundingShadowQuality;
      this.Text = this.m_kParentForm.GetTranslation("launcherOptionsButton", "launcherOptionsButton");
    }

    private void SetCurrentDisplayAdapter(int iAdapterIndex)
    {
      this.m_kRenderSettings.iAdapterIndex = iAdapterIndex;
      Program.ValidDisplayDevice validDisplayDevice = Program.ValidDisplayDevices[iAdapterIndex];
      this.resolutionComboBox.Items.Clear();
      int num = 0;
      bool flag = false;
      foreach (Program.ValidResolution displayResolution in validDisplayDevice.kValidDisplayResolutions)
      {
        string str = "";
        if ((double) displayResolution.iWidth * 3.0 / (double) displayResolution.iHeight == 4.0)
          str = "   (4:3)";
        else if ((double) displayResolution.iWidth * 9.0 / (double) displayResolution.iHeight == 16.0)
          str = "   (16:9)";
        else if ((double) displayResolution.iWidth * 10.0 / (double) displayResolution.iHeight == 16.0)
          str = "   (16:10)";
        else if ((double) displayResolution.iWidth * 10.0 / (double) displayResolution.iHeight == 15.0)
          str = "   (15:10)";
        else if ((double) displayResolution.iWidth * 9.0 / (double) displayResolution.iHeight == 15.0)
          str = "   (15:9)";
        else if ((double) displayResolution.iWidth * 4.0 / (double) displayResolution.iHeight == 5.0)
          str = "   (5:4)";
        this.resolutionComboBox.Items.Add((object) (displayResolution.iWidth.ToString() + "x" + (object) displayResolution.iHeight + str));
        if (displayResolution.iWidth == this.m_kRenderSettings.iWindowWidth && displayResolution.iHeight == this.m_kRenderSettings.iWindowHeight)
        {
          this.resolutionComboBox.SelectedIndex = num;
          flag = true;
        }
        ++num;
      }
      if (flag)
        return;
      this.resolutionComboBox.SelectedIndex = num - 1;
    }

    private void displayDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.SetCurrentDisplayAdapter(this.displayDeviceComboBox.SelectedIndex);

    private void displayModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.m_kRenderSettings.iWindowDisplayMode = this.displayModeComboBox.SelectedIndex;
      this.resolutionComboBox.Enabled = this.m_kRenderSettings.iWindowDisplayMode != 2;
    }

    private void resolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      string[] strArray = ((string) this.resolutionComboBox.SelectedItem).Split('x', ' ');
      this.m_kRenderSettings.iWindowWidth = Convert.ToInt32(strArray[0]);
      this.m_kRenderSettings.iWindowHeight = Convert.ToInt32(strArray[1]);
    }

    private void vSyncCheckBox_CheckedChanged(object sender, EventArgs e) => this.m_kRenderSettings.bVSync = this.launcherVSyncCheckBox.Checked;

    private void antiAliasingCheckBox_CheckedChanged(object sender, EventArgs e) => this.m_kRenderSettings.bAntiAliasing = this.launcherAntiAliasingCheckBox.Checked;

    private void dynamicShadowQualityComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.m_kRenderSettings.iDynamicShadowQuality = this.dynamicShadowQualityComboBox.SelectedIndex;

    private void groundingShadowQualityComboBox_SelectedIndexChanged(object sender, EventArgs e) => this.m_kRenderSettings.iGroundingShadowQuality = this.groundingShadowQualityComboBox.SelectedIndex;

    private void acceptButton_Click(object sender, EventArgs e)
    {
      this.m_kParentForm.m_kRenderSettings = this.m_kRenderSettings;
      this.Close();
    }

    private void cancelButton_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OptionsForm));
      this.launcherDisplayModeLabel = new Label();
      this.displayModeComboBox = new ComboBox();
      this.resolutionComboBox = new ComboBox();
      this.launcherVSyncCheckBox = new CheckBox();
      this.launcherGroundingShadowResLabel = new Label();
      this.displayDeviceComboBox = new ComboBox();
      this.launcherAntiAliasingCheckBox = new CheckBox();
      this.launcherDynamicShadowResLabel = new Label();
      this.dynamicShadowQualityComboBox = new ComboBox();
      this.launcherResolutionLabel = new Label();
      this.launcherDisplayDeviceLabel = new Label();
      this.groundingShadowQualityComboBox = new ComboBox();
      this.launcherAcceptButton = new Button();
      this.launcherCancelButton = new Button();
      this.SuspendLayout();
      this.launcherDisplayModeLabel.BackColor = Color.Transparent;
      this.launcherDisplayModeLabel.Location = new Point(12, 36);
      this.launcherDisplayModeLabel.Name = "launcherDisplayModeLabel";
      this.launcherDisplayModeLabel.Size = new Size(211, 13);
      this.launcherDisplayModeLabel.TabIndex = 3;
      this.launcherDisplayModeLabel.Text = "Display Mode";
      this.launcherDisplayModeLabel.TextAlign = ContentAlignment.TopRight;
      this.displayModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.displayModeComboBox.FormattingEnabled = true;
      this.displayModeComboBox.Location = new Point(229, 33);
      this.displayModeComboBox.Name = "displayModeComboBox";
      this.displayModeComboBox.Size = new Size(260, 21);
      this.displayModeComboBox.TabIndex = 2;
      this.displayModeComboBox.SelectedIndexChanged += new EventHandler(this.displayModeComboBox_SelectedIndexChanged);
      this.resolutionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.resolutionComboBox.FormattingEnabled = true;
      this.resolutionComboBox.Location = new Point(229, 60);
      this.resolutionComboBox.Name = "resolutionComboBox";
      this.resolutionComboBox.Size = new Size(260, 21);
      this.resolutionComboBox.TabIndex = 3;
      this.resolutionComboBox.SelectedIndexChanged += new EventHandler(this.resolutionComboBox_SelectedIndexChanged);
      this.launcherVSyncCheckBox.BackColor = Color.Transparent;
      this.launcherVSyncCheckBox.CheckAlign = ContentAlignment.MiddleRight;
      this.launcherVSyncCheckBox.Location = new Point(12, 100);
      this.launcherVSyncCheckBox.Name = "launcherVSyncCheckBox";
      this.launcherVSyncCheckBox.Size = new Size(231, 17);
      this.launcherVSyncCheckBox.TabIndex = 4;
      this.launcherVSyncCheckBox.Text = "Vertical Sync";
      this.launcherVSyncCheckBox.TextAlign = ContentAlignment.TopRight;
      this.launcherVSyncCheckBox.UseVisualStyleBackColor = false;
      this.launcherVSyncCheckBox.CheckedChanged += new EventHandler(this.vSyncCheckBox_CheckedChanged);
      this.launcherGroundingShadowResLabel.BackColor = Color.Transparent;
      this.launcherGroundingShadowResLabel.Location = new Point(12, 180);
      this.launcherGroundingShadowResLabel.Name = "launcherGroundingShadowResLabel";
      this.launcherGroundingShadowResLabel.Size = new Size(211, 13);
      this.launcherGroundingShadowResLabel.TabIndex = 28;
      this.launcherGroundingShadowResLabel.Text = "Grounding Shadow Quality";
      this.launcherGroundingShadowResLabel.TextAlign = ContentAlignment.TopRight;
      this.displayDeviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.displayDeviceComboBox.FormattingEnabled = true;
      this.displayDeviceComboBox.Location = new Point(229, 6);
      this.displayDeviceComboBox.Name = "displayDeviceComboBox";
      this.displayDeviceComboBox.Size = new Size(260, 21);
      this.displayDeviceComboBox.TabIndex = 1;
      this.displayDeviceComboBox.SelectedIndexChanged += new EventHandler(this.displayDeviceComboBox_SelectedIndexChanged);
      this.launcherAntiAliasingCheckBox.BackColor = Color.Transparent;
      this.launcherAntiAliasingCheckBox.CheckAlign = ContentAlignment.MiddleRight;
      this.launcherAntiAliasingCheckBox.Location = new Point(12, 123);
      this.launcherAntiAliasingCheckBox.Name = "launcherAntiAliasingCheckBox";
      this.launcherAntiAliasingCheckBox.Size = new Size(231, 17);
      this.launcherAntiAliasingCheckBox.TabIndex = 5;
      this.launcherAntiAliasingCheckBox.Text = "Anti-aliasing";
      this.launcherAntiAliasingCheckBox.TextAlign = ContentAlignment.TopRight;
      this.launcherAntiAliasingCheckBox.UseVisualStyleBackColor = false;
      this.launcherAntiAliasingCheckBox.CheckedChanged += new EventHandler(this.antiAliasingCheckBox_CheckedChanged);
      this.launcherDynamicShadowResLabel.BackColor = Color.Transparent;
      this.launcherDynamicShadowResLabel.Location = new Point(12, 153);
      this.launcherDynamicShadowResLabel.Name = "launcherDynamicShadowResLabel";
      this.launcherDynamicShadowResLabel.Size = new Size(211, 13);
      this.launcherDynamicShadowResLabel.TabIndex = 28;
      this.launcherDynamicShadowResLabel.Text = "Dynamic Shadow Quality";
      this.launcherDynamicShadowResLabel.TextAlign = ContentAlignment.TopRight;
      this.dynamicShadowQualityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.dynamicShadowQualityComboBox.FormattingEnabled = true;
      this.dynamicShadowQualityComboBox.Location = new Point(229, 150);
      this.dynamicShadowQualityComboBox.Name = "dynamicShadowQualityComboBox";
      this.dynamicShadowQualityComboBox.Size = new Size(260, 21);
      this.dynamicShadowQualityComboBox.TabIndex = 29;
      this.dynamicShadowQualityComboBox.SelectedIndexChanged += new EventHandler(this.dynamicShadowQualityComboBox_SelectedIndexChanged);
      this.launcherResolutionLabel.BackColor = Color.Transparent;
      this.launcherResolutionLabel.Location = new Point(12, 63);
      this.launcherResolutionLabel.Name = "launcherResolutionLabel";
      this.launcherResolutionLabel.Size = new Size(211, 13);
      this.launcherResolutionLabel.TabIndex = 1;
      this.launcherResolutionLabel.Text = "Display Resolution";
      this.launcherResolutionLabel.TextAlign = ContentAlignment.TopRight;
      this.launcherDisplayDeviceLabel.BackColor = Color.Transparent;
      this.launcherDisplayDeviceLabel.Location = new Point(12, 9);
      this.launcherDisplayDeviceLabel.Name = "launcherDisplayDeviceLabel";
      this.launcherDisplayDeviceLabel.Size = new Size(211, 13);
      this.launcherDisplayDeviceLabel.TabIndex = 4;
      this.launcherDisplayDeviceLabel.Text = "Display Device";
      this.launcherDisplayDeviceLabel.TextAlign = ContentAlignment.TopRight;
      this.groundingShadowQualityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.groundingShadowQualityComboBox.FormattingEnabled = true;
      this.groundingShadowQualityComboBox.Location = new Point(229, 177);
      this.groundingShadowQualityComboBox.Name = "groundingShadowQualityComboBox";
      this.groundingShadowQualityComboBox.Size = new Size(260, 21);
      this.groundingShadowQualityComboBox.TabIndex = 29;
      this.groundingShadowQualityComboBox.SelectedIndexChanged += new EventHandler(this.groundingShadowQualityComboBox_SelectedIndexChanged);
      this.launcherAcceptButton.BackColor = Color.Transparent;
      this.launcherAcceptButton.Location = new Point(15, 218);
      this.launcherAcceptButton.Name = "launcherAcceptButton";
      this.launcherAcceptButton.Size = new Size(303, 23);
      this.launcherAcceptButton.TabIndex = 30;
      this.launcherAcceptButton.Text = "Accept";
      this.launcherAcceptButton.UseVisualStyleBackColor = false;
      this.launcherAcceptButton.Click += new EventHandler(this.acceptButton_Click);
      this.launcherCancelButton.BackColor = Color.Transparent;
      this.launcherCancelButton.DialogResult = DialogResult.Cancel;
      this.launcherCancelButton.Location = new Point(324, 218);
      this.launcherCancelButton.Name = "launcherCancelButton";
      this.launcherCancelButton.Size = new Size(165, 23);
      this.launcherCancelButton.TabIndex = 30;
      this.launcherCancelButton.Text = "Cancel";
      this.launcherCancelButton.UseVisualStyleBackColor = false;
      this.launcherCancelButton.Click += new EventHandler(this.cancelButton_Click);
      this.AcceptButton = (IButtonControl) this.launcherAcceptButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.CancelButton = (IButtonControl) this.launcherCancelButton;
      this.ClientSize = new Size(499, 253);
      this.Controls.Add((Control) this.launcherCancelButton);
      this.Controls.Add((Control) this.launcherAcceptButton);
      this.Controls.Add((Control) this.groundingShadowQualityComboBox);
      this.Controls.Add((Control) this.dynamicShadowQualityComboBox);
      this.Controls.Add((Control) this.launcherDisplayDeviceLabel);
      this.Controls.Add((Control) this.displayDeviceComboBox);
      this.Controls.Add((Control) this.launcherVSyncCheckBox);
      this.Controls.Add((Control) this.displayModeComboBox);
      this.Controls.Add((Control) this.launcherDisplayModeLabel);
      this.Controls.Add((Control) this.resolutionComboBox);
      this.Controls.Add((Control) this.launcherGroundingShadowResLabel);
      this.Controls.Add((Control) this.launcherAntiAliasingCheckBox);
      this.Controls.Add((Control) this.launcherDynamicShadowResLabel);
      this.Controls.Add((Control) this.launcherResolutionLabel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OptionsForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Display Options";
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}

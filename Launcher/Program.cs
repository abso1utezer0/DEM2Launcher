// Decompiled with JetBrains decompiler
// Type: Launcher.Program
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using InputMapper;
using Launcher.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Launcher
{
  public static class Program
  {
    public static List<Program.ValidDisplayDevice> ValidDisplayDevices = new List<Program.ValidDisplayDevice>();
    public static Program.WindowsVersion DetectedWindowsVersion = Program.WindowsVersion.Unknown;
    public static Dictionary<Program.LauncherLanguage, Dictionary<string, string>> m_akTranslations = (Dictionary<Program.LauncherLanguage, Dictionary<string, string>>) null;
    public static List<Program.LauncherLanguage> m_kValidLanguageList = new List<Program.LauncherLanguage>();
    public static Program.LauncherLanguage DefaultSystemLanguage = Program.LauncherLanguage.English;

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayDevices(
      string lpDevice,
      uint uiDeviceIndex,
      ref Program.DisplayDevice lpDisplayDevice,
      uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool EnumDisplaySettings(
      string lpDevice,
      uint uiDeviceIndex,
      ref Program.DisplayMode kDisplayMode);

    [DllImport("DEM2_lsi.dll")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static extern string GetSteamLanguage();

    private static void DetectWindowsVersion()
    {
      OperatingSystem osVersion = Environment.OSVersion;
      switch (osVersion.Platform)
      {
        case PlatformID.Win32S:
          Program.DetectedWindowsVersion = Program.WindowsVersion.Windows31;
          break;
        case PlatformID.Win32Windows:
          switch (osVersion.Version.Minor)
          {
            case 0:
              Program.DetectedWindowsVersion = Program.WindowsVersion.Windows95;
              return;
            case 10:
              if (osVersion.Version.Revision.ToString() == "2222A")
              {
                Program.DetectedWindowsVersion = Program.WindowsVersion.Windows98SE;
                return;
              }
              Program.DetectedWindowsVersion = Program.WindowsVersion.Windows98;
              return;
            case 90:
              Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsME;
              return;
            default:
              return;
          }
        case PlatformID.Win32NT:
          switch (osVersion.Version.Major)
          {
            case 3:
              Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsNT351;
              break;
            case 4:
              Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsNT4;
              break;
            case 5:
              switch (osVersion.Version.Minor)
              {
                case 0:
                  Program.DetectedWindowsVersion = Program.WindowsVersion.Windows2K;
                  break;
                case 1:
                  Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsXP;
                  break;
                case 2:
                  Program.DetectedWindowsVersion = Program.WindowsVersion.Windows2003;
                  break;
              }
              break;
            case 6:
              switch (osVersion.Version.Minor)
              {
                case 0:
                  Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsVista;
                  break;
                case 1:
                case 2:
                  Program.DetectedWindowsVersion = Program.WindowsVersion.Windows7;
                  break;
              }
              break;
          }
          if (osVersion.Version.Major <= 6)
            break;
          Program.DetectedWindowsVersion = Program.WindowsVersion.Newer;
          break;
        case PlatformID.WinCE:
          Program.DetectedWindowsVersion = Program.WindowsVersion.WindowsCE;
          break;
      }
    }

    private static void LoadTranslations()
    {
      Program.m_akTranslations = new Dictionary<Program.LauncherLanguage, Dictionary<string, string>>();
      for (int index = 0; index < Enum.GetNames(typeof (Program.LauncherLanguage)).Length; ++index)
      {
        Program.LauncherLanguage key = (Program.LauncherLanguage) index;
        Program.m_akTranslations.Add(key, new Dictionary<string, string>());
      }
      string[] strArray1 = new string[0];
      try
      {
        strArray1 = Directory.GetFiles("Localize", "*_WIN32.dct", SearchOption.TopDirectoryOnly);
      }
      catch (DirectoryNotFoundException ex)
      {
      }
      catch (IOException ex)
      {
      }
      for (int index1 = 0; index1 < Enum.GetNames(typeof (Program.LauncherLanguage)).Length; ++index1)
      {
        Program.LauncherLanguage launcherLanguage = (Program.LauncherLanguage) index1;
        string lower1 = launcherLanguage.ToString().ToLower();
        if (strArray1.Length == 0)
        {
          Program.m_kValidLanguageList.Add(launcherLanguage);
        }
        else
        {
          for (int index2 = 0; index2 < strArray1.Length; ++index2)
          {
            string lower2 = strArray1[index2].Split(new string[2]
            {
              "Localize\\",
              "_WIN32.dct"
            }, StringSplitOptions.None)[1].ToLower();
            if (lower2 == lower1 || lower2 == lower1 + "_pal" || lower2 == lower1 + "_ntsc")
            {
              Program.m_kValidLanguageList.Add(launcherLanguage);
              break;
            }
          }
        }
      }
      string locStrings = Assets.LocStrings;
      char[] chArray = new char[1]{ '\n' };
      foreach (string str in locStrings.Split(chArray))
      {
        char[] separator = new char[2]{ '\t', '\r' };
        string[] strArray2 = str.Split(separator, StringSplitOptions.None);
        if (strArray2.Length >= 2)
        {
          for (int index = 0; index < Enum.GetNames(typeof (Program.LauncherLanguage)).Length; ++index)
          {
            Program.LauncherLanguage key = (Program.LauncherLanguage) index;
            Dictionary<string, string> dictionary;
            if (Program.m_akTranslations.TryGetValue(key, out dictionary))
            {
              if (index + 1 < strArray2.Length && strArray2[index + 1].Length > 0)
                dictionary.Add(strArray2[0], strArray2[index + 1]);
              else
                dictionary.Add(strArray2[0], strArray2[0]);
            }
          }
        }
      }
    }

    private static void DetectDisplayDevices()
    {
      Program.DisplayDevice lpDisplayDevice = new Program.DisplayDevice();
      lpDisplayDevice.cb = Marshal.SizeOf((object) lpDisplayDevice);
      for (uint index = 0; Program.EnumDisplayDevices((string) null, index, ref lpDisplayDevice, 0U); ++index)
      {
        if ((lpDisplayDevice.StateFlags & Program.DisplayDeviceStateFlags.AttachedToDesktop) != (Program.DisplayDeviceStateFlags) 0)
        {
          List<Program.ValidResolution> _kValidDisplayResolutions = new List<Program.ValidResolution>();
          Program.DisplayMode kDisplayMode = new Program.DisplayMode();
          for (uint uiDeviceIndex = 0; Program.EnumDisplaySettings(lpDisplayDevice.DeviceName, uiDeviceIndex, ref kDisplayMode); ++uiDeviceIndex)
          {
            if (kDisplayMode.dmBitsPerPel == 32 && kDisplayMode.dmPelsWidth >= 640 && kDisplayMode.dmPelsHeight >= 480)
            {
              Program.ValidResolution validResolution = new Program.ValidResolution(kDisplayMode.dmPelsWidth, kDisplayMode.dmPelsHeight);
              if (!_kValidDisplayResolutions.Contains(validResolution))
                _kValidDisplayResolutions.Add(validResolution);
            }
          }
          bool _bPrimaryDevice = (lpDisplayDevice.StateFlags & Program.DisplayDeviceStateFlags.PrimaryDevice) != (Program.DisplayDeviceStateFlags) 0;
          Program.ValidDisplayDevice validDisplayDevice = new Program.ValidDisplayDevice(index, lpDisplayDevice.DeviceString.Trim(), _bPrimaryDevice, _kValidDisplayResolutions);
          if (!Program.ValidDisplayDevices.Contains(validDisplayDevice))
            Program.ValidDisplayDevices.Add(validDisplayDevice);
        }
      }
    }

    private static bool GetCurrentSystemLanguage()
    {
      string str = (string) null;
      try
      {
        str = Program.GetSteamLanguage();
      }
      catch (Exception ex)
      {
        StreamWriter streamWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Disney Interactive Studios\\Epic Mickey 2\\LauncherErrors.txt");
        streamWriter.WriteLine(ex.ToString());
        streamWriter.Flush();
        streamWriter.Close();
      }
      if (str != null)
      {
        if (str.Equals("__restart__"))
          return false;
        Program.DefaultSystemLanguage = Program.LauncherLanguage.English;
        if (str.Equals("brazilian"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.BrazilianPortuguese;
        else if (str.Equals("czech"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Czech;
        else if (str.Equals("danish"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Danish;
        else if (str.Equals("dutch"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Dutch;
        else if (str.Equals("english"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.English;
        else if (str.Equals("french"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.French;
        else if (str.Equals("german"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.German;
        else if (str.Equals("hungarian"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Hungarian;
        else if (str.Equals("italian"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Italian;
        else if (str.Equals("norwegian"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Norwegian;
        else if (str.Equals("polish"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Polish;
        else if (str.Equals("portuguese"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Portuguese;
        else if (str.Equals("russian"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Russian;
        else if (str.Equals("spanish"))
          Program.DefaultSystemLanguage = !Program.m_kValidLanguageList.Contains(Program.LauncherLanguage.Spanish_NTSC) ? Program.LauncherLanguage.Spanish_PAL : Program.LauncherLanguage.Spanish_NTSC;
        else if (str.Equals("swedish"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Swedish;
        else if (str.Equals("turkish"))
          Program.DefaultSystemLanguage = Program.LauncherLanguage.Turkish;
      }
      else
      {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        switch (currentCulture.TwoLetterISOLanguageName)
        {
          case "en":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.English;
            break;
          case "fr":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.French;
            break;
          case "es":
            Program.DefaultSystemLanguage = !(currentCulture.Name == "es-MX") ? Program.LauncherLanguage.Spanish_PAL : Program.LauncherLanguage.Spanish_NTSC;
            break;
          case "de":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.German;
            break;
          case "it":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Italian;
            break;
          case "pt":
            Program.DefaultSystemLanguage = !(currentCulture.Name == "pt-BR") ? Program.LauncherLanguage.Portuguese : Program.LauncherLanguage.BrazilianPortuguese;
            break;
          case "nl":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Dutch;
            break;
          case "sv":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Swedish;
            break;
          case "da":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Danish;
            break;
          case "no":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Norwegian;
            break;
          case "ru":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Russian;
            break;
          case "ar":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Arabic;
            break;
          case "cs":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Czech;
            break;
          case "hu":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Hungarian;
            break;
          case "pl":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Polish;
            break;
          case "tr":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Turkish;
            break;
          case "el":
            Program.DefaultSystemLanguage = Program.LauncherLanguage.Greek;
            break;
          default:
            Program.DefaultSystemLanguage = Program.LauncherLanguage.English;
            break;
        }
      }
      return true;
    }

    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Program.LoadTranslations();
      Program.DetectWindowsVersion();
      Program.DetectDisplayDevices();
      bool currentSystemLanguage = Program.GetCurrentSystemLanguage();
      Initializer.Initialize();
      bool createdNew;
      Mutex mutex = new Mutex(true, "Global\\DEM2_Launcher", out createdNew);
      if (!createdNew || !currentSystemLanguage)
        return;
      Application.Run((Form) new MainForm());
      mutex.ReleaseMutex();
    }

    public struct DisplayDevice
    {
      [MarshalAs(UnmanagedType.U4)]
      public int cb;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string DeviceName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceString;
      [MarshalAs(UnmanagedType.U4)]
      public Program.DisplayDeviceStateFlags StateFlags;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceID;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceKey;
    }

    [Flags]
    public enum DisplayDeviceStateFlags
    {
      AttachedToDesktop = 1,
      MultiDriver = 2,
      PrimaryDevice = 4,
      MirroringDriver = 8,
      VGACompatible = 16, // 0x00000010
      Removable = 32, // 0x00000020
      UnsafeMode = 524288, // 0x00080000
      TSCompatible = 2097152, // 0x00200000
      Disconnect = 33554432, // 0x02000000
      Remote = 67108864, // 0x04000000
      ModesPruned = 134217728, // 0x08000000
    }

    public struct DisplayMode
    {
      private const int CCHDEVICENAME = 32;
      private const int CCHFORMNAME = 32;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string dmDeviceName;
      public short dmSpecVersion;
      public short dmDriverVersion;
      public short dmSize;
      public short dmDriverExtra;
      public int dmFields;
      public int dmPositionX;
      public int dmPositionY;
      public ScreenOrientation dmDisplayOrientation;
      public int dmDisplayFixedOutput;
      public short dmColor;
      public short dmDuplex;
      public short dmYResolution;
      public short dmTTOption;
      public short dmCollate;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string dmFormName;
      public short dmLogPixels;
      public int dmBitsPerPel;
      public int dmPelsWidth;
      public int dmPelsHeight;
      public int dmDisplayFlags;
      public int dmDisplayFrequency;
      public int dmICMMethod;
      public int dmICMIntent;
      public int dmMediaType;
      public int dmDitherType;
      public int dmReserved1;
      public int dmReserved2;
      public int dmPanningWidth;
      public int dmPanningHeight;
    }

    public struct ValidResolution
    {
      public int iWidth;
      public int iHeight;

      public ValidResolution(int _iWidth, int _iHeight)
      {
        this.iWidth = _iWidth;
        this.iHeight = _iHeight;
      }
    }

    public struct ValidDisplayDevice
    {
      public uint uiAdapterIndex;
      public string kDisplayName;
      public bool bPrimaryDevice;
      public List<Program.ValidResolution> kValidDisplayResolutions;

      public ValidDisplayDevice(
        uint _uiAdapterIndex,
        string _kDisplayName,
        bool _bPrimaryDevice,
        List<Program.ValidResolution> _kValidDisplayResolutions)
      {
        this.uiAdapterIndex = _uiAdapterIndex;
        this.kDisplayName = _kDisplayName;
        this.bPrimaryDevice = _bPrimaryDevice;
        this.kValidDisplayResolutions = _kValidDisplayResolutions;
      }
    }

    public enum WindowsVersion
    {
      Unknown,
      Windows31,
      Windows95,
      Windows98,
      Windows98SE,
      WindowsME,
      WindowsCE,
      WindowsNT351,
      WindowsNT4,
      Windows2K,
      WindowsXP,
      Windows2003,
      WindowsVista,
      Windows7,
      Newer,
    }

    public enum LauncherLanguage
    {
      English,
      French,
      Spanish_PAL,
      Spanish_NTSC,
      German,
      Italian,
      Portuguese,
      Dutch,
      Swedish,
      Danish,
      Norwegian,
      Russian,
      Arabic,
      Czech,
      Hungarian,
      BrazilianPortuguese,
      Polish,
      Turkish,
      Greek,
    }

    public struct SteamLanguageData
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string Language;
    }
  }
}

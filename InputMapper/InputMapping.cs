// Decompiled with JetBrains decompiler
// Type: InputMapper.InputMapping
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace InputMapper
{
  [XmlType("mapping")]
  public class InputMapping : ICloneable
  {
    private int m_iDevice;
    private bool m_boInvert;
    private float m_fRepeatTime;
    [XmlIgnore]
    private int m_iPlayer;
    private int m_iValueIndex;
    [XmlIgnore]
    private bool m_boHasPlayer1Mapping;
    [XmlIgnore]
    private InputActionPlatform m_kOwnerObject;
    public static List<string> ms_kPlayerStrings;
    public static List<string> ms_kAllDeviceStrings;
    public static List<string> ms_kValueIndexStrings;

    [Category("Mapping")]
    [XmlAttribute("device")]
    [TypeConverter(typeof (InputDeviceConverter))]
    public string InputDeviceString
    {
      get => InputMapping.ms_kAllDeviceStrings[this.m_iDevice];
      set
      {
        for (int index = 0; index < InputMapping.ms_kAllDeviceStrings.Count; ++index)
        {
          if (InputMapping.ms_kAllDeviceStrings[index] == value)
          {
            this.m_iDevice = index;
            break;
          }
        }
      }
    }

    [XmlAttribute("invert")]
    [Category("Mapping")]
    public bool Inverted
    {
      get => this.m_boInvert;
      set => this.m_boInvert = value;
    }

    [XmlAttribute("keyRepeat")]
    [Category("Mapping")]
    public float RepeatTime
    {
      get => this.m_fRepeatTime;
      set => this.m_fRepeatTime = value;
    }

    [XmlIgnore]
    [Browsable(false)]
    public int Player
    {
      get => this.m_iPlayer;
      set => this.m_iPlayer = value;
    }

    [XmlAttribute("player")]
    [TypeConverter(typeof (InputPlayerConverter))]
    [Category("Mapping")]
    public string InputPlayerString
    {
      get => InputMapping.ms_kPlayerStrings[this.m_iPlayer];
      set
      {
        for (int index = 0; index < InputMapping.ms_kPlayerStrings.Count; ++index)
        {
          if (InputMapping.ms_kPlayerStrings[index] == value)
          {
            this.m_iPlayer = index;
            break;
          }
        }
      }
    }

    [Category("Mapping")]
    [TypeConverter(typeof (InputValueIndexConverter))]
    [XmlAttribute("valueIndex")]
    public string InputValueIndexString
    {
      get => InputMapping.ms_kValueIndexStrings[this.m_iValueIndex];
      set
      {
        for (int index = 0; index < InputMapping.ms_kValueIndexStrings.Count; ++index)
        {
          if (InputMapping.ms_kValueIndexStrings[index] == value)
          {
            this.m_iValueIndex = index;
            break;
          }
        }
      }
    }

    public override string ToString() => this.Owner.ValueMappingString == "VM_1D" || this.Owner.ValueMappingString == "VM_Combo" ? this.InputDeviceString : this.InputDeviceString + ": " + this.InputValueIndexString;

    [XmlIgnore]
    [Browsable(false)]
    public bool HasPlayer1Mapping => this.m_boHasPlayer1Mapping;

    public void CheckPlayerMappings() => this.m_boHasPlayer1Mapping = this.Player == 0 || this.Player == 7;

    [XmlIgnore]
    [Browsable(false)]
    public InputActionPlatform Owner
    {
      get => this.m_kOwnerObject;
      set => this.m_kOwnerObject = value;
    }

    public static void InitializeStrings()
    {
      InputMapping.ms_kPlayerStrings = new List<string>();
      InputMapping.ms_kPlayerStrings.Add("PlayerOne");
      InputMapping.ms_kPlayerStrings.Add("PlayerTwo");
      InputMapping.ms_kPlayerStrings.Add("PlayerThree");
      InputMapping.ms_kPlayerStrings.Add("PlayerFour");
      InputMapping.ms_kPlayerStrings.Add("PlayerFive");
      InputMapping.ms_kPlayerStrings.Add("PlayerSix");
      InputMapping.ms_kPlayerStrings.Add("PlayerSeven");
      InputMapping.ms_kPlayerStrings.Add("PlayerAll");
      InputMapping.ms_kValueIndexStrings = new List<string>();
      InputMapping.ms_kValueIndexStrings.Add("VI_ValueX");
      InputMapping.ms_kValueIndexStrings.Add("VI_ValuePosX");
      InputMapping.ms_kValueIndexStrings.Add("VI_ValueNegX");
      InputMapping.ms_kValueIndexStrings.Add("VI_ValueY");
      InputMapping.ms_kValueIndexStrings.Add("VI_ValuePosY");
      InputMapping.ms_kValueIndexStrings.Add("VI_ValueNegY");
      InputMapping.ms_kAllDeviceStrings = new List<string>();
      InputMapping.ms_kAllDeviceStrings.Add("ID_Button");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Axis");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_ESCAPE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_3");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_4");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_5");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_6");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_7");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_8");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_9");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_0");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MINUS");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_EQUALS");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_BACK");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_TAB");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_Q");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_W");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_E");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_R");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_T");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_Y");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_U");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_I");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_O");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_P");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LBRACKET");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RBRACKET");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RETURN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LCONTROL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_A");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_S");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_D");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_G");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_H");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_J");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_K");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_L");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SEMICOLON");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_APOSTROPHE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_GRAVE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LSHIFT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_BACKSLASH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_Z");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_X");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_C");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_V");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_B");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_N");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_M");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_COMMA");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_PERIOD");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SLASH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RSHIFT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MULTIPLY");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LMENU");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SPACE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_CAPITAL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F3");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F4");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F5");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F6");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F7");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F8");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F9");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F10");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMLOCK");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SCROLL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD7");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD8");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD9");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SUBTRACT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD4");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD5");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD6");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_ADD");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD3");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPAD0");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_DECIMAL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_OEM_102");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F11");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F12");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F13");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F14");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_F15");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_KANA");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_ABNT_C1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_CONVERT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NOCONVERT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_YEN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_ABNT_C2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPADEQUALS");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_PREVTRACK");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_AT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_COLON");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_UNDERLINE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_KANJI");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_STOP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_AX");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_UNLABELED");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NEXTTRACK");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPADENTER");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RCONTROL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MUTE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_CALCULATOR");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_PLAYPAUSE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MEDIASTOP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_VOLUMEDOWN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_VOLUMEUP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBHOME");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NUMPADCOMMA");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_DIVIDE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SYSRQ");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RMENU");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_PAUSE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_HOME");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_UP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_PRIOR");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LEFT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RIGHT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_END");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_DOWN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_NEXT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_INSERT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_DELETE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_LWIN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_RWIN");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_APPS");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_POWER");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_SLEEP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WAKE");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBSEARCH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBFAVORITES");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBREFRESH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBSTOP");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBFORWARD");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_WEBBACK");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MYCOMPUTER");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MAIL");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Keyboard_MEDIASELECT");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Mouse");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseLButton");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseMButton");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseRButton");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisWheel");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisDeltaX");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisDeltaY");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisDeltaWheel");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisPosX");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisPosY");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseAxisPosWheel");
      InputMapping.ms_kAllDeviceStrings.Add("ID_MouseInsideWindow");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_LPadUp");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_LPadDown");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_LPadLeft");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_LPadRight");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_RPadUp");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_RPadDown");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_RPadLeft");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_RPadRight");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_L1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_L2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_R1");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_R2");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_A");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_B");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Start");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Select");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_X");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Y");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_L3");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_R3");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Axis_LeftH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Axis_LeftV");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Axis_RightH");
      InputMapping.ms_kAllDeviceStrings.Add("ID_Gamepad_Axis_RightV");
    }

    public object Clone()
    {
      InputMapping inputMapping = new InputMapping();
      inputMapping.m_iDevice = this.m_iDevice;
      inputMapping.m_boInvert = this.m_boInvert;
      inputMapping.m_fRepeatTime = this.m_fRepeatTime;
      inputMapping.m_iPlayer = this.m_iPlayer;
      inputMapping.m_iValueIndex = this.m_iValueIndex;
      inputMapping.m_kOwnerObject = (InputActionPlatform) null;
      inputMapping.CheckPlayerMappings();
      return (object) inputMapping;
    }
  }
}

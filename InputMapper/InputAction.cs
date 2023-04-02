// Decompiled with JetBrains decompiler
// Type: InputMapper.InputAction
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Xml.Serialization;

namespace InputMapper
{
  public class InputAction : IComparable<InputAction>, ICloneable
  {
    private string m_kName = "DefaultAction";
    private InputActionPlatform m_kWin32Actions;
    [XmlIgnore]
    private Context m_kOwnerObject;

    public InputAction() => this.m_kWin32Actions = new InputActionPlatform();

    [XmlAttribute("name")]
    public string Name
    {
      get => this.m_kName;
      set => this.m_kName = value;
    }

    [XmlElement("win32")]
    public InputActionPlatform Win32Actions
    {
      get => this.m_kWin32Actions;
      set
      {
        this.m_kWin32Actions = value;
        this.m_kWin32Actions.CheckPlayerMappings();
      }
    }

    [XmlIgnore]
    public Context Owner
    {
      get => this.m_kOwnerObject;
      set
      {
        this.m_kOwnerObject = value;
        this.m_kWin32Actions.Owner = this;
      }
    }

    public override string ToString() => this.m_kName;

    public int CompareTo(InputAction other) => this.Name.CompareTo(other.Name);

    public object Clone()
    {
      InputAction inputAction = new InputAction();
      inputAction.m_kName = (string) this.m_kName.Clone();
      inputAction.m_kOwnerObject = this.m_kOwnerObject;
      inputAction.m_kWin32Actions = this.m_kWin32Actions.Clone() as InputActionPlatform;
      inputAction.m_kWin32Actions.CheckPlayerMappings();
      return (object) inputAction;
    }
  }
}

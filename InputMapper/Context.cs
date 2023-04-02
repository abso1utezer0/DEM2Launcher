// Decompiled with JetBrains decompiler
// Type: InputMapper.Context
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace InputMapper
{
  [XmlType("context")]
  public class Context : IComparable<Context>
  {
    private string m_kName = "MainGame";
    [XmlElement("action")]
    public List<InputAction> m_kActions = new List<InputAction>();
    [XmlIgnore]
    private InputContainer m_kOwnerObject;

    [XmlAttribute("name")]
    public string Name
    {
      get => this.m_kName;
      set => this.m_kName = value;
    }

    [XmlIgnore]
    public InputContainer Owner
    {
      get => this.m_kOwnerObject;
      set
      {
        this.m_kOwnerObject = value;
        foreach (InputAction kAction in this.m_kActions)
          kAction.Owner = this;
      }
    }

    public override string ToString() => this.m_kName;

    public void Sort() => this.m_kActions.Sort();

    public int GetNumActions() => this.m_kActions.Count;

    public InputAction GetAction(int i) => this.m_kActions[i];

    public bool HasAction(string actionName)
    {
      foreach (InputAction kAction in this.m_kActions)
      {
        if (kAction.Name == actionName)
          return true;
      }
      return false;
    }

    public void AddAction(InputAction kAction)
    {
      this.m_kActions.Add(kAction);
      this.Sort();
    }

    public void RemoveAction(InputAction kAction) => this.m_kActions.Remove(kAction);

    public int CompareTo(Context other) => this.Name.CompareTo(other.Name);
  }
}

// Decompiled with JetBrains decompiler
// Type: InputMapper.InputContainer
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace InputMapper
{
  [XmlRoot("input")]
  public class InputContainer
  {
    [XmlElement("context")]
    public List<Context> m_kContexts = new List<Context>();

    public void SetupOwners()
    {
      foreach (Context kContext in this.m_kContexts)
        kContext.Owner = this;
    }

    public void Sort()
    {
      this.m_kContexts.Sort();
      foreach (Context kContext in this.m_kContexts)
        kContext.Sort();
    }

    public bool HasContext(string kContextName)
    {
      foreach (Context kContext in this.m_kContexts)
      {
        if (kContext.Name == kContextName)
          return true;
      }
      return false;
    }

    public void AddContext(Context kContext)
    {
      this.m_kContexts.Add(kContext);
      this.Sort();
    }

    public void RemoveContext(Context kContext) => this.m_kContexts.Remove(kContext);
  }
}

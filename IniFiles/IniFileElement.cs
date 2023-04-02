// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileElement
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public class IniFileElement
  {
    private string line;
    protected string formatting = "";

    protected IniFileElement() => this.line = "";

    public IniFileElement(string _content) => this.line = _content.TrimEnd();

    public string Formatting
    {
      get => this.formatting;
      set => this.formatting = value;
    }

    public string Indentation
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < this.formatting.Length && char.IsWhiteSpace(this.formatting[index]); ++index)
          stringBuilder.Append(this.formatting[index]);
        return stringBuilder.ToString();
      }
      set
      {
        if (value.TrimStart().Length > 0)
          throw new ArgumentException("Indentation property cannot contain any characters which are not considered as white ones.");
        if (IniFileSettings.TabReplacement != null)
          value = value.Replace("\t", IniFileSettings.TabReplacement);
        this.formatting = value + this.formatting.TrimStart();
        this.line = value + this.line.TrimStart();
      }
    }

    public string Content
    {
      get => this.line.TrimStart();
      protected set => this.line = value;
    }

    public string Line
    {
      get
      {
        if (!this.line.Contains(Environment.NewLine))
          return this.line;
        string[] strArray = this.line.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(strArray[0]);
        for (int index = 1; index < strArray.Length; ++index)
          stringBuilder.Append(Environment.NewLine + this.Indentation + strArray[index]);
        return stringBuilder.ToString();
      }
    }

    public override string ToString() => "Line: \"" + this.line + "\"";

    public virtual void FormatDefault() => this.Indentation = "";
  }
}

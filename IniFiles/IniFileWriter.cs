// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileWriter
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IniFiles
{
  public class IniFileWriter : StreamWriter
  {
    public IniFileWriter(Stream str)
      : base(str)
    {
    }

    public IniFileWriter(string str)
      : base(str)
    {
    }

    public IniFileWriter(Stream str, Encoding enc)
      : base(str, enc)
    {
    }

    public IniFileWriter(string str, bool append)
      : base(str, append)
    {
    }

    public void WriteElement(IniFileElement element)
    {
      if (!IniFileSettings.PreserveFormatting)
        element.FormatDefault();
      if (element is IniFileBlankLine && !IniFileSettings.AllowBlankLines || !IniFileSettings.AllowEmptyValues && element is IniFileValue && ((IniFileValue) element).Value == "")
        return;
      this.WriteLine(element.Line);
    }

    public void WriteElements(IEnumerable<IniFileElement> elements)
    {
      lock (elements)
      {
        foreach (IniFileElement element in elements)
          this.WriteElement(element);
      }
    }

    public void WriteIniFile(IniFile file) => this.WriteElements((IEnumerable<IniFileElement>) file.elements);

    public void WriteSection(IniFileSection section)
    {
      this.WriteElement((IniFileElement) section.sectionStart);
      for (int index = section.parent.elements.IndexOf((IniFileElement) section.sectionStart) + 1; index < section.parent.elements.Count && !(section.parent.elements[index] is IniFileSectionStart); ++index)
        this.WriteElement(section.parent.elements[index]);
    }
  }
}

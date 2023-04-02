// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFile
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace IniFiles
{
  public class IniFile
  {
    internal List<IniFileSection> sections = new List<IniFileSection>();
    internal List<IniFileElement> elements = new List<IniFileElement>();

    public IniFileSection this[string sectionName]
    {
      get
      {
        IniFileSection section = this.getSection(sectionName);
        if (section != null)
          return section;
        IniFileSectionStart sect = this.sections.Count <= 0 ? IniFileSectionStart.FromName(sectionName) : this.sections[this.sections.Count - 1].sectionStart.CreateNew(sectionName);
        this.elements.Add((IniFileElement) sect);
        IniFileSection iniFileSection = new IniFileSection(this, sect);
        this.sections.Add(iniFileSection);
        return iniFileSection;
      }
    }

    private IniFileSection getSection(string name)
    {
      string lowerInvariant = name.ToLowerInvariant();
      for (int index = 0; index < this.sections.Count; ++index)
      {
        if (this.sections[index].Name == name || !IniFileSettings.CaseSensitive && this.sections[index].Name.ToLowerInvariant() == lowerInvariant)
          return this.sections[index];
      }
      return (IniFileSection) null;
    }

    public string[] GetSectionNames()
    {
      string[] sectionNames = new string[this.sections.Count];
      for (int index = 0; index < this.sections.Count; ++index)
        sectionNames[index] = this.sections[index].Name;
      return sectionNames;
    }

    public static IniFile FromFile(string path)
    {
      if (!File.Exists(path))
      {
        File.Create(path).Close();
        return new IniFile();
      }
      IniFileReader reader = new IniFileReader(path);
      IniFile iniFile = IniFile.FromStream(reader);
      reader.Close();
      return iniFile;
    }

    public static IniFile FromElements(IEnumerable<IniFileElement> elemes)
    {
      IniFile _parent = new IniFile();
      _parent.elements.AddRange(elemes);
      if (_parent.elements.Count > 0)
      {
        IniFileSection iniFileSection = (IniFileSection) null;
        if (_parent.elements[_parent.elements.Count - 1] is IniFileBlankLine)
          _parent.elements.RemoveAt(_parent.elements.Count - 1);
        for (int index = 0; index < _parent.elements.Count; ++index)
        {
          IniFileElement element = _parent.elements[index];
          if (element is IniFileSectionStart)
          {
            iniFileSection = new IniFileSection(_parent, (IniFileSectionStart) element);
            _parent.sections.Add(iniFileSection);
          }
          else if (iniFileSection != null)
            iniFileSection.elements.Add(element);
          else if (_parent.sections.Exists((Predicate<IniFileSection>) (a => a.Name == "")))
            _parent.sections[0].elements.Add(element);
          else if (element is IniFileValue)
          {
            iniFileSection = new IniFileSection(_parent, IniFileSectionStart.FromName(""));
            iniFileSection.elements.Add(element);
            _parent.sections.Add(iniFileSection);
          }
        }
      }
      return _parent;
    }

    public static IniFile FromStream(IniFileReader reader) => IniFile.FromElements((IEnumerable<IniFileElement>) reader.ReadElementsToEnd());

    public void Save(string path)
    {
      IniFileWriter writer = new IniFileWriter(path);
      this.Save(writer);
      writer.Close();
    }

    public void Save(IniFileWriter writer) => writer.WriteIniFile(this);

    public void DeleteSection(string name)
    {
      IniFileSection section = this.getSection(name);
      if (section == null)
        return;
      IniFileSectionStart sectionStart = section.sectionStart;
      this.elements.Remove((IniFileElement) sectionStart);
      for (int index = this.elements.IndexOf((IniFileElement) sectionStart) + 1; index < this.elements.Count && !(this.elements[index] is IniFileSectionStart); ++index)
        this.elements.RemoveAt(index);
    }

    public void Format(bool preserveIndentation)
    {
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < this.elements.Count; ++index)
      {
        IniFileElement element = this.elements[index];
        if (preserveIndentation)
        {
          switch (element)
          {
            case IniFileSectionStart _:
              str2 = str1 = element.Indentation;
              break;
            case IniFileValue _:
              str2 = element.Indentation;
              break;
          }
        }
        element.FormatDefault();
        if (preserveIndentation)
        {
          switch (element)
          {
            case IniFileSectionStart _:
              element.Indentation = str1;
              continue;
            case IniFileCommentary _:
              if (index != this.elements.Count - 1 && !(this.elements[index + 1] is IniFileBlankLine))
              {
                element.Indentation = this.elements[index + 1].Indentation;
                continue;
              }
              break;
          }
          element.Indentation = str2;
        }
      }
    }

    public void UnifySections()
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      for (int index1 = 0; index1 < this.sections.Count; ++index1)
      {
        IniFileSection section = this.sections[index1];
        if (dictionary.ContainsKey(section.Name))
        {
          int index2 = dictionary[section.Name] + 1;
          this.elements.Remove((IniFileElement) section.sectionStart);
          this.sections.Remove(section);
          for (int index3 = section.elements.Count - 1; index3 >= 0; --index3)
          {
            IniFileElement element = section.elements[index3];
            if (index3 != section.elements.Count - 1 || !(element is IniFileCommentary))
              this.elements.Remove(element);
            if (!(element is IniFileBlankLine))
            {
              this.elements.Insert(index2, element);
              IniFileValue iniFileValue = this[section.Name].firstValue();
              element.Indentation = iniFileValue == null ? this[section.Name].sectionStart.Indentation : iniFileValue.Indentation;
            }
          }
        }
        else
          dictionary.Add(section.Name, this.elements.IndexOf((IniFileElement) section.sectionStart));
      }
    }

    public string Header
    {
      get => this.elements.Count > 0 && this.elements[0] is IniFileCommentary && (IniFileSettings.SeparateHeader || this.elements.Count <= 1 || this.elements[1] is IniFileBlankLine) ? ((IniFileCommentary) this.elements[0]).Comment : "";
      set
      {
        if (this.elements.Count > 0 && this.elements[0] is IniFileCommentary && (IniFileSettings.SeparateHeader || this.elements.Count <= 1 || this.elements[1] is IniFileBlankLine))
        {
          if (value == "")
          {
            this.elements.RemoveAt(0);
            if (!IniFileSettings.SeparateHeader || this.elements.Count <= 0 || !(this.elements[0] is IniFileBlankLine))
              return;
            this.elements.RemoveAt(0);
          }
          else
            ((IniFileCommentary) this.elements[0]).Comment = value;
        }
        else
        {
          if (!(value != ""))
            return;
          if ((this.elements.Count == 0 || !(this.elements[0] is IniFileBlankLine)) && IniFileSettings.SeparateHeader)
            this.elements.Insert(0, (IniFileElement) new IniFileBlankLine(1));
          this.elements.Insert(0, (IniFileElement) IniFileCommentary.FromComment(value));
        }
      }
    }

    public string Foot
    {
      get => this.elements.Count > 0 && this.elements[this.elements.Count - 1] is IniFileCommentary ? ((IniFileCommentary) this.elements[this.elements.Count - 1]).Comment : "";
      set
      {
        if (value == "")
        {
          if (this.elements.Count <= 0 || !(this.elements[this.elements.Count - 1] is IniFileCommentary))
            return;
          this.elements.RemoveAt(this.elements.Count - 1);
          if (this.elements.Count <= 0 || !(this.elements[this.elements.Count - 1] is IniFileBlankLine))
            return;
          this.elements.RemoveAt(this.elements.Count - 1);
        }
        else if (this.elements.Count > 0)
        {
          if (this.elements[this.elements.Count - 1] is IniFileCommentary)
            ((IniFileCommentary) this.elements[this.elements.Count - 1]).Comment = value;
          else
            this.elements.Add((IniFileElement) IniFileCommentary.FromComment(value));
          if (this.elements.Count <= 2)
            return;
          if (!(this.elements[this.elements.Count - 2] is IniFileBlankLine) && IniFileSettings.SeparateHeader)
          {
            this.elements.Insert(this.elements.Count - 1, (IniFileElement) new IniFileBlankLine(1));
          }
          else
          {
            if (!(value == ""))
              return;
            this.elements.RemoveAt(this.elements.Count - 2);
          }
        }
        else
          this.elements.Add((IniFileElement) IniFileCommentary.FromComment(value));
      }
    }
  }
}

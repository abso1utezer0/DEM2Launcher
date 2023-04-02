// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileReader
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IniFiles
{
  public class IniFileReader : StreamReader
  {
    private IniFileElement current;
    public IniFileSectionStart line;

    public IniFileReader(Stream str)
      : base(str)
    {
    }

    public IniFileReader(Stream str, Encoding enc)
      : base(str, enc)
    {
    }

    public IniFileReader(string path)
      : base(path)
    {
    }

    public IniFileReader(string path, Encoding enc)
      : base(path, enc)
    {
    }

    public static IniFileElement ParseLine(string line)
    {
      if (line == null)
        return (IniFileElement) null;
      string testString = !line.Contains("\n") ? line.Trim() : throw new ArgumentException("String passed to the ParseLine method cannot contain more than one line.");
      IniFileElement iniFileElement = (IniFileElement) null;
      if (IniFileBlankLine.IsLineValid(testString))
        iniFileElement = (IniFileElement) new IniFileBlankLine(1);
      else if (IniFileCommentary.IsLineValid(line))
        iniFileElement = (IniFileElement) new IniFileCommentary(line);
      else if (IniFileSectionStart.IsLineValid(testString))
        iniFileElement = (IniFileElement) new IniFileSectionStart(line);
      else if (IniFileValue.IsLineValid(testString))
        iniFileElement = (IniFileElement) new IniFileValue(line);
      return iniFileElement ?? new IniFileElement(line);
    }

    public static List<IniFileElement> ParseText(string text)
    {
      if (text == null)
        return (List<IniFileElement>) null;
      List<IniFileElement> text1 = new List<IniFileElement>();
      IniFileElement iniFileElement = (IniFileElement) null;
      string str = text;
      string[] separator = new string[1]
      {
        Environment.NewLine
      };
      foreach (string line1 in str.Split(separator, StringSplitOptions.None))
      {
        IniFileElement line2 = IniFileReader.ParseLine(line1);
        if (IniFileSettings.GroupElements)
        {
          if (iniFileElement != null)
          {
            switch (line2)
            {
              case IniFileBlankLine _ when iniFileElement is IniFileBlankLine:
                ++((IniFileBlankLine) iniFileElement).Amount;
                continue;
              case IniFileCommentary _ when iniFileElement is IniFileCommentary:
                IniFileCommentary iniFileCommentary = (IniFileCommentary) iniFileElement;
                iniFileCommentary.Comment = iniFileCommentary.Comment + Environment.NewLine + ((IniFileCommentary) line2).Comment;
                continue;
            }
          }
        }
        iniFileElement = line2;
        text1.Add(line2);
      }
      return text1;
    }

    public IniFileElement ReadElement()
    {
      this.current = IniFileReader.ParseLine(this.ReadLine());
      return this.current;
    }

    public List<IniFileElement> ReadElementsToEnd() => IniFileReader.ParseText(this.ReadToEnd());

    public IniFileSectionStart GotoSection(string sectionName)
    {
      string str;
      do
      {
        str = this.ReadLine();
        if (str == null)
        {
          this.current = (IniFileElement) null;
          return (IniFileSectionStart) null;
        }
      }
      while (!IniFileSectionStart.IsLineValid(str) || !(IniFileReader.ParseLine(str) is IniFileSectionStart line) || !(line.SectionName == sectionName) && (IniFileSettings.CaseSensitive || !(line.SectionName.ToLowerInvariant() == sectionName)));
      this.current = (IniFileElement) line;
      return line;
    }

    public List<IniFileElement> ReadSection()
    {
      if (this.current == null || !(this.current is IniFileSectionStart))
        throw new InvalidOperationException("The current position of the reader must be at IniFileSectionStart. Use GotoSection method");
      List<IniFileElement> iniFileElementList = new List<IniFileElement>();
      iniFileElementList.Add(this.current);
      string text = "";
      string content;
      while ((content = this.ReadLine()) != null)
      {
        if (IniFileSectionStart.IsLineValid(content.Trim()))
        {
          this.current = (IniFileElement) new IniFileSectionStart(content);
          break;
        }
        text = text + content + Environment.NewLine;
      }
      if (text.EndsWith(Environment.NewLine) && text != Environment.NewLine)
        text = text.Substring(0, text.Length - Environment.NewLine.Length);
      iniFileElementList.AddRange((IEnumerable<IniFileElement>) IniFileReader.ParseText(text));
      return iniFileElementList;
    }

    public IniFileElement Current => this.current;

    public List<IniFileValue> ReadSectionValues()
    {
      List<IniFileElement> iniFileElementList = this.ReadSection();
      List<IniFileValue> iniFileValueList = new List<IniFileValue>();
      for (int index = 0; index < iniFileElementList.Count; ++index)
      {
        if (iniFileElementList[index] is IniFileValue)
          iniFileValueList.Add((IniFileValue) iniFileElementList[index]);
      }
      return iniFileValueList;
    }

    public IniFileValue GotoValue(string key) => this.GotoValue(key, false);

    public IniFileValue GotoValue(string key, bool searchWholeFile)
    {
      string line1;
      do
      {
        line1 = this.ReadLine();
        if (line1 == null)
          return (IniFileValue) null;
        if (IniFileValue.IsLineValid(line1.Trim()) && IniFileReader.ParseLine(line1) is IniFileValue line2 && (line2.Key == key || !IniFileSettings.CaseSensitive && line2.Key.ToLowerInvariant() == key.ToLowerInvariant()))
          return line2;
      }
      while (searchWholeFile || !IniFileSectionStart.IsLineValid(line1.Trim()));
      return (IniFileValue) null;
    }
  }
}

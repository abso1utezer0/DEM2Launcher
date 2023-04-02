// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileSection
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IniFiles
{
  public class IniFileSection
  {
    internal List<IniFileElement> elements = new List<IniFileElement>();
    internal IniFileSectionStart sectionStart;
    internal IniFile parent;

    internal IniFileSection(IniFile _parent, IniFileSectionStart sect)
    {
      this.sectionStart = sect;
      this.parent = _parent;
    }

    public string Name
    {
      get => this.sectionStart.SectionName;
      set => this.sectionStart.SectionName = value;
    }

    public string Comment
    {
      get => !(this.Name == "") ? this.getComment((IniFileElement) this.sectionStart) : "";
      set
      {
        if (!(this.Name != ""))
          return;
        this.setComment((IniFileElement) this.sectionStart, value);
      }
    }

    private void setComment(IniFileElement el, string comment)
    {
      int index = this.parent.elements.IndexOf(el);
      if (IniFileSettings.CommentChars.Length == 0)
        throw new NotSupportedException("Comments are currently disabled. Setup ConfigFileSettings.CommentChars property to enable them.");
      if (index > 0 && this.parent.elements[index - 1] is IniFileCommentary)
      {
        IniFileCommentary element = (IniFileCommentary) this.parent.elements[index - 1];
        if (comment == "")
        {
          this.parent.elements.Remove((IniFileElement) element);
        }
        else
        {
          element.Comment = comment;
          element.Indentation = el.Indentation;
        }
      }
      else
      {
        if (!(comment != ""))
          return;
        IniFileCommentary iniFileCommentary = IniFileCommentary.FromComment(comment);
        iniFileCommentary.Indentation = el.Indentation;
        this.parent.elements.Insert(index, (IniFileElement) iniFileCommentary);
      }
    }

    private string getComment(IniFileElement el)
    {
      int num = this.parent.elements.IndexOf(el);
      return num != 0 && this.parent.elements[num - 1] is IniFileCommentary ? ((IniFileCommentary) this.parent.elements[num - 1]).Comment : "";
    }

    private IniFileValue getValue(string key)
    {
      string lowerInvariant = key.ToLowerInvariant();
      for (int index = 0; index < this.elements.Count; ++index)
      {
        if (this.elements[index] is IniFileValue)
        {
          IniFileValue element = (IniFileValue) this.elements[index];
          if (element.Key == key || !IniFileSettings.CaseSensitive && element.Key.ToLowerInvariant() == lowerInvariant)
            return element;
        }
      }
      return (IniFileValue) null;
    }

    public void SetComment(string key, string comment)
    {
      IniFileValue el = this.getValue(key);
      if (el == null)
        return;
      this.setComment((IniFileElement) el, comment);
    }

    public void SetInlineComment(string key, string comment)
    {
      IniFileValue iniFileValue = this.getValue(key);
      if (iniFileValue == null)
        return;
      iniFileValue.InlineComment = comment;
    }

    public string GetInlineComment(string key) => this.getValue(key)?.InlineComment;

    public string InlineComment
    {
      get => this.sectionStart.InlineComment;
      set => this.sectionStart.InlineComment = value;
    }

    public string GetComment(string key)
    {
      IniFileValue el = this.getValue(key);
      return el == null ? (string) null : this.getComment((IniFileElement) el);
    }

    public void RenameKey(string key, string newName)
    {
      IniFileValue iniFileValue = this.getValue(key);
      if (key == null)
        return;
      iniFileValue.Key = newName;
    }

    public void DeleteKey(string key)
    {
      IniFileValue iniFileValue = this.getValue(key);
      if (key == null)
        return;
      this.parent.elements.Remove((IniFileElement) iniFileValue);
      this.elements.Remove((IniFileElement) iniFileValue);
    }

    public string this[string key]
    {
      get => this.getValue(key)?.Value;
      set
      {
        IniFileValue iniFileValue = this.getValue(key);
        if (iniFileValue != null)
          iniFileValue.Value = value;
        else
          this.setValue(key, value);
      }
    }

    public string this[string key, string defaultValue]
    {
      get
      {
        string str = this[key];
        return str == "" || str == null ? defaultValue : str;
      }
      set => this[key] = value;
    }

    private void setValue(string key, string value)
    {
      IniFileValue iniFileValue1 = (IniFileValue) null;
      IniFileValue iniFileValue2 = this.lastValue();
      if (IniFileSettings.PreserveFormatting)
      {
        if (iniFileValue2 != null && iniFileValue2.Indentation.Length >= this.sectionStart.Indentation.Length)
        {
          iniFileValue1 = iniFileValue2.CreateNew(key, value);
        }
        else
        {
          bool flag = false;
          for (int index = this.parent.elements.IndexOf((IniFileElement) this.sectionStart) - 1; index >= 0; --index)
          {
            IniFileElement element = this.parent.elements[index];
            if (element is IniFileValue)
            {
              iniFileValue1 = ((IniFileValue) element).CreateNew(key, value);
              flag = true;
              break;
            }
          }
          if (!flag)
            iniFileValue1 = IniFileValue.FromData(key, value);
          if (iniFileValue1.Indentation.Length < this.sectionStart.Indentation.Length)
            iniFileValue1.Indentation = this.sectionStart.Indentation;
        }
      }
      else
        iniFileValue1 = IniFileValue.FromData(key, value);
      if (iniFileValue2 == null)
      {
        this.elements.Insert(this.elements.IndexOf((IniFileElement) this.sectionStart) + 1, (IniFileElement) iniFileValue1);
        this.parent.elements.Insert(this.parent.elements.IndexOf((IniFileElement) this.sectionStart) + 1, (IniFileElement) iniFileValue1);
      }
      else
      {
        this.elements.Insert(this.elements.IndexOf((IniFileElement) iniFileValue2) + 1, (IniFileElement) iniFileValue1);
        this.parent.elements.Insert(this.parent.elements.IndexOf((IniFileElement) iniFileValue2) + 1, (IniFileElement) iniFileValue1);
      }
    }

    internal IniFileValue lastValue()
    {
      for (int index = this.elements.Count - 1; index >= 0; --index)
      {
        if (this.elements[index] is IniFileValue)
          return (IniFileValue) this.elements[index];
      }
      return (IniFileValue) null;
    }

    internal IniFileValue firstValue()
    {
      for (int index = 0; index < this.elements.Count; ++index)
      {
        if (this.elements[index] is IniFileValue)
          return (IniFileValue) this.elements[index];
      }
      return (IniFileValue) null;
    }

    public ReadOnlyCollection<string> GetKeys()
    {
      List<string> list = new List<string>(this.elements.Count);
      for (int index = 0; index < this.elements.Count; ++index)
      {
        if (this.elements[index] is IniFileValue)
          list.Add(((IniFileValue) this.elements[index]).Key);
      }
      return new ReadOnlyCollection<string>((IList<string>) list);
    }

    public override string ToString() => this.sectionStart.ToString() + " (" + this.elements.Count.ToString() + " elements)";

    public void Format(bool preserveIndentation)
    {
      for (int index = 0; index < this.elements.Count; ++index)
      {
        IniFileElement element = this.elements[index];
        string indentation = element.Indentation;
        element.FormatDefault();
        if (preserveIndentation)
          element.Indentation = indentation;
      }
    }
  }
}

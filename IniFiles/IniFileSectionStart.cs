// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileSectionStart
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public class IniFileSectionStart : IniFileElement
  {
    private string sectionName;
    private string textOnTheRight;
    private string inlineComment;
    private string inlineCommentChar;

    private IniFileSectionStart()
    {
    }

    public IniFileSectionStart(string content)
      : base(content)
    {
      this.formatting = IniFileSectionStart.ExtractFormat(content);
      content = content.TrimStart();
      if (IniFileSettings.AllowInlineComments)
      {
        IniFileSettings.indexOfAnyResult indexOfAnyResult = IniFileSettings.indexOfAny(content, IniFileSettings.CommentChars);
        if (indexOfAnyResult.index > content.IndexOf(IniFileSettings.SectionCloseBracket))
        {
          this.inlineComment = content.Substring(indexOfAnyResult.index + indexOfAnyResult.any.Length);
          this.inlineCommentChar = indexOfAnyResult.any;
          content = content.Substring(0, indexOfAnyResult.index);
        }
      }
      if (IniFileSettings.AllowTextOnTheRight)
      {
        int length = content.LastIndexOf(IniFileSettings.SectionCloseBracket);
        if (length != content.Length - 1)
        {
          this.textOnTheRight = content.Substring(length + 1);
          content = content.Substring(0, length);
        }
      }
      this.sectionName = content.Substring(IniFileSettings.SectionOpenBracket.Length, content.Length - IniFileSettings.SectionCloseBracket.Length - IniFileSettings.SectionOpenBracket.Length).Trim();
      this.Content = content;
      this.Format();
    }

    public string SectionName
    {
      get => this.sectionName;
      set
      {
        this.sectionName = value;
        this.Format();
      }
    }

    public string InlineComment
    {
      get => this.inlineComment;
      set
      {
        if (!IniFileSettings.AllowInlineComments || IniFileSettings.CommentChars.Length == 0)
          throw new NotSupportedException("Inline comments are disabled.");
        this.inlineComment = value;
        this.Format();
      }
    }

    public static bool IsLineValid(string testString) => testString.StartsWith(IniFileSettings.SectionOpenBracket) && testString.EndsWith(IniFileSettings.SectionCloseBracket);

    public override string ToString() => "Section: \"" + this.sectionName + "\"";

    public IniFileSectionStart CreateNew(string sectName)
    {
      IniFileSectionStart fileSectionStart = new IniFileSectionStart();
      fileSectionStart.sectionName = sectName;
      if (IniFileSettings.PreserveFormatting)
      {
        fileSectionStart.formatting = this.formatting;
        fileSectionStart.Format();
      }
      else
        fileSectionStart.Format();
      return fileSectionStart;
    }

    public static string ExtractFormat(string content)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = true;
      string str = "";
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < content.Length; ++index)
      {
        char c = content[index];
        if (char.IsLetterOrDigit(c) && flag1)
        {
          flag2 = true;
          flag1 = false;
          stringBuilder.Append('$');
        }
        else if (flag2 && char.IsLetterOrDigit(c))
          str = "";
        else if (content.Length - index >= IniFileSettings.SectionOpenBracket.Length && content.Substring(index, IniFileSettings.SectionOpenBracket.Length) == IniFileSettings.SectionOpenBracket && flag3)
        {
          flag1 = true;
          flag3 = false;
          stringBuilder.Append('[');
        }
        else if (content.Length - index >= IniFileSettings.SectionCloseBracket.Length && content.Substring(index, IniFileSettings.SectionOpenBracket.Length) == IniFileSettings.SectionCloseBracket && flag2)
        {
          stringBuilder.Append(str);
          flag2 = false;
          stringBuilder.Append(IniFileSettings.SectionCloseBracket);
        }
        else if (IniFileSettings.ofAny(index, content, IniFileSettings.CommentChars) != null)
          stringBuilder.Append(';');
        else if (char.IsWhiteSpace(c))
        {
          if (flag2)
            str += (string) (object) c;
          else
            stringBuilder.Append(c);
        }
      }
      string format = stringBuilder.ToString();
      if (format.IndexOf(';') == -1)
        format += "   ;";
      return format;
    }

    public override void FormatDefault()
    {
      this.Formatting = IniFileSettings.DefaultSectionFormatting;
      this.Format();
    }

    public void Format() => this.Format(this.formatting);

    public void Format(string formatting)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < formatting.Length; ++index)
      {
        switch (formatting[index])
        {
          case '$':
            stringBuilder.Append(this.sectionName);
            break;
          case ';':
            if (IniFileSettings.CommentChars.Length > 0 && this.inlineComment != null)
            {
              stringBuilder.Append(IniFileSettings.CommentChars[0]).Append(this.inlineComment);
              break;
            }
            goto default;
          case '[':
            stringBuilder.Append(IniFileSettings.SectionOpenBracket);
            break;
          case ']':
            stringBuilder.Append(IniFileSettings.SectionCloseBracket);
            break;
          default:
            if (char.IsWhiteSpace(formatting[index]))
            {
              stringBuilder.Append(formatting[index]);
              break;
            }
            break;
        }
      }
      this.Content = stringBuilder.ToString().TrimEnd() + (IniFileSettings.AllowTextOnTheRight ? this.textOnTheRight : "");
    }

    public static IniFileSectionStart FromName(string sectionName)
    {
      IniFileSectionStart fileSectionStart = new IniFileSectionStart();
      fileSectionStart.SectionName = sectionName;
      fileSectionStart.FormatDefault();
      return fileSectionStart;
    }
  }
}

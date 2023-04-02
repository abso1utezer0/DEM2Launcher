// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileValue
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public class IniFileValue : IniFileElement
  {
    private string key;
    private string value;
    private string textOnTheRight;
    private string inlineComment;
    private string inlineCommentChar;

    private IniFileValue()
    {
    }

    public IniFileValue(string content)
      : base(content)
    {
      string[] strArray = this.Content.Split(new string[1]
      {
        IniFileSettings.EqualsString
      }, StringSplitOptions.None);
      this.formatting = this.ExtractFormat(content);
      string str = strArray[0].Trim();
      string text = strArray.Length >= 1 ? strArray[1].Trim() : "";
      if (str.Length > 0)
      {
        if (IniFileSettings.AllowInlineComments)
        {
          IniFileSettings.indexOfAnyResult indexOfAnyResult = IniFileSettings.indexOfAny(text, IniFileSettings.CommentChars);
          if (indexOfAnyResult.index != -1)
          {
            this.inlineComment = text.Substring(indexOfAnyResult.index + indexOfAnyResult.any.Length);
            text = text.Substring(0, indexOfAnyResult.index).TrimEnd();
            this.inlineCommentChar = indexOfAnyResult.any;
          }
        }
        char? quoteChar = IniFileSettings.QuoteChar;
        if ((quoteChar.HasValue ? new int?((int) quoteChar.GetValueOrDefault()) : new int?()).HasValue && text.Length >= 2)
        {
          char ch = IniFileSettings.QuoteChar.Value;
          if ((int) text[0] == (int) ch)
          {
            int num;
            if (IniFileSettings.AllowTextOnTheRight)
            {
              num = text.LastIndexOf(ch);
              if (num != text.Length - 1)
                this.textOnTheRight = text.Substring(num + 1);
            }
            else
              num = text.Length - 1;
            if (num > 0)
              text = text.Length != 2 ? text.Substring(1, num - 1) : "";
          }
        }
        this.key = str;
        this.value = text;
      }
      this.Format();
    }

    public string Key
    {
      get => this.key;
      set
      {
        this.key = value;
        this.Format();
      }
    }

    public string Value
    {
      get => this.value;
      set
      {
        this.value = value;
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
        if (this.inlineCommentChar == null)
          this.inlineCommentChar = IniFileSettings.CommentChars[0];
        this.inlineComment = value;
        this.Format();
      }
    }

    public string ExtractFormat(string content)
    {
      IniFileValue.feState feState = IniFileValue.feState.BeforeEvery;
      string str1 = "";
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < content.Length; ++index)
      {
        char c = content[index];
        if (char.IsLetterOrDigit(c))
        {
          switch (feState)
          {
            case IniFileValue.feState.BeforeEvery:
              stringBuilder.Append('?');
              feState = IniFileValue.feState.AfterKey;
              break;
            case IniFileValue.feState.BeforeVal:
              stringBuilder.Append('$');
              feState = IniFileValue.feState.AfterVal;
              break;
          }
        }
        else if (feState == IniFileValue.feState.AfterKey && content.Length - index >= IniFileSettings.EqualsString.Length && content.Substring(index, IniFileSettings.EqualsString.Length) == IniFileSettings.EqualsString)
        {
          stringBuilder.Append(str1);
          feState = IniFileValue.feState.BeforeVal;
          stringBuilder.Append('=');
        }
        else if (IniFileSettings.ofAny(index, content, IniFileSettings.CommentChars) != null)
        {
          stringBuilder.Append(str1);
          stringBuilder.Append(';');
        }
        else if (char.IsWhiteSpace(c))
        {
          string str2 = c != '\t' || IniFileSettings.TabReplacement == null ? c.ToString() : IniFileSettings.TabReplacement;
          if (feState == IniFileValue.feState.AfterKey || feState == IniFileValue.feState.AfterVal)
          {
            str1 += str2;
            continue;
          }
          stringBuilder.Append(str2);
        }
        str1 = "";
      }
      if (feState == IniFileValue.feState.BeforeVal)
        stringBuilder.Append('$');
      string format = stringBuilder.ToString();
      if (format.IndexOf(';') == -1)
        format += "   ;";
      return format;
    }

    public void Format() => this.Format(this.formatting);

    public void Format(string formatting)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < formatting.Length; ++index)
      {
        char ch1 = formatting[index];
        switch (ch1)
        {
          case '$':
            char? quoteChar = IniFileSettings.QuoteChar;
            if ((quoteChar.HasValue ? new int?((int) quoteChar.GetValueOrDefault()) : new int?()).HasValue)
            {
              char ch2 = IniFileSettings.QuoteChar.Value;
              stringBuilder.Append(ch2).Append(this.value).Append(ch2);
              break;
            }
            stringBuilder.Append(this.value);
            break;
          case ';':
            stringBuilder.Append(this.inlineCommentChar + this.inlineComment);
            break;
          case '=':
            stringBuilder.Append(IniFileSettings.EqualsString);
            break;
          case '?':
            stringBuilder.Append(this.key);
            break;
          default:
            if (char.IsWhiteSpace(formatting[index]))
            {
              stringBuilder.Append(ch1);
              break;
            }
            break;
        }
      }
      this.Content = stringBuilder.ToString().TrimEnd() + (IniFileSettings.AllowTextOnTheRight ? this.textOnTheRight : "");
    }

    public override void FormatDefault()
    {
      this.Formatting = IniFileSettings.DefaultValueFormatting;
      this.Format();
    }

    public IniFileValue CreateNew(string key, string value)
    {
      IniFileValue iniFileValue = new IniFileValue();
      iniFileValue.key = key;
      iniFileValue.value = value;
      if (IniFileSettings.PreserveFormatting)
      {
        iniFileValue.formatting = this.formatting;
        if (IniFileSettings.AllowInlineComments)
          iniFileValue.inlineCommentChar = this.inlineCommentChar;
        iniFileValue.Format();
      }
      else
        iniFileValue.FormatDefault();
      return iniFileValue;
    }

    public static bool IsLineValid(string testString) => testString.IndexOf(IniFileSettings.EqualsString) > 0;

    public void Set(string key, string value)
    {
      this.key = key;
      this.value = value;
      this.Format();
    }

    public override string ToString() => "Value: \"" + this.key + " = " + this.value + "\"";

    public static IniFileValue FromData(string key, string value)
    {
      IniFileValue iniFileValue = new IniFileValue();
      iniFileValue.key = key;
      iniFileValue.value = value;
      iniFileValue.FormatDefault();
      return iniFileValue;
    }

    private enum feState
    {
      BeforeEvery,
      AfterKey,
      BeforeVal,
      AfterVal,
    }
  }
}

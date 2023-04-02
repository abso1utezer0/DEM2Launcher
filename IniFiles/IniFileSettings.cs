// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileSettings
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public static class IniFileSettings
  {
    private static IniFileSettings.iniFlags flags = IniFileSettings.iniFlags.PreserveFormatting | IniFileSettings.iniFlags.AllowEmptyValues | IniFileSettings.iniFlags.AllowTextOnTheRight | IniFileSettings.iniFlags.GroupElements | IniFileSettings.iniFlags.CaseSensitive | IniFileSettings.iniFlags.SeparateHeader | IniFileSettings.iniFlags.AllowBlankLines | IniFileSettings.iniFlags.AllowInlineComments;
    private static string[] commentChars = new string[2]
    {
      ";",
      "#"
    };
    private static char? quoteChar = new char?();
    private static string defaultValueFormatting = "?=$   ;";
    private static string defaultSectionFormatting = "[$]   ;";
    private static string sectionCloseBracket = "]";
    private static string equalsString = "=";
    private static string tabReplacement = (string) null;
    private static string sectionOpenBracket = "[";

    public static bool PreserveFormatting
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.PreserveFormatting) == IniFileSettings.iniFlags.PreserveFormatting;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.PreserveFormatting;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.PreserveFormatting;
      }
    }

    public static bool AllowEmptyValues
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.AllowEmptyValues) == IniFileSettings.iniFlags.AllowEmptyValues;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.AllowEmptyValues;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.AllowEmptyValues;
      }
    }

    public static bool AllowTextOnTheRight
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.AllowTextOnTheRight) == IniFileSettings.iniFlags.AllowTextOnTheRight;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.AllowTextOnTheRight;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.AllowTextOnTheRight;
      }
    }

    public static bool GroupElements
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.GroupElements) == IniFileSettings.iniFlags.GroupElements;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.GroupElements;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.GroupElements;
      }
    }

    public static bool CaseSensitive
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.CaseSensitive) == IniFileSettings.iniFlags.CaseSensitive;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.CaseSensitive;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.CaseSensitive;
      }
    }

    public static bool SeparateHeader
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.SeparateHeader) == IniFileSettings.iniFlags.SeparateHeader;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.SeparateHeader;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.SeparateHeader;
      }
    }

    public static bool AllowBlankLines
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.AllowBlankLines) == IniFileSettings.iniFlags.AllowBlankLines;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.AllowBlankLines;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.AllowBlankLines;
      }
    }

    public static bool AllowInlineComments
    {
      get => (IniFileSettings.flags & IniFileSettings.iniFlags.AllowInlineComments) != (IniFileSettings.iniFlags) 0;
      set
      {
        if (value)
          IniFileSettings.flags |= IniFileSettings.iniFlags.AllowInlineComments;
        else
          IniFileSettings.flags &= ~IniFileSettings.iniFlags.AllowInlineComments;
      }
    }

    public static string SectionCloseBracket
    {
      get => IniFileSettings.sectionCloseBracket;
      set => IniFileSettings.sectionCloseBracket = value != null ? value : throw new ArgumentNullException(nameof (SectionCloseBracket));
    }

    public static string[] CommentChars
    {
      get => IniFileSettings.commentChars;
      set => IniFileSettings.commentChars = value != null ? value : throw new ArgumentNullException(nameof (CommentChars), "Use empty array to disable comments instead of null");
    }

    public static char? QuoteChar
    {
      get => IniFileSettings.quoteChar;
      set => IniFileSettings.quoteChar = value;
    }

    public static string DefaultSectionFormatting
    {
      get => IniFileSettings.defaultSectionFormatting;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (DefaultSectionFormatting));
        if (value.Replace("$", "").Replace("[", "").Replace("]", "").Replace(";", "").TrimStart().Length > 0)
          throw new ArgumentException("DefaultSectionFormatting property cannot contain other characters than [,$,],; and white spaces.");
        IniFileSettings.defaultSectionFormatting = value.IndexOf('[') < value.IndexOf('$') && value.IndexOf('$') < value.IndexOf(']') && (value.IndexOf(';') == -1 || value.IndexOf(']') < value.IndexOf(';')) ? value : throw new ArgumentException("Special charcters in the formatting strings are in the incorrect order. The valid is: [, $, ], ;.");
      }
    }

    public static string DefaultValueFormatting
    {
      get => IniFileSettings.defaultValueFormatting;
      set
      {
        string str = value != null ? value.Replace("?", "").Replace("$", "").Replace("=", "").Replace(";", "") : throw new ArgumentNullException(nameof (DefaultValueFormatting));
        if (str.TrimStart().Length > 0)
          throw new ArgumentException("DefaultValueFormatting property cannot contain other characters than ?,$,=,; and white spaces.");
        IniFileSettings.defaultValueFormatting = (value.IndexOf('?') < value.IndexOf('=') && value.IndexOf('=') < value.IndexOf('$') || value.IndexOf('=') == -1 && str.IndexOf('?') < value.IndexOf('$')) && (value.IndexOf(';') == -1 || value.IndexOf('$') < value.IndexOf(';')) ? value : throw new ArgumentException("Special charcters in the formatting strings are in the incorrect order. The valid is: ?, =, $, ;.");
      }
    }

    public static string SectionOpenBracket
    {
      get => IniFileSettings.sectionOpenBracket;
      set => IniFileSettings.sectionOpenBracket = value != null ? value : throw new ArgumentNullException("SectionCloseBracket");
    }

    public static string EqualsString
    {
      get => IniFileSettings.equalsString;
      set => IniFileSettings.equalsString = value != null ? value : throw new ArgumentNullException(nameof (EqualsString));
    }

    public static string TabReplacement
    {
      get => IniFileSettings.tabReplacement;
      set => IniFileSettings.tabReplacement = value;
    }

    internal static string trimLeft(ref string str)
    {
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder();
      for (; num < str.Length && char.IsWhiteSpace(str, num); ++num)
        stringBuilder.Append(str[num]);
      str = str.Length <= num ? "" : str.Substring(num);
      return stringBuilder.ToString();
    }

    internal static string trimRight(ref string str)
    {
      int index1 = str.Length - 1;
      StringBuilder stringBuilder1 = new StringBuilder();
      for (; index1 >= 0 && char.IsWhiteSpace(str, index1); --index1)
        stringBuilder1.Append(str[index1]);
      StringBuilder stringBuilder2 = new StringBuilder();
      for (int index2 = stringBuilder1.Length - 1; index2 >= 0; --index2)
        stringBuilder2.Append(stringBuilder1[index2]);
      str = str.Length - index1 <= 0 ? "" : str.Substring(0, index1 + 1);
      return stringBuilder2.ToString();
    }

    internal static string startsWith(string line, string[] array)
    {
      if (array == null)
        return (string) null;
      for (int index = 0; index < array.Length; ++index)
      {
        if (line.StartsWith(array[index]))
          return array[index];
      }
      return (string) null;
    }

    internal static IniFileSettings.indexOfAnyResult indexOfAny(
      string text,
      string[] array)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        if (text.Contains(array[index]))
          return new IniFileSettings.indexOfAnyResult(text.IndexOf(array[index]), array[index]);
      }
      return new IniFileSettings.indexOfAnyResult(-1, (string) null);
    }

    internal static string ofAny(int index, string text, string[] array)
    {
      for (int index1 = 0; index1 < array.Length; ++index1)
      {
        if (text.Length - index >= array[index1].Length && text.Substring(index, array[index1].Length) == array[index1])
          return array[index1];
      }
      return (string) null;
    }

    private enum iniFlags
    {
      PreserveFormatting = 1,
      AllowEmptyValues = 2,
      AllowTextOnTheRight = 4,
      GroupElements = 8,
      CaseSensitive = 16, // 0x00000010
      SeparateHeader = 32, // 0x00000020
      AllowBlankLines = 64, // 0x00000040
      AllowInlineComments = 128, // 0x00000080
    }

    internal struct indexOfAnyResult
    {
      public int index;
      public string any;

      public indexOfAnyResult(int i, string _any)
      {
        this.any = _any;
        this.index = i;
      }
    }
  }
}

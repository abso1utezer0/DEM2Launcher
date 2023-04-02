// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileBlankLine
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public class IniFileBlankLine : IniFileElement
  {
    public IniFileBlankLine(int amount)
      : base("")
    {
      this.Amount = amount;
    }

    public int Amount
    {
      get => this.Line.Length / Environment.NewLine.Length + 1;
      set
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException("Cannot set Amount to less than 1.");
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 1; index < value; ++index)
          stringBuilder.Append(Environment.NewLine);
        this.Content = stringBuilder.ToString();
      }
    }

    public static bool IsLineValid(string testString) => testString == "";

    public override string ToString() => this.Amount.ToString() + " blank line(s)";

    public override void FormatDefault()
    {
      this.Amount = 1;
      base.FormatDefault();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: IniFiles.IniFileCommentary
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Text;

namespace IniFiles
{
  public class IniFileCommentary : IniFileElement
  {
    private string comment;
    private string commentChar;

    private IniFileCommentary()
    {
    }

    public IniFileCommentary(string content)
      : base(content)
    {
      this.commentChar = IniFileSettings.CommentChars.Length != 0 ? IniFileSettings.startsWith(this.Content, IniFileSettings.CommentChars) : throw new NotSupportedException("Comments are disabled. Set the IniFileSettings.CommentChars property to turn them on.");
      if (this.Content.Length > this.commentChar.Length)
        this.comment = this.Content.Substring(this.commentChar.Length);
      else
        this.comment = "";
    }

    public string CommentChar
    {
      get => this.commentChar;
      set
      {
        if (!(this.commentChar != value))
          return;
        this.commentChar = value;
        this.rewrite();
      }
    }

    public string Comment
    {
      get => this.comment;
      set
      {
        if (!(this.comment != value))
          return;
        this.comment = value;
        this.rewrite();
      }
    }

    private void rewrite()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string[] strArray = this.comment.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      stringBuilder.Append(this.commentChar + strArray[0]);
      for (int index = 1; index < strArray.Length; ++index)
        stringBuilder.Append(Environment.NewLine + this.commentChar + strArray[index]);
      this.Content = stringBuilder.ToString();
    }

    public static bool IsLineValid(string testString) => IniFileSettings.startsWith(testString.TrimStart(), IniFileSettings.CommentChars) != null;

    public override string ToString() => "Comment: \"" + this.comment + "\"";

    public static IniFileCommentary FromComment(string comment) => IniFileSettings.CommentChars.Length != 0 ? new IniFileCommentary()
    {
      comment = comment,
      CommentChar = IniFileSettings.CommentChars[0]
    } : throw new NotSupportedException("Comments are disabled. Set the IniFileSettings.CommentChars property to turn them on.");

    public override void FormatDefault()
    {
      base.FormatDefault();
      this.CommentChar = IniFileSettings.CommentChars[0];
      this.rewrite();
    }
  }
}

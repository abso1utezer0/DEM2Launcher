// Decompiled with JetBrains decompiler
// Type: InputMapper.InputValueIndexConverter
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System.Collections;
using System.ComponentModel;

namespace InputMapper
{
  public class InputValueIndexConverter : StringConverter
  {
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

    public override TypeConverter.StandardValuesCollection GetStandardValues(
      ITypeDescriptorContext context)
    {
      return new TypeConverter.StandardValuesCollection((ICollection) InputMapping.ms_kValueIndexStrings.ToArray());
    }
  }
}

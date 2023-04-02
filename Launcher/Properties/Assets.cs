// Decompiled with JetBrains decompiler
// Type: Launcher.Properties.Assets
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Launcher.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  [CompilerGenerated]
  internal class Assets
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Assets()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Assets.resourceMan, (object) null))
          Assets.resourceMan = new ResourceManager("Launcher.Properties.Assets", typeof (Assets).Assembly);
        return Assets.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Assets.resourceCulture;
      set => Assets.resourceCulture = value;
    }



    internal static Bitmap DEM2_Black_ARA => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_ARA), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_CZH => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_CZH), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_DAN => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_DAN), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_DUT => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_DUT), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_ENG => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_ENG), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_ENG2 => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_ENG2), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_ESP => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_ESP), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_FRA => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_FRA), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_GER => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_GER), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_ITA => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_ITA), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_NOR => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_NOR), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_PBR => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_PBR), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_POL => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_POL), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_POR => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_POR), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_RUS => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_RUS), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_SPA => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_SPA), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_SWE => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_SWE), Assets.resourceCulture);

    internal static Bitmap DEM2_Black_TUR => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Black_TUR), Assets.resourceCulture);

    internal static Bitmap DEM2_Logo_HUN => (Bitmap) Assets.ResourceManager.GetObject(nameof (DEM2_Logo_HUN), Assets.resourceCulture);

    internal static string LocStrings => Assets.ResourceManager.GetString(nameof (LocStrings), Assets.resourceCulture);
  }
}

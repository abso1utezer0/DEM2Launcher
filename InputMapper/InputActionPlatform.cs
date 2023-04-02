// Decompiled with JetBrains decompiler
// Type: InputMapper.InputActionPlatform
// Assembly: Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A48A1CB-4146-4CD9-ACAF-0BE6947FBF01
// Assembly location: E:\SteamLibrary\steamapps\common\Disney Epic Mickey 2\Launch.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace InputMapper
{
  public class InputActionPlatform : ICloneable
  {
    private int m_iEvent;
    private float m_fExponent = 1f;
    private int m_iInputModifier;
    private float m_fMinValueThreshold;
    private float m_fMaxValueThreshold = 1f;
    private float m_fScalar = 1f;
    private int m_iValueMapping;
    [XmlElement("mapping")]
    public List<InputMapping> m_kMappings;
    [XmlIgnore]
    private InputAction m_kOwnerObject;
    [XmlIgnore]
    private bool m_boHasPlayer1Mappings;
    public static List<string> ms_kEventStrings;
    public static List<string> ms_kValueMappingStrings;
    public static List<string> ms_kInputModifierStrings;

    public InputActionPlatform() => this.m_kMappings = new List<InputMapping>();

    [XmlIgnore]
    public int Event
    {
      get => this.m_iEvent;
      set => this.m_iEvent = value;
    }

    [XmlAttribute("event")]
    public string EventString
    {
      get => InputActionPlatform.ms_kEventStrings[this.m_iEvent];
      set
      {
        for (int index = 0; index < InputActionPlatform.ms_kEventStrings.Count; ++index)
        {
          if (InputActionPlatform.ms_kEventStrings[index] == value)
          {
            this.m_iEvent = index;
            break;
          }
        }
      }
    }

    [XmlAttribute("exponent")]
    public float Exponent
    {
      get => this.m_fExponent;
      set => this.m_fExponent = value;
    }

    [XmlIgnore]
    public int InputModifier
    {
      get => this.m_iInputModifier;
      set => this.m_iInputModifier = value;
    }

    [XmlAttribute("inputModifier")]
    public string InputModifierString
    {
      get => InputActionPlatform.ms_kInputModifierStrings[this.m_iInputModifier];
      set
      {
        for (int index = 0; index < InputActionPlatform.ms_kInputModifierStrings.Count; ++index)
        {
          if (InputActionPlatform.ms_kInputModifierStrings[index] == value)
          {
            this.m_iInputModifier = index;
            break;
          }
        }
      }
    }

    [XmlAttribute("minValue")]
    public float MinValueThreshold
    {
      get => this.m_fMinValueThreshold;
      set => this.m_fMinValueThreshold = value;
    }

    [XmlAttribute("maxValue")]
    public float MaxValueThreshold
    {
      get => this.m_fMaxValueThreshold;
      set => this.m_fMaxValueThreshold = value;
    }

    [XmlAttribute("scalar")]
    public float Scalar
    {
      get => this.m_fScalar;
      set => this.m_fScalar = value;
    }

    [XmlIgnore]
    public int ValueMapping
    {
      get => this.m_iValueMapping;
      set => this.m_iValueMapping = value;
    }

    [XmlAttribute("valueMapping")]
    public string ValueMappingString
    {
      get => InputActionPlatform.ms_kValueMappingStrings[this.m_iValueMapping];
      set
      {
        for (int index = 0; index < InputActionPlatform.ms_kValueMappingStrings.Count; ++index)
        {
          if (InputActionPlatform.ms_kValueMappingStrings[index] == value)
          {
            this.m_iValueMapping = index;
            break;
          }
        }
      }
    }

    [XmlIgnore]
    public InputAction Owner
    {
      get => this.m_kOwnerObject;
      set
      {
        this.m_kOwnerObject = value;
        foreach (InputMapping kMapping in this.m_kMappings)
          kMapping.Owner = this;
      }
    }

    [XmlIgnore]
    [Browsable(false)]
    public bool HasPlayer1Mappings => this.m_boHasPlayer1Mappings;

    public void CheckPlayerMappings()
    {
      this.m_boHasPlayer1Mappings = false;
      foreach (InputMapping kMapping in this.m_kMappings)
      {
        kMapping.CheckPlayerMappings();
        if (kMapping.HasPlayer1Mapping)
          this.m_boHasPlayer1Mappings = true;
      }
    }

    public static void InitializeStrings()
    {
      InputActionPlatform.ms_kEventStrings = new List<string>();
      InputActionPlatform.ms_kEventStrings.Add("IE_Tapped");
      InputActionPlatform.ms_kEventStrings.Add("IE_Held");
      InputActionPlatform.ms_kEventStrings.Add("IE_Pressed");
      InputActionPlatform.ms_kEventStrings.Add("IE_Released");
      InputActionPlatform.ms_kEventStrings.Add("IE_Poll");
      InputActionPlatform.ms_kEventStrings.Add("IE_Accumulate");
      InputActionPlatform.ms_kValueMappingStrings = new List<string>();
      InputActionPlatform.ms_kValueMappingStrings.Add("VM_1D");
      InputActionPlatform.ms_kValueMappingStrings.Add("VM_2D_Independent");
      InputActionPlatform.ms_kValueMappingStrings.Add("VM_2D_Circle");
      InputActionPlatform.ms_kValueMappingStrings.Add("VM_Combo");
      InputActionPlatform.ms_kInputModifierStrings = new List<string>();
      InputActionPlatform.ms_kInputModifierStrings.Add("IM_NONE");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM1_ON");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM1_OFF");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM2_ON");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM2_OFF");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM3_ON");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM3_OFF");
      InputActionPlatform.ms_kInputModifierStrings.Add("IM23_OFF");
    }

    public object Clone()
    {
      InputActionPlatform inputActionPlatform = new InputActionPlatform();
      inputActionPlatform.m_iEvent = this.m_iEvent;
      inputActionPlatform.m_fExponent = this.m_fExponent;
      inputActionPlatform.m_iInputModifier = this.m_iInputModifier;
      inputActionPlatform.m_fMinValueThreshold = this.m_fMinValueThreshold;
      inputActionPlatform.m_fMaxValueThreshold = this.m_fMaxValueThreshold;
      inputActionPlatform.m_fScalar = this.m_fScalar;
      inputActionPlatform.m_iValueMapping = this.m_iValueMapping;
      inputActionPlatform.m_kMappings = new List<InputMapping>(this.m_kMappings.Count);
      foreach (InputMapping kMapping in this.m_kMappings)
        inputActionPlatform.m_kMappings.Add((InputMapping) kMapping.Clone());
      foreach (InputMapping kMapping in inputActionPlatform.m_kMappings)
        kMapping.Owner = inputActionPlatform;
      inputActionPlatform.m_kOwnerObject = this.m_kOwnerObject;
      inputActionPlatform.CheckPlayerMappings();
      return (object) inputActionPlatform;
    }
  }
}

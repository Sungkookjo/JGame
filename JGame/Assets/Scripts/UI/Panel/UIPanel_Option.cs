﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JGame.Sound;

namespace JGame
{
    public class UIPanel_Option : UIPanel
    {
        public Slider sd_EffectVolume;
        public Slider sd_BGMVolume;

        protected override void InitFromAwake()
        {
            base.InitFromAwake();

            if( sd_EffectVolume != null )
            {
                sd_EffectVolume.value = SoundManager.instance.GetVolume(SoundType.Effect);
                sd_EffectVolume.onValueChanged.AddListener((value) => { SoundManager.instance.SetVolume(SoundType.Effect, value); });
            }

            if( sd_BGMVolume != null )
            {
                sd_BGMVolume.value = SoundManager.instance.GetVolume(SoundType.BGM);
                sd_BGMVolume.onValueChanged.AddListener((value) => { SoundManager.instance.SetVolume(SoundType.BGM, value); });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BookExample
{
    static public class AudioSupport
    {
        // Audio effect files
        private static Dictionary<String, SoundEffect> sAudioEffects = 
                        new Dictionary<string,SoundEffect>();

        // Constant background audio
        private static SoundEffectInstance sBackgroundAudio = null;

        /// <summary>
        /// Plays a effect cue. Once started cannot stop. 
        /// </summary>
        /// <param name="cueName">Audio file name representing the cue</param>
        static public void PlayACue(String cueName)
        {
            SoundEffect sound = FindAudioClip(cueName);
            if (null != sound)
                sound.Play();
        }

        /// <summary>
        /// Plays the background audio. Null or "" name stops the background audio.
        /// </summary>
        /// <param name="bgAudio">Audio file name.</param>
        /// <param name="level">Volumd to play (0 to 1)</param>
        static public void PlayBackgroundAudio(String bgAudio, float level)
        {
            StopBg();
            if (("" != bgAudio) || (null != bgAudio))
            {
                level = MathHelper.Clamp(level, 0f, 1f);
                StartBg(bgAudio, level);
            }
        }

        /// <summary>
        /// Finds and loads a audio file name, create a SoundEffect object
        /// and store into the class dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static private SoundEffect FindAudioClip(String name)
        {
            SoundEffect sound = null;
            if (sAudioEffects.ContainsKey(name))
                sound = sAudioEffects[name];
            else
            {
                sound = Game1.sContent.Load<SoundEffect>(name);
                if (null != sound)
                    sAudioEffects.Add(name, sound);
            }
            return sound;
        }

        /// <summary>
        ///  start background audio with the given audio name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="level"></param>
        static private void StartBg(String name, float level)
        {
            SoundEffect bgm = FindAudioClip(name);
            sBackgroundAudio = bgm.CreateInstance();
            sBackgroundAudio.IsLooped = true;
            sBackgroundAudio.Volume = level;
            sBackgroundAudio.Play();
        }

        /// <summary>
        ///  Stop the background audio if it is playing.
        /// </summary>
        static private void StopBg()
        {
            if (null != sBackgroundAudio)
            {
                sBackgroundAudio.Pause();
                sBackgroundAudio.Stop();
                sBackgroundAudio.Volume = 0f;
                sBackgroundAudio.Dispose();
            }
            sBackgroundAudio = null;
        }

    }
}

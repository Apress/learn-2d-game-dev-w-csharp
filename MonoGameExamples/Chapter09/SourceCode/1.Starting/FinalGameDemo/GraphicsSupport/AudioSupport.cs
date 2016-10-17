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
    /// <summary>
    /// FontSupport: for demo purposes, this is defined to be a static class
    /// </summary>
    static public class AudioSupport
    {
        private static Dictionary<String, SoundEffect> sAudioEffects = 
                        new Dictionary<string,SoundEffect>();
                    // Audio effect files

        private static SoundEffectInstance sBackgroundAudio = null;
                    // constant background audio

        /// <summary>
        /// Plays a effect cue. Once started cannot stop. 
        /// </summary>
        /// <param name="cueName">Audio file name representing the cue</param>
        static public void PlayACue(String cueName)
        {
            SoundEffect e = FindAudioClip(cueName);
            if (null != e)
                e.Play();
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
            SoundEffect e = null;
            if (sAudioEffects.ContainsKey(name))
                e = sAudioEffects[name];
            else
            {
                e = Game1.sContent.Load<SoundEffect>(name);
                if (null != e)
                    sAudioEffects.Add(name, e);
            }
            return e;
        }

        /// <summary>
        ///  Stop the backgorund audio if it is playing.
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

        /// <summary>
        ///  start background audio with the given audio name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="level"></param>
        static private void StartBg(String name, float level)
        {
            SoundEffect bg = FindAudioClip(name);
            sBackgroundAudio = bg.CreateInstance();
            sBackgroundAudio.IsLooped = true;
            sBackgroundAudio.Volume = level;
            sBackgroundAudio.Play();
        }

    }
}

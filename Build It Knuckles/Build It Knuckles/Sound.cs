using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build_It_Knuckles
{
    /// <summary>
    /// Public Class that represents the functionality of playing sounds
    /// </summary>
    public class Sound
    {
        SoundEffect sound;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sound">Name of the sound</param>
        public Sound(string sound)
        {
            this.sound = GameWorld.ContentManager.Load<SoundEffect>(sound);
        }

        /// <summary>
        /// Play the sound file
        /// </summary>
        public void Play()
        {
            sound.Play();
        }

        /// <summary>
        /// Play the sound file with set volume
        /// </summary>
        /// <param name="volume">how loud it should be 0-1f</param>
        public void Play(float volume)
        {
            sound.Play(volume, 0f, 0f);
        }

        /// <summary>
        /// Play the sound file with set volume, pitch and pan
        /// </summary>
        /// <param name="volume">how loud it should be 0-1f</param>
        /// <param name="pitch">pitch of the sound 0-1f. 0 default</param>
        /// <param name="pan">pan of the sound 0-1f. 0 default</param>
        public void Play(float volume, float pitch, float pan)
        {
            sound.Play(volume, pitch, pan);
        }
    }
}

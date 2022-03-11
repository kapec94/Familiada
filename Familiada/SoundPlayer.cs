using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Familiada
{
    sealed class SoundPlayer
    {
        private static Dictionary<string, MediaPlayer> playerCache = new Dictionary<string, MediaPlayer>();
        private static string basePath = getBasePath();

        private static string getBasePath()
        {
            var fullPath = System.Reflection.Assembly.GetAssembly(typeof(SoundPlayer)).Location;
            return Path.GetDirectoryName(fullPath);
        }

        private static Uri getSoundUri(string soundName)
        {
            var path = string.Format("file:///{0}/Sounds/{1}.mp3", basePath, soundName);
            return new Uri(path);
        }

        private static MediaPlayer getMediaPlayer(string soundName)
        {
            if (playerCache.ContainsKey(soundName))
            {
                return playerCache[soundName];
            }

            var mp = new MediaPlayer();
            var soundUri = getSoundUri(soundName);
            mp.Open(soundUri);
            playerCache[soundName] = mp;
            return mp;
        }

        static void printObject(object obj)
        {
            var print = "";
            foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                print += string.Format("{0}={1}\n", name, value);
            }
            MessageBox.Show(print);
        }

        public static void PlaySound(string soundName)
        {
            var mp = getMediaPlayer(soundName);
            mp.Stop();
            mp.Play();
        }
    }
}
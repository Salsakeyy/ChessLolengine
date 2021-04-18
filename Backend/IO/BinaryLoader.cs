﻿using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Backend.Model;

namespace Backend.IO
{
    public class BinaryLoader : ILoader
    {
        public Container Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var container = formatter.Deserialize(stream) as Container;
            stream.Close();

            if (container == null)
                throw new SerializationException("Cast exception");

            return container;
        }

        public string Filter()
        {
            return "LolEngine Binary Save Files (*.we)|*.we";
        }
    }
}
﻿using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Backend.Model;

namespace Backend.IO
{
    public class BinarySaver : ISaver
    {
        public void Save(Container container, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, container);
            stream.Close();
        }

        public string Filter()
        {
            return "LolEngine Binary Save Files (*.we)|*.we";
        }
    }
}
#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using WheelChairGameLibrary.Sprites;
using WheelChairGameLibrary.Helpers;
using System.Xml.Serialization;
using System.IO;
#endregion

namespace WheelChairCollaborativeGame
{
    public static class Config
    {
        public static Vector2 resolution = new Vector2(1280, 720); //480, 640//576, 1024

        public static Vector2 cameraResolution = new Vector2(480, 640); //480, 640


        public enum ControlSelect
        {
            Joystick = 0,
            Front,
            Side,
            FrontAssyncronous
        }

        private static ControlSelect controlSelect = ControlSelect.Joystick;
        public static ControlSelect ControlSelected
        {
            get { return controlSelect; }
            set { controlSelect = value; }
        }

        private static int groupId = 1;
        public static int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public static void Load()
        {
            SerializableConfig savedConfig = null;
            XmlSerializer x = new XmlSerializer(typeof(SerializableConfig));
            using (FileStream st = File.Open("config.xml", FileMode.Open))
            {
                savedConfig = (SerializableConfig)x.Deserialize(st);
            }
            groupId = savedConfig.GroupId;
            ControlSelected = (ControlSelect)(savedConfig.InputId - 1);
        }

        public static void Save()
        {
            XmlSerializer x = new XmlSerializer(typeof(SerializableConfig));
            SerializableConfig serializaleConfig = new SerializableConfig();
            serializaleConfig.setValues(GroupId, (int)ControlSelected + 1);
            using (FileStream st = File.Open("config.xml", FileMode.Create))
            {
                x.Serialize(st, serializaleConfig);
            }
        }


    }

    [Serializable]
    public class SerializableConfig
    {
        public int GroupId { get; set; }
        public int InputId { get; set; }

        public void setValues(int groupId, int inputId){
            this.GroupId = groupId;
            this.InputId = inputId;
        }
    }


}

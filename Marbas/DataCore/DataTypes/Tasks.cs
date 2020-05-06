using System;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Marbas.DataCore.DataTypes
{
    [AddINotifyPropertyChangedInterface, MessagePackObject]
    public class Tasks
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Text { get; set; }
        [Key(2)] public string Strike { get; set; }
        [Key(3)] public bool Completed { get; set; }
        [Key(4)] public bool Display { get; set; }
        [Key(5)] public bool Editing { get; set; }
        [Key(6)] public DateTime DateAdded { get; set; }
        [Key(7)] public DateTime DateEnding { get; set; }
        [Key(8)] public string Alarm { get; set; }

        public static string DataFile =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data");

        public static ObservableCollection<Tasks> GetFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return new ObservableCollection<Tasks>();
            }

            return MessagePackSerializer.Deserialize<ObservableCollection<Tasks>>(File.ReadAllBytes(path));
        }

        public static void SaveToFile(string path, ObservableCollection<Tasks> tasks) 
            => File.WriteAllBytes(path, MessagePackSerializer.Serialize(tasks));
    }
}

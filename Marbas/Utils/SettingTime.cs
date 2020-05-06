using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marbas.DataCore;

namespace Marbas.Utils
{
    public class SettingTime
    {
        public static CoreModel Core { get; } = CoreModel.Instance;
        public static void SetHours()
        {
            for (int i = 0; i < 25; i++)
            {
                if (i < 10)
                {
                    Core.TimeHours.Add(new Container<string>($"0{i}")); 
                    continue;
                }

                Core.TimeHours.Add(new Container<string>($"{i}"));
            }
        }
        public static void SetMins()
        {
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    Core.TimeMins.Add(new Container<string>($"0{i}"));
                    continue;
                }

                Core.TimeMins.Add(new Container<string>($"{i}"));
            }
        
        }
    }
}

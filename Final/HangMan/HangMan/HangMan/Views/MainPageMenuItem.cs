using System;

namespace HangMan.Forms
{

    public class MainPageMenuItem
    {
        public MainPageMenuItem(Type targetType)
        {
            TargetType = targetType;
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
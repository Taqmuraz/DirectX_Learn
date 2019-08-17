using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectX_Learn
{
    public class Time
    {
        static Time ()
        {
            startTime = (float)DateTime.Now.TimeOfDay.TotalSeconds;
        }

        private static float startTime = 0f;

        public static float time
        {
            get
            {
                return (float)DateTime.Now.TimeOfDay.TotalSeconds - startTime;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl
{
    public static class Config
    {
        //public static string ApiUrl = "http://10.0.2.2:60592/api";//Android Emulator
        //public static string ApiUrl = "http://192.168.1.14:45455/api";//Conveyor for IIS Express in VS
        public static string ApiUrl = "https://services.merck-animal-health.com/sdct-service/v1/api";
        public static string AppKey = "ac12f743-14c0-4281-9540-ec2ab2753998";

        public static string AppCenterAndroidKey = "android=f4a15771-9b69-4103-b448-a5a2a500bc24;";
        public static string AppCenterIosKey = "ios=e106c265-bdad-4dd2-96ca-64677c07d169;";

        public static double MonkeyCacheExpiry = 7;
        public static bool TestMode = false;

        public static string SurveyFileName = "sdct.survey.json";
    }
}

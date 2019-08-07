using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl
{
    public static class Config
    {
        public static string ApiUrl = "http://localhost:60592/";
        public static string AppCenterAndroidKey = "android=f4a15771-9b69-4103-b448-a5a2a500bc24;";
        public static string AppCenterIosKey = "ios=e106c265-bdad-4dd2-96ca-64677c07d169;";
        public static string MonkeyCacheFarms = "UcFarms";
        public static string MonkeyCacheCows = "UcCows";
        public static string MonkeyCacheSurvey = "UcSurvey";
        public static double MonkeyCacheExpiry = 7;
        public static bool TestMode = true;

        public static string SurveyFileName = "sdct.survey.json";
    }
}

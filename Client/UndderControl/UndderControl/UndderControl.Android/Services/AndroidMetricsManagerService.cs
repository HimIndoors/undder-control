using System;
using System.Collections.Generic;
using UndderControl.Services;
using UndderControl.Droid.Services;
using Firebase.Analytics;
using Xamarin.Forms;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMetricsManagerService))]
namespace UndderControl.Droid.Services
{
    class AndroidMetricsManagerService : IMetricsManagerService
    {
        public void TrackEvent(string eventName)
        {
            SendEvent(eventName, null);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties)
        {
            SendEvent(eventName, properties);
        }

        public void TrackException(string eventName, Exception exception)
        {
            var properties = new Dictionary<string, string>
            {
                {"error", exception.Message},
            };

            SendEvent(eventName, properties);
        }

        public void TrackLatency(string eventName, TimeSpan latency)
        {
            var properties = new Dictionary<string, string>
            {
                { "latency", latency.TotalMilliseconds.ToString() },
            };

            SendEvent(eventName, properties);
        }

        private void SendEvent(string eventName, Dictionary<string,string> parameters)
        {
            var firebaseAnalytics =  FirebaseAnalytics.GetInstance(Android.App.Application.Context);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventName, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventName, bundle);
        }
    }
}
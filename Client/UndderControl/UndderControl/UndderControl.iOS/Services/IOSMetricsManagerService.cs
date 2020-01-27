using Firebase.Analytics;
using Foundation;
using System;
using System.Collections.Generic;
using UndderControl.iOS.Services;
using UndderControl.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSMetricsManagerService))]

namespace UndderControl.iOS.Services
{
    class IOSMetricsManagerService : IMetricsManagerService
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

        private void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                Analytics.LogEvent(eventId, (Dictionary<object, object>)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }
    }
}
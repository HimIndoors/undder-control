﻿using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using UndderControl.iOS.Services;
using UndderControl.Services;

[assembly: Xamarin.Forms.Dependency(typeof(IOSMetricsManagerService))]

namespace UndderControl.iOS.Services
{
    class IOSMetricsManagerService : IMetricsManagerService
    {
        public void TrackEvent(string eventName)
        {
            Analytics.TrackEvent(eventName);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties)
        {
            Analytics.TrackEvent(eventName, properties);
        }

        public void TrackException(string eventName, Exception exception)
        {
            var properties = new Dictionary<string, string>
            {
                {"error", exception.Message},
            };

            Analytics.TrackEvent(eventName, properties);
        }

        public void TrackLatency(string eventName, TimeSpan latency)
        {
            var properties = new Dictionary<string, string>
            {
                { "latency", latency.TotalMilliseconds.ToString() },
            };

            Analytics.TrackEvent(eventName, properties);
        }
    }
}
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl.Events
{
    public class CowStatusResultsEvent : PubSubEvent
    { }
    public class SurveyResultsEvent : PubSubEvent
    { }
    public class QuestionChangedEvent : PubSubEvent
    { }
    public class HtmlChangedEvent : PubSubEvent
    { }
    public class FarmNavigationEvent : PubSubEvent
    { }
    public class CowStatusRefreshEvent : PubSubEvent
    { }
    public class RootPageRefreshEvent : PubSubEvent
    { }
    public class StatementsUpdatedEvent : PubSubEvent
    { }
    public class LogOutEvent : PubSubEvent
    { }
}

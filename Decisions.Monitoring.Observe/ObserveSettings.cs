using System.Collections.Generic;
using System.Runtime.Serialization;
using DecisionsFramework;
using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.ServiceLayer.Actions;
using DecisionsFramework.ServiceLayer.Actions.Common;
using DecisionsFramework.ServiceLayer.Utilities;
using DecisionsFramework.Utilities.validation.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Decisions.Monitoring.Observe
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Fatal = 16
    }

    [ORMEntity]
    [DataContract]
    public class ObserveSettings : AbstractModuleSettings, IValidationSource, IInitializable
    {
        public const string DefaultBaseUrl = "https://collect.observeinc.com/v1/observations/test";

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Minimum Log Level to Send", 1)]
        [EnumEditor]
        public LogLevel MinSendLogLevel { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Log Base Url", 2)]
        [PropertyHiddenByValue(nameof(MinSendLogLevel), LogLevel.None, true)]
        //[EmptyStringRule("Base Url is required")]
        public string LogBaseUrl { get; set; }

        [DataMember]
        [PropertyHiddenByValue(nameof(MinSendLogLevel), LogLevel.None, true)]
        [PropertyClassificationAttribute("Log Token", 3)]
        public string LogToken { get; set; }


        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Send Metrics", 4)]
        public bool SendMetrics { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Metrics Base Url", 5)]
        [PropertyHiddenByValue(nameof(SendMetrics), false, true)]
        //[EmptyStringRule("Base Url is required")]
        public string MetricsBaseUrl { get; set; }

        [ORMField]
        [DataMember]
        [PropertyHiddenByValue(nameof(SendMetrics), false, true)]
        [PropertyClassificationAttribute("Metrics Token", 6)]
        public string MetricsToken { get; set; }

        public void Initialize()
        {
            var me = ObserveSettings.Instance();
            if (string.IsNullOrEmpty(Id))
            {
                me.LogBaseUrl = DefaultBaseUrl;
                me.MetricsBaseUrl = DefaultBaseUrl;
                me.MinSendLogLevel = LogLevel.None;
                ModuleSettingsAccessor<ObserveSettings>.SaveSettings();
            }
        }

        public ValidationIssue[] GetValidationIssues()
        {
            var issues = new List<ValidationIssue>();

            if ((MinSendLogLevel != LogLevel.None) )
            {
                if (string.IsNullOrEmpty(LogBaseUrl))
                    issues.Add(new ValidationIssue(this, "Log Base URL must be supplied", "", BreakLevel.Fatal, nameof(LogToken)));
                if (string.IsNullOrEmpty(LogToken))
                    issues.Add(new ValidationIssue(this, "Log Token must be supplied", "", BreakLevel.Fatal, nameof(LogToken)));

            };

            if (SendMetrics)
            {
                if (string.IsNullOrEmpty(MetricsBaseUrl))
                    issues.Add(new ValidationIssue(this, "Metrics Base URL must be supplied", "", BreakLevel.Fatal, nameof(LogToken)));
                if (string.IsNullOrEmpty(MetricsToken))
                    issues.Add(new ValidationIssue(this, "Metrics Token must be supplied", "", BreakLevel.Fatal, nameof(MetricsToken)));
            }

            return issues.ToArray();
        }

        public override BaseActionType[] GetActions(AbstractUserContext userContext, EntityActionType[] types)
        {
            var all = new List<BaseActionType>();
            all.Add(new EditEntityAction(typeof(ObserveSettings), "Edit", "Edit"));
            return all.ToArray();
        }

        static public ObserveSettings Instance() { 
            return ModuleSettingsAccessor<ObserveSettings>.GetSettings();
        }
    }
}
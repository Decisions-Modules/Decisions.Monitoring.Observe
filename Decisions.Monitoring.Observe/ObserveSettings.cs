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
    public class ObserveSettings : AbstractModuleSettings, IInitializable
    {
        public const string DefaultBaseUrl = "https://collect.observeinc.com/v1/observations";

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Log Base Url", 1)]
        [EmptyStringRule("Base Url is required")]
        public string BaseUrl { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Account Number", 2)]
        [EmptyStringRule("Account Number is required")]
        public string AccountNumber { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Minimum Log Level to Send", 3)]
        [EnumEditor]
        public LogLevel MinSendLogLevel { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Send Metrics", 5)]
        public bool SendMetrics { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Auth Token", 6)]
        [EmptyStringRule("Auth Token is required")]
        public string AuthToken { get; set; }

        public void Initialize()
        {
            var me = Instance();
            if (string.IsNullOrEmpty(Id))
            {
                me.BaseUrl = DefaultBaseUrl;
                me.MinSendLogLevel = LogLevel.None;
                ModuleSettingsAccessor<ObserveSettings>.SaveSettings();
            }
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
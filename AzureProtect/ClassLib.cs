using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureProtect
{
    public class GetSitedata
    {
        public string SiteType { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string SiteLongitude { get; set; }
        public string SiteLatitude { get; set; }
        public string SitePhone { get; set; }
    }

    public class WeatherInformation
    {
        public string LocationName { get; set; }
        public List<weatherElement> Elementweather { get; set; }
        public WeatherInformation()
        {
            Elementweather = new List<weatherElement>();
        }
    }

    public class weatherElement
    {
        public string weatherType { get; set; }
        public List<weatherElementTimeData> TimeData { get; set; }
        public weatherElement()
        {
            TimeData = new List<weatherElementTimeData>();
        }
    }

    public class weatherElementTimeData
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string parameterName { get; set; }
        public string parameterUnit { get; set; }
        public string parameterUnitType1 { get; set; }
    }

    public class UVData
    {
        public string LocationName { get; set; }
        public string UVI { get; set; }
        public string UVIStatus { get; set; }
        public string PublishTime { get; set; }
    }

    public class AQXData
    {
        public string SiteName{ get; set; }
        public string County{ get; set; }
        public string PSI{ get; set; }
        public string MajorPollutant{ get; set; }
        public string Status{ get; set; }
        public string SO2{ get; set; }
        public string CO{ get; set; }
        public string O3{ get; set; }
        public string PM10{ get; set; }
        public string PM25{ get; set; }
        public string NO2{ get; set; }
        public string WindSpeed{ get; set; }
        public string WindDirec{ get; set; }
        public string FPMI{ get; set; }
        public string PublishTime{ get; set; }
    }

    public class MarkerLocation
    {
        public string title { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class directionsLocation
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class UserInformation
    {
        public string PhoneType { get; set; }
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string PhoneURL { get; set; }
        public string UserLongitude { get; set; }
        public string UserLatitude { get; set; }
        public string PhoneNumber { get; set; }
        public string result { get; set; }
    }

    public class GroupInfo
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public List<GroupUserInfo> GroupUser;
        public GroupInfo()
        {
            GroupUser = new List<GroupUserInfo>();
        }
        public string master { get; set; }
    }

    public class GroupUserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public string UserLongitude { get; set; }
        public string UserLatitude { get; set; }
        public List<AlarmInfomation> alarm;
        public GroupUserInfo()
        {
            alarm = new List<AlarmInfomation>();
        }
    }

    public class AlarmInfomation
    {
        public string AlarmType { get; set; }
        public string AlarmMessage { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AlarmTime { get; set; }
        public List<HideoutInfomation> hideout;
        public AlarmInfomation()
        {
            hideout = new List<HideoutInfomation>();
        }
    }

    public class UpdateAlarmInfo
    {
        public string UserName { get; set; }
        public string AlarmMessage { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AlarmTime { get; set; }
    }

    public class HideoutInfomation
    {
        public string SiteAddress { get; set; }
        public string SiteLongitude { get; set; }
        public string SiteLatitude { get; set; }
        public string SiteName { get; set; }
        public string SitePhone { get; set; }
    }

    public class hazards
    {
        public string locationName { get; set; }
        public hazardConditions hazardConditionsData { get; set; }
        //public hazards()
        //{
        //    hazardConditionsData = new List<hazardConditions>();
        //}

    }

    public class hazardConditions
    {
        public hazardinfo info;
        public hazardvalidTime validTime;
        public hazardshazard hazard;
    }

    public class hazardinfo
    {
        public string phenomena{ get; set; }
        public string significance{ get; set; }
    }

    public class hazardvalidTime
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
    }

    public class hazardshazard
    {
        public string phenomena { get; set; }
        public List<string> affectedAreas { get; set; }

        public hazardshazard()
        {
            affectedAreas = new List<string>();
        }
    }

    public class MessageInfo
    {
        public string MessageID { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public List<MessageGroup> MessageGroup;
        public MessageInfo()
        {
            MessageGroup = new List<MessageGroup>();
        }
    }

    public class MessageGroup
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class MessageManagment
    {
        public string MessageID { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PriolotyID { get; set; }
        public string UserID { get; set; }
        public string type { get; set; }
        public string trigger { get; set; }
    }

    public class OpenDataInfo
    {
        public string OpenDataID { get; set; }
        public string OpenDataTitle { get; set; }
        public string OpenDataUpdate { get; set; }
        public string OpenDataSummary { get; set; }
    }
    //public class hazardsaffectedAreas
    //{
    //    public string locationName;
    //}

    public class WeatherAlarm
    {
        public string Location { get; set; }
        public string Summary { get; set; }
    }

    public class UserMessage
    {
        public string Message { get; set; }
    }

}
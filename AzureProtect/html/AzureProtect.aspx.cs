using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace AzureProtect.html
{
    public partial class AzureProtect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)//Get Method要做的事
            {
                //List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
                //MarkerLocation MarkerLocationItem = new MarkerLocation();
                //MarkerLocationItem.title = "總統府";
                //MarkerLocationItem.lat = 25.040282;
                //MarkerLocationItem.lng = 121.511901;
                //MarkerLocationItem.description = "";
                ////MarkerLocationItem.icon = "/Icon/people1.png";
                //MarkerLocationItemList.Add(MarkerLocationItem);
                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "ShowMapInit();", true);
            }

        }

        public void SetMap()
        {
            //List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            //MarkerLocation MarkerLocationItem = new MarkerLocation();
            //MarkerLocationItem.title = "總統府";
            //MarkerLocationItem.lat = 25.040282;
            //MarkerLocationItem.lng = 121.511901;
            //MarkerLocationItem.description = "";
            ////MarkerLocationItem.icon = "/Icon/people1.png";
            //MarkerLocationItemList.Add(MarkerLocationItem);
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "ShowMapInit();", true);
        }

        [System.Web.Services.WebMethod]
        public static string OpenDataGet(string inputValue)
        {
            FunctionLib FunctionList = new FunctionLib();
            string Result="";
            if (inputValue == "weather")
            {
                Result = FunctionList.WeatherDataParse();
            }
            else if (inputValue == "radiation")
            {
                Result = FunctionList.radiationInformation();
            }
            else if (inputValue == "uv")
            {
                Result = FunctionList.UVInformation();
            }
            else if (inputValue == "ocean")
            {
                Result = FunctionList.OceanDataParse();
            }
            else if (inputValue == "air")
            {
                Result = FunctionList.AQXInformation();
            }
            else
            {
                List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/GetOpenData/" + inputValue + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<GetSitedata>));
                List<GetSitedata> GetData = obj.ReadObject(resStream) as List<GetSitedata>;
                foreach (var item in GetData)
                {
                    MarkerLocation MarkerLocationItem = new MarkerLocation();
                    if (string.IsNullOrEmpty(item.SiteName) == false)
                    {
                        MarkerLocationItem.title = item.SiteName.Replace(@"""", "");
                    }
                    else
                    {
                        MarkerLocationItem.title = item.SiteType.Replace(@"""", "");
                    }
                    MarkerLocationItem.lat = double.Parse(item.SiteLatitude);
                    MarkerLocationItem.lng = double.Parse(item.SiteLongitude);
                    MarkerLocationItem.description = item.SiteAddress.Replace(@"""", "");
                    MarkerLocationItem.icon = "";
                    MarkerLocationItemList.Add(MarkerLocationItem);
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
                return json;
            }
            return Result;
        }

        [System.Web.Services.WebMethod]
        public static string GoogleRoute(string FormTextBox, string ToTextBox)
        {
            directionsLocation directionsLocationItem = new directionsLocation();
            directionsLocationItem.from = FormTextBox;
            directionsLocationItem.to = ToTextBox;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(directionsLocationItem);
            return json;
        }

        [System.Web.Services.WebMethod]
        public static string UserLogin(string UserName, string Password)
        {
            string Result = "";
            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/Login/" + UserName + "/" + Password + "/" + Guid.NewGuid();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
            string GetData = obj.ReadObject(resStream) as string;
            
            int InsertValue;
            if (int.TryParse(GetData, out InsertValue) == true)
            {
                Result= InsertValue.ToString();
            }
            else
            {
                Result= GetData;
            }
            return Result;
        }

        [System.Web.Services.WebMethod]
        public static string UserRegister(string UserName, string Password)
        {
            UserInformation UserInformationItem = new UserInformation();
            UserInformationItem.UserName = UserName;
            UserInformationItem.UserPassword = Password;

            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/UserRegister";
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(UserInformationItem);
            byte[] postBytes = Encoding.UTF8.GetBytes(json);
            httpWebRequest.ContentLength = postBytes.Length;
            using (Stream postStream = httpWebRequest.GetRequestStream())
            {
                postStream.Write(postBytes, 0, postBytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
            string GetData = obj.ReadObject(resStream) as string;
            return GetData;
        }

        [System.Web.Services.WebMethod]
        public static string UserAlarmMessage(string UserID)
        {
            string Result = "";
            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/UpdateAlarmMessage/" + UserID + "/" + Guid.NewGuid();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<UpdateAlarmInfo>));
            List<UpdateAlarmInfo> GetData = obj.ReadObject(resStream) as List<UpdateAlarmInfo>;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(GetData);
            return json;
        }


        //[System.Web.Services.WebMethod]
        public void GetMessage(string GetText)
        {
            //string result = "";
            //result = GetMessageText(GetText);
            //return result;
            List<UserMessage> UserMessageList = new List<UserMessage>();
            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/GetMessageInfo/" + GetText + "/0/" + Guid.NewGuid();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<MessageInfo>));
            List<MessageInfo> GetDataItem = obj.ReadObject(resStream) as List<MessageInfo>;

            var GetData =
                from GetDataInfo in GetDataItem
                orderby DateTime.Parse(GetDataInfo.Time) descending
                select GetDataInfo;

            if (GetData.Count() != 0)
            {
                ListBoxTalk.Items.Clear();
                foreach (var item in GetData)
                {
                    foreach (var items in item.MessageGroup)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        string Message = "[" + item.Time + "]";
                        if (string.IsNullOrEmpty(items.GroupName) == false)
                        {
                            Message += "(" + items.GroupName + ")" + item.UserName + ":" + item.Content;
                        }
                        else
                        {
                            Message += item.UserName + ":" + item.Content;
                        }
                        ListBoxTalk.Items.Add(Message);
                        newUserMessage.Message = Message;
                        UserMessageList.Add(newUserMessage);
                    }

                }

            }
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(UserMessageList);
            //return json;
        }

        public string GetMessageText(string UserID)
        {
            List<UserMessage> UserMessageList = new List<UserMessage>();
            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/GetMessageInfo/" + UserID + "/0/" + Guid.NewGuid();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<MessageInfo>));
            List<MessageInfo> GetDataItem = obj.ReadObject(resStream) as List<MessageInfo>;

            var GetData =
                from GetDataInfo in GetDataItem
                orderby GetDataInfo.MessageID descending
                select GetDataInfo;

            if (GetData.Count() != 0)
            {
                //ListBoxTalk.Items.Clear();
                foreach (var item in GetData)
                {
                    foreach (var items in item.MessageGroup)
                    {
                        UserMessage newUserMessage = new UserMessage();
                        string Message = "[" + item.Time + "]";
                        if (string.IsNullOrEmpty(items.GroupName) == false)
                        {
                            Message += "(" + items.GroupName + ")" + item.UserName + ":" + item.Content;
                        }
                        else
                        {
                            Message += item.UserName + ":" + item.Content;
                        }
                        //ListBoxTalk.Items.Add(Message);
                        newUserMessage.Message = Message;
                        UserMessageList.Add(newUserMessage);
                    }

                }
                
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(UserMessageList);
            return json;
        }

        //[System.Web.Services.WebMethod]
        public void AlarmWearher()
        {
            List<WeatherAlarm> WeatherAlarmList = new List<WeatherAlarm>();
            List<hazards> hazardsListData = new List<hazards>();
            string webAddr = "http://opendata.cwb.gov.tw/opendata/MFC/W-C0033-001.xml";
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader stmReader = new StreamReader(response.GetResponseStream());

            string stringResult = stmReader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringResult);

            XmlNamespaceManager xnm = new XmlNamespaceManager(xml.NameTable);
            xnm.AddNamespace("x", "urn:cwb:gov:tw:cwbcommon:0.1");

            //xmlDoc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
            string xpath = "/x:cwbopendata/x:dataset/x:location";

            foreach (XmlNode xn in xml.SelectNodes(xpath, xnm))
            {
                hazards hazardsInformationItem = new hazards();
                var wc = xn.ChildNodes;//(ypath, xnm);
                foreach (XmlNode ChileNodeInfo in wc)
                {
                    if (ChileNodeInfo.Name == "locationName")
                    {
                        hazardsInformationItem.locationName = ChileNodeInfo.InnerText;
                    }
                    else if (ChileNodeInfo.Name == "hazardConditions")
                    {
                        hazardConditions hazardConditionsItems = new hazardConditions();
                        foreach (XmlNode ChileNodeInfoElemat in ChileNodeInfo)
                        {

                            if (ChileNodeInfoElemat.Name == "hazards")
                            {
                                foreach (XmlNode ChileNodeInfoTimeData in ChileNodeInfoElemat)
                                {
                                    if (ChileNodeInfoTimeData.Name == "info")
                                    {
                                        hazardinfo hazardinfoItem = new hazardinfo();
                                        foreach (XmlNode ChileNodeInfoTimeDatainfo in ChileNodeInfoTimeData)
                                        {
                                            if (ChileNodeInfoTimeDatainfo.Name == "phenomena")
                                            {
                                                hazardinfoItem.phenomena = ChileNodeInfoTimeDatainfo.InnerText;
                                            }
                                            else if (ChileNodeInfoTimeDatainfo.Name == "significance")
                                            {
                                                hazardinfoItem.significance = ChileNodeInfoTimeDatainfo.InnerText;
                                            }
                                        }
                                        hazardConditionsItems.info = hazardinfoItem;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "validTime")
                                    {
                                        hazardvalidTime hazardvalidTimeItem = new hazardvalidTime();
                                        foreach (XmlNode ChileNodeInfoTimeDatainfo in ChileNodeInfoTimeData)
                                        {
                                            if (ChileNodeInfoTimeDatainfo.Name == "startTime")
                                            {
                                                hazardvalidTimeItem.startTime = ChileNodeInfoTimeDatainfo.InnerText;
                                            }
                                            else if (ChileNodeInfoTimeDatainfo.Name == "endTime")
                                            {
                                                hazardvalidTimeItem.endTime = ChileNodeInfoTimeDatainfo.InnerText;
                                            }
                                        }
                                        hazardConditionsItems.validTime = hazardvalidTimeItem;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "hazard")
                                    {
                                        hazardshazard hazardshazardItem = new hazardshazard();
                                        foreach (XmlNode ChileNodeInfoTimeDatainfo in ChileNodeInfoTimeData)
                                        {
                                            if (ChileNodeInfoTimeDatainfo.Name == "info")
                                            {
                                                foreach (XmlNode ChileNodeInfoTimeDatainfoItem in ChileNodeInfoTimeDatainfo)
                                                {
                                                    if (ChileNodeInfoTimeDatainfoItem.Name == "phenomena")
                                                    {
                                                        hazardshazardItem.phenomena = ChileNodeInfoTimeDatainfoItem.InnerText;
                                                    }
                                                    else if (ChileNodeInfoTimeDatainfoItem.Name == "affectedAreas")
                                                    {
                                                        foreach (XmlNode ChileNodeInfoTimeDatainfoItem1 in ChileNodeInfoTimeDatainfoItem)
                                                        {
                                                            if (ChileNodeInfoTimeDatainfoItem1.Name == "location")
                                                            {
                                                                foreach (XmlNode ChileNodeInfoTimeDatainfoItem2 in ChileNodeInfoTimeDatainfoItem1)
                                                                {
                                                                    if (ChileNodeInfoTimeDatainfoItem2.Name == "locationName")
                                                                    {
                                                                        hazardshazardItem.affectedAreas.Add(ChileNodeInfoTimeDatainfoItem2.InnerText);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        hazardConditionsItems.hazard = hazardshazardItem;
                                    }
                                }
                            }
                        }
                        hazardsInformationItem.hazardConditionsData = hazardConditionsItems;
                    }
                }
                hazardsListData.Add(hazardsInformationItem);
            }
            //ListBoxAlarm.Items.Clear();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Location", typeof(string)), new DataColumn("Summary", typeof(string)) });
            foreach (hazards item in hazardsListData)
            {

                WeatherAlarm newWeatherAlarm = new WeatherAlarm();
                ListItem newListItem = new ListItem();
                string HazardsInfo = item.locationName;
                
                string SummaryText = "";
                //if (string.IsNullOrEmpty(item.hazardConditionsData.hazard.phenomena) == false)
                if (item.hazardConditionsData.validTime != null)
                {
                    SummaryText = item.hazardConditionsData.info.phenomena + item.hazardConditionsData.info.significance + " ";
                    if (item.hazardConditionsData.hazard != null)
                    {
                        HazardsInfo += item.hazardConditionsData.hazard.phenomena + "(";
                        foreach (string items in item.hazardConditionsData.hazard.affectedAreas)
                        {
                            HazardsInfo += items + ",";
                        }
                        HazardsInfo += ")";
                    }
                }
                newWeatherAlarm.Location = HazardsInfo;
                newWeatherAlarm.Summary = SummaryText;
                WeatherAlarmList.Add(newWeatherAlarm);
                dt.Rows.Add(HazardsInfo, SummaryText);
                //ListBoxAlarm.Items.Add(HazardsInfo);

            }

            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(WeatherAlarmList);
            //return json;
            ListBoxAlarm.DataSource = dt;
            ListBoxAlarm.DataBind();
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            SetMap();
            MultiView1.ActiveViewIndex = int.Parse(HiddenField1.Value);
            if (HiddenField1.Value == "1")
            {
                if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false)
                {
                    TreeViewGet(HiddenFieldUserID.Value);
                }
                else
                {
                    if (DropDownListGroup.Items.Count != 0)
                    {
                        DropDownListGroup.Items.Clear();
                    }
                    if (TreeViewUser.Nodes.Count != 0)
                    {
                        TreeViewUser.Nodes.Clear();
                    }
                    if (ListBoxTalk.Items.Count != 0)
                    {
                        ListBoxTalk.Items.Clear();
                    }
                    Session["TreeNodeItem"] = null;

                }
            }
            else if (HiddenField1.Value == "2")
            {
                AlarmWearher();
            }
            else if (HiddenField1.Value == "4")
            {
                if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false)
                {
                    TreeViewGet(HiddenFieldUserID.Value);
                    GetMessage(HiddenFieldUserID.Value);
                }
                else
                {
                    if (DropDownListGroup.Items.Count != 0)
                    {
                        DropDownListGroup.Items.Clear();
                    }
                    if (TreeViewUser.Nodes.Count != 0)
                    {
                        TreeViewUser.Nodes.Clear();
                    }
                    if (ListBoxTalk.Items.Count != 0)
                    {
                        ListBoxTalk.Items.Clear();
                    }
                    Session["TreeNodeItem"] = null;
                }
            }

        }

        public void TreeViewGet(string UserID)
        {
            SetMap();
            if (Session["TreeNodeItem"] == null)
            {
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/GetGroupInfo/" + UserID + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<GroupInfo>));
                List<GroupInfo> GetDataItemInfo = obj.ReadObject(resStream) as List<GroupInfo>;

                TreeViewUser.Nodes.Clear();

                for (int i = 0; i < GetDataItemInfo.Count(); i++)
                {
                    if (i == 0)
                    {
                        DropDownListGroup.Items.Clear();
                        ListItem NewListItemInit = new ListItem();
                        NewListItemInit.Text = "全部";
                        NewListItemInit.Value = "0";
                        DropDownListGroup.Items.Add(NewListItemInit);
                    }
                    ListItem NewListItem = new ListItem();
                    NewListItem.Text = GetDataItemInfo[i].GroupName;
                    NewListItem.Value = GetDataItemInfo[i].GroupID;
                    DropDownListGroup.Items.Add(NewListItem);
                    TreeNode GroupNode = new TreeNode();
                    GroupNode.Text = GetDataItemInfo[i].GroupName + "(" + GetDataItemInfo[i].GroupID + ")";
                    GroupNode.Value = "G_" + GetDataItemInfo[i].GroupID;
                    foreach (var items in GetDataItemInfo[i].GroupUser)
                    {
                        string AlarmMessage = "";
                        TreeNode GroupNodeUser = new TreeNode();
                        GroupNodeUser.Text = items.UserName;
                        GroupNodeUser.Value = "U_" + items.UserID + "_" + items.UserLatitude + "_" + items.UserLongitude;
                        foreach (var messageItem in items.alarm)
                        {
                            AlarmMessage += "(" + messageItem.AlarmTime + ")" + messageItem.AlarmMessage + "<br/>";
                        }
                        GroupNodeUser.Value += "_" + AlarmMessage;
                        GroupNode.ChildNodes.Add(GroupNodeUser);
                    }
                    TreeViewUser.Nodes.Add(GroupNode);
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(GetDataItemInfo);
                Session["TreeNodeItem"] = json;
            }
            else
            {
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/GetGroupInfo/" + UserID + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<GroupInfo>));
                List<GroupInfo> GetDataItemInfo = obj.ReadObject(resStream) as List<GroupInfo>;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(GetDataItemInfo);
                if (Session["TreeNodeItem"].ToString().Length != json.Length)
                {
                    TreeViewUser.Nodes.Clear();
                    for (int i = 0; i < GetDataItemInfo.Count(); i++)
                    {
                        if (i == 0)
                        {
                            DropDownListGroup.Items.Clear();
                            ListItem NewListItemInit = new ListItem();
                            NewListItemInit.Text = "全部";
                            NewListItemInit.Value = "0";
                            DropDownListGroup.Items.Add(NewListItemInit);
                        }
                        ListItem NewListItem = new ListItem();
                        NewListItem.Text = GetDataItemInfo[i].GroupName;
                        NewListItem.Value = GetDataItemInfo[i].GroupID;
                        DropDownListGroup.Items.Add(NewListItem);
                        TreeNode GroupNode = new TreeNode();
                        GroupNode.Text = GetDataItemInfo[i].GroupName + "(" + GetDataItemInfo[i].GroupID + ")";
                        GroupNode.Value = "G_" + GetDataItemInfo[i].GroupID ;
                        foreach (var items in GetDataItemInfo[i].GroupUser)
                        {
                            string AlarmMessage ="";
                            TreeNode GroupNodeUser = new TreeNode();
                            GroupNodeUser.Text = items.UserName;
                            GroupNodeUser.Value = "U_" + items.UserID + "_" + items.UserLatitude + "_" + items.UserLongitude;
                            foreach (var messageItem in items.alarm)
                            {
                                AlarmMessage += "(" + messageItem.AlarmTime + ")" + messageItem.AlarmMessage + "<br/>";
                            }
                            GroupNodeUser.Value += "_" + AlarmMessage;
                            GroupNode.ChildNodes.Add(GroupNodeUser);
                        }
                        TreeViewUser.Nodes.Add(GroupNode);
                    }
                    Session["TreeNodeItem"] = json;
                }
            }
        }


        protected void TreeViewUser_SelectedNodeChanged(object sender, EventArgs e)
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            string GetValue = TreeViewUser.SelectedNode.Value;
            string[] TreeToken = GetValue.Split('_');
            if (TreeToken[0] == "G")
            {
                foreach (TreeNode item in TreeViewUser.SelectedNode.ChildNodes)
                {
                    string[] ChildTreeToken = item.Value.Split('_');
                    if (string.IsNullOrEmpty(ChildTreeToken[1]) == false && string.IsNullOrEmpty(ChildTreeToken[2]) == false && string.IsNullOrEmpty(ChildTreeToken[3]) == false)
                    {
                        MarkerLocation MarkerLocationItem = new MarkerLocation();
                        MarkerLocationItem.title = item.Text;
                        MarkerLocationItem.lat = double.Parse(ChildTreeToken[2]);
                        MarkerLocationItem.lng = double.Parse(ChildTreeToken[3]);
                        if (string.IsNullOrEmpty(ChildTreeToken[4]) == false)
                        {
                            MarkerLocationItem.description = ChildTreeToken[4];
                        }
                        else
                        {
                            MarkerLocationItem.description = "<div><h>" + item.Text + "</h><br/>" + "</div>";
                        }
                        //MarkerLocationItem.icon = "/Icon/people1.png";
                        MarkerLocationItemList.Add(MarkerLocationItem);
                    }

                }
                if (MarkerLocationItemList.Count() != 0)
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('以下成員未提供所在位置')", true);
                }
            }
            else if (TreeToken[0] == "U")
            {
                if (string.IsNullOrEmpty(TreeToken[1]) == false && string.IsNullOrEmpty(TreeToken[2]) == false && string.IsNullOrEmpty(TreeToken[3]) == false)
                {
                    MarkerLocation MarkerLocationItem = new MarkerLocation();
                    MarkerLocationItem.title = TreeViewUser.SelectedNode.Text;
                    MarkerLocationItem.lat = double.Parse(TreeToken[2]);
                    MarkerLocationItem.lng = double.Parse(TreeToken[3]);
                    //MarkerLocationItem.description = TreeViewUser.SelectedNode.Text;
                    if (string.IsNullOrEmpty(TreeToken[4]) == false)
                    {
                        MarkerLocationItem.description = TreeToken[4];
                    }
                    else
                    {
                        MarkerLocationItem.description = "<div><h>" + TreeViewUser.SelectedNode.Text + "</h><br/>" + "</div>";
                    }
                    //MarkerLocationItem.icon = "/Icon/people1.png";
                    MarkerLocationItemList.Add(MarkerLocationItem);

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('成員未提供所在位置')", true);
                }
            }
        }

        protected void SeleteButton_Click(object sender, EventArgs e)
        {
            SetMap();
            string webAddr = "http://protecttw.cloudapp.net/Service1.svc/Get_OpenData/" + DropDownFunction.SelectedValue + "/30/" + Guid.NewGuid();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream resStream = response.GetResponseStream();
            DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(List<OpenDataInfo>));
            List<OpenDataInfo> GetData = obj.ReadObject(resStream) as List<OpenDataInfo>;
            

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Time", typeof(string)), new DataColumn("Type", typeof(string)), new DataColumn("Summary", typeof(string)) });

            foreach (var item in GetData)
            {
                dt.Rows.Add(item.OpenDataUpdate, item.OpenDataTitle, item.OpenDataSummary);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void TreeTimer_Tick(object sender, EventArgs e)
        {
            //ListBoxTalk.Items.Add("test");
            if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false)
            {
                GetMessage(HiddenFieldUserID.Value);
                TreeViewGet(HiddenFieldUserID.Value);
            }
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false && string.IsNullOrEmpty(MessageText.Text) == false)
            {
                MessageManagment MessageManagmentItem = new MessageManagment();
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/message_add/" + HiddenFieldUserID.Value + "/" + MessageText.Text + "/0/0/" + DropDownListGroup.SelectedValue + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
                string GetData = obj.ReadObject(resStream) as string;
                if (GetData == "success")
                {
                    System.Threading.Thread.Sleep(1000);
                    GetMessage(HiddenFieldUserID.Value);
                    MessageText.Text = "";
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('請確認使用者是否登入或訊息欄是否有填字')", true);
            }
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false && string.IsNullOrEmpty(GroupCreate.Text) == false)
            {
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/CreateGroup/" + HiddenFieldUserID.Value + "/" + GroupCreate.Text + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
                string GetData = obj.ReadObject(resStream) as string;
                if (GetData == "success")
                {
                    TreeViewGet(HiddenFieldUserID.Value);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('" + GetData + "')", true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('請登入帳號')", true);
                }
                else if (string.IsNullOrEmpty(GroupCreate.Text) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('請輸入群組名稱')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('未知的錯誤')", true);
                }
            }
            SetMap();
        }

        protected void AddGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == false && string.IsNullOrEmpty(GroupID.Text) == false)
            {
                string webAddr = "http://protecttw.cloudapp.net/Service1.svc/JoinGroup/" + HiddenFieldUserID.Value + "/" + GroupID.Text + "/" + Guid.NewGuid();
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream resStream = response.GetResponseStream();
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
                string GetData = obj.ReadObject(resStream) as string;
                if (GetData == "success")
                {
                    TreeViewGet(HiddenFieldUserID.Value);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('" + GetData + "')", true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(HiddenFieldUserID.Value) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('請登入帳號')", true);
                }
                else if (string.IsNullOrEmpty(GroupCreate.Text) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('請輸入群組名稱')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('未知的錯誤')", true);
                }
            }
            SetMap();
        }


    }
}
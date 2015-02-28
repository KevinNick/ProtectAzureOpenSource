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

namespace AzureProtect
{
    public class FunctionLib
    {
        public string WeatherDataParse()
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            List<WeatherInformation> WeatherInformationList = new List<WeatherInformation>();
            string webAddr = "http://opendata.cwb.gov.tw/opendata/MFC/F-C0032-001.xml";
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
                WeatherInformation WeatherInformationItem = new WeatherInformation();
                var wc = xn.ChildNodes;//(ypath, xnm);
                foreach (XmlNode ChileNodeInfo in wc)
                {
                    if (ChileNodeInfo.Name == "locationName")
                    {
                        WeatherInformationItem.LocationName = ChileNodeInfo.InnerText;
                    }
                    else if (ChileNodeInfo.Name == "weatherElement")
                    {
                        weatherElement weatherElementItem = new weatherElement();
                        foreach (XmlNode ChileNodeInfoElemat in ChileNodeInfo)
                        {
                            if (ChileNodeInfoElemat.Name == "elementName")
                            {
                                weatherElementItem.weatherType = ChileNodeInfoElemat.InnerText;
                            }
                            else if (ChileNodeInfoElemat.Name == "time")
                            {
                                weatherElementTimeData weatherElementTimeDataItem = new weatherElementTimeData();
                                foreach (XmlNode ChileNodeInfoTimeData in ChileNodeInfoElemat)
                                {
                                    if (ChileNodeInfoTimeData.Name == "startTime")
                                    {
                                        weatherElementTimeDataItem.startTime = ChileNodeInfoTimeData.InnerText;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "endTime")
                                    {
                                        weatherElementTimeDataItem.endTime = ChileNodeInfoTimeData.InnerText;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "parameter")
                                    {
                                        foreach (XmlNode ChileNodeparameterData in ChileNodeInfoTimeData)
                                        {
                                            if (ChileNodeparameterData.Name == "parameterName")
                                            {
                                                weatherElementTimeDataItem.parameterName = ChileNodeparameterData.InnerText;
                                            }
                                            else if (ChileNodeparameterData.Name == "parameterValue")
                                            {
                                                weatherElementTimeDataItem.parameterUnit = ChileNodeparameterData.InnerText;
                                            }
                                        }
                                    }
                                }
                                weatherElementItem.TimeData.Add(weatherElementTimeDataItem);
                            }
                        }
                        WeatherInformationItem.Elementweather.Add(weatherElementItem);
                    }
                    Console.WriteLine(ChileNodeInfo.InnerText);
                }
                WeatherInformationList.Add(WeatherInformationItem);
            }
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("title"), new DataColumn("lat"), new DataColumn("lng"), new DataColumn("description"), new DataColumn("icon") });
            double lat = 0, lng = 0;
            foreach (var item in WeatherInformationList)
            {

                if (item.LocationName == "臺北市")
                {
                    lat = 25.05;
                    lng = 121.55;
                }
                else if (item.LocationName == "新北市")
                {
                    lat = 25.016135;
                    lng = 121.465202;
                }
                else if (item.LocationName == "花蓮縣")
                {
                    lat = 23.991637;
                    lng = 121.619850;
                }
                else if (item.LocationName == "臺東縣")
                {
                    lat = 22.750;
                    lng = 121.150;
                }
                else if (item.LocationName == "澎湖縣")
                {
                    lat = 23.667;
                    lng = 119.583;
                }
                else if (item.LocationName == "金門縣")
                {
                    lat = 24.457;
                    lng = 118.358;
                }
                else if (item.LocationName == "宜蘭縣")
                {
                    lat = 24.731163;
                    lng = 121.763223;
                }
                else if (item.LocationName == "新竹縣")
                {
                    lat = 24.833;
                    lng = 121.000;
                }
                else if (item.LocationName == "苗栗縣")
                {
                    lat = 24.550;
                    lng = 120.817;
                }
                else if (item.LocationName == "雲林縣")
                {
                    lat = 23.717;
                    lng = 120.433;
                }
                else if (item.LocationName == "嘉義縣")
                {
                    lat = 23.459242;
                    lng = 120.293525;
                }
                else if (item.LocationName == "屏東縣")
                {
                    lat = 22.683605;
                    lng = 120.487922;
                }
                else if (item.LocationName == "臺中市")
                {
                    lat = 24.161822;
                    lng = 120.646969;
                }
                else if (item.LocationName == "臺南市")
                {
                    lat = 23.198;
                    lng = 120.248;
                }
                else if (item.LocationName == "高雄市")
                {
                    lat = 22.631967;
                    lng = 120.343937;
                }
                else if (item.LocationName == "嘉義市")
                {
                    lat = 23.481546;
                    lng = 120.453556;
                }
                else if (item.LocationName == "彰化縣")
                {
                    lat = 24.075854;
                    lng = 120.544192;
                }
                else if (item.LocationName == "桃園市")
                {
                    lat = 24.995887;
                    lng = 121.300796;
                }
                else if (item.LocationName == "基隆市")
                {
                    lat = 25.131883;
                    lng = 121.744510;
                }
                else if (item.LocationName == "新竹市")
                {
                    lat = 24.806976;
                    lng = 120.968781;
                }
                else if (item.LocationName == "南投縣")
                {
                    lat = 23.902850;
                    lng = 120.690457;
                }
                else if (item.LocationName == "連江縣")
                {
                    lat = 26.159297;
                    lng = 119.951958;
                }



                string WearherString = item.LocationName + "<br>";
                foreach (var items in item.Elementweather)
                {
                    if (items.weatherType == "Wx")
                    {
                        WearherString += "天氣狀況:" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "MaxT")
                    {
                        WearherString += "氣溫:" + items.TimeData.First().parameterName + "℃ ~";
                    }
                    else if (items.weatherType == "MinT")
                    {
                        WearherString += items.TimeData.First().parameterName + "℃<br>";
                    }
                    else if (items.weatherType == "CI")
                    {
                        WearherString += "舒適度:" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "PoP")
                    {
                        WearherString += "降雨機率:" + items.TimeData.First().parameterName + "% <br>";
                    }
                }
                //dt.Rows.Add(item.LocationName, lat, lng, WearherString, "");

                MarkerLocation MarkerLocationItem = new MarkerLocation();
                MarkerLocationItem.title = item.LocationName;
                MarkerLocationItem.lat = lat;
                MarkerLocationItem.lng = lng;
                MarkerLocationItem.description = WearherString;
                MarkerLocationItem.icon = "";
                MarkerLocationItemList.Add(MarkerLocationItem);

            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            return json;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
            //rptMarkers.DataSource = dt;
            //rptMarkers.DataBind();
        }

        public string radiationInformation()
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            List<string> DataString = new List<string>();
            List<WeatherInformation> WeatherInformationList = new List<WeatherInformation>();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("title"), new DataColumn("lat"), new DataColumn("lng"), new DataColumn("description"), new DataColumn("icon") });
            string webAddr = "http://www.aec.gov.tw/open/gammamonitor.csv";
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            TextFieldParser parser = new TextFieldParser(response.GetResponseStream(), Encoding.GetEncoding("Big5"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                DataString.Add(parser.ReadLine());
            }
            parser.Close();
            for (int i = 1; i < DataString.Count(); i++)
            {
                string SiteInformation = "";
                double lat = 0, lng = 0;
                string[] fields = DataString[i].Split(',');
                string MonitorSite = fields[0];
                if (MonitorSite == "石門")
                {
                    lat = 25.291177777778;
                    lng = 121.56235833333;
                }
                else if (MonitorSite == "三芝")
                {
                    lat = 25.233761111111;
                    lng = 121.51585277778;
                }
                else if (MonitorSite == "石崩山")
                {
                    lat = 25.26285;
                    lng = 121.56554722222;
                }
                else if (MonitorSite == "茂林")
                {
                    lat = 25.270061111111;
                    lng = 121.59109722222;
                }
                else if (MonitorSite == "金山")
                {
                    lat = 25.220908333333;
                    lng = 121.63544722222;
                }
                else if (MonitorSite == "野柳")
                {
                    lat = 25.206275;
                    lng = 121.68911388889;
                }
                else if (MonitorSite == "大鵬")
                {
                    lat = 25.208122222222;
                    lng = 121.65156111111;
                }
                else if (MonitorSite == "陽明山")
                {
                    lat = 25.162358333333;
                    lng = 121.54443055556;
                }
                else if (MonitorSite == "大坪")
                {
                    lat = 25.167986111111;
                    lng = 121.6386;
                }
                else if (MonitorSite == "萬里")
                {
                    lat = 25.176480555556;
                    lng = 121.68851666667;
                }
                else if (MonitorSite == "台北")
                {
                    lat = 25.079077777778;
                    lng = 121.57386944444;
                }
                else if (MonitorSite == "宜蘭")
                {
                    lat = 24.763738888889;
                    lng = 121.75609444444;
                }
                else if (MonitorSite == "龍潭")
                {
                    lat = 24.840011111111;
                    lng = 121.24026388889;
                }
                else if (MonitorSite == "台中")
                {
                    lat = 24.145947222222;
                    lng = 120.68403611111;
                }
                else if (MonitorSite == "台東")
                {
                    lat = 22.752413888889;
                    lng = 121.15524722222;
                }
                else if (MonitorSite == "高雄")
                {
                    lat = 22.650588888889;
                    lng = 120.34689722222;
                }
                else if (MonitorSite == "恆春")
                {
                    lat = 22.003755555556;
                    lng = 120.74675277778;
                }
                else if (MonitorSite == "龍泉")
                {
                    lat = 21.980541666667;
                    lng = 120.72974444444;
                }
                else if (MonitorSite == "大光")
                {
                    lat = 21.951361111111;
                    lng = 120.74048333333;
                }
                else if (MonitorSite == "墾丁")
                {
                    lat = 21.945133333333;
                    lng = 120.80132777778;
                }
                else if (MonitorSite == "後壁湖")
                {
                    lat = 21.944544444444;
                    lng = 120.74337222222;
                }
                else if (MonitorSite == "澳底")
                {
                    lat = 25.047575;
                    lng = 121.92381111111;
                }
                else if (MonitorSite == "貢寮")
                {
                    lat = 25.010791666667;
                    lng = 121.91975277778;
                }
                else if (MonitorSite == "阿里山")
                {
                    lat = 23.508177777778;
                    lng = 120.81319166667;
                }
                else if (MonitorSite == "金門")
                {
                    lat = 24.409044444444;
                    lng = 118.28927222222;
                }
                else if (MonitorSite == "蘭嶼")
                {
                    lat = 22.049308333333;
                    lng = 121.51243333333;
                }
                else if (MonitorSite == "台南")
                {
                    lat = 23.037958333333;
                    lng = 120.23673055556;
                }
                else if (MonitorSite == "龍門")
                {
                    lat = 25.030563888889;
                    lng = 121.92868333333;
                }
                else if (MonitorSite == "雙溪")
                {
                    lat = 25.035308333333;
                    lng = 121.86281944444;
                }
                else if (MonitorSite == "三港")
                {
                    lat = 25.053694444444;
                    lng = 121.88053055556;
                }
                else if (MonitorSite == "新竹")
                {
                    lat = 24.784136111111;
                    lng = 120.99300555556;
                }
                else if (MonitorSite == "花蓮")
                {
                    lat = 23.977619444444;
                    lng = 121.61317222222;
                }
                else if (MonitorSite == "澎湖")
                {
                    lat = 23.565319444444;
                    lng = 119.563225;
                }
                else if (MonitorSite == "馬祖")
                {
                    lat = 26.169322222222;
                    lng = 119.92323333333;
                }
                else if (MonitorSite == "滿州")
                {
                    lat = 22.006041666667;
                    lng = 120.817175;
                }
                else if (MonitorSite == "板橋")
                {
                    lat = 24.997872222222;
                    lng = 121.44254722222;
                }
                else if (MonitorSite == "屏東市")
                {
                    lat = 22.692866666667;
                    lng = 120.48907222222;
                }
                else if (MonitorSite == "基隆")
                {
                    lat = 25.13955;
                    lng = 121.71506111111;
                }
                else if (MonitorSite == "頭城")
                {
                    lat = 24.944558333333;
                    lng = 121.90321388889;
                }
                else if (MonitorSite == "竹北")
                {
                    lat = 24.827833333333;
                    lng = 121.01428888889;
                }
                else if (MonitorSite == "苗栗")
                {
                    lat = 24.581844444444;
                    lng = 120.84264722222;
                }
                else if (MonitorSite == "南投")
                {
                    lat = 23.881361111111;
                    lng = 120.90805833333;
                }
                else if (MonitorSite == "彰化")
                {
                    lat = 24.063530555556;
                    lng = 120.536;
                }
                else if (MonitorSite == "雲林")
                {
                    lat = 23.6989;
                    lng = 120.53083611111;
                }
                else if (MonitorSite == "嘉義")
                {
                    lat = 23.495969444444;
                    lng = 120.43309444444;
                }

                SiteInformation = fields[0] + "輻射監測站 <br>時間:" + fields[3] + "<br>輻射數值:" + fields[2];


                dt.Rows.Add(fields[0], lat, lng, SiteInformation, "");

                MarkerLocation MarkerLocationItem = new MarkerLocation();
                MarkerLocationItem.title = fields[0];
                MarkerLocationItem.lat = lat;
                MarkerLocationItem.lng = lng;
                MarkerLocationItem.description = SiteInformation;
                MarkerLocationItem.icon = "";
                MarkerLocationItemList.Add(MarkerLocationItem);

            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            return json;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
            //rptMarkers.DataSource = dt;
            //rptMarkers.DataBind();
        }

        public string UVInformation()
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            List<UVData> UVDataList = new List<UVData>();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("title"), new DataColumn("lat"), new DataColumn("lng"), new DataColumn("description"), new DataColumn("icon") });
            string webAddr = "http://opendata.epa.gov.tw/ws/Data/UVIF/?format=xml";
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader stmReader = new StreamReader(response.GetResponseStream());

            string stringResult = stmReader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringResult);

            //XmlNamespaceManager xnm = new XmlNamespaceManager(xml.NameTable);
            //xnm.AddNamespace("x", "urn:cwb:gov:tw:cwbcommon:0.1"); 

            //xmlDoc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
            string xpath = "/UVIF/Data";

            foreach (XmlNode uvn in xml.SelectNodes(xpath))
            {
                UVData UVDataItem = new UVData();
                foreach (XmlNode item in uvn.ChildNodes)
                {
                    if (item.Name == "Name")
                    {
                        UVDataItem.LocationName = item.InnerText;
                    }
                    else if (item.Name == "UVI")
                    {
                        UVDataItem.UVI = item.InnerText;
                    }
                    else if (item.Name == "UVIStatus")
                    {
                        UVDataItem.UVIStatus = item.InnerText;
                    }
                    else if (item.Name == "PublishTime")
                    {
                        UVDataItem.PublishTime = item.InnerText;
                    }
                }
                UVDataList.Add(UVDataItem);
            }

            foreach (var item in UVDataList)
            {
                double lat = 0, lng = 0;
                if (item.LocationName == "宜蘭縣")
                {
                    lat = 24.7639610000;
                    lng = 121.7565420000;
                }
                else if (item.LocationName == "花蓮縣")
                {
                    lat = 23.9750860000;
                    lng = 121.6131500000;
                }
                else if (item.LocationName == "台東縣")
                {
                    lat = 22.7523030000;
                    lng = 121.1546940000;
                }
                else if (item.LocationName == "屏東縣")
                {
                    lat = 22.6730810000;
                    lng = 120.4880330000;
                }
                else if (item.LocationName == "高雄市")
                {
                    lat = 22.7575060000;
                    lng = 120.3056890000;
                }
                else if (item.LocationName == "台南市")
                {
                    lat = 22.9936420000;
                    lng = 120.2050670000;
                }
                else if (item.LocationName == "嘉義縣")
                {
                    lat = 23.4653080000;
                    lng = 120.2473500000;
                }
                else if (item.LocationName == "嘉義市")
                {
                    lat = 23.4959110000;
                    lng = 120.4329060000;
                }
                else if (item.LocationName == "南投縣")
                {
                    lat = 23.9130000000;
                    lng = 120.6853060000;
                }
                else if (item.LocationName == "雲林縣")
                {
                    lat = 23.7118530000;
                    lng = 120.5449940000;
                }
                else if (item.LocationName == "彰化縣")
                {
                    lat = 24.0660000000;
                    lng = 120.5415190000;
                }
                else if (item.LocationName == "台中市")
                {
                    lat = 24.1457080000;
                    lng = 120.6840830000;
                }
                else if (item.LocationName == "苗栗縣")
                {
                    lat = 22.6730810000;
                    lng = 120.4880330000;
                }
                else if (item.LocationName == "新竹縣")
                {
                    lat = 24.8278190000;
                    lng = 121.0142280000;
                }
                else if (item.LocationName == "新竹市")
                {
                    lat = 24.806976;
                    lng = 120.968781;
                }
                else if (item.LocationName == "桃園市")
                {
                    lat = 25.0135750000;
                    lng = 121.0354170000;
                }
                else if (item.LocationName == "新北市")
                {
                    lat = 25.1645000000;
                    lng = 121.4492390000;
                }
                else if (item.LocationName == "台北市")
                {
                    lat = 25.1826280000;
                    lng = 121.5296690000;
                }
                else if (item.LocationName == "基隆市")
                {
                    lat = 25.1329560000;
                    lng = 121.7403280000;
                }
                else if (item.LocationName == "三仙台")
                {
                    lat = 23.123889;
                    lng = 121.412956;
                }
                else if (item.LocationName == "太平山")
                {
                    lat = 24.495931;
                    lng = 121.533868;
                }
                else if (item.LocationName == "墾丁")
                {
                    lat = 21.948644;
                    lng = 120.779827;
                }
                else if (item.LocationName == "日月潭")
                {
                    lat = 23.8755920000;
                    lng = 120.9139390000;
                }
                else if (item.LocationName == "阿里山")
                {
                    lat = 23.4706080000;
                    lng = 120.8805720000;
                }
                else if (item.LocationName == "太魯閣")
                {
                    lat = 24.135060;
                    lng = 121.382489;
                }
                else if (item.LocationName == "梨山")
                {
                    lat = 24.255922;
                    lng = 121.248657;
                }
                else if (item.LocationName == "合歡山")
                {
                    lat = 24.181688;
                    lng = 121.281346;
                }
                else if (item.LocationName == "玉山")
                {
                    lat = 23.4869890000;
                    lng = 120.9591580000;
                }
                else if (item.LocationName == "溪頭")
                {
                    lat = 23.673270;
                    lng = 120.798030;
                }
                else if (item.LocationName == "龍洞")
                {
                    lat = 25.111013;
                    lng = 121.917305;
                }
                else if (item.LocationName == "陽明山")
                {
                    lat = 25.1826280000;
                    lng = 121.5296690000;
                }
                else if (item.LocationName == "小琉球")
                {
                    lat = 22.338907;
                    lng = 120.369806;
                }
                else if (item.LocationName == "蘭嶼")
                {
                    lat = 22.0369330000;
                    lng = 121.5583420000;
                }
                else if (item.LocationName == "綠島")
                {
                    lat = 22.6604816;
                    lng = 121.48541;
                }
                else if (item.LocationName == "澎湖")
                {
                    lat = 23.5650110000;
                    lng = 119.5636780000;
                }
                else if (item.LocationName == "馬祖")
                {
                    lat = 26.1691920000;
                    lng = 119.9231440000;
                }
                else if (item.LocationName == "金門")
                {
                    lat = 24.4120220000;
                    lng = 118.2905190000;
                }
                else if (item.LocationName == "花東空品區")
                {
                    lat = 22.756135;
                    lng = 121.151034;
                }
                else if (item.LocationName == "宜蘭空品區")
                {
                    lat = 24.632152;
                    lng = 121.791266;
                }
                else if (item.LocationName == "高屏空品區")
                {
                    lat = 22.563708;
                    lng = 120.425967;
                }
                else if (item.LocationName == "雲嘉南空品區")
                {
                    lat = 23.758253;
                    lng = 120.349140;
                }
                else if (item.LocationName == "中部空品區")
                {
                    lat = 24.160939;
                    lng = 120.617255;
                }
                else if (item.LocationName == "竹苗空品區")
                {
                    lat = 24.565153;
                    lng = 120.820728;
                }
                else if (item.LocationName == "北部空品區")
                {
                    lat = 25.047099;
                    lng = 121.508905;
                }
                string SiteInformation = "";

                SiteInformation = item.LocationName + "<br>紫外線指數:" + item.UVI + "<br>紫外線狀態:" + item.UVIStatus + "<br>監測時間:" + item.PublishTime;
                dt.Rows.Add(item.LocationName, lat, lng, SiteInformation, "");

                MarkerLocation MarkerLocationItem = new MarkerLocation();
                MarkerLocationItem.title = item.LocationName;
                MarkerLocationItem.lat = lat;
                MarkerLocationItem.lng = lng;
                MarkerLocationItem.description = SiteInformation;
                MarkerLocationItem.icon = "";
                MarkerLocationItemList.Add(MarkerLocationItem);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            return json;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
            //rptMarkers.DataSource = dt;
            //rptMarkers.DataBind();
        }

        public string OceanDataParse()
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            List<WeatherInformation> WeatherInformationList = new List<WeatherInformation>();
            string webAddr = "http://opendata.cwb.gov.tw/opendata/MFC/F-A0012-001.xml";
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
                WeatherInformation WeatherInformationItem = new WeatherInformation();
                var wc = xn.ChildNodes;//(ypath, xnm);
                foreach (XmlNode ChileNodeInfo in wc)
                {
                    if (ChileNodeInfo.Name == "locationName")
                    {
                        WeatherInformationItem.LocationName = ChileNodeInfo.InnerText;
                    }
                    else if (ChileNodeInfo.Name == "weatherElement")
                    {
                        weatherElement weatherElementItem = new weatherElement();
                        foreach (XmlNode ChileNodeInfoElemat in ChileNodeInfo)
                        {
                            if (ChileNodeInfoElemat.Name == "elementName")
                            {
                                weatherElementItem.weatherType = ChileNodeInfoElemat.InnerText;
                            }
                            else if (ChileNodeInfoElemat.Name == "time")
                            {
                                weatherElementTimeData weatherElementTimeDataItem = new weatherElementTimeData();
                                foreach (XmlNode ChileNodeInfoTimeData in ChileNodeInfoElemat)
                                {
                                    if (ChileNodeInfoTimeData.Name == "startTime")
                                    {
                                        weatherElementTimeDataItem.startTime = ChileNodeInfoTimeData.InnerText;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "endTime")
                                    {
                                        weatherElementTimeDataItem.endTime = ChileNodeInfoTimeData.InnerText;
                                    }
                                    else if (ChileNodeInfoTimeData.Name == "parameter")
                                    {
                                        foreach (XmlNode ChileNodeparameterData in ChileNodeInfoTimeData)
                                        {
                                            if (ChileNodeparameterData.Name == "parameterName")
                                            {
                                                weatherElementTimeDataItem.parameterName = ChileNodeparameterData.InnerText;
                                            }
                                            else if (ChileNodeparameterData.Name == "parameterValue")
                                            {
                                                weatherElementTimeDataItem.parameterUnit = ChileNodeparameterData.InnerText;
                                            }
                                            else if (ChileNodeparameterData.Name == "parameterUnit")
                                            {
                                                weatherElementTimeDataItem.parameterUnitType1 = ChileNodeparameterData.InnerText;
                                            }
                                        }
                                    }
                                }
                                weatherElementItem.TimeData.Add(weatherElementTimeDataItem);
                            }
                        }
                        WeatherInformationItem.Elementweather.Add(weatherElementItem);
                    }
                    Console.WriteLine(ChileNodeInfo.InnerText);
                }
                WeatherInformationList.Add(WeatherInformationItem);
            }
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("title"), new DataColumn("lat"), new DataColumn("lng"), new DataColumn("description"), new DataColumn("icon") });
            double lat = 0, lng = 0;
            foreach (var item in WeatherInformationList)
            {

                if (item.LocationName == "釣魚台海面")
                {
                    lat = 25.734150;
                    lng = 123.476997;
                }
                else if (item.LocationName == "彭佳嶼基隆海面")
                {
                    lat = 25.259638;
                    lng = 122.015989;
                }
                else if (item.LocationName == "宜蘭蘇澳沿海")
                {
                    lat = 24.601633;
                    lng = 121.903727;
                }
                else if (item.LocationName == "新竹鹿港沿海")
                {
                    lat = 24.613311;
                    lng = 120.466898;
                }
                else if (item.LocationName == "澎湖海面")
                {
                    lat = 23.604437;
                    lng = 119.551220;
                }
                else if (item.LocationName == "鹿港東石沿海")
                {
                    lat = 23.815155;
                    lng = 119.980658;
                }
                else if (item.LocationName == "東石安平沿海")
                {
                    lat = 23.789083;
                    lng = 120.034047;
                }
                else if (item.LocationName == "安平高雄沿海")
                {
                    lat = 23.289681;
                    lng = 119.947359;
                }
                else if (item.LocationName == "高雄枋寮沿海")
                {
                    lat = 22.509147;
                    lng = 120.136873;
                }
                else if (item.LocationName == "枋寮恆春沿海")
                {
                    lat = 22.032581;
                    lng = 120.533755;
                }
                else if (item.LocationName == "鵝鑾鼻沿海")
                {
                    lat = 21.853000;
                    lng = 120.898431;
                }
                else if (item.LocationName == "成功臺東沿海")
                {
                    lat = 22.013192;
                    lng = 120.940316;
                }
                else if (item.LocationName == "臺東大武沿海")
                {
                    lat = 22.362151;
                    lng = 120.981225;
                }
                else if (item.LocationName == "綠島蘭嶼海面")
                {
                    lat = 22.367133;
                    lng = 121.524768;
                }
                else if (item.LocationName == "花蓮沿海")
                {
                    lat = 23.624762;
                    lng = 121.812995;
                }
                else if (item.LocationName == "金門海面")
                {
                    lat = 24.338395;
                    lng = 118.451763;
                }
                else if (item.LocationName == "馬祖海面")
                {
                    lat = 26.093170;
                    lng = 119.972005;
                }
                else if (item.LocationName == "黃海南部海面")
                {
                    lat = 33.367842;
                    lng = 123.958837;
                }
                else if (item.LocationName == "花鳥山海面")
                {
                    lat = 31.262604;
                    lng = 122.762223;
                }
                else if (item.LocationName == "浙江海面")
                {
                    lat = 30.522537;
                    lng = 121.718522;
                }
                else if (item.LocationName == "東海北部海面")
                {
                    lat = 32.387305;
                    lng = 126.058646;
                }
                else if (item.LocationName == "東海南部海面")
                {
                    lat = 27.367166;
                    lng = 125.102874;
                }
                else if (item.LocationName == "臺灣北部海面")
                {
                    lat = 26.170509;
                    lng = 121.894866;
                }
                else if (item.LocationName == "臺灣東北部海面")
                {
                    lat = 25.080907;
                    lng = 123.454925;
                }
                else if (item.LocationName == "臺灣東南部海面")
                {
                    lat = 23.155800;
                    lng = 121.850921;
                }
                else if (item.LocationName == "臺灣海峽北部")
                {
                    lat = 25.279753;
                    lng = 120.224944;
                }
                else if (item.LocationName == "臺灣海峽南部")
                {
                    lat = 22.872667;
                    lng = 119.400970;
                }
                else if (item.LocationName == "巴士海峽")
                {
                    lat = 21.567214;
                    lng = 121.480774;
                }
                else if (item.LocationName == "廣東海面")
                {
                    lat = 21.326770;
                    lng = 114.680189;
                }
                else if (item.LocationName == "東沙島海面")
                {
                    lat = 21.220246;
                    lng = 117.249248;
                }
                else if (item.LocationName == "中西沙島海面")
                {
                    lat = 19.040988;
                    lng = 114.475290;
                }
                else if (item.LocationName == "南沙島海面")
                {
                    lat = 12.043912;
                    lng = 115.733726;
                }


                string WearherString = item.LocationName + "<br>";
                foreach (var items in item.Elementweather)
                {
                    if (items.weatherType == "Wx")
                    {
                        WearherString += "天氣狀況:" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "WindDir")
                    {
                        WearherString += "風向:" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "WindSpeed")
                    {
                        WearherString += items.TimeData.First().parameterUnitType1 + ":" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "WaveHeight")
                    {
                        WearherString += "海浪高度:" + items.TimeData.First().parameterName + "<br>";
                    }
                    else if (items.weatherType == "WaveType")
                    {
                        WearherString += "海浪程度:" + items.TimeData.First().parameterName + "<br>";
                    }
                }
                dt.Rows.Add(item.LocationName, lat, lng, WearherString, "");

                MarkerLocation MarkerLocationItem = new MarkerLocation();
                MarkerLocationItem.title = item.LocationName;
                MarkerLocationItem.lat = lat;
                MarkerLocationItem.lng = lng;
                MarkerLocationItem.description = WearherString;
                MarkerLocationItem.icon = "";
                MarkerLocationItemList.Add(MarkerLocationItem);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            return json;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
            //rptMarkers.DataSource = dt;
            //rptMarkers.DataBind();
        }

        public string AQXInformation()
        {
            List<MarkerLocation> MarkerLocationItemList = new List<MarkerLocation>();
            List<AQXData> AQXDataList = new List<AQXData>();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("title"), new DataColumn("lat"), new DataColumn("lng"), new DataColumn("description"), new DataColumn("icon") });
            string webAddr = "http://opendata.epa.gov.tw/ws/Data/AQX/?format=xml";
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/xml";
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader stmReader = new StreamReader(response.GetResponseStream());

            string stringResult = stmReader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringResult);

            //XmlNamespaceManager xnm = new XmlNamespaceManager(xml.NameTable);
            //xnm.AddNamespace("x", "urn:cwb:gov:tw:cwbcommon:0.1"); 

            //xmlDoc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
            string xpath = "/AQX/Data";

            foreach (XmlNode uvn in xml.SelectNodes(xpath))
            {
                AQXData AQXDataItem = new AQXData();
                foreach (XmlNode item in uvn.ChildNodes)
                {
                    if (item.Name == "SiteName")
                    {
                        AQXDataItem.SiteName = item.InnerText;
                    }
                    else if (item.Name == "County")
                    {
                        AQXDataItem.County = item.InnerText;
                    }
                    else if (item.Name == "PSI")
                    {
                        AQXDataItem.PSI = item.InnerText;
                    }
                    else if (item.Name == "MajorPollutant")
                    {
                        AQXDataItem.MajorPollutant = item.InnerText;
                    }
                    else if (item.Name == "Status")
                    {
                        AQXDataItem.Status = item.InnerText;
                    }
                    else if (item.Name == "SO2")
                    {
                        AQXDataItem.SO2 = item.InnerText;
                    }
                    else if (item.Name == "CO")
                    {
                        AQXDataItem.CO = item.InnerText;
                    }
                    else if (item.Name == "O3")
                    {
                        AQXDataItem.O3 = item.InnerText;
                    }
                    else if (item.Name == "PM10")
                    {
                        AQXDataItem.PM10 = item.InnerText;
                    }
                    else if (item.Name == "PM2.5")
                    {
                        AQXDataItem.PM25 = item.InnerText;
                    }
                    else if (item.Name == "NO2")
                    {
                        AQXDataItem.NO2 = item.InnerText;
                    }
                    else if (item.Name == "WindSpeed")
                    {
                        AQXDataItem.WindSpeed = item.InnerText;
                    }
                    else if (item.Name == "WindDirec")
                    {
                        AQXDataItem.WindDirec = item.InnerText;
                    }
                    else if (item.Name == "FPMI")
                    {
                        AQXDataItem.FPMI = item.InnerText;
                    }
                    else if (item.Name == "PublishTime")
                    {
                        AQXDataItem.PublishTime = item.InnerText;
                    }
                }
                AQXDataList.Add(AQXDataItem);
            }

            foreach (var item in AQXDataList)
            {
                double lat = 0, lng = 0;
                if (item.SiteName == "仁武")
                {
                    lat = 22.688738;
                    lng = 120.334100;
                }
                else if (item.SiteName == "麥寮")
                {
                    lat = 23.753788;
                    lng = 120.252013;
                }
                else if (item.SiteName == "關山")
                {
                    lat = 23.045381;
                    lng = 121.161349;
                }
                else if (item.SiteName == "馬公")
                {
                    lat = 23.569903;
                    lng = 119.566295;
                }
                else if (item.SiteName == "金門")
                {
                    lat = 24.432303;
                    lng = 118.313775;
                }
                else if (item.SiteName == "馬祖")
                {
                    lat = 26.160759;
                    lng = 119.949763;
                }
                else if (item.SiteName == "埔里")
                {
                    lat = 23.967720;
                    lng = 120.968026;
                }
                else if (item.SiteName == "復興")
                {
                    lat = 22.608544;
                    lng = 120.311677;
                }
                else if (item.SiteName == "永和")
                {
                    lat = 25.016787;
                    lng = 121.516299;
                }
                else if (item.SiteName == "竹山")
                {
                    lat = 23.756568;
                    lng = 120.678263;
                }
                else if (item.SiteName == "中壢")
                {
                    lat = 24.954303;
                    lng = 121.222006;
                }
                else if (item.SiteName == "三重")
                {
                    lat = 25.072858;
                    lng = 121.493568;
                }
                else if (item.SiteName == "冬山")
                {
                    lat = 24.632132;
                    lng = 121.791244;
                }
                else if (item.SiteName == "宜蘭")
                {
                    lat = 24.749406;
                    lng = 121.746817;
                }
                else if (item.SiteName == "陽明")
                {
                    lat = 25.199261;
                    lng = 121.526819;
                }
                else if (item.SiteName == "花蓮")
                {
                    lat = 23.972175;
                    lng = 121.599553;
                }
                else if (item.SiteName == "臺東")
                {
                    lat = 22.756065;
                    lng = 121.151002;
                }
                else if (item.SiteName == "恆春")
                {
                    lat = 21.958559;
                    lng = 120.809542;
                }
                else if (item.SiteName == "潮州")
                {
                    lat = 22.523177;
                    lng = 120.560215;
                }
                else if (item.SiteName == "屏東")
                {
                    lat = 22.673314;
                    lng = 120.488330;
                }
                else if (item.SiteName == "小港")
                {
                    lat = 22.566587;
                    lng = 120.338365;
                }
                else if (item.SiteName == "前鎮")
                {
                    lat = 22.605495;
                    lng = 120.308302;
                }
                else if (item.SiteName == "前金")
                {
                    lat = 22.632380;
                    lng = 120.287066;
                }
                else if (item.SiteName == "左營")
                {
                    lat = 22.674499;
                    lng = 120.292841;
                }
                else if (item.SiteName == "楠梓")
                {
                    lat = 22.733792;
                    lng = 120.327958;
                }
                else if (item.SiteName == "林園")
                {
                    lat = 22.479075;
                    lng = 120.411514;
                }
                else if (item.SiteName == "大寮")
                {
                    lat = 22.563718;
                    lng = 120.425988;
                }
                else if (item.SiteName == "鳳山")
                {
                    lat = 22.627911;
                    lng = 120.357432;
                }
                else if (item.SiteName == "橋頭")
                {
                    lat = 22.753256;
                    lng = 120.307137;
                }
                else if (item.SiteName == "美濃")
                {
                    lat = 22.884359;
                    lng = 120.530283;
                }
                else if (item.SiteName == "臺南")
                {
                    lat = 22.985395;
                    lng = 120.203223;
                }
                else if (item.SiteName == "安南")
                {
                    lat = 23.048596;
                    lng = 120.218256;
                }
                else if (item.SiteName == "善化")
                {
                    lat = 23.114653;
                    lng = 120.298445;
                }
                else if (item.SiteName == "新營")
                {
                    lat = 23.306791;
                    lng = 120.316272;
                }
                else if (item.SiteName == "嘉義")
                {
                    lat = 23.463616;
                    lng = 120.440846;
                }
                else if (item.SiteName == "臺西")
                {
                    lat = 23.717839;
                    lng = 120.202769;
                }
                else if (item.SiteName == "朴子")
                {
                    lat = 23.465406;
                    lng = 120.246911;
                }
                else if (item.SiteName == "新港")
                {
                    lat = 23.555995;
                    lng = 120.344906;
                }
                else if (item.SiteName == "崙背")
                {
                    lat = 23.757939;
                    lng = 120.349118;
                }
                else if (item.SiteName == "斗六")
                {
                    lat = 23.713117;
                    lng = 120.544411;
                }
                else if (item.SiteName == "南投")
                {
                    lat = 23.913455;
                    lng = 120.686743;
                }
                else if (item.SiteName == "二林")
                {
                    lat = 23.925309;
                    lng = 120.408965;
                }
                else if (item.SiteName == "線西")
                {
                    lat = 24.132090;
                    lng = 120.469221;
                }
                else if (item.SiteName == "彰化")
                {
                    lat = 24.066489;
                    lng = 120.541807;
                }
                else if (item.SiteName == "西屯")
                {
                    lat = 24.160704;
                    lng = 120.617169;
                }
                else if (item.SiteName == "忠明")
                {
                    lat = 24.151428;
                    lng = 120.641065;
                }
                else if (item.SiteName == "大里")
                {
                    lat = 24.099455;
                    lng = 120.677809;
                }
                else if (item.SiteName == "沙鹿")
                {
                    lat = 24.225200;
                    lng = 120.569209;
                }
                else if (item.SiteName == "豐原")
                {
                    lat = 24.255853;
                    lng = 120.741428;
                }
                else if (item.SiteName == "三義")
                {
                    lat = 24.383651;
                    lng = 120.756915;
                }
                else if (item.SiteName == "苗栗")
                {
                    lat = 24.564860;
                    lng = 120.820685;
                }
                else if (item.SiteName == "頭份")
                {
                    lat = 24.696371;
                    lng = 120.898363;
                }
                else if (item.SiteName == "新竹")
                {
                    lat = 24.804954;
                    lng = 120.972333;
                }
                else if (item.SiteName == "竹東")
                {
                    lat = 24.740002;
                    lng = 121.088121;
                }
                else if (item.SiteName == "湖口")
                {
                    lat = 24.899170;
                    lng = 121.039017;
                }
                else if (item.SiteName == "龍潭")
                {
                    lat = 24.864309;
                    lng = 121.217091;
                }
                else if (item.SiteName == "平鎮")
                {
                    lat = 24.953165;
                    lng = 121.203346;
                }
                else if (item.SiteName == "觀音")
                {
                    lat = 25.035486;
                    lng = 121.083052;
                }
                else if (item.SiteName == "大園")
                {
                    lat = 25.059119;
                    lng = 121.202681;
                }
                else if (item.SiteName == "桃園")
                {
                    lat = 24.995136;
                    lng = 121.320733;
                }
                else if (item.SiteName == "大同")
                {
                    lat = 25.064286;
                    lng = 121.513313;
                }
                else if (item.SiteName == "松山")
                {
                    lat = 25.050433;
                    lng = 121.579822;
                }
                else if (item.SiteName == "古亭")
                {
                    lat = 25.021699;
                    lng = 121.527437;
                }
                else if (item.SiteName == "萬華")
                {
                    lat = 25.046788;
                    lng = 121.508894;
                }
                else if (item.SiteName == "中山")
                {
                    lat = 25.061971;
                    lng = 121.525745;
                }
                else if (item.SiteName == "士林")
                {
                    lat = 25.105939;
                    lng = 121.514755;
                }
                else if (item.SiteName == "淡水")
                {
                    lat = 25.163559;
                    lng = 121.451459;
                }
                else if (item.SiteName == "林口")
                {
                    lat = 25.075932;
                    lng = 121.377333;
                }
                else if (item.SiteName == "菜寮")
                {
                    lat = 25.069041;
                    lng = 121.480246;
                }
                else if (item.SiteName == "新莊")
                {
                    lat = 25.035220;
                    lng = 121.432019;
                }
                else if (item.SiteName == "板橋")
                {
                    lat = 25.011664;
                    lng = 121.459037;
                }
                else if (item.SiteName == "土城")
                {
                    lat = 24.983972;
                    lng = 121.450919;
                }
                else if (item.SiteName == "新店")
                {
                    lat = 24.977523;
                    lng = 121.538222;
                }
                else if (item.SiteName == "萬里")
                {
                    lat = 25.180242;
                    lng = 121.689377;
                }
                else if (item.SiteName == "汐止")
                {
                    lat = 25.066503;
                    lng = 121.641767;
                }
                else if (item.SiteName == "基隆")
                {
                    lat = 25.128644;
                    lng = 121.758566;
                }
                string SiteInformation = "";
                SiteInformation = "站台:" + item.SiteName + "<br>所在城市:" + item.County;
                SiteInformation += "<br>空氣污染指標:" + item.PSI + " " + item.Status + "<br>";
                SiteInformation += "二氧化硫:" + item.SO2 + "<br>";
                SiteInformation += "一氧化碳:" + item.CO + "<br>";
                SiteInformation += "臭氧:" + item.O3 + "<br>";
                SiteInformation += "懸浮微粒:" + item.PM10 + "<br>";
                SiteInformation += "細懸浮微粒:" + item.PM25 + "<br>";
                SiteInformation += "二氧化氮:" + item.NO2 + "<br>";
                SiteInformation += "風速:" + item.WindSpeed + "<br>";
                SiteInformation += "風向:" + item.WindDirec + "<br>";
                SiteInformation += "細懸浮微粒指標:" + item.FPMI + "<br>";
                SiteInformation += "發布時間:" + item.PublishTime + "<br>";
                dt.Rows.Add(item.SiteName, lat, lng, SiteInformation, "");

                MarkerLocation MarkerLocationItem = new MarkerLocation();
                MarkerLocationItem.title = item.SiteName;
                MarkerLocationItem.lat = lat;
                MarkerLocationItem.lng = lng;
                MarkerLocationItem.description = SiteInformation;
                MarkerLocationItem.icon = "";
                MarkerLocationItemList.Add(MarkerLocationItem);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(MarkerLocationItemList);
            return json;
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JustAlert", "GetGoogleMap('" + json + "');", true);
            //rptMarkers.DataSource = dt;
            //rptMarkers.DataBind();
        }
    }


}
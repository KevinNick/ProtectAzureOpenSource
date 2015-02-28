<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AzureProtect.aspx.cs" Inherits="AzureProtect.html.AzureProtect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>雲端資料</title>
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href='http://fonts.googleapis.com/css?family=Alegreya+Sans:100,300,400,500,700,800,900,100italic,300italic,400italic,500italic,700italic,800italic,900italic' rel='stylesheet' type='text/css'>
    <script src="//code.jquery.com/jquery-1.11.2.min.js"></script>
    <script src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <script type="text/javascript" <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&signed_in=true&libraries=places"></script>
</head>

    <script type="text/javascript">
        var UserID;
        var Message;
        window.onload = function () {
            //var mapOptions = {
            //    center: new google.maps.LatLng(25.040282, 121.511901),
            //    zoom: 8,
            //    mapTypeId: google.maps.MapTypeId.ROADMAP
            //    //  marker:true
            //};
            //var infoWindow = new google.maps.InfoWindow();
            //var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
            //var trafficLayer = new google.maps.TrafficLayer();
            //trafficLayer.setMap(map);
            if (document.getElementById("HiddenFieldUserID").value.length > 0) {
                document.getElementById("LogoutState").style.display = "none";
                document.getElementById("LoginState").style.display = "block";
            }
            else {
                document.getElementById("LogoutState").style.display = "block";
                document.getElementById("LoginState").style.display = "none";
            }
            if (document.getElementById("HiddenFieldUserName").value.length >0)
            {
                document.getElementById("NameBlock").innerText = document.getElementById("HiddenFieldUserName").value;
            }
        }

        function ShowMapInit() {
            var mapOptions = {
                center: new google.maps.LatLng(25.040282, 121.511901),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
                //  marker:true
            };
            var infoWindow = new google.maps.InfoWindow();
            var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
            var trafficLayer = new google.maps.TrafficLayer();
            trafficLayer.setMap(map);
        }

        function showSteps(directionResult) {
            // For each step, place a marker, and add the text to the marker's
            // info window. Also attach the marker to an array so we
            // can keep track of it and remove it when calculating new
            // routes.
            var myRoute = directionResult.routes[0].legs[0];

            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
            for (var i = 0; i < myRoute.steps.length; i++) {
                var marker = new google.maps.Marker({
                    position: myRoute.steps[i].start_location,
                    map: map
                });
                attachInstructionText(marker, myRoute.steps[i].instructions);
            }
        }

        function attachInstructionText(marker, text) {
            google.maps.event.addListener(marker, 'click', function () {
                var mapOptions = {
                    center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                    zoom: 8,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
                var infoWindow = new google.maps.InfoWindow();
                infoWindow.setContent(text);
                infoWindow.open(map, marker);
            });
        }


    </script>

    <script type="text/javascript" language="javascript">

        function InformationWindows(index) {
            //var WindowsInfo = document.getElementById("MultiView1");
            //MultiView1.setActive = index;
            //WindowsInfo.setActive = index;
            var MView1 = document.getElementById("MultiView1");
            MView1.ActiveViewIndex = index;
        }

        function Getdirections() {
            //alert(document.getElementById("FromText").value);
            GetGoogledirections(document.getElementById("FromText").value, document.getElementById("ToText").value);
        }

        function GetGoogledirections(FromString, ToString) {
            var mapOptions = {
                center: new google.maps.LatLng(23.744329, 120.971664),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
                //  marker:true
            };
            var infoWindow = new google.maps.InfoWindow();
            var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
            var trafficLayer = new google.maps.TrafficLayer();
            trafficLayer.setMap(map);
            var oldDirections = [];
            var currentDirections = null;
            var directionsDisplay;
            var directionsService = new google.maps.DirectionsService();//路線資訊回傳
            directionsDisplay = new google.maps.DirectionsRenderer({
                polylineOptions: {
                    strokeColor: "black"
                }
            });
            directionsDisplay.setMap(map);

            var request = {
                origin: FromString,
                destination: ToString,
                travelMode: google.maps.DirectionsTravelMode.DRIVING
            };

            //directionsDisplay.setPanel(document.getElementById("Panel5"));

            //google.maps.event.addListener(directionsDisplay, 'directions_changed',
            //    function () {

            //    });

            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                    //showSteps(response);
                    var myRoute = response.routes[0].legs[0];

                    for (var i = 0; i < myRoute.steps.length; i++) {
                        var RouteData = myRoute.steps[i];
                        var marker = new google.maps.Marker({
                            position: RouteData.start_location,
                            map: map
                        });
                        (function (marker, RouteData) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                infoWindow.setContent(RouteData.instructions);
                                //SearchLocation(RouteData.start_location);
                                //map.center =  new google.maps.LatLng(markers[0].lat,markers[0].lng),
                                infoWindow.open(map, marker);
                            });
                        })(marker, RouteData);
                    }
                }
            })
        }

        function showMessage(inputString) {
            PageMethods.OpenDataGet(inputString, success, null);
            //alert(UserID);
        }

        function success(result, context, method) {
            GetGoogleMap(result);
        }

        function GetGoogleMap(jsonData) {
            var obj = JSON.parse(jsonData);
            if (obj.length != 0) {
                var mapOptions = {
                    center: new google.maps.LatLng(obj[0].lat, obj[0].lng),
                    zoom: 8,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                    //  marker:true
                };
                var infoWindow = new google.maps.InfoWindow();
                var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
                var trafficLayer = new google.maps.TrafficLayer();
                trafficLayer.setMap(map);
                for (i = 0; i < obj.length; i++) {
                    var DataItem = obj[i];
                    var myLatlng = new google.maps.LatLng(DataItem.lat, DataItem.lng);
                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: DataItem.title,
                        icon: DataItem.icon,
                        //animation: google.maps.Animation.BOUNCE
                    });
                    (function (marker, DataItem) {
                        google.maps.event.addListener(marker, "click", function (e) {
                            infoWindow.setContent(DataItem.description);

                            //map.center =  new google.maps.LatLng(markers[0].lat,markers[0].lng),
                            infoWindow.open(map, marker);
                        });
                    })(marker, DataItem);
                }
            }
        }


        function OnClientClick(ServerControID, IndexControlID, Index) {
            //if (Index == 2) {
            //    showAlarmMessage();
            //    document.getElementById("HiddenField1").value = Index;
            //}
            //else {
            //ShowMapInit();
                document.getElementById("HiddenField1").value = Index;
                var objDemo = document.getElementById(ServerControID);
                if (objDemo) {
                    document.getElementById(IndexControlID).value = Index;
                    objDemo.click();
                }
            //}


            //}
        }


        function Alarmsuccess(result, context, method) {

            var objDemo = document.getElementById("butSubmit");
            if (objDemo) {

                objDemo.click();
            }       
        }

        function LoginRequest() {
            var UserNameTest = document.getElementById("UserName").value;
            var UserPassword = document.getElementById("Password").value
            PageMethods.UserLogin(UserNameTest, UserPassword, Loginsuccess, null);
        }

        function LogOutRequest() {
            document.getElementById("LogoutState").style.display = "block";
            document.getElementById("LoginState").style.display = "none";
            document.getElementById("HiddenFieldUserID").value = "";
            document.getElementById("HiddenFieldUserName").value = "";
        }

        function RegisterRequest() {
            var UserNameTest = document.getElementById("UserName").value;
            var UserPassword = document.getElementById("Password").value
            PageMethods.UserRegister(UserNameTest, UserPassword, Registersuccess, null);
        }

        function Registersuccess(result, context, method) {
            alert(result);
        }

        function Loginsuccess(result, context, method) {
            //var LoginState1 = document.getElementById("LoginState");
            //LogoutState1.style = "display:run-in;"
            //var LogoutState1 = document.getElementById("LogoutState");
            //LogoutState1.style = "display:none;"
            //if (isInteger(result) == true) {
            if (result.length < 3){
                document.getElementById("LogoutState").style.display = "none";
                document.getElementById("LoginState").style.display = "block";
                UserID = result;
                document.getElementById("HiddenFieldUserID").value = result;

                var UserNameTest = document.getElementById("Password").value;
                document.getElementById("HiddenFieldUserName").value = "Hello  " + UserNameTest;
                document.getElementById("NameBlock").innerText = document.getElementById("HiddenFieldUserName").value;
            }
            else {
                alert(result);
            }
            //alert(result);
        }

        //function OnWeatherClientClick(ServerControID, IndexControlID, Index) {
        //    var objDemo = document.getElementById(ServerControID);
        //    if (objDemo) {
        //        document.getElementById(IndexControlID).value = Index;
        //        objDemo.click();
        //    }
        //}

        var myVar = setInterval(function () { myTimer() }, 1000);

        function myTimer() {
            if (document.getElementById("HiddenFieldUserID").value.length > 0) {
                PageMethods.UserAlarmMessage(document.getElementById("HiddenFieldUserID").value, successAlarmMessage, null);
            }
            
        }

        var OldString = "";

        function successAlarmMessage(inputString) {
            var obj = JSON.parse(inputString);
            if (OldString != inputString)
            {
                OldString = inputString;
                if (obj.length != 0) {
                    var mapOptions = {
                        center: new google.maps.LatLng(obj[0].Latitude, obj[0].Longitude),
                        zoom: 8,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                        //  marker:true
                    };
                    var infoWindow = new google.maps.InfoWindow();
                    var map = new google.maps.Map(document.getElementById("map_canvas_custom_238233"), mapOptions);
                    var trafficLayer = new google.maps.TrafficLayer();
                    trafficLayer.setMap(map);
                    for (i = 0; i < obj.length; i++) {
                        var DataItem = obj[i];
                        var myLatlng = new google.maps.LatLng(DataItem.Latitude, DataItem.Longitude);
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            title: DataItem.UserName,
                            //icon: DataItem.icon,
                            //animation: google.maps.Animation.BOUNCE
                        });
                        (function (marker, DataItem) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                //string Alarmdescription; = "(" +DataItem.AlarmTime + ")" +DataItem.AlarmMessage;
                                infoWindow.setContent("(" + DataItem.AlarmTime + ")" + DataItem.AlarmMessage);

                                //map.center =  new google.maps.LatLng(markers[0].lat,markers[0].lng),
                                infoWindow.open(map, marker);
                            });
                        })(marker, DataItem);
                    }
                }
            }
        }


    </script>
    <%--<script runat="server">
    
</script>--%>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"/>
    
    <nav>
        <!-- 登入後 -->

        <div id="LoginState" class="login_area" style="display:none;">
            <div class="btn_log"><a href="javascript:void(0)" onclick="LogOutRequest();">LOGOUT</a>
            </div>
            <div id="NameBlock" class="name"></div>
            <div class="profile"><img src="images/green.png" width="40" height="40" />
            </div>
        </div>

        <!-- 登入前 -->

        <div id="LogoutState" class="login_area" style="display:none;">
            <div class="btn_log"><a href="javascript:void(0)" onclick="RegisterRequest();">註冊</a>
            <div class="btn_log"><a href="javascript:void(0)" onclick="LoginRequest();">LOGIN</a>
            
            </div>

            <input id="UserName" name="" type="text" style="margin-right:0px;" />
            <div class="name">PASSWORD</div>
            <input id="Password" name="" type="text" />
            <div class="name">ACCOUNT</div>


        </div>


    </nav>
    
    
    <div class="wrap">
        <div class="left">
            <div class="route">
                <h2 style="margin-left:25px;">FROM</h2>
                <input id="FromText" name="" type="text"/>
                <h2>TO</h2>
                <input id ="ToText" name="" type="text" style="margin-right:0px;" />
                <div class="btn_log btn_route"><a href="javascript:void(0)" onclick="Getdirections();">ROUTE</a>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
            <div class="map">
                <div id="map_canvas_custom_238233" style="width:650px; height:440px"></div>
                <%--<script type="text/javascript">
                    (function (d, t) {
                        var g = d.createElement(t),
                            s = d.getElementsByTagName(t)[0];
                        g.src = "http://map-generator.net/en/maps/238233.js?point=Taiwan";
                        s.parentNode.insertBefore(g, s);
                    }(document, "script"));
                </script>--%>

            </div>
                            </ContentTemplate>
                </asp:UpdatePanel>
        </div>
        <div class="right">
            <div class="submenu">
                <ul>
                    <li style="margin-left:18px;"><a href="javascript:void(0)" onclick="OnClientClick('butSubmit','HiddenField1','0')">雲端資訊</a><%--<a href="jindex.html">雲端資訊</a>--%>
                    </li>
                    <li><a href="javascript:void(0)" onclick="OnClientClick('butSubmit','HiddenField1','1')">我的群組</a><%--<a href="group.html">我的群組</a>--%>
                    </li>
                    <li><a href="javascript:;">即時資訊</a>
                        <ul>
                            <li><a href="javascript:void(0)" onclick="OnClientClick('butSubmit','HiddenField1','2')">天氣特報</a><%--<a href="weather.html">天氣特報</a></li>--%>
                            <li><a href="javascript:void(0)" onclick="OnClientClick('butSubmit','HiddenField1','3')">天災警告</a><%--<a href="alert.html">天災警告</a></li>--%>
                            <li><a href="javascript:void(0)" onclick="OnClientClick('butSubmit','HiddenField1','4')">聊天室</a><%--<a href="chat.html">聊天室</a></li>--%>
                        </ul>
                    </li>
                </ul>
            </div>
            <asp:MultiView
                id="MultiView1"
                ActiveViewIndex="0"
                Runat="server">
                <asp:View ID="View1" runat="server" >
                    <div class="cloud">
                        <ul>
                            <li><a href="javascript:;">環境監測數據</a>
                                <ul>
                                    <li><a href="javascript:void(0)" onclick="showMessage('weather');">天氣</a>
                                    </li>
                                    <li><a href="javascript:void(0)" onclick="showMessage('radiation');">輻射</a>
                                    </li>
                                    <li><a href="javascript:void(0)" onclick="showMessage('uv');">紫外線</a>
                                    </li>
                                    <li><a href="javascript:void(0)" onclick="showMessage('ocean');">海面氣象</a>
                                    </li>
                                    <li><a href="javascript:void(0)" onclick="showMessage('air');">空氣品質</a>
                                    </li>
                                </ul>

                            </li>
                            <li><a href="javascript:;">緊急防災處理中心</a>

                                <ul>
                                    <li><a href="javascript:void(0)" onclick="showMessage('1');">海巡</a>
                                    </li>
                                    <li><a href="javascript:void(0)" onclick="showMessage('2');">派出所</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('3');">消防隊</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('4');">警察總局</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('5');">醫院</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('6');">應變中心</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('7');">清潔隊</a>
                                    </li>
                                    <li><li><a href="javascript:void(0)" onclick="showMessage('8');">災民收容所</a>
                                    </li>
                                </ul>

                            </li>

                        </ul>
                    </div>
                </asp:View>
                <asp:View ID="View2" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                    <div id="Div1"  style="height: 350px; overflow: scroll;">
                    <asp:TreeView ID="TreeViewUser" runat="server"  Height="127px" Width="248px" OnSelectedNodeChanged="TreeViewUser_SelectedNodeChanged" >
                            <Nodes>
 
                            </Nodes>
                        </asp:TreeView>
                        </div>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="GroupCreate" runat="server"></asp:TextBox>
                                <asp:Button ID="CreateButton" runat="server" Text="建立群組" OnClick="CreateButton_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="GroupID" runat="server"></asp:TextBox>
                                <asp:Button ID="AddGroup" runat="server" Text="加入群組ID" OnClick="AddGroup_Click" />
                            </td>
                        </tr>
                    </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </asp:View>
                <asp:View ID="View3" runat="server" >
                    <div id="weatherTable" class="weather" style="height: 420px; overflow: scroll;">
<%--            <button onclick="myFunction()">Try it</button>--%>
                <%--<table id="myTable" cellspacing="10">
                    <tr class="title">
                        <td width="30%">地區</td>
                        <td>事件</td>
                    </tr>
                    
                </table>--%>
                <asp:GridView ID="ListBoxAlarm" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Location" HeaderText="地區"  />
                        <asp:BoundField DataField="Summary" HeaderText="事件"  />
                        </Columns>
                </asp:GridView>
            </div>

                </asp:View>
                <asp:View ID="View4" runat="server" >
                     <div id="Div2" class="weather" style="height: 420px; ">
                    <asp:DropDownList ID="DropDownFunction" runat="server" >
                        <asp:ListItem Text ="全部" Value ="0"/>
                        <asp:ListItem Text ="地震" Value ="1"/>
                        <asp:ListItem Text ="海嘯" Value ="2"/>
                        <asp:ListItem Text ="降雨" Value ="3"/>
                        <asp:ListItem Text ="颱風" Value ="4"/>
                        <asp:ListItem Text ="水庫洩洪" Value ="5"/>
                        <asp:ListItem Text ="淹水" Value ="6"/>
                        <asp:ListItem Text ="河川高水位" Value ="8"/>
                        <asp:ListItem Text ="道路封閉" Value ="9"/>
                    </asp:DropDownList>
                    <asp:Button ID="SeleteButton" runat="server" Text="搜尋" selectionmode="Multiple" OnClick="SeleteButton_Click"/>
                    <div style="width: 320px; height: 420px; overflow: scroll"> 
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="時間"  />
                            <asp:BoundField DataField="Type" HeaderText="類型"  />
                            <asp:BoundField DataField="Summary" HeaderText="事件"  />
                            </Columns>
                    </asp:GridView>
                        </div>
                    </asp:View>
                <asp:View ID="View5" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                    <asp:Timer runat ="server" ID ="TreeTimer" Interval="10000" OnTick="TreeTimer_Tick"> 
                        </asp:Timer>
                    <div class="chat">
                    <div class="chat_area" style="height: 350px; overflow: scroll;">     
                        <asp:ListBox ID="ListBoxTalk" runat="server" Rows="25"  ></asp:ListBox>            
                           <%--<textarea id="TextArea1" rows="20" cols="40"></textarea>--%>
                    </div>
                        
                    <div class="chat_form">
                    <asp:DropDownList ID="DropDownListGroup" runat="server" />
                        <asp:TextBox ID="MessageText" runat="server" Width="230px" class="form_style"></asp:TextBox>

                     <%--<textarea name="textarea" id="textarea"  class="form_style"></textarea>--%>
                    </div>
                <%--<div class="clear"></div>--%>
                        <asp:Button ID="SendButton" runat="server" Text="送出" class="search_btn" OnClick="SendButton_Click"></asp:Button>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                <%--<a href="#" class="search_btn">送出</a>          --%>      
            </div>
                            
                </asp:View>
            </asp:MultiView>
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenFieldUserID" runat="server" />
        <asp:HiddenField ID="HiddenFieldUserName" runat="server" />
        <div style="display: none">
            <asp:Button ID="butSubmit" runat="server" OnClick="butSubmit_Click" Text="Submit" /></div>

    </form>
</body>

</html>
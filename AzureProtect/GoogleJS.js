// Author: Etechpulse
// Summary: Show multiple markers on Google Map

var map;
var infoWindowOpenClose = { "infoWindowOpen": 1, "infoWindowClose": 2 }
var isDraggableMarker = { "Yes": 1, "No": 2 }
var isContentHeaderOrNot = { "Yes": 1, "No": 2 }
var MarkerArray = [];

// Load Google Map
function initialize() {
    var mapOptions = {
        zoom: 5,
        center: new google.maps.LatLng(22.754742017182636, 82.78772795999998),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
}

// Function to show data on map    
function ShowOnMap(Name, Address, Latitude, Longitude) {
    // RemoveSearchMarker();
    var imgName = 'mm_20_green';
    var toolTiptext = "Hey there!";

    var Content = '<b>Customer Details:</b><br><br>' +
        '<b>Name:</b>' + Name + '<br>' +
       '<b>Address:</b>' + Address + '<br>' +
       '<b>Latitude:</b>' + Latitude + '<br>' +
      '<b>Longitude:</b>' + Longitude + '<br>' + '<br>';

    var objImage = CreateImageObject(imgName, 24, 22, 10, 12);  //32,30,18,20 (For 32x32 Size)


    var objLatLng = CreateLatLngObject(Latitude, Longitude);
    var mapCustomer = AddMarkerOnMap(objImage, objLatLng, 24, 24, toolTiptext, Content, infoWindowOpenClose.infoWindowClose, isDraggableMarker.No, isContentHeaderOrNot.No);
}

// Create a image object       
function CreateImageObject(ImageName, sizeX, sizeY, anchorX, anchorY) {
    var image = new google.maps.MarkerImage("images/" + ImageName + ".png",
    new google.maps.Size(sizeX, sizeY),         // This marker is 20 pixels wide by 32 pixels tall.
    new google.maps.Point(0, 0),                // The origin for this image is 0,0.
    new google.maps.Point(anchorX, anchorY));   // The anchor for this image is the base of the flagpole at 0,32.
    return image;
}

// Create Lat/Lon object
function CreateLatLngObject(Latitude, Longitude) {
    var latlng = new google.maps.LatLng(parseFloat(Latitude), parseFloat(Longitude));
    return latlng;
}

// Draw markers on map
function AddMarkerOnMap(objImage, objLatLng, objsizeX, objsizeY, toolTiptext, infoWindowContent, infoWindowOpenClose, isDraggable, isContent) {
    var dragFlag = true;

    if (isDraggable == 2) dragFlag = false;
    var marker = new google.maps.Marker
        ({
            position: objLatLng,
            title: toolTiptext,
            icon: objImage,
            size: google.maps.Size(objsizeX, objsizeY),
            draggable: dragFlag,
            map: map
        });

    var ContentHeader = '';
    if (isContent == 1) {
    }
    else if (isContent == 2) {
        ContentHeader = infoWindowContent;
    }
    if (infoWindowOpenClose == 1) {
        if (infoWindow) infoWindow.close();
        infoWindow = new google.maps.InfoWindow({ content: ContentHeader, position: objLatLng, maxWidth: 200 });
        infoWindow.open(map, marker);
    }
    else if (infoWindowOpenClose == 2) {
        infoWindow = new google.maps.InfoWindow({ content: ContentHeader, position: objLatLng, maxWidth: 200 });
    }

    google.maps.event.addListener(marker, "click", function (event) {//            
        if (infoWindow) infoWindow.close();
        infoWindow = new google.maps.InfoWindow({ content: ContentHeader, position: marker.position, maxWidth: 200 });
        infoWindow.open(map, marker);
    });

    MarkerArray.push(marker);
    return marker;
}

// Remove all markers from the map
function RemoveMarkersFromArray() {
    if (MarkerArray) {
        for (var i = 0; i < MarkerArray.length; i++) {
            MarkerArray[i].setMap(null);
        }
    }
    MarkerArray = new Array();
}
mergeInto(LibraryManager.library, {

    loadDataJS: function(player) {
        var baseData = "";
        var getdata = ""
        var DBplayers = parseInt(getGamePlayers());
        var userCookie = getCookie("UserSettings");
        if ((userCookie != null && userCookie != "undefined" && userCookie != "") && DBplayers > 0) {
            baseData = loadGameBasaData(player);
            if (baseData != "" && baseData != null && baseData != "undefined") {
                getdata = JSON.stringify(baseData)
                console.log("getdata: " + getdata)
            }
            if (getdata == null || getdata == "undefined" || getdata == "") {
                getdata = localStorage.getItem("BigHead" + player)
            }
        } else {
            getdata = localStorage.getItem("BigHead" + player)
        }
        if (getdata != "" && getdata != null && getdata != "undefined") {
            var bufferSize = lengthBytesUTF8(getdata) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(getdata, buffer, bufferSize);
            return buffer;
        }
    },

    saveDataJS: function(data, player) {
        var jsonToSend = Pointer_stringify(data);
        console.log("sendData: " + jsonToSend)

        var userCookie = getCookie("UserSettings");
        if (userCookie != null && userCookie != "undefined" && userCookie != "") {
            saveGameBaseData(jsonToSend, player)
            localStorage.setItem("BigHead" + player, jsonToSend)
        } else {
            localStorage.setItem("BigHead" + player, jsonToSend)
        }
    },

    getPlayersCountJS: function() {
        var playersCount = parseInt(getGamePlayers());
        console.log("Players from DB: " + playersCount)
        //console.log("players * 2 :"+playersCount*2)
        if (playersCount == 0) {
            var BHlocalStorage = localStorage.getItem("BigHead0")
            if (BHlocalStorage != null && BHlocalStorage != undefined && BHlocalStorage != "") {
                playersCount = 4;
                console.log("players from LS: " + playersCount)
            }
        }
        return playersCount;
    },
    getLastNameJS: function() {
        var userName = getUserName();
        var bufferSize = lengthBytesUTF8(userName) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(userName, buffer, bufferSize);
        return buffer;
    },
})
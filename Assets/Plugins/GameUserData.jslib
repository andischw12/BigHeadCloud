mergeInto(LibraryManager.library, {

    loadDataJS: function(player) {
        var baseData = "";
        var getdata = ""
        

        var PersonID = 0;
        var LSPersonID = parseInt(localStorage.getItem("BigHeadPersonID"+getIdxNumber()));

        if( LSPersonID != undefined && LSPersonID >0 ){
            PersonID = LSPersonID;
        } else {
            PersonID = getPersonID();
        }

        if (PersonID == undefined || PersonID == null){
            PersonID = 0;
        }

        var DBplayers = 0;
            if(PersonID>0){
                DBplayers = parseInt(getGamePlayers(PersonID));
            }
        
        //var userCookie = getCookie("UserSettings");
        //if ((userCookie != null && userCookie != "undefined" && userCookie != "") && DBplayers > 0) {
            baseData = loadGameBasaData(player, PersonID);
            if (baseData != "" && baseData != null && baseData != "undefined") {
                getdata = JSON.stringify(baseData)
                console.log("getdata: " + getdata)
            }
            if (getdata == null || getdata == "undefined" || getdata == "") {
                getdata = localStorage.getItem("BigHead" + player)
            }
        //} else {
        //    getdata = localStorage.getItem("BigHead" + player)
        //}
        if (getdata != "" && getdata != null && getdata != "undefined") {
            var bufferSize = lengthBytesUTF8(getdata) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(getdata, buffer, bufferSize);
            return buffer;
        }
    },

    saveDataJS: function(data, player, points, shabbatPoints) {
        var jsonToSend = Pointer_stringify(data);
        console.log("sendData: " + jsonToSend)

        //var userCookie = getCookie("UserSettings");
        var PersonID = getPersonID();
        var LSPersonID = parseInt(localStorage.getItem("BigHeadPersonID"+getIdxNumber()));
        if (PersonID == undefined || PersonID == null || PersonID == 0 && LSPersonID != undefined){
            PersonID = LSPersonID;
        } else {
            localStorage.setItem("BigHeadPersonID"+getIdxNumber(),PersonID)
        }
            if(PersonID>0){
            saveGameBaseData(jsonToSend, player, PersonID, points, shabbatPoints)
            }
            localStorage.setItem("BigHead" + player, jsonToSend)
    },

    getPlayersCountJS: function() {

        //PersonID = getPersonID();
        var PersonID = 0;
        var LSPersonID = parseInt(localStorage.getItem("BigHeadPersonID"+getIdxNumber()));

        if(LSPersonID != undefined && LSPersonID >0 ){
            PersonID = LSPersonID;
        } else {
            PersonID = getPersonID();
        }

        if (PersonID == undefined || PersonID == null){
            PersonID = 0;
        }

        var playersCount = 0;
            if(PersonID>0){
                playersCount = parseInt(getGamePlayers(PersonID));
            }

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

        saveWinnersJS: function(winner,loser) {
            var winnerToSent = Pointer_stringify(winner);
            var loserToSent = Pointer_stringify(loser);
		$.ajax({
            type: "POST",
            cache: false,
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            url: "https://meirkids.co.il/asp/BigHeadWinGames.asp",
            data: "FirstName=BigHeadWinners&LastName=AAA&Phone=080808&Address=AAA&Email=AAA@AAA.AAA&Winner="+winnerToSent+"&Loser="+loserToSent,
            success: function (value) {
			
			}
        })

    },

    pushWinnersJS: function(){
        localStorage.setItem("pushWinners",1);
    },
})
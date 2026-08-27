<%@ LANGUAGE="VBScript" CODEPAGE="65001" %>
<%
Response.CodePage = 65001
Response.CharSet = "UTF-8"
Response.ContentType = "application/json"
Response.Buffer = True
Response.CacheControl = "no-cache"
Session.CodePage = 65001
%>
<!-- #include file="common.asp" -->
<%
Sub SendJson(statusText, jsonText)
    Response.Status = statusText
    Response.Write jsonText
    Response.End
End Sub

Function MatchesPattern(value, pattern)
    Dim re
    Set re = New RegExp
    re.Pattern = pattern
    re.Global = False
    MatchesPattern = re.Test(CStr(value))
    Set re = Nothing
End Function

Function CurrentOid()
    Dim rawCookie, re, matches
    CurrentOid = ""
    rawCookie = Request.Cookies("UserSettings")
    If Len(rawCookie) = 0 Then Exit Function
    On Error Resume Next
    rawCookie = Server.URLDecode(rawCookie)
    On Error GoTo 0
    Set re = New RegExp
    re.Pattern = """OID""\s*:\s*""([A-Za-z0-9_-]{1,200})"""
    re.IgnoreCase = True
    re.Global = False
    Set matches = re.Execute(rawCookie)
    If matches.Count = 1 Then CurrentOid = matches(0).SubMatches(0)
    Set matches = Nothing
    Set re = Nothing
End Function

Function JsonString(value)
    Dim safeValue
    If IsNull(value) Then
        JsonString = "null"
        Exit Function
    End If
    safeValue = CStr(value)
    safeValue = Replace(safeValue, "\", "\\")
    safeValue = Replace(safeValue, """", "\""")
    safeValue = Replace(safeValue, vbCrLf, "\n")
    safeValue = Replace(safeValue, vbCr, "\n")
    safeValue = Replace(safeValue, vbLf, "\n")
    JsonString = """" & safeValue & """"
End Function

Function SafeInt(value, fallbackValue)
    Dim textValue
    textValue = Trim(CStr(value))
    If MatchesPattern(textValue, "^-?[0-9]{1,10}$") Then
        On Error Resume Next
        SafeInt = CLng(textValue)
        If Err.Number <> 0 Then SafeInt = fallbackValue
        Err.Clear
        On Error GoTo 0
    Else
        SafeInt = fallbackValue
    End If
End Function

Dim actionName, oid, gameKey, player
actionName = LCase(Trim(Request("action")))
oid = CurrentOid()
gameKey = Trim(Request("gameKey"))
player = Trim(Request("player"))

If gameKey <> "2" Then SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_game""}"
If Len(oid) = 0 Then SendJson "401 Unauthorized", "{""success"":false,""message"":""login_required""}"

If actionName = "count" Then
    Dim countRs, playerCount
    On Error Resume Next
    Set countRs = Conn.Execute("SELECT COUNT_BIG(1) AS PlayerCount FROM dbo.BigHeadGameData WHERE OID = '" & oid & "' AND GameKey = 2")
    If Err.Number <> 0 Then
        Err.Clear
        On Error GoTo 0
        SendJson "500 Internal Server Error", "{""success"":false,""message"":""database_error""}"
    End If
    On Error GoTo 0
    playerCount = CLng(countRs("PlayerCount"))
    countRs.Close
    Set countRs = Nothing
    SendJson "200 OK", "{""success"":true,""count"":" & CStr(playerCount) & "}"
End If

If actionName = "load" Then
    Dim loadRs, loadSql, loadJson
    If Not MatchesPattern(player, "^[0-3]$") Then SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_player""}"
    loadSql = _
        "SELECT TOP (1) DataJson, Points, ShabbatPoints, HanukkaPoints, PurimPoints, " & _
        "CONVERT(VARCHAR(23), UpdatedAt, 126) AS UpdatedAtIso FROM dbo.BigHeadGameData " & _
        "WHERE OID = '" & oid & "' AND GameKey = 2 AND PlayerSlot = " & player
    On Error Resume Next
    Set loadRs = Conn.Execute(loadSql)
    If Err.Number <> 0 Then
        Err.Clear
        On Error GoTo 0
        SendJson "500 Internal Server Error", "{""success"":false,""message"":""database_error""}"
    End If
    On Error GoTo 0
    If loadRs.EOF Then
        loadRs.Close
        Set loadRs = Nothing
        SendJson "200 OK", "{""success"":true,""found"":false}"
    End If
    loadJson = "{""success"":true,""found"":true,""data"":" & CStr(loadRs("DataJson")) & _
        ",""points"":" & CStr(loadRs("Points")) & _
        ",""shabbatPoints"":" & CStr(loadRs("ShabbatPoints")) & _
        ",""hanukkaPoints"":" & CStr(loadRs("HanukkaPoints")) & _
        ",""purimPoints"":" & CStr(loadRs("PurimPoints")) & _
        ",""updatedAt"":" & JsonString(loadRs("UpdatedAtIso")) & "}"
    loadRs.Close
    Set loadRs = Nothing
    SendJson "200 OK", loadJson
End If

If actionName = "save" Then
    Dim dataJson, points, shabbatPoints, hanukkaPoints, purimPoints
    Dim saveCommand, saveSql, dataLength
    If Request.ServerVariables("HTTP_X_BIGHEAD_REQUEST") <> "1" Then SendJson "403 Forbidden", "{""success"":false,""message"":""request_header_required""}"
    If Request.ServerVariables("REQUEST_METHOD") <> "POST" Then SendJson "405 Method Not Allowed", "{""success"":false,""message"":""post_required""}"
    If Not MatchesPattern(player, "^[0-3]$") Then SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_player""}"
    dataJson = Request.Form("data")
    dataLength = Len(dataJson)
    If dataLength < 2 Or dataLength > 250000 Or Left(Trim(dataJson), 1) <> "{" Or Right(Trim(dataJson), 1) <> "}" Then
        SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_data""}"
    End If
    points = SafeInt(Request.Form("points"), 0)
    shabbatPoints = SafeInt(Request.Form("shabbatPoints"), 0)
    hanukkaPoints = SafeInt(Request.Form("hanukkaPoints"), 0)
    purimPoints = SafeInt(Request.Form("purimPoints"), 0)

    saveSql = _
        "SET NOCOUNT ON; SET XACT_ABORT ON; BEGIN TRANSACTION; " & _
        "MERGE dbo.BigHeadGameData WITH (HOLDLOCK) AS Target " & _
        "USING (SELECT CAST(? AS VARCHAR(200)) AS OID, CAST(? AS TINYINT) AS GameKey, CAST(? AS TINYINT) AS PlayerSlot, " & _
        "CAST(? AS NVARCHAR(MAX)) AS DataJson, CAST(? AS INT) AS Points, CAST(? AS INT) AS ShabbatPoints, " & _
        "CAST(? AS INT) AS HanukkaPoints, CAST(? AS INT) AS PurimPoints) AS Source " & _
        "ON Target.OID = Source.OID AND Target.GameKey = Source.GameKey AND Target.PlayerSlot = Source.PlayerSlot " & _
        "WHEN MATCHED THEN UPDATE SET DataJson = Source.DataJson, Points = Source.Points, ShabbatPoints = Source.ShabbatPoints, " & _
        "HanukkaPoints = Source.HanukkaPoints, PurimPoints = Source.PurimPoints, UpdatedAt = GETDATE() " & _
        "WHEN NOT MATCHED THEN INSERT (OID, GameKey, PlayerSlot, DataJson, Points, ShabbatPoints, HanukkaPoints, PurimPoints) " & _
        "VALUES (Source.OID, Source.GameKey, Source.PlayerSlot, Source.DataJson, Source.Points, Source.ShabbatPoints, Source.HanukkaPoints, Source.PurimPoints); " & _
        "COMMIT TRANSACTION;"

    Set saveCommand = Server.CreateObject("ADODB.Command")
    Set saveCommand.ActiveConnection = Conn
    saveCommand.CommandType = 1
    saveCommand.CommandText = saveSql
    saveCommand.Parameters.Append saveCommand.CreateParameter("p1", 200, 1, 200, oid)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p2", 17, 1, , 2)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p3", 17, 1, , CInt(player))
    saveCommand.Parameters.Append saveCommand.CreateParameter("p4", 203, 1, dataLength, dataJson)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p5", 3, 1, , points)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p6", 3, 1, , shabbatPoints)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p7", 3, 1, , hanukkaPoints)
    saveCommand.Parameters.Append saveCommand.CreateParameter("p8", 3, 1, , purimPoints)
    On Error Resume Next
    saveCommand.Execute
    If Err.Number <> 0 Then
        Err.Clear
        Set saveCommand = Nothing
        On Error GoTo 0
        SendJson "500 Internal Server Error", "{""success"":false,""message"":""database_error""}"
    End If
    On Error GoTo 0
    Set saveCommand = Nothing
    SendJson "200 OK", "{""success"":true}"
End If

If actionName = "result" Then
    Dim winner, loser, resultCommand
    If Request.ServerVariables("HTTP_X_BIGHEAD_REQUEST") <> "1" Or Request.ServerVariables("REQUEST_METHOD") <> "POST" Then
        SendJson "403 Forbidden", "{""success"":false,""message"":""invalid_request""}"
    End If
    winner = Left(Request.Form("winner"), 200)
    loser = Left(Request.Form("loser"), 200)
    If Len(winner) = 0 Or Len(loser) = 0 Then SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_result""}"
    Set resultCommand = Server.CreateObject("ADODB.Command")
    Set resultCommand.ActiveConnection = Conn
    resultCommand.CommandType = 1
    resultCommand.CommandText = "INSERT INTO dbo.BigHeadGameResults (OID, GameKey, WinnerName, LoserName) VALUES (CAST(? AS VARCHAR(200)), CAST(? AS TINYINT), CAST(? AS NVARCHAR(200)), CAST(? AS NVARCHAR(200)))"
    resultCommand.Parameters.Append resultCommand.CreateParameter("p1", 200, 1, 200, oid)
    resultCommand.Parameters.Append resultCommand.CreateParameter("p2", 17, 1, , 2)
    resultCommand.Parameters.Append resultCommand.CreateParameter("p3", 202, 1, 200, winner)
    resultCommand.Parameters.Append resultCommand.CreateParameter("p4", 202, 1, 200, loser)
    On Error Resume Next
    resultCommand.Execute
    If Err.Number <> 0 Then
        Err.Clear
        Set resultCommand = Nothing
        On Error GoTo 0
        SendJson "500 Internal Server Error", "{""success"":false,""message"":""database_error""}"
    End If
    On Error GoTo 0
    Set resultCommand = Nothing
    SendJson "200 OK", "{""success"":true}"
End If

SendJson "400 Bad Request", "{""success"":false,""message"":""invalid_action""}"
%>

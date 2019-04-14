VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "C_Timer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
'C_Timer Class

Option Explicit

Private pInterval As String
Private pName As String
Private pRunning As Boolean
Private pNextTick

Public Function Process(func)
    If func <> "" Then
        Application.Run func
        pNextTick = Now + TimeValue(generateRandomTime)
        Application.OnTime pNextTick, pName & "_Tick"
    End If
End Function

Public Function StartTimer() As Boolean
On Error GoTo hell
    Randomize
    If TimeValue(pInterval) > TimeValue("00.00.00") And pName <> "" And pRunning <> True Then
        pNextTick = Now + TimeValue(generateRandomTime)
        Application.OnTime pNextTick, pName & "_Tick"
        pRunning = True
    Else
        GoTo hell
    End If
    Exit Function
hell:
        pRunning = False
        Application.StatusBar = "Failed to update, close & reopen the document"
End Function

Public Property Get Start() As C_Timer
    StartTimer
    Set Start = Me
End Property

Public Function StopTimer() As Boolean
On Error GoTo hell
    If pRunning = True Then
        Application.OnTime pNextTick, pName & "_Tick", , False
        pRunning = False
    Else
        GoTo hell
    End If
    Exit Function
hell:
        
End Function


Public Property Get interval() As String
    interval = pInterval
End Property
Public Property Let interval(Value As String)
    pInterval = Value
End Property

Public Property Get Name() As String
    Name = pName
End Property
Public Property Let Name(Value As String)
    pName = Value
End Property


Private Function generateRandomTime() As String
    Dim dInterval As Double
    
    dInterval = DateDiff("s", "00:00:00", pInterval)
    dInterval = dInterval * (0.9 + 0.2 * Rnd) / DAY_SECONDS
    
    generateRandomTime = Format(dInterval, "hh:mm:ss")
End Function

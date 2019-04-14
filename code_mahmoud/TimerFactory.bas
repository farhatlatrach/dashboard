Attribute VB_Name = "TimerFactory"

Public Function CreateTimer(Name As String, interval As String) As C_Timer
    Set newTimer_ = New C_Timer
    newTimer_.Name = Name
    newTimer_.interval = interval
    Set CreateTimer = newTimer_
End Function

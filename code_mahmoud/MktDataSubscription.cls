VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "MktDataSubscription"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' --------------------------------------------------------------------------
' Bloomberg L.L.P.
'
' --------------------------------------------------------------------------
' Bloomberg L.L.P.
'
' COM v3 Data Control code example - Visual Basic.
'
' Demonstrates the use of the Bloomberg Data Control for Subscription data.
'
' ** Registered Data Control Class Module **
'
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A  PARTICULAR PURPOSE.
'
' --------------------------------------------------------------------------

Option Explicit

Private WithEvents session As blpapicomLib2.session
Attribute session.VB_VarHelpID = -1
'Private cid  As blpapicomLib2.CorrelationId

Private m_bSubscribed As Boolean

Dim m_oLinkedPositions As Collection ' this is storing all positions attached to this subscription

' Instantiate the Bloomberg COM Control
Private Sub class_initialize()
    
    Set session = New blpapicomLib2.session
   
    session.QueueEvents = True
    session.Start
   
    session.OpenService ("//blp/mktdata")
       
    Set m_oLinkedPositions = Nothing
    m_bSubscribed = False
End Sub

'Destroy the Bloomberg COM Control
Private Sub Class_Terminate()

    Set session = Nothing
   
End Sub

Public Sub unsubscribe()
    
    Dim oLinkedPos As Position
    Dim subs As blpapicomLib2.SubscriptionList
   
    Set subs = session.CreateSubscriptionList()
   
    If (m_oLinkedPositions Is Nothing) Then
        Exit Sub
    End If
    
    For Each oLinkedPos In m_oLinkedPositions
        Dim corID As blpapicomLib2.CorrelationId
        Set corID = session.CreateCorrelationId(oLinkedPos.ID)
        subs.Add "", "", corID
    Next
     
    If (m_oLinkedPositions.Count > 0) Then
        session.unsubscribe subs
    End If
    
    m_bSubscribed = False
End Sub

Public Sub subscribeToMktDataFeeds(oPosCol As Collection)

    Dim sTicker As String
    Dim subsList As blpapicomLib2.SubscriptionList
    
    Dim oPos As Position
    
    Set subsList = session.CreateSubscriptionList()
        
    Set m_oLinkedPositions = oPosCol
       
    ' Unsubscribe first before making new subscriptions
    unsubscribe
    
    'bear in mind collection count starts at 1.
    
    For Each oPos In oPosCol
        Dim corID As blpapicomLib2.CorrelationId
        Set corID = session.CreateCorrelationId(oPos.ID)
        sTicker = oPos.Ticker & " " & oPos.SecType
        subsList.AddEx sTicker, LAST_PRICE, "Interval=5.0", corID
        
    Next
    
    session.Subscribe subsList
    
    m_bSubscribed = True
End Sub

Private Sub session_ProcessEvent(ByVal obj As Object)

    On Error GoTo errHandler
    
    Dim eventObj As blpapicomLib2.Event
    
    Set eventObj = obj
    
    If Application.Ready Then
        If eventObj.EventType = SUBSCRIPTION_DATA Then
            Dim it As blpapicomLib2.MessageIterator
            Set it = eventObj.CreateMessageIterator()
            Do While it.Next()
                Dim msg As Message
                Set msg = it.Message
                Dim fld As Element
                
                If (msg.AsElement.HasElement(LAST_PRICE)) Then
                    Set fld = msg.GetElement(LAST_PRICE)
                    Dim sCID As String
                    sCID = msg.CorrelationId.Value

                    If (Not fld Is Nothing) Then
                        m_oLinkedPositions.Item(sCID).LastPrice = fld.GetValue(0)
                    End If
                    
                End If
                
            Loop
        End If
    End If

    Exit Sub
    
errHandler:
    MsgBox Err.Description
    
End Sub

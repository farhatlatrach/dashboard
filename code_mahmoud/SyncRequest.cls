VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "SyncRequest"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' --------------------------------------------------------------------------
' Bloomberg L.L.P.
'
' COM v3 Data Control code example - Visual Basic.
'
' Demonstrates the use of the Bloomberg Data Control for Reference data.
' The class processes returned data events synchronously.
'
' ** Registered Data Control Class Module **
'
' Copyright 2012. Bloomberg Finance L.P.
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of
' this software and associated documentation files (the "Software"), to deal in
' the Software without restriction, including without limitation the rights to
' use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
' of the Software, and to permit persons to whom the Software is furnished to do
' so, subject to the following conditions:  The above copyright notice and this
' permission notice shall be included in all copies or substantial portions of
' the Software.  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
' EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
' MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
' EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
' OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
' ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
' --------------------------------------------------------------------------
Option Explicit

Private session As blpapicomLib2.session
Dim refdataservice As blpapicomLib2.Service


' Instantiate the Bloomberg ActiveX Control
Private Sub class_initialize()

    Set session = New blpapicomLib2.session
    session.QueueEvents = True
    session.Start
   
    session.OpenService ("//blp/refdata")
    Set refdataservice = session.GetService("//blp/refdata")
    
End Sub

'Destroy the Bloomberg COM Control
Private Sub Class_Terminate()
   Set session = Nothing
End Sub
Public Sub getFXRates()

    Dim req As Request
    Dim sCrncyPair As String
    Dim oCCYPair As CurrencyPair
    Dim i As Integer
    Dim bSendRequest As Boolean
    
    bSendRequest = False
    Set req = refdataservice.CreateRequest("ReferenceDataRequest")
    
    For Each oCCYPair In myCurrencies
        If (oCCYPair.update) Then
            req.GetElement("securities").AppendValue oCCYPair.Ticker & " BGN CURNCY"
            bSendRequest = True
        End If
    Next
    
    If (bSendRequest = False) Then
        Exit Sub
    End If
    req.GetElement("fields").AppendValue LAST_PRICE
    
   ' Send the request
    session.SendRequest req
       
    Dim eventObj As blpapicomLib2.Event
    Do
        Set eventObj = session.NextEvent() ' this will block code execution until the next event is received
        If eventObj.EventType = PARTIAL_RESPONSE Or eventObj.EventType = RESPONSE Then
            Dim it As blpapicomLib2.MessageIterator
            Set it = eventObj.CreateMessageIterator()
            
            Do While it.Next()
                Dim msg As Message
                Set msg = it.Message
                
                Dim numSecurities As Integer
                numSecurities = msg.GetElement("securityData").NumValues
                                
                For i = 0 To numSecurities - 1
                    Dim Security As Element
                    Set Security = msg.GetElement("securityData").GetValue(i)
                    Dim sSec
                    
                    sSec = Security.GetElement("security").Value
                    
                    Dim spltSec()  As String
                                      
                    spltSec = Split(sSec, " ")
                    sSec = spltSec(0)
                    
'                    Set oCCYPair = myCurrencies.Item(sSec)
                    Dim fields As Element
                    Set fields = Security.GetElement("fieldData")
                    Dim field As Element
                                       
                    If (fields.NumElements > 0) Then
                        Set field = fields.GetElement(0)
                    
                        If (field.Name = LAST_PRICE) Then
                            myCurrencies.Item(sSec).Rate = field.Value
                        End If
                    End If
                    
                Next
            Loop
            
            If eventObj.EventType = RESPONSE Then Exit Do
            
        End If
    Loop

End Sub


Public Sub MakeRequest(posList As Collection, sFldList As Variant, _
                        Optional bNewPositions As Boolean = False)

    Dim req As Request
    Dim nRow As Long
    Dim oPos As Position
    Dim sTicker As String
    Dim posMapping As Collection
    
    If (bNewPositions) Then
        Set posMapping = New Collection
    End If
    
    Dim i As Integer
    i = 0
    
    Set req = refdataservice.CreateRequest("ReferenceDataRequest")
    
    For Each oPos In posList
        'if it is a new position for today and we did not get the static data for it yet
        ' then add the ticker to the request
        If (bNewPositions) Then
            If (oPos.Crncy = "") Then
                posMapping.Add Item:=oPos.ID, Key:=oPos.Ticker
                sTicker = oPos.Ticker & " " & oPos.SecType
                req.GetElement("securities").AppendValue sTicker
            End If
        Else
            sTicker = oPos.Ticker & " " & oPos.SecType
            req.GetElement("securities").AppendValue sTicker
        End If
    Next

    For nRow = LBound(sFldList, 1) To UBound(sFldList, 1)
        req.GetElement("fields").AppendValue sFldList(nRow)
    Next
    
    ' Send the request
    session.SendRequest req
    
    Dim eventObj As blpapicomLib2.Event
    Do
        Set eventObj = session.NextEvent() ' this will block code execution until the next event is received
        If eventObj.EventType = PARTIAL_RESPONSE Or eventObj.EventType = RESPONSE Then
            Dim it As blpapicomLib2.MessageIterator
            Set it = eventObj.CreateMessageIterator()
            
            Do While it.Next()
                Dim msg As Message
                Set msg = it.Message
                
                Dim numSecurities As Integer
                numSecurities = msg.GetElement("securityData").NumValues
                
                For i = 0 To numSecurities - 1
                    Dim Security As Element
                    Set Security = msg.GetElement("securityData").GetValue(i)
                    Dim sSec
                    
                    sSec = Security.GetElement("security").Value
                    
                    Dim spltSec()  As String
                    
                    Dim iCount As Integer
                    
                    spltSec = Split(sSec, " ")
                    iCount = UBound(spltSec)
                    
                    If (iCount = 2) Then
                        sSec = spltSec(0) & " " & spltSec(1)
                    End If
                    If (iCount = 1) Then
                        sSec = spltSec(0)
                    End If
                    
                    If (bNewPositions) Then
                        Dim sID As String
                        sID = CStr(posMapping.Item(sSec))
                        Set oPos = posList.Item(sID)
                    Else
                        Set oPos = posList.Item(sSec)
                    End If
                    
                    Dim fields As Element
                    Set fields = Security.GetElement("fieldData")
                    Dim numFields As Integer
                    numFields = fields.NumElements
                    
                    Dim a As Integer
                    For a = 0 To numFields - 1
                        Dim field As Element
                        Set field = fields.GetElement(a)
                        
                        Select Case field.Name
                            Case PREV_CLOSE
                                oPos.PrevClose = field.Value
                            Case PREV_ADJ_CLOSE
                                oPos.AdjPrevClose = field.Value
                            Case QUOTED_CRNCY
                                oPos.Crncy = field.Value
                                Dim sCrncyPair As String
                                sCrncyPair = UCase(field.Value) & g_REF_CCY
                                If (ContainsKey(myCurrencies, sCrncyPair) = False) Then
                                    Dim oCurncyPair As CurrencyPair
                                    Set oCurncyPair = New CurrencyPair
                                    oCurncyPair.Ticker = sCrncyPair
                                    If (UCase(oPos.Crncy) = g_REF_CCY) Then
                                        oCurncyPair.update = False
                                    End If
                                    
                                    myCurrencies.Add Item:=oCurncyPair, Key:=sCrncyPair
                                    Set oCurncyPair = Nothing
                                End If
                                
                            Case LAST_PRICE
                                oPos.LastPrice = field.Value
                        End Select
                    Next
                    
                    Set oPos = Nothing
                Next
            Loop
            
            If eventObj.EventType = RESPONSE Then Exit Do
            
        End If
    Loop
End Sub




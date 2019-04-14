VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Portfolio"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_oPositions As Collection

Private m_sName As String

Private m_lID As Long

Private m_lPosCount As Integer

Private m_dLongDelta, m_dShortDelta As Double
Private m_dDayPnL, m_dTdingPnL, m_dDivPnL, m_dBODPnL As Double

Private m_lLongCount, m_lShortCount As Integer

Private Const m_lRefRow As Long = 12

' Historical PnL
Private m_dYTDPnL, m_dMTDPnL, m_dWTDPnL As Double

Property Get YTDPnL() As Double
    YTDPnL = m_dYTDPnL
End Property
Property Let YTDPnL(ByVal dYTDPnL As Double)
    m_dYTDPnL = dYTDPnL
End Property

Property Get MTDPnL() As Double
    MTDPnL = m_dMTDPnL
End Property
Property Let MTDPnL(ByVal dMTDPnL As Double)
    m_dMTDPnL = dMTDPnL
End Property
Property Get WTDPnL() As Double
    WTDPnL = m_dWTDPnL
End Property
Property Let WTDPnL(ByVal dWTDPnL As Double)
    m_dWTDPnL = dWTDPnL
End Property


Property Get LongDelta() As Double
    LongDelta = m_dLongDelta
End Property
Property Let LongDelta(ByVal dLongDelta As Double)
    m_dLongDelta = dLongDelta
End Property

Property Get ShortDelta() As Double
    ShortDelta = m_dShortDelta
End Property
Property Let ShortDelta(ByVal dShortDelta As Double)
    m_dShortDelta = dShortDelta
End Property

Property Get LongCount() As Long
    LongCount = m_lLongCount
End Property
Property Let LongCount(ByVal lLongCount As Long)
    m_lLongCount = lLongCount
End Property

Property Get ID() As Long
    ID = m_lID
End Property
Property Let ID(ByVal lID As Long)
    m_lID = lID
End Property


Property Get ShortCount() As Long
    ShortCount = m_lShortCount
End Property
Property Let ShortCount(ByVal lShortCount As Long)
    m_lShortCount = lShortCount
End Property
Property Get dayPnL() As Double
    dayPnL = m_dDayPnL
End Property
Property Let dayPnL(ByVal dDayPnL As Double)
    m_dDayPnL = dDayPnL
End Property

Property Get TdingPnL() As Double
    TdingPnL = m_dTdingPnL
End Property
Property Let TdingPnL(ByVal dTdingPnL As Double)
    m_dTdingPnL = dTdingPnL
End Property

Property Get DivPnL() As Double
    DivPnL = m_dDivPnL
End Property
Property Let DivPnL(ByVal dDivPnL As Double)
    m_dDivPnL = dDivPnL
End Property

Property Get BODPnL() As Double
    BODPnL = m_dBODPnL
End Property
Property Let BODPnL(ByVal dBODPnL As Double)
    m_dBODPnL = dBODPnL
End Property

Sub loadPositions(ByRef oPositions As Collection)
    
    For Each oPos In oPositions
        m_oPositions.Add Item:=oPos, Key:=oPos.Ticker
        m_lPosCount = m_lPosCount + 1
        If (oPos.currentPos > 0) Then
            m_lLongCount = m_lLongCount + 1
        Else
            m_lShortCount = m_lShortCount + 1
        End If
    Next
    
End Sub

Sub addPosition(ByVal oPos As Position)

    If (m_sName = "") Then
        m_sName = oPos.Ptfl
        Set m_oPositions = New Collection
    End If
    
    m_oPositions.Add Item:=oPos, Key:=oPos.Ticker
    m_lPosCount = m_lPosCount + 1
    If (oPos.currentPos > 0) Then
        m_lLongCount = m_lLongCount + 1
    Else
        m_lShortCount = m_lShortCount + 1
    End If

End Sub

Private Sub class_initialize()
    'MsgBox "Class is being initialized"
    m_sName = ""
    m_lPosCount = 0
    m_dLongDelta = 0#
    m_dShortDelta = 0#
    m_lLongCount = 0
    m_lShortCount = 0
    m_dDayPnL = 0#
    m_dTdingPnL = 0#
    m_dDivPnL = 0#
    m_dBODPnL = 0#
    m_dYTDPnL = 0#
    m_dMTDPnL = 0#
    m_dWTDPnL = 0#
    Set m_oPositions = Nothing
           
End Sub

Property Get Positions() As Collection
    Set Positions = m_oPositions
End Property


Property Get Name() As String
    Name = m_sName
End Property
Property Get PosCount() As String
    PosCount = m_lPosCount
End Property


Sub loadPortfolio(sPortfolio As String) ' to be called only at start of day
    
    Dim tradingSheet As Variant
    Dim rowCounter As Integer
    Dim ptflDataArray() As Variant
    
    rowCounter = 0
    
    Set tradingSheet = Workbooks("PL_Main.xlsm").Sheets(sPortfolio)
        
    'check if the portfolio name is correct
    If (tradingSheet.Cells(m_lRefRow, 3).Value = sPortfolio) Then
        
        rowCounter = 50

        Do While (tradingSheet.Cells(m_lRefRow + rowCounter, 3).Value = sPortfolio)
            rowCounter = rowCounter + 50
        Loop
        
        ReDim ptflDataArray(0 To rowCounter, 1 To 21)
        
        ptflDataArray = Range(tradingSheet.Cells(m_lRefRow, 1), _
                                tradingSheet.Cells(m_lRefRow + rowCounter, 21)).Value
                                
        rowCounter = 1
        Do While (ptflDataArray(rowCounter, 3) = sPortfolio)
            
            Dim oPosition As New Position
            oPosition.ID = Me.ID * 1000 + ptflDataArray(rowCounter, 1)
            oPosition.Ptfl = sPortfolio
            
            oPosition.loadPosition rowCounter, ptflDataArray
                       
            If (m_sName = "") Then
                m_sName = sPortfolio
                Set m_oPositions = New Collection
            End If
    
            m_oPositions.Add Item:=oPosition, Key:=oPosition.Ticker
            m_lPosCount = m_lPosCount + 1
            If (oPosition.currentPos > 0) Then
                    m_lLongCount = m_lLongCount + 1
            Else
                    m_lShortCount = m_lShortCount + 1
            End If
            
            ' Add Position to BOD positions
            g_BODPositions.Add Item:=oPosition, Key:=CStr(oPosition.ID)
            
            Set oPosition = Nothing
            
            rowCounter = rowCounter + 1
        Loop
    End If
    
'LastLine:
 '   MsgBox "Portfolio name is not matching Sheet name. Abort"
 
End Sub

Sub reload() ' to be called intraday here assume portfolios already exist.
    
    Dim tradingSheet As Variant
    Dim rowCounter As Integer
    Dim ptflDataArray() As Variant
   
    Set tradingSheet = Workbooks("PL_Main.xlsm").Sheets(m_sName)
        
    rowCounter = Me.Positions.Count

    Do While (tradingSheet.Cells(m_lRefRow + rowCounter, 3).Value = m_sName)
        rowCounter = rowCounter + 10
    Loop
        
    ReDim ptflDataArray(0 To rowCounter, 1 To 21)
        
    ptflDataArray = Range(tradingSheet.Cells(m_lRefRow, 1), _
                            tradingSheet.Cells(m_lRefRow + rowCounter, 21)).Value
    
    rowCounter = 1
    
    'check if the portfolio name is correct
    If (ptflDataArray(rowCounter, 3) = m_sName) Then
            
        Do While (ptflDataArray(rowCounter, 3) = m_sName)
            ' check if this position exists already
            Dim sKey As String
            Dim posId As Long
            posId = Me.ID * 1000 + ptflDataArray(rowCounter, 1)
            sKey = CStr(posId)
        
            If (ContainsKey(g_BODPositions, sKey) = False) Then
                ' This is a new position for today
                ' Check if it has been loaded earlier
                If (ContainsKey(g_NewPositions, sKey) = False) Then
                    'this is a new position for today and it has not been loaded yet
                    
                    Dim oPosition As New Position
                
                    oPosition.ID = Me.ID * 1000 + ptflDataArray(rowCounter, 1)
                    oPosition.Ptfl = m_sName
                    
                    oPosition.loadPosition rowCounter, ptflDataArray
    
                    ' add the position to the portfolio
                    m_oPositions.Add Item:=oPosition, Key:=oPosition.Ticker
                    m_lPosCount = m_lPosCount + 1
                    If (oPosition.currentPos > 0) Then
                        m_lLongCount = m_lLongCount + 1
                    Else
                        m_lShortCount = m_lShortCount + 1
                    End If
                    
                    'add to today's New Positions collection
                    g_NewPositions.Add Item:=oPosition, Key:=sKey
                
                    Set oPosition = Nothing
                
                Else 'this position is a new position for today's trading
                     ' but it is already loaded, check if it needs updating
                    
                    g_NewPositions.Item(sKey).refresh rowCounter, ptflDataArray
                End If 'end of position check in NewPositions
            
            Else ' postion is a BOD position refresh it.
            
                g_BODPositions.Item(sKey).refresh rowCounter, ptflDataArray
            End If
            
            rowCounter = rowCounter + 1
        Loop

    End If
'LastLine:
 '   MsgBox "Portfolio name is not matching Sheet name. Abort"
End Sub
Sub prepareToUpdate()

    m_lPosCount = m_oPositions.Count
    m_dLongDelta = 0#
    m_dShortDelta = 0#
    m_lLongCount = 0
    m_lShortCount = 0
    m_dTdingPnL = 0#
    m_dDayPnL = 0#
    m_dDivPnL = 0#
    m_dBODPnL = 0#
    m_dDivPnL = 0#

End Sub

Public Sub eod()
    ' shut down the book - archive the PnL
    Dim todayPnL As Double
    m_dDayPnL = (m_dTdingPnL + m_dBODPnL + m_dDivPnL)
    m_dWTDPnL = m_dWTDPnL + m_dDayPnL
    m_dMTDPnL = m_dMTDPnL + m_dDayPnL
    m_dYTDPnL = m_dYTDPnL + m_dDayPnL
    
End Sub

Public Function printMe() As String
    Dim sLine As String
    sLine = m_sName & "|" & m_dLongDelta & "|" & m_dShortDelta & "|" & _
            m_dDayPnL & "|" & m_dBODPnL & "|" & m_dTdingPnL & "|" & m_dDivPnL & "|" & _
            m_dYTDPnL & "|" & m_dMTDPnL & "|" & m_dWTDPnL
    
    printMe = sLine
End Function


Sub calculatePnL(ByVal refRow As Integer, dataArray() As Variant)
    
    Dim oPos As Position
    Dim sCurncyPair As String
    Dim rowIndex As Integer
    
    Dim dFXRate, dCrncyMultiplier As Double
    dFXRate = 1#
    dCrncyMultiplier = 1#
    
    Me.prepareToUpdate
    
    rowIndex = refRow
    
    For Each oPos In m_oPositions
        
        sCurncyPair = UCase(oPos.Crncy) & g_REF_CCY
        
        If (ContainsKey(myCurrencies, sCurncyPair) = False) Then
            Dim oCurncyPair As CurrencyPair
            Set oCurncyPair = New CurrencyPair
            oCurncyPair.Ticker = sCurncyPair
            
            If (UCase(oPos.Crncy) = g_REF_CCY) Then
                oCurncyPair.update = False
            End If
                                    
            myCurrencies.Add Item:=oCurncyPair, Key:=sCurncyPair
            Set oCurncyPair = Nothing
            
            refreshFX
        End If
            
        dFXRate = myCurrencies.Item(sCurncyPair).Rate
        
        If (oPos.isMinorCurncy) Then
            dCrncyMultiplier = 0.01
        Else
            dCrncyMultiplier = 1#
        End If
        
        ' Quoted curency corrected for mkt data : lastprice / prevClose
        ' Buy and Sell prices are in major currency
        
        oPos.TdingPnL = dFXRate * oPos.ContractSize * _
                        (oPos.BoughtQty * (oPos.LastPrice * dCrncyMultiplier - oPos.AvgBuyPrice) _
                        - oPos.SoldQty * (oPos.LastPrice * dCrncyMultiplier - oPos.AvgSellPrice))
        
        oPos.BODPnL = dFXRate * oPos.ContractSize * _
                            oPos.BODPos * (oPos.LastPrice - oPos.PrevClose) * dCrncyMultiplier
                            
        If (oPos.AdjPrevClose > 0) Then
            oPos.DivPnL = dFXRate * oPos.ContractSize * _
                                oPos.BODPos * (oPos.PrevClose - oPos.AdjPrevClose) * dCrncyMultiplier
        End If
        
        oPos.Delta = dFXRate * oPos.ContractSize * oPos.currentPos * oPos.LastPrice * dCrncyMultiplier
        
        oPos.dayPnL = oPos.TdingPnL + oPos.BODPnL + oPos.DivPnL
            
        dataArray(rowIndex, 0) = m_sName
        dataArray(rowIndex, 1) = oPos.Ticker
        dataArray(rowIndex, 2) = oPos.Delta
        dataArray(rowIndex, 3) = oPos.dayPnL
        dataArray(rowIndex, 4) = oPos.BODPnL
        dataArray(rowIndex, 5) = oPos.TdingPnL
        dataArray(rowIndex, 6) = oPos.DivPnL
        dataArray(rowIndex, 7) = oPos.LastPrice
        dataArray(rowIndex, 8) = oPos.PrevClose
        dataArray(rowIndex, 9) = oPos.currentPos
        dataArray(rowIndex, 10) = oPos.BODPos
        dataArray(rowIndex, 11) = oPos.BoughtQty
        dataArray(rowIndex, 12) = oPos.AvgBuyPrice / dCrncyMultiplier
        dataArray(rowIndex, 13) = oPos.SoldQty
        dataArray(rowIndex, 14) = oPos.AvgSellPrice / dCrncyMultiplier
        dataArray(rowIndex, 15) = oPos.ContractSize
        dataArray(rowIndex, 16) = oPos.SecType
        dataArray(rowIndex, 17) = oPos.YTDPnL
        dataArray(rowIndex, 18) = oPos.MTDPnL
        dataArray(rowIndex, 19) = oPos.WTDPnL
        
        'update portfolio values
        m_dBODPnL = m_dBODPnL + oPos.BODPnL
        m_dTdingPnL = m_dTdingPnL + oPos.TdingPnL
        m_dDivPnL = m_dDivPnL + oPos.DivPnL
        
        If (oPos.Delta > 0) Then
            m_dLongDelta = m_dLongDelta + oPos.Delta
            m_lLongCount = m_lLongCount + 1
        Else
            m_dShortDelta = m_dShortDelta + oPos.Delta
            m_lShortCount = m_lShortCount + 1
        End If
        
        rowIndex = rowIndex + 1
    Next
    
    m_dDayPnL = m_dBODPnL + m_dTdingPnL + m_dDivPnL
    
End Sub

Public Sub dumpfile()
    Dim objFSO, oFile As Object
    Dim sFileName, sPath, sDate As String
    Dim oPos As Position
    
    sDate = Format(Now(), "YYYYMMDD")
    sPath = "P:\RiskReport\"
    
    Set objFSO = CreateObject("Scripting.FileSystemObject")
    
    sFileName = sPath & sDate & "_" & Me.Name & ".txt"
    Set oFile = objFSO.CreateTextFile(sFileName)
    
    For Each oPos In Me.Positions
        oFile.WriteLine oPos.Ticker & "|" & oPos.currentPos
    Next
    
    oFile.Close
    
    Set objFSO = Nothing
    Set oFile = Nothing
End Sub

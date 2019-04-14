VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Position"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_sTicker As String
Private m_lID As Long

Private m_sSecType As String
Private m_sCrncy As String

Private m_bIsMinorCrncy As Boolean

Private m_sPtfl As String

Private m_lCurrentPos As Long 'current position in shares

Private m_lBODPos As Long 'start of day position

'today's traded quantities and prices
Private m_lBoughtQty, m_lSoldQty As Long
Private m_dAvgBuyPrice, m_dAvgSellPrice As Double

' bear in mind quoting currency - painful for UK listed.
Private m_dAdjPrevClose As Double
Private m_dPrevClose As Double
Private m_dLastPrice As Double

Private m_dContractSize As Double

Private m_bisFuture As Boolean
Private m_bUpdate As Boolean

' Daily PnL
Private m_dDayPnL, m_dTdingPnL, m_dBODPnL, m_dDivPnL, m_dDelta As Double

' Historical PnL
Private m_dYTDPnL, m_dMTDPnL, m_dWTDPnL As Double

Property Get Ticker() As String
     Ticker = m_sTicker
End Property

Property Let Ticker(sTicker As String)
    m_sTicker = sTicker
End Property

Property Get SecType() As String
     SecType = m_sSecType
End Property
Property Let SecType(sType As String)
    m_sSecType = sType
End Property
Property Get Crncy() As String
     Crncy = m_sCrncy
End Property
Property Let Crncy(sCCY As String)
    If (sCCY = "GBp" Or sCCY = "ZAr") Then
        m_bIsMinorCrncy = True
    End If
    m_sCrncy = sCCY
End Property

Property Get Ptfl() As String
     Ptfl = m_sPtfl
End Property
Property Let Ptfl(sPtfl As String)
    m_sPtfl = sPtfl
End Property

Property Get ID() As Long
     ID = m_lID
End Property
Property Let ID(lID As Long)
    m_lID = lID
End Property

Property Get currentPos() As Long
    currentPos = m_lCurrentPos
End Property
Property Let currentPos(ByVal lCurrentPos As Long)
    m_lCurrentPos = lCurrentPos
End Property


Property Get BODPos() As Long
    BODPos = m_lBODPos
End Property
Property Let BODPos(ByVal lBODPos As Long)
    m_lBODPos = lBODPos
End Property


Property Get BoughtQty() As Long
    BoughtQty = m_lBoughtQty
End Property
Property Let BoughtQty(ByVal lBoughtQty As Long)
    m_lBoughtQty = lBoughtQty
End Property

Property Get SoldQty() As Long
    SoldQty = m_lSoldQty
End Property
Property Let SoldQty(ByVal lSoldQty As Long)
    m_lSoldQty = lSoldQty
End Property

Property Get AvgBuyPrice() As Double
    AvgBuyPrice = m_dAvgBuyPrice
End Property
Property Let AvgBuyPrice(ByVal dAvgBuyPrice As Double)
    m_dAvgBuyPrice = dAvgBuyPrice
End Property

Property Get AvgSellPrice() As Double
    AvgSellPrice = m_dAvgSellPrice
End Property
Property Let AvgSellPrice(ByVal dAvgSellPrice As Double)
    Let m_dAvgSellPrice = dAvgSellPrice
End Property

Property Get AdjPrevClose() As Double
    AdjPrevClose = m_dAdjPrevClose
End Property
Property Let AdjPrevClose(ByVal dAdjPrevClose As Double)
    m_dAdjPrevClose = dAdjPrevClose
End Property
Property Get PrevClose() As Double
    PrevClose = m_dPrevClose
End Property
Property Let PrevClose(ByVal dPrevClose As Double)
    m_dPrevClose = dPrevClose
End Property
Property Get LastPrice() As Double
    LastPrice = m_dLastPrice
End Property
Property Let LastPrice(ByVal dLastPrice As Double)
    m_dLastPrice = dLastPrice
End Property


Property Get ContractSize() As Double
    ContractSize = m_dContractSize
End Property
Property Let ContractSize(ByVal dContractSize As Double)
    m_dContractSize = dContractSize
End Property
Property Get isFuture() As Boolean
    isFuture = m_bisFuture
End Property
Property Let isFuture(ByVal bisFuture As Boolean)
    m_bisFuture = bisFuture
End Property

Property Get isMinorCurncy() As Boolean
    isMinorCurncy = m_bIsMinorCrncy
End Property
Property Let isMinorCurncy(ByVal bisMinorCurncy As Boolean)
    m_bIsMinorCrncy = bisMinorCurncy
End Property

Property Get UpdateMe() As Boolean
    UpdateMe = m_bUpdate
End Property
Property Let UpdateMe(ByVal bUpdate As Boolean)
    m_bUpdate = bUpdate
End Property

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

Property Get BODPnL() As Double
    BODPnL = m_dBODPnL
End Property
Property Let BODPnL(ByVal dBODPnL As Double)
    m_dBODPnL = dBODPnL
End Property
Property Get DivPnL() As Double
    DivPnL = m_dDivPnL
End Property
Property Let DivPnL(ByVal dDivPnL As Double)
    m_dDivPnL = dDivPnL
End Property

Property Get Delta() As Double
    Delta = m_dDelta
End Property
Property Let Delta(ByVal dDelta As Double)
    m_dDelta = dDelta
End Property



Private Sub class_initialize()
    'MsgBox "Class is being initialized"
    m_sTicker = ""
    m_sSecType = "E"
    m_sPtfl = ""
    m_lID = 0
    m_sCrncy = ""
    m_lCurrentPos = 0
    m_lBODPos = 0
    m_lBoughtQty = 0
    m_lSoldQty = 0
    m_dAvgBuyPrice = 0#
    m_dAvgSellPrice = 0#
    
    m_dPrevClose = 0#
    m_dAdjPrevClose = 0#
    m_dLastPrice = 0#
    m_dContractSize = 1#

    m_dYTDPnL = 0#
    m_dMTDPnL = 0#
    m_dWTDPnL = 0#
    
    m_dDayPnL = 0#
    m_dTdingPnL = 0#
    m_dBODPnL = 0#
    m_dDivPnL = 0#
    
    m_dDelta = 0#
    
    m_bisFuture = False
    m_bUpdate = True
    m_bIsMinorCrncy = False
    
End Sub

Public Sub populatePosition(ByVal sTicker As String, ByVal sPtfl As String, ByVal lCurrentPos As Long, _
                            ByVal lBODPos As Long, ByVal lBoughtQty As Long, ByVal lSoldQty As Long)
    m_sTicker = sTicker
    m_sPtfl = sPtfl
    m_lCurrentPos = lCurrentPos
    m_lBODPos = lBODPos
    m_lBoughtQty = lBoughtQty
    m_lSodlQty = lSoldQty
    
End Sub
                            
                            
Public Sub loadPosition(ByVal rowIndex As Integer, dataArray() As Variant)
    
    Dim strPattern As String: strPattern = "^(\w+)[\._](\w+)"
    
    Dim regEx As New RegExp
    Dim strInput As String
    Dim strResult As String
    
    If (dataArray(rowIndex, 6) = "F") Then
        Me.isFuture = True
        Me.SecType = "INDEX"
        strInput = dataArray(rowIndex, 7)
    Else
        Me.isFuture = False
        strInput = dataArray(rowIndex, 5)
        Me.SecType = "EQUITY"
    End If

    If strPattern <> "" Then
            
        With regEx
            .Global = True
            .MultiLine = True
            .IgnoreCase = False
            .Pattern = strPattern
        End With

        If regEx.Test(strInput) Then
            strResult = regEx.Replace(strInput, "$1 $2")
        Else
            strResult = strInput
        End If
    End If
                
    Me.Ticker = strResult
            
    Me.currentPos = dataArray(rowIndex, 8)
    Me.BODPos = dataArray(rowIndex, 9)
    Me.BoughtQty = dataArray(rowIndex, 10)
    Me.AvgBuyPrice = dataArray(rowIndex, 11)
    Me.SoldQty = dataArray(rowIndex, 12)
    Me.AvgSellPrice = dataArray(rowIndex, 13)
    Me.ContractSize = dataArray(rowIndex, 14)

End Sub

Public Sub refresh(ByVal rowIndex As Integer, dataArray() As Variant)
        
    Me.currentPos = dataArray(rowIndex, 8)
    Me.BODPos = dataArray(rowIndex, 9)
    Me.BoughtQty = dataArray(rowIndex, 10)
    Me.AvgBuyPrice = dataArray(rowIndex, 11)
    Me.SoldQty = dataArray(rowIndex, 12)
    Me.AvgSellPrice = dataArray(rowIndex, 13)
    Me.ContractSize = dataArray(rowIndex, 14)
    Me.UpdateMe = True

End Sub


Public Function printMe() As String
    Dim sLine As String
    sLine = m_sPtfl & "|" & m_sTicker & "|" & m_dDelta & "|" & m_dDayPnL & "|" & _
            m_dBODPnL & "|" & m_dTdingPnL & "|" & m_dDivPnL & "|" & m_dLastPrice & "|" & m_dPrevClose _
            & "|" & m_lCurrentPos & "|" & m_lBODPos & "|" & m_lBoughtQty & "|" & m_dAvgBuyPrice _
            & "|" & m_lSoldQty & "|" & m_dAvgSellPrice & "|" & m_dContractSize & "|" & m_sSecType _
            & "|" & m_sCrncy & "|" & m_dYTDPnL & "|" & m_dMTDPnL & "|" & m_dWTDPnL
    
    printMe = sLine
End Function

Public Sub eod()
    ' shut down the book - archive the PnL
    Dim todayPnL As Double
    m_dDayPnL = (m_dTdingPnL + m_dBODPnL + m_dDivPnL)
    
    m_dWTDPnL = m_dWTDPnL + m_dDayPnL
    m_dMTDPnL = m_dMTDPnL + m_dDayPnL
    m_dYTDPnL = m_dYTDPnL + m_dDayPnL
    
End Sub


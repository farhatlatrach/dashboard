VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "PnLCube"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_sName, m_sBook As String

Private m_dDayPnL, m_dYTDPnL, m_dMTDPnL, m_dWTDPnL As Double

Property Get dayPnL() As Double
    dayPnL = m_dDayPnL
End Property
Property Let dayPnL(ByVal dDayPnL As Double)
    m_dDayPnL = dDayPnL
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

Property Get Name() As String
    Name = m_sName
End Property
Property Let Name(ByVal sName As String)
    m_sName = sName
End Property


Property Get Book() As String
    Book = m_sBook
End Property
Property Let Book(ByVal sBook As String)
    m_sBook = sBook
End Property

Private Sub class_initialize()
    'MsgBox "Class is being initialized"
    m_sName = ""
    m_sBook = ""
    m_dDayPnL = 0#
    m_dYTDPnL = 0#
    m_dMTDPnL = 0#
    m_dWTDPnL = 0#
End Sub


Public Function printMe() As String
    Dim sLine As String
    If (m_sName = "") Then
        sLine = Me.Book & "|" & Me.dayPnL _
                    & "|" & Me.YTDPnL & "|" & Me.MTDPnL & "|" & Me.WTDPnL
    Else
        sLine = Me.Book & "|" & Me.Name & "|" & Me.dayPnL _
                    & "|" & Me.YTDPnL & "|" & Me.MTDPnL & "|" & Me.WTDPnL
    End If
    printMe = sLine
End Function

Public Sub eod()
    ' shut down the book - archive the PnL
    
    m_dWTDPnL = m_dWTDPnL + m_dDayPnL
    m_dMTDPnL = m_dMTDPnL + m_dDayPnL
    m_dYTDPnL = m_dYTDPnL + m_dDayPnL
    
End Sub



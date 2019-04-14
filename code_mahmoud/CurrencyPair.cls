VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "CurrencyPair"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit


Private m_sTicker As String
Private m_dRate As Double
Private m_bUpdate As Boolean

Property Get Ticker() As String
     Ticker = m_sTicker
End Property

Property Let Ticker(sTicker As String)
    m_sTicker = sTicker
End Property

Property Get Rate() As String
     Rate = m_dRate
End Property

Property Let Rate(dRate As String)
    m_dRate = dRate
End Property

Property Get update() As String
     update = m_bUpdate
End Property

Property Let update(bUpdate As String)
    m_bUpdate = bUpdate
End Property


Private Sub class_initialize()
    'MsgBox "Class is being initialized"
    m_sTicker = ""
    m_dRate = 1#
    m_bUpdate = True
End Sub


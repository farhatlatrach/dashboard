VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "CubeWraper"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_sKey As String
Private m_oCube As PnLCube

Private m_oCollection As Collection

Property Get Key() As String
    Key = m_sKey
End Property
Property Let Key(ByVal sKey As String)
    m_sKey = sKey
End Property

Property Get Cube() As PnLCube
    Set Cube = m_oCube
End Property
Property Set Cube(ByVal oCube As PnLCube)
    Set m_oCube = oCube
End Property


Property Get inCollection() As Collection
    Set inCollection = m_oCollection
End Property

Private Sub class_initialize()
    'MsgBox "Class is being initialized"
    m_sKey = ""
    Set m_oCollection = New Collection
    Set m_oCube = New PnLCube
End Sub

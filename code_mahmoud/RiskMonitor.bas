Attribute VB_Name = "RiskMonitor"
' PL Calculation is based on :
'   1. A BOD file - containing positions of previous close
'   2. Trading file containing the trading done today
'   3. Pricing data.
'


' define location of the BOD files
' define loacation of the Trade files
Option Explicit

Public Const PREV_CLOSE As String = "PX_YEST_CLOSE"
Public Const PREV_ADJ_CLOSE As String = "PX_DIV_ADJ_CLOSE_1D"
Public Const QUOTED_CRNCY As String = "QUOTED_CRNCY"
Public Const LAST_PRICE As String = "LAST_PRICE"

Public Const END_DAY As String = "17:35:00"
Public Const MARK_BOOKS As String = "17:05:00"

Public Const TICKER_REF_ROW As Integer = 13
Public Const BOOK_REF_ROW As Integer = 5
Public Const REF_COLUMN = 2


Public myCurrencies As Collection

Public g_OpenPortfolios As Collection

Public g_BODPositions As Collection ' key = PORT_ID x 1000 + POS_INDEX
Public g_NewPositions As Collection ' key = PORT_ID x 1000 + POS_INDEX

' manage overnight positions MD subscriptions seperately from new ones
Dim g_oInitialMDS As New MktDataSubscription

Public bUpDateMktDataSubs As Boolean

' use shorter list where we will subscribe/unsubscribe every time new name is added
Dim g_oNewPosMDS As New MktDataSubscription

Public g_REF_CCY As String

Public g_oFXTimer As C_Timer
Public g_oTPOSTimer As C_Timer
Public g_oRiskAndPLTimer As C_Timer

Public Const DAY_SECONDS As Double = 86400#

Public g_dFX_UPDATE_INTERVAL As Double
Public g_dPOSITIONS_UPDATE_INTERVAL As Double
Public g_dRISK_UPDATE_INTERVAL As Long

Public MAIN_SHEET As Variant

Private m_bMonitorActive As Boolean

Sub unsubscribe()

    g_oInitialMDS.unsubscribe
    g_oNewPosMDS.unsubscribe

End Sub

' first sub to be called.

Sub loadPorfolios(oPtflCollection As Collection)

    Dim prtfList() As String
    Dim numPortfolios As Integer
        
    Dim anchorRange As Range
    
    Set MAIN_SHEET = Workbooks("PL_Main.xlsm").Sheets("Main")
    
    Set anchorRange = MAIN_SHEET.Cells(5, 2)
    If (anchorRange.Value = "") Then
        MsgBox "No Portfolio names are provided, Cell B5 is empty"
        Exit Sub
    End If
    
    numPortfolios = 0
    Do While anchorRange.Offset(numPortfolios, 0).Value <> ""
        numPortfolios = numPortfolios + 1
    Loop
    
    ReDim prtfList(0 To numPortfolios - 1) As String
    
    Dim i As Integer
    
    For i = 0 To numPortfolios - 1
        prtfList(i) = anchorRange.Offset(i, 0).Value
    Next i
    
    Dim sPtflName As Variant
    
    sPtflName = ""
    
    For i = 0 To numPortfolios - 1
        Dim currentPortfolio As Portfolio
        Set currentPortfolio = New Portfolio
        
        currentPortfolio.ID = i + 1
        sPtflName = prtfList(i)
        currentPortfolio.loadPortfolio (sPtflName)
        
        oPtflCollection.Add Item:=currentPortfolio, Key:=sPtflName
        
        Set currentPortfolio = Nothing
    Next
    
End Sub

Sub startOfDay(oPortfolioList As Collection)

' Load Portfolios
' get all previous closes
' Positions from TPOS and Prices from Bloomberg
' Get close prices / Adj Close

    Dim bbControl As New SyncRequest
    Dim oPortfolio As Portfolio
    
    Set MAIN_SHEET = Workbooks("PL_Main.xlsm").Sheets("Main")
        
    ' Calculate number of securities and number of fields
    Dim numSecurity As Integer
    Dim sFields(4) As Variant
    sFields(0) = PREV_CLOSE
    sFields(1) = PREV_ADJ_CLOSE
    sFields(2) = QUOTED_CRNCY
    sFields(3) = LAST_PRICE
        
    Dim sCCY As String
    g_REF_CCY = "USD"
    
    sCCY = MAIN_SHEET.Cells(1, 3).Value
    'set ref currency
    Select Case sCCY
        Case "", "USD"
            g_REF_CCY = "USD"
        Case "GBP"
            g_REF_CCY = "GBP"
        Case "EUR"
            g_REF_CCY = "EUR"
    End Select
    
    For Each oPortfolio In oPortfolioList
        
        Dim allPositions As Collection
        Set allPositions = oPortfolio.Positions
        
        bbControl.MakeRequest allPositions, sFields
    Next
    
    bbControl.getFXRates
    
End Sub

Sub startMonitor()
   
On Error GoTo errHandler
    If (m_bMonitorActive = True) Then
        Exit Sub
    End If
    
    ' allocate memory to global variables
    Set g_OpenPortfolios = New Collection
    Set g_BODPositions = New Collection
    Set g_NewPositions = New Collection
    Set myCurrencies = New Collection
        
    'get all portfolios
    loadPorfolios g_OpenPortfolios
    
    ' load PnL history
    Daily_Jobs.loadPnLData g_OpenPortfolios
    
    ' get start of day data
    startOfDay g_OpenPortfolios
    
    'subscribe to market data
    g_oInitialMDS.subscribeToMktDataFeeds g_BODPositions
    
    ' run risk and PnL
    updateRiskAndPnL
        
    'create timers
    initialiseTimers
    
    m_bMonitorActive = True
Done:
    Exit Sub
errHandler:
    Dim ErrMsg As String
    ErrMsg = Err.Description
    MsgBox ErrMsg
End Sub

Sub stopMonitor()
    
    If (m_bMonitorActive) Then
        StopTimers
        unsubscribe
    End If
    m_bMonitorActive = False
End Sub

Public Function markBooks()
       
    If (g_OpenPortfolios.Count < 1) Then
        loadPorfolios g_OpenPortfolios
        startOfDay g_OpenPortfolios
    End If
    
    updateRiskAndPnL
    
    Daily_Jobs.storeDailyPnL g_OpenPortfolios
    
    Daily_Jobs.archivePL
    
End Function
    
Sub initialiseTimers()

    StopTimers
    
    Set MAIN_SHEET = Workbooks("PL_Main.xlsm").Sheets("Main")
    
    g_dFX_UPDATE_INTERVAL = MAIN_SHEET.Cells(2, 18).Value
    g_dPOSITIONS_UPDATE_INTERVAL = MAIN_SHEET.Cells(3, 18).Value
    g_dRISK_UPDATE_INTERVAL = MAIN_SHEET.Cells(4, 18).Value
    
    Dim sInterval As String
    g_dFX_UPDATE_INTERVAL = g_dFX_UPDATE_INTERVAL / DAY_SECONDS
    sInterval = Format(g_dFX_UPDATE_INTERVAL, "hh:mm:ss")
    Set g_oFXTimer = TimerFactory.CreateTimer("FXTimer", sInterval).Start
    
    sInterval = Format(g_dPOSITIONS_UPDATE_INTERVAL / DAY_SECONDS, "hh:mm:ss")
    Set g_oTPOSTimer = TimerFactory.CreateTimer("TPOSTimer", sInterval).Start
    
    sInterval = Format(g_dRISK_UPDATE_INTERVAL / DAY_SECONDS, "hh:mm:ss")
    Set g_oRiskAndPLTimer = TimerFactory.CreateTimer("RiskTimer", sInterval).Start
    
    Application.OnTime TimeValue(MARK_BOOKS), "markBooks"
    
    Application.OnTime TimeValue(END_DAY), "stopMonitor"

End Sub

Public Function FXTimer_Tick()
    g_oFXTimer.Process "refreshFX"
End Function

Public Function TPOSTimer_Tick()
    g_oTPOSTimer.Process "updatePositions"
End Function

Public Function RiskTimer_Tick()
    g_oRiskAndPLTimer.Process "updateRiskAndPnL"
End Function

Public Sub stopFXTimer()
    g_oFXTimer.StopTimer
End Sub

Public Sub stopTPOSTimer()
    g_oTPOSTimer.StopTimer
End Sub

Public Sub stopRiskTimer()
    g_oRiskAndPLTimer.StopTimer
End Sub

Public Sub StopTimers()
    If (g_oFXTimer Is Nothing) = False Then
        g_oFXTimer.StopTimer
    End If
    If (g_oTPOSTimer Is Nothing) = False Then
        g_oTPOSTimer.StopTimer
    End If
    If (g_oRiskAndPLTimer Is Nothing) = False Then
        g_oRiskAndPLTimer.StopTimer
    End If
End Sub

Public Sub StartTimers()
    g_oFXTimer = g_oFXTimer.Start
    g_oTPOSTimer = g_oTPOSTimer.Start
    g_oRiskAndPLTimer = g_oRiskAndPLTimer.Start
End Sub

Public Sub prepareRiskAnalysis()

    Dim oPort As Portfolio
    
    For Each oPort In g_OpenPortfolios
        oPort.dumpfile
    Next
    
    'system call to run the analysis
End Sub

Function refreshFX()

'reresh fx rates
    Dim bbRefDataRequest As New SyncRequest
    bbRefDataRequest.getFXRates
    
End Function

Function updatePositions()
    
'    Application.StatusBar = "Updating Positions"
    Dim oPortfolio As Portfolio
    Dim lCountNewPostions As Long
    
    lCountNewPostions = g_NewPositions.Count
    
'loop through all open portfolios and reload positions
    For Each oPortfolio In g_OpenPortfolios
        oPortfolio.reload
    Next
    
    ' check if new positions have been added since last time
    If (lCountNewPostions < g_NewPositions.Count) Then
        'get static data for new positions
        getStaticDataForNewPositions g_NewPositions
        ' reinitialise mktdata subscriptions for new positions
        g_oNewPosMDS.subscribeToMktDataFeeds g_NewPositions
    End If
    
'    Application.StatusBar = False
End Function
Sub getStaticDataForNewPositions(oPosCol As Collection)

' Load Portfolios
' get all previous closes
' Positions from TPOS and Prices from Bloomberg
' Get close prices / Adj Close

    Dim bbControl As New SyncRequest
    
' Calculate number of securities and number of fields
    Dim numSecurity As Integer
    Dim sFields(4) As Variant
    sFields(0) = PREV_CLOSE
    sFields(1) = PREV_ADJ_CLOSE
    sFields(2) = QUOTED_CRNCY
    sFields(3) = LAST_PRICE
        
    Dim sCCY As String
    g_REF_CCY = "USD"
    
    sCCY = MAIN_SHEET.Cells(1, 3).Value
    'set ref currency
    Select Case sCCY
        Case "", "USD"
            g_REF_CCY = "USD"
        Case "GBP"
            g_REF_CCY = "GBP"
        Case "EUR"
            g_REF_CCY = "EUR"
    End Select
          
    bbControl.MakeRequest oPosCol, sFields, True
    
    bbControl.getFXRates
End Sub

Function updateRiskAndPnL()
       
    Set MAIN_SHEET = Workbooks("PL_Main.xlsm").Sheets("Main")
    
    Dim oPortfolio As Portfolio
    Dim tickerRefRow, bookCount, rowsCount As Integer
    Dim oBookRange, outputRange As Range
    
    Dim dataArray(), bookSummary As Variant
    
    ReDim bookSummary(1 To g_OpenPortfolios.Count, 1 To 10)
    
    rowsCount = 0
    For Each oPortfolio In g_OpenPortfolios
        rowsCount = rowsCount + oPortfolio.Positions.Count
    Next
    
    ReDim dataArray(1 To rowsCount, 0 To 19)
                               
    tickerRefRow = 1
    bookCount = 1
    
    For Each oPortfolio In g_OpenPortfolios
        
        oPortfolio.calculatePnL tickerRefRow, dataArray
        
        bookSummary(bookCount, 1) = oPortfolio.dayPnL
        bookSummary(bookCount, 2) = oPortfolio.LongDelta + oPortfolio.ShortDelta
        bookSummary(bookCount, 3) = oPortfolio.LongDelta
        bookSummary(bookCount, 4) = oPortfolio.ShortDelta
        bookSummary(bookCount, 5) = oPortfolio.BODPnL
        bookSummary(bookCount, 6) = oPortfolio.TdingPnL
        bookSummary(bookCount, 7) = oPortfolio.DivPnL
        bookSummary(bookCount, 8) = oPortfolio.YTDPnL
        bookSummary(bookCount, 9) = oPortfolio.MTDPnL
        bookSummary(bookCount, 10) = oPortfolio.WTDPnL
        
        bookCount = bookCount + 1
        tickerRefRow = tickerRefRow + oPortfolio.Positions.Count
    Next
    
    Set oBookRange = MAIN_SHEET.Range(MAIN_SHEET.Cells(BOOK_REF_ROW, REF_COLUMN + 1), _
                            MAIN_SHEET.Cells(BOOK_REF_ROW + bookCount - 2, REF_COLUMN + 10))
    
    oBookRange.Value = bookSummary
    
    MAIN_SHEET.Range(MAIN_SHEET.Cells(TICKER_REF_ROW, REF_COLUMN), _
        MAIN_SHEET.Cells(TICKER_REF_ROW + 500, REF_COLUMN + 19)).ClearContents
    
    Set outputRange = MAIN_SHEET.Range(MAIN_SHEET.Cells(TICKER_REF_ROW, REF_COLUMN), _
                            MAIN_SHEET.Cells(TICKER_REF_ROW + rowsCount - 1, REF_COLUMN + 19))
    outputRange.Value = dataArray
    
End Function



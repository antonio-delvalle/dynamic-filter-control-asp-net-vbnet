Imports AjaxControlToolkit

Public Class cldUserControl
    Inherits System.Web.UI.UserControl

    Public Delegate Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event btnPostClk As btnDelete_Click

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent btnPostClk(sender, e)
    End Sub

    Private _isCalendarSingle As Boolean
    Public Property isCalendarSingle() As Boolean
        Get
            Return _isCalendarSingle
        End Get
        Set(ByVal value As Boolean)
            _isCalendarSingle = value
            TxtFecSolFin.Visible = Not (_isCalendarSingle)
            imgFec2.Visible = Not (_isCalendarSingle)
        End Set
    End Property


    Private _SessionID As String
    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
        End Set
    End Property

    Public Property labelText() As String
        Get
            Return Label1.Text
        End Get
        Set(ByVal value As String)
            Label1.Text = value
        End Set
    End Property

    Private _typeOfControl As String
    Public Property TypeOfControl() As String
        Get
            Return _typeOfControl
        End Get
        Set(ByVal value As String)
            _typeOfControl = value
        End Set
    End Property

    Private _startDate As String
    Public Property StartDate() As String
        Get
            Return _startDate
        End Get
        Set(ByVal value As String)
            _startDate = value
            TxtFecSolIni.Text = _startDate
        End Set
    End Property


    Private _dateFormat As String
    Public Property dateFormat() As String
        Get
            Return _dateFormat
        End Get
        Set(ByVal value As String)
            _dateFormat = value
            TxtFecSolIni_CalendarExtender.Format = _dateFormat
            TxtFecSolFin_CalendarExtender.Format = _dateFormat
        End Set
    End Property


    Private _endDate As String
    Public Property endDate() As String
        Get
            Return _endDate
        End Get
        Set(ByVal value As String)
            _endDate = value
            TxtFecSolFin.Text = _endDate
        End Set
    End Property

    Private _dataValueField As String
    Public Property DataValueField() As String
        Get
            Return _dataValueField
        End Get
        Set(ByVal value As String)
            _dataValueField = value
        End Set
    End Property

    Private _isFixed As Boolean
    Public Property isFixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(ByVal value As Boolean)
            _isFixed = value
            btnDelete.Visible = Not _isFixed
        End Set
    End Property

    Public Sub FetchValues()
        _startDate = TxtFecSolIni.Text
        _endDate = TxtFecSolFin.Text
    End Sub

    Protected Sub TxtFecSolIni_TextChanged(sender As Object, e As EventArgs) _
        Handles TxtFecSolIni.TextChanged, TxtFecSolFin.TextChanged
        FetchValues()
    End Sub

End Class
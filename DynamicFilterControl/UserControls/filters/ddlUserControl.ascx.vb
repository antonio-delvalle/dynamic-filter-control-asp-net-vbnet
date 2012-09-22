Public Class ddlUserControl
    Inherits System.Web.UI.UserControl

    'Create Delegate to Handle Click event in Default page
    Public Delegate Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event btnPostClk As btnDelete_Click

    Private _DataValueType As ucFiltro.DataValueType
    Public Property DataValueType() As ucFiltro.DataValueType
        Get
            Return _DataValueType
        End Get
        Set(ByVal value As ucFiltro.DataValueType)
            _DataValueType = value
        End Set
    End Property


    Public Property ddlData() As DropDownList
        Get
            Return ucDropDownList
        End Get
        Set(ByVal value As DropDownList)
            ucDropDownList = value
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

    Private _SessionID As String
    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
        End Set
    End Property

    Private _source As Object
    Public Property source() As Object
        Get
            Return _source
        End Get
        Set(ByVal value As Object)
            _source = value
            ddlData.DataSource = _source
        End Set
    End Property

    Private _dataTextField As String
    Public Property DataTextField() As String
        Get
            Return _dataTextField
        End Get
        Set(ByVal value As String)
            _dataTextField = value
            ddlData.DataTextField = _dataTextField
        End Set
    End Property

    Public Sub Bind()
        If _source IsNot Nothing Then
            ddlData.DataBind()
        End If
    End Sub

    Private _dataValueField As String
    Public Property DataValueField() As String
        Get
            Return _dataValueField
        End Get
        Set(ByVal value As String)
            _dataValueField = value
            ddlData.DataValueField = _dataValueField
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

    Private _selectedItem As String
    Public Property selectedItem() As String
        Get
            Return _selectedItem
        End Get
        Set(value As String)
            _selectedItem = value
            ddlData.SelectedItem.Text = _selectedItem
        End Set
    End Property

    Private _selectedValue As String
    Public Property selectedValue() As String
        Get
            Return _selectedValue
        End Get
        Set(value As String)
            _selectedValue = value
            ddlData.SelectedValue = _selectedValue
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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent btnPostClk(sender, e)
    End Sub

    Public Sub FetchValues()
        _selectedItem = ucDropDownList.SelectedItem.Text
        _selectedValue = ucDropDownList.SelectedValue
    End Sub

    Protected Sub ucDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ucDropDownList.SelectedIndexChanged
        FetchValues()
    End Sub

    Public Sub initSelectedItem()
        If ucDropDownList.Items.Count > 0 Then
            ucDropDownList.SelectedIndex = 1
            FetchValues()
        End If
        
    End Sub

    

End Class
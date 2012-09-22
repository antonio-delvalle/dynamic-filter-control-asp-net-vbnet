Public Class rbUserControl
    Inherits System.Web.UI.UserControl

    Public Delegate Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event btnPostClk As btnDelete_Click
    Private _SessionID As String

    Private _DataValueType As ucFiltro.DataValueType
    Public Property DataValueType() As ucFiltro.DataValueType
        Get
            Return _DataValueType
        End Get
        Set(ByVal value As ucFiltro.DataValueType)
            _DataValueType = value
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

    Private _source As Object
    Public Property source() As Object
        Get
            Return _source
        End Get
        Set(ByVal value As Object)
            _source = value
            ucRadioButton.DataSource = _source
        End Set
    End Property

    Private _selectedItem As String
    Public Property selectedItem() As String
        Get
            Return _selectedItem
        End Get
        Set(value As String)
            _selectedItem = value
            ucRadioButton.SelectedItem.Text = _selectedItem
        End Set
    End Property

    Private _selectedValue As String
    Public Property selectedValue() As String
        Get
            Return _selectedValue
        End Get
        Set(value As String)
            _selectedValue = value
            ucRadioButton.SelectedValue = _selectedValue
        End Set
    End Property

    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
        End Set
    End Property

    Private _dataValueField As String
    Public Property DataValueField() As String
        Get
            Return _dataValueField
        End Get
        Set(ByVal value As String)
            _dataValueField = value
            ucRadioButton.DataValueField = _dataValueField
        End Set
    End Property

    Private _dataTextField As String
    Public Property DataTextField() As String
        Get
            Return _dataTextField
        End Get
        Set(ByVal value As String)
            _dataTextField = value
            ucRadioButton.DataTextField = _dataTextField
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

    Public Sub bind()
        If _source IsNot Nothing Then
            ucRadioButton.DataBind()
        End If
    End Sub

    Public Sub FetchValues()
        If ucRadioButton.SelectedItem IsNot Nothing Then
            _selectedItem = ucRadioButton.SelectedItem.Text
            _selectedValue = ucRadioButton.SelectedValue
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent btnPostClk(sender, e)
    End Sub

    Private Sub ucRadioButton_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ucRadioButton.SelectedIndexChanged
        FetchValues()
    End Sub

    Public Sub initSelectedItem()
        If ucRadioButton.Items.Count > 0 Then
            ucRadioButton.SelectedIndex = 0
            FetchValues()
        End If
    End Sub

End Class
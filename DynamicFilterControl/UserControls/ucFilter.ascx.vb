Imports System.Globalization

<Themeable(True)>
Public Class ucFiltro
    Inherits System.Web.UI.UserControl

    ' UC = User Control

    ''' <summary>
    ''' Enum with the accepted controls by the UC
    ''' </summary>
    ''' <remarks></remarks>
    <Flags()> Public Enum AcceptedControls As Integer
        TextBox = 0
        DropDownList = 1
        Calendar = 2
        RadioButton = 3
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private _ddlList As New List(Of String)

    ''' <summary>
    ''' Enum for the type of output (single quotes in output)
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DataValueType As Integer
        IntegerType = 0
        StringType = 1
    End Enum

    ''' <summary>
    ''' Used to redraw the controls
    ''' </summary>
    ''' <remarks></remarks>
    Shared createAgain As Boolean = False

    ''' <summary>
    ''' Friend class instance to save the startup options for each filter
    ''' </summary>
    ''' <remarks></remarks>
    Shared criteriaSelection As New List(Of CriteriaSelection)

    ''' <summary>
    ''' Counter of the number of controls in Session
    ''' </summary>
    ''' <remarks></remarks>
    Shared consSessionID As Integer = 0


    Shared _setSelectionButton As String
    ''' <summary>
    ''' ID of button to retrieve the filter's values.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Property SelectionButton() As String
        Get
            Return _setSelectionButton
        End Get
        Set(ByVal value As String)
            _setSelectionButton = value
        End Set
    End Property

    Private _ucTemplateControlPath As String = "ASP.usercontrols_filters"
    ''' <summary>
    ''' Control prefix
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ucTemplateControlPath() As String
        Get
            Return _ucTemplateControlPath
        End Get
        Set(ByVal value As String)
            _ucTemplateControlPath = value
        End Set
    End Property

    Private _ucPath As String = "~/UserControls/filters/"
    ''' <summary>
    ''' Path to template controls
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ucPath() As String
        Get
            Return _ucPath
        End Get
        Set(ByVal value As String)
            _ucPath = value
        End Set
    End Property

    Public Property dateFormat() As String
        Get
            Return Session("ucFilter_dateFormat")
        End Get
        Set(ByVal value As String)
            Session("ucFilter_dateFormat") = value
        End Set
    End Property

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim ctrl As Control = GetPostBackControl(Me.Page)
        'Verify if PostBack was sent by dropdown
        ' or
        ' if createAgain is true
        ' which means that the call was made while control was active

        If (ctrl IsNot Nothing AndAlso ctrl.ClientID = ddlAddFilter.ClientID) _
                OrElse (ctrl IsNot Nothing AndAlso ctrl.ClientID = _setSelectionButton) _
                    OrElse createAgain Then
            'must be set before calling CreateUserControl
            createAgain = True
            CreateUserControl()
        End If

    End Sub

#Region "AddFilter methods"

    Public Sub AddDropDownFilter(ByVal Name As String, _
                                ByVal DataSource As Object, _
                                ByVal DataTextField As String, _
                                ByVal DataValueField As String, _
                                ByVal DataValueType As DataValueType,
                                ByVal isDefault As Boolean,
                                ByVal isFixed As Boolean,
                                ByVal InitValue As Object
                                )

        '-------------------------------------------------
        'Checar si elemento ya existe en el dropdownlist
        '-------------------------------------------------
        If ddlAddFilter.Items.Count > 1 Then
            Dim li As ListItem = ddlAddFilter.Items.FindByText(Name)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriteriaSelection
        cs.Name = Name
        cs.Type = AcceptedControls.DropDownList
        cs.Source = DataSource
        cs.DataTextField = DataTextField
        cs.DataValueField = DataValueField
        cs.DataValueType = DataValueType
        cs.IsFixed = isFixed
        cs.InitValue = InitValue
        criteriaSelection.Add(cs)

        If isDefault Then

            CreateDropDownList(Name, DataSource, DataTextField, DataValueField, DataValueType, isFixed, InitValue)

        Else
            _ddlList.Add(Name)
        End If

    End Sub

    Public Sub AddCalendarFilter(ByVal Name As String, _
                                ByVal DataValueField As String, _
                                ByVal isDefault As Boolean,
                                ByVal isFixed As Boolean,
                                ByVal isCalendarSingle As Boolean,
                                ByVal initValue As Date
                                )

        '-------------------------------------------------
        'Check if control is already added to the dropdown
        '-------------------------------------------------
        If ddlAddFilter.Items.Count > 1 Then
            Dim li As ListItem = ddlAddFilter.Items.FindByText(Name)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriteriaSelection
        cs.Name = Name
        cs.Type = AcceptedControls.Calendar
        cs.DataValueField = DataValueField
        cs.IsFixed = isFixed

        cs.isCalendarSingle = isCalendarSingle
        cs.InitValue = initValue
        criteriaSelection.Add(cs)

        If dateFormat Is Nothing Then dateFormat = "yyyy-MM-dd"

        If isDefault Then
            CreateCalendar(Name, DataValueField, isCalendarSingle, isFixed, initValue)
        Else
            _ddlList.Add(Name)
        End If

    End Sub

    Public Sub AddRadioButtonFilter(ByVal Name As String, _
                                    ByVal DataSource As Object, _
                                    ByVal DataTextField As String, _
                                    ByVal DataValueField As String, _
                                    ByVal DataValueType As DataValueType,
                                    ByVal isDefault As Boolean,
                                    ByVal isFixed As Boolean,
                                    ByVal initValue As String
                                    )

        '-------------------------------------------------
        'Check if control is already added to the dropdown
        '-------------------------------------------------
        If ddlAddFilter.Items.Count > 1 Then
            Dim li As ListItem = ddlAddFilter.Items.FindByText(Name)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriteriaSelection
        cs.Name = Name
        cs.Type = AcceptedControls.RadioButton
        cs.Source = DataSource
        cs.DataTextField = DataTextField
        cs.DataValueField = DataValueField
        cs.DataValueType = DataValueType
        cs.IsFixed = isFixed
        cs.InitValue = initValue

        criteriaSelection.Add(cs)

        If isDefault Then
            CreateRadioButton(Name, DataSource, DataValueField, DataTextField, DataValueType, isFixed, initValue)
        Else
            _ddlList.Add(Name)
        End If

    End Sub

    Public Sub AddTextboxFilter(ByVal Name As String, _
                                ByVal DataValueField As String, _
                                ByVal DataValueType As DataValueType,
                                ByVal isDefault As Boolean,
                                ByVal isFixed As Boolean,
                                ByVal isTextBoxLike As Boolean,
                                ByVal IsTextBoxRange As Boolean
                                )

        '-------------------------------------------------
        'Check if control is already added to the dropdown
        '-------------------------------------------------
        If ddlAddFilter.Items.Count > 1 Then
            Dim li As ListItem = ddlAddFilter.Items.FindByText(Name)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriteriaSelection
        cs.Name = Name
        cs.Type = AcceptedControls.TextBox
        cs.DataValueField = DataValueField
        cs.DataValueType = DataValueType
        cs.IsFixed = isFixed

        cs.isTextBoxLike = isTextBoxLike
        cs.IsTextBoxRange = IsTextBoxRange

        criteriaSelection.Add(cs)

        If isDefault Then
            CreateTextbox(Name, DataValueField, DataValueType, isTextBoxLike, IsTextBoxRange, isFixed)
        Else
            _ddlList.Add(Name)
        End If

    End Sub
#End Region

    Public Sub LoadDDL()
        For Each item As String In _ddlList
            ddlAddFilter.Items.Add(item)
        Next
        SortDDL(ddlAddFilter)
    End Sub


#Region "Click Event of the Controls' Delete Button"

    Protected Sub ucDropDownList_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucDropDownList As ddlUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, ddlUserControl)
        removeControl(ucDropDownList)
    End Sub


    Protected Sub ucTextBox_onPostClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucTextBox As txtUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, txtUserControl)
        removeControl(ucTextBox)
    End Sub


    Protected Sub ucCalendar_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucCalendar As cldUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, cldUserControl)
        removeControl(ucCalendar)
    End Sub

    Protected Sub ucRadioButton_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucRadioButton As rbUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, rbUserControl)
        removeControl(ucRadioButton)
    End Sub
#End Region

    ''' <summary>
    ''' Remove a control from the PlaceHolder and from Session. Puts back item to ddlAddFilter
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub removeControl(ByVal obj As Object)
        PlaceHolder1.Controls.Remove(obj)
        Session.Remove(obj.SessionID)
        ddlAddFilter.Items.Add(obj.labelText)
        SortDDL(ddlAddFilter)
    End Sub


    ''' <summary>
    ''' Creates a new UC of the Textbox type
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="isTextBoxLike"></param>
    ''' <remarks></remarks>
    Private Sub CreateTextbox( _
                             ByVal name As String, _
                             ByVal DataValueField As String, _
                             ByVal DataValueType As DataValueType, _
                             ByVal isTextBoxLike As Boolean,
                             ByVal isTextBoxRange As Boolean,
                             ByVal isFixed As Boolean)
        '-------------------------------------------
        'Instance of UC
        '-------------------------------------------
        Dim ucTextBox As txtUserControl = LoadControl(ucPath & "txtUserControl.ascx")

        '-------------------------------------------
        'Set properties
        '-------------------------------------------
        ucTextBox.labelText = name
        ucTextBox.Text1 = ""
        ucTextBox.Text2 = ""
        ucTextBox.SessionID = ucTextBox.TemplateControl.ToString & consSessionID.ToString
        ucTextBox.DataValueField = DataValueField
        ucTextBox.isTextBoxLike = isTextBoxLike
        ucTextBox.isTextBoxRange = isTextBoxRange
        ucTextBox.DataValueType = DataValueType
        ucTextBox.ID = ucTextBox.SessionID
        ucTextBox.isFixed = isFixed

        '-------------------------------------------
        ' Add control to PlaceHolder and Session
        '-------------------------------------------
        addControl(ucTextBox, ucTextBox.SessionID)

    End Sub

    ''' <summary>
    ''' Creates a new UC of the DropDownList type
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="source"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateDropDownList( _
                                  ByVal name As String, _
                                  ByVal source As Object, _
                                  ByVal DataTextField As String, _
                                  ByVal DataValueField As String, _
                                  ByVal DataValueType As DataValueType,
                                  ByVal isFixed As Boolean,
                                  ByVal initValue As String)

        '-------------------------------------------
        'Instance of UC
        '-------------------------------------------
        Dim ucDropDownList As ddlUserControl = LoadControl(ucPath & "ddlUserControl.ascx")

        '-------------------------------------------
        'Set properties
        '-------------------------------------------
        ucDropDownList.labelText = name
        ucDropDownList.source = source
        ucDropDownList.DataValueField = DataValueField
        ucDropDownList.DataTextField = DataTextField
        ucDropDownList.Bind()
        ucDropDownList.SessionID = ucDropDownList.TemplateControl.ToString & consSessionID.ToString
        ucDropDownList.ID = ucDropDownList.SessionID
        ucDropDownList.DataValueType = DataValueType
        ucDropDownList.isFixed = isFixed
        ucDropDownList.selectedValue = initValue

        If Not initValue Is Nothing AndAlso initValue IsNot String.Empty Then
            ucDropDownList.selectedValue = initValue
        End If

        '-------------------------------------------
        ' Add control to PlaceHolder and Session
        '-------------------------------------------
        addControl(ucDropDownList, ucDropDownList.SessionID)
    End Sub

    ''' <summary>
    ''' Creates a new UC of the Calendar type
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="isCalendarSingle"></param>
    ''' <remarks></remarks>

    Private Sub CreateCalendar(ByVal name As String,
                                    ByVal dataValueField As String,
                                    ByVal isCalendarSingle As Boolean,
                                    ByVal isFixed As Boolean,
                                    ByVal initValue As Date)

        '-------------------------------------------
        'Instance of UC
        '-------------------------------------------
        Dim ucCalendar As cldUserControl = LoadControl(ucPath & "cldUserControl.ascx")

        '----------------------------------------------
        'Set properties
        '----------------------------------------------
        ucCalendar.labelText = name
        ucCalendar.SessionID = ucCalendar.TemplateControl.ToString & consSessionID.ToString
        ucCalendar.StartDate = ""
        ucCalendar.endDate = ""
        ucCalendar.DataValueField = dataValueField
        ucCalendar.isCalendarSingle = isCalendarSingle
        ucCalendar.ID = ucCalendar.SessionID
        ucCalendar.isFixed = isFixed
        ucCalendar.dateFormat = dateFormat

        If Not initValue = Date.MinValue Then
            '------------------------------------------------
            ' If initValueDate is newer than today, set it as end date
            '------------------------------------------------
            If initValue > Today.Date Then
                ucCalendar.StartDate = Date.Today.ToString(dateFormat)
                ucCalendar.endDate = initValue.ToString(dateFormat)
            ElseIf initValue < Today.Date Then
                '------------------------------------------------
                ' If initValueDate is older than today, set it as start date
                '------------------------------------------------
                ucCalendar.StartDate = initValue.ToString(dateFormat)
                ucCalendar.endDate = Date.Today.ToString(dateFormat)
            Else
                '------------------------------------------------
                ' If they are equal, set both to today
                '------------------------------------------------
                ucCalendar.StartDate = Date.Today.ToString(dateFormat)
                ucCalendar.endDate = Date.Today.ToString(dateFormat)
            End If
        Else
            ucCalendar.StartDate = Date.Today.ToString(dateFormat)
            ucCalendar.endDate = Date.Today.ToString(dateFormat)
        End If

        '-------------------------------------------
        ' Add control to PlaceHolder and Session
        '-------------------------------------------
        addControl(ucCalendar, ucCalendar.SessionID)

    End Sub

    ''' <summary>
    ''' Creates a new UC of the RadioButtonList type
    ''' </summary>
    ''' <param name="Nombre"></param>
    ''' <param name="Source"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateRadioButton( _
                                 ByVal Nombre As String, _
                                 ByVal Source As Object, _
                                 ByVal DataValueField As String, _
                                 ByVal DataTextField As String, _
                                 ByVal DataValueType As DataValueType,
                                 ByVal isFixed As Boolean,
                                 ByVal initVAlue As String)

        '-------------------------------------------
        'Instance of UC
        '-------------------------------------------
        Dim ucRadioButton As rbUserControl = LoadControl(ucPath & "rbUserControl.ascx")

        '-------------------------------------------
        'Set properties
        '-------------------------------------------
        ucRadioButton.labelText = Nombre
        ucRadioButton.source = Source
        ucRadioButton.DataValueField = DataValueField
        ucRadioButton.DataTextField = DataTextField
        ucRadioButton.DataValueType = DataValueType
        ucRadioButton.bind()
        ucRadioButton.initSelectedItem()
        ucRadioButton.SessionID = ucRadioButton.TemplateControl.ToString & consSessionID.ToString
        ucRadioButton.ID = ucRadioButton.SessionID
        ucRadioButton.isFixed = isFixed
        ucRadioButton.selectedValue = initVAlue

        '-------------------------------------------
        ' Add control to PlaceHolder and Session
        '-------------------------------------------
        addControl(ucRadioButton, ucRadioButton.SessionID)
    End Sub

    ''' <summary>
    ''' Add control to PlaceHolder and to Session
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="SessionID"></param>
    ''' <remarks></remarks>
    Private Sub addControl(ByVal obj As Control, ByVal SessionID As String)

        ' Add control to placeholder
        PlaceHolder1.Controls.Add(obj)

        'Add control to Session
        Session.Add(SessionID, obj)

        'Increase control counter in Session
        consSessionID += 1

        ' Set createAgain = true
        createAgain = True
    End Sub

    ''' <summary>
    ''' Valid date
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function bValidDate(ByVal str As String) As Boolean
        Try

            DateTime.ParseExact(str, dateFormat, System.Globalization.CultureInfo.InvariantCulture)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function dGetDate(ByVal str As String, dateFormat As String) As Date
        Dim dDate As Date
        Try
            dDate = DateTime.ParseExact(str, dateFormat, System.Globalization.CultureInfo.InvariantCulture)
        Catch ex As Exception
            dDate = #12:00:00 AM#
        End Try

        Return dDate

    End Function

    ''' <summary>
    ''' Verify if startdate is older than enddate
    ''' </summary>
    ''' <param name="sStartDate"></param>
    ''' <param name="sEndDate"></param>
    ''' <remarks></remarks>
    Private Sub VerifyDates(ByVal sStartDate As Date, ByVal sEndDate As Date)
        Dim returnValue As Boolean = True

        '--------------------------------------------------
        ' Verify range dates are valid
        '--------------------------------------------------
        If sStartDate.Year = 1 OrElse sEndDate.Year = 1 OrElse sStartDate > sEndDate Then Throw New ApplicationException("Invalid date range.")


    End Sub

    ''' <summary>
    ''' Prepare the value to be extracted
    ''' </summary>
    ''' <returns>List (Of String)</returns>
    ''' <remarks></remarks>
    Public Function getFilterSelection() As List(Of String)

        Dim SelValues As New List(Of String)

        Try
            If PlaceHolder1 IsNot Nothing Then
                For Each Control As Control In PlaceHolder1.Controls
                    Select Case Control.TemplateControl.ToString
                        Case ucTemplateControlPath & "_txtusercontrol_ascx"
                            Dim ucTextBox As txtUserControl = TryCast(Control, txtUserControl)
                            ucTextBox.FetchValues()
                            Dim currentSelValue As String = String.Empty

                            If ucTextBox.Text1 IsNot String.Empty Then
                                If ucTextBox.isTextBoxLike Then
                                    If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                        currentSelValue = ucTextBox.DataValueField & " LIKE %" & ucTextBox.Text1 & "%"
                                    Else
                                        currentSelValue = ucTextBox.DataValueField & " LIKE '%" & ucTextBox.Text1 & "%'"
                                    End If

                                Else
                                    If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                        currentSelValue = ucTextBox.DataValueField & "=" & ucTextBox.Text1 & ""
                                    Else
                                        currentSelValue = ucTextBox.DataValueField & "='" & ucTextBox.Text1 & "'"
                                    End If
                                End If

                                If ucTextBox.isTextBoxRange Then
                                    If ucTextBox.Text2 IsNot String.Empty Then
                                        If ucTextBox.isTextBoxLike Then
                                            currentSelValue = ucTextBox.DataValueField & " BETWEEN '%" & ucTextBox.Text1 & "%' AND '%" & ucTextBox.Text2 & "%'"
                                        Else
                                            If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                                currentSelValue = ucTextBox.DataValueField & " BETWEEN " & ucTextBox.Text1 & " AND " & ucTextBox.Text2
                                            Else
                                                currentSelValue = ucTextBox.DataValueField & " BETWEEN '" & ucTextBox.Text1 & "' AND '" & ucTextBox.Text2 & "'"
                                            End If

                                        End If
                                    End If
                                End If

                                SelValues.Add(currentSelValue)

                            End If

                        Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                            Dim ucDropDownList As ddlUserControl = TryCast(Control, ddlUserControl)
                            ucDropDownList.FetchValues()
                            If ucDropDownList.DataValueType = DataValueType.IntegerType Then
                                SelValues.Add(ucDropDownList.DataValueField & "=" & ucDropDownList.selectedValue)
                            Else
                                SelValues.Add(ucDropDownList.DataValueField & "='" & ucDropDownList.selectedValue & "'")
                            End If
                        Case ucTemplateControlPath & "_cldusercontrol_ascx"
                            Dim ucCalendar As cldUserControl = TryCast(Control, cldUserControl)
                            ucCalendar.FetchValues()

                            If ucCalendar.isCalendarSingle Then
                                If ucCalendar.StartDate.Length > 0 Then

                                    Dim dStartDate As Date = dGetDate(ucCalendar.StartDate, dateFormat)
                                    If dStartDate.Year > 1 Then
                                        SelValues.Add(ucCalendar.DataValueField & " = '" & dStartDate.ToString("yyyy-MM-dd") & "'")
                                    Else
                                        Throw New ApplicationException("Invalid date")
                                    End If

                                End If

                            Else

                                If ucCalendar.StartDate.Length > 0 AndAlso ucCalendar.endDate.Length > 0 Then

                                    Dim dStartDate As Date = dGetDate(ucCalendar.StartDate, dateFormat)
                                    Dim dEndDate As Date = dGetDate(ucCalendar.endDate, dateFormat)

                                    VerifyDates(dStartDate, dEndDate)

                                    SelValues.Add(
                                                    ucCalendar.DataValueField & " BETWEEN '" & _
                                                    dStartDate.ToString("yyyy-MM-dd") & _
                                                    "' AND '" & _
                                                    dEndDate.ToString("yyyy-MM-dd") & "'")

                                End If
                            End If
                        Case ucTemplateControlPath & "_rbusercontrol_ascx"
                            Dim ucRadioButton As rbUserControl = TryCast(Control, rbUserControl)
                            ucRadioButton.FetchValues()
                            If ucRadioButton.selectedValue IsNot Nothing AndAlso ucRadioButton.selectedValue IsNot String.Empty Then
                                If ucRadioButton.DataValueType = DataValueType.IntegerType Then
                                    SelValues.Add(ucRadioButton.DataValueField & "=" & ucRadioButton.selectedValue)
                                Else
                                    SelValues.Add(ucRadioButton.DataValueField & "='" & ucRadioButton.selectedValue & "'")
                                End If
                            End If
                    End Select
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return SelValues
    End Function

    ''' <summary>
    ''' Get the PostBack control
    ''' </summary>
    ''' <param name="thePage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPostBackControl(ByVal thePage As Page) As Control
        Dim myControl As Control = Nothing
        Dim ctrlName As String = thePage.Request.Params.Get("__EVENTTARGET")
        If ((ctrlName IsNot Nothing) And (ctrlName <> String.Empty)) Then
            myControl = thePage.FindControl(ctrlName)
        Else
            For Each Item As String In thePage.Request.Form
                Dim c As Control = thePage.FindControl(Item)
                If (TypeOf (c) Is System.Web.UI.WebControls.DropDownList) Or (TypeOf (c) Is Button) Then
                    myControl = c
                End If
            Next

        End If
        Return myControl
    End Function

    ''' <summary>
    ''' createAgain se estableció como true en el método Pre_init cuando se seleccionó del dropdown una opción
    ''' este campo se utiliza para verificar si el UC está en la página antes del llamado, si es así, se agrega
    ''' al Control de Jerarquías.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CreateUserControl()
        Try
            If createAgain AndAlso PlaceHolder1 IsNot Nothing Then
                If Session.Count > 0 Then
                    PlaceHolder1.Controls.Clear()
                    For i As Integer = 0 To Session.Count - 1
                        If Session(i) IsNot Nothing Then
                            Select Case Session(i).ToString()
                                Case ucTemplateControlPath & "_txtusercontrol_ascx"
                                    Dim ucTextBox As txtUserControl = TryCast(LoadControl(ucPath & "txtUserControl.ascx"), txtUserControl)

                                    ucTextBox.ID = DirectCast(Session(i), txtUserControl).ID
                                    ucTextBox.SessionID = DirectCast(Session(i), txtUserControl).SessionID
                                    ucTextBox.labelText = DirectCast(Session(i), txtUserControl).labelText
                                    ucTextBox.isTextBoxLike = DirectCast(Session(i), txtUserControl).isTextBoxLike
                                    ucTextBox.isTextBoxRange = DirectCast(Session(i), txtUserControl).isTextBoxRange
                                    ucTextBox.DataValueType = DirectCast(Session(i), txtUserControl).DataValueType
                                    ucTextBox.DataValueField = DirectCast(Session(i), txtUserControl).DataValueField
                                    ucTextBox.isFixed = DirectCast(Session(i), txtUserControl).isFixed

                                    AddHandler ucTextBox.btnPostClk, AddressOf ucTextBox_onPostClk
                                    PlaceHolder1.Controls.Add(ucTextBox)
                                    Exit Select
                                Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                                    Dim ucDropDownList As ddlUserControl = TryCast(LoadControl(ucPath & "ddlUserControl.ascx"), ddlUserControl)

                                    ucDropDownList.ID = DirectCast(Session(i), ddlUserControl).ID
                                    ucDropDownList.SessionID = DirectCast(Session(i), ddlUserControl).SessionID
                                    ucDropDownList.source = DirectCast(Session(i), ddlUserControl).source
                                    ucDropDownList.DataTextField = DirectCast(Session(i), ddlUserControl).DataTextField
                                    ucDropDownList.DataValueField = DirectCast(Session(i), ddlUserControl).DataValueField
                                    ucDropDownList.Bind()
                                    ucDropDownList.labelText = DirectCast(Session(i), ddlUserControl).labelText
                                    ucDropDownList.DataValueType = DirectCast(Session(i), ddlUserControl).DataValueType
                                    ucDropDownList.isFixed = DirectCast(Session(i), ddlUserControl).isFixed
                                    ucDropDownList.selectedValue = DirectCast(Session(i), ddlUserControl).selectedValue

                                    AddHandler ucDropDownList.btnPostClk, AddressOf ucDropDownList_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucDropDownList)
                                    Exit Select
                                Case ucTemplateControlPath & "_cldusercontrol_ascx"
                                    Dim ucCalendar As cldUserControl = TryCast(LoadControl(ucPath & "cldUserControl.ascx"), cldUserControl)

                                    ucCalendar.ID = DirectCast(Session(i), cldUserControl).ID
                                    ucCalendar.SessionID = DirectCast(Session(i), cldUserControl).SessionID
                                    ucCalendar.endDate = DirectCast(Session(i), cldUserControl).endDate
                                    ucCalendar.StartDate = DirectCast(Session(i), cldUserControl).StartDate

                                    ucCalendar.labelText = DirectCast(Session(i), cldUserControl).labelText
                                    ucCalendar.isCalendarSingle = DirectCast(Session(i), cldUserControl).isCalendarSingle
                                    ucCalendar.DataValueField = DirectCast(Session(i), cldUserControl).DataValueField
                                    ucCalendar.isFixed = DirectCast(Session(i), cldUserControl).isFixed
                                    ucCalendar.dateFormat = DirectCast(Session(i), cldUserControl).dateFormat

                                    AddHandler ucCalendar.btnPostClk, AddressOf ucCalendar_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucCalendar)
                                    Exit Select
                                Case ucTemplateControlPath & "_rbusercontrol_ascx"
                                    Dim ucRadioButton As rbUserControl = TryCast(LoadControl(ucPath & "rbUserControl.ascx"), rbUserControl)

                                    ucRadioButton.ID = DirectCast(Session(i), rbUserControl).ID
                                    ucRadioButton.SessionID = DirectCast(Session(i), rbUserControl).SessionID
                                    ucRadioButton.source = DirectCast(Session(i), rbUserControl).source
                                    ucRadioButton.DataValueField = DirectCast(Session(i), rbUserControl).DataValueField
                                    ucRadioButton.DataTextField = DirectCast(Session(i), rbUserControl).DataTextField
                                    ucRadioButton.bind()
                                    ucRadioButton.labelText = DirectCast(Session(i), rbUserControl).labelText
                                    ucRadioButton.DataValueType = DirectCast(Session(i), rbUserControl).DataValueType
                                    ucRadioButton.isFixed = DirectCast(Session(i), rbUserControl).isFixed
                                    ucRadioButton.selectedValue = DirectCast(Session(i), rbUserControl).selectedValue

                                    AddHandler ucRadioButton.btnPostClk, AddressOf ucRadioButton_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucRadioButton)
                                    Exit Select
                            End Select
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Evento SelectedIndexChanged del DropDownList que contiene las opciones
    ''' Supervisa el agregado de controles al PlaceHolder
    ''' apoyándose en la clase auxiliar criterioSeleccion que guarda la info de configuración.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlAddFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddFilter.SelectedIndexChanged
        Dim result As CriteriaSelection = criteriaSelection.Find(AddressOf FindNombre)
        If result IsNot Nothing Then
            Select Case result.Type
                Case AcceptedControls.TextBox
                    CreateTextbox(result.Name, result.DataValueField, result.DataValueType, result.isTextBoxLike, result.IsTextBoxRange, result.IsFixed)
                Case AcceptedControls.DropDownList
                    CreateDropDownList(result.Name, result.Source, result.DataTextField, result.DataValueField, result.DataValueType, result.IsFixed, result.InitValue)
                Case AcceptedControls.Calendar
                    CreateCalendar(result.Name, result.DataValueField, result.isCalendarSingle, result.IsFixed, result.InitValue)
                Case AcceptedControls.RadioButton
                    CreateRadioButton(result.Name, result.Source, result.DataValueField, result.DataTextField, result.DataValueType, result.IsFixed, result.InitValue)
            End Select
            ddlAddFilter.Items.Remove(ddlAddFilter.SelectedItem.Text)
            SortDDL(ddlAddFilter)
        End If

    End Sub

    ''' <summary>
    ''' Funcion auxiliar para recorrer en bucle la Lista criteriosSeleccion
    ''' </summary>
    ''' <param name="cs"></param>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Private Function FindNombre(ByVal cs As CriteriaSelection) As Boolean
        If cs.Name = ddlAddFilter.SelectedItem.Text Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Alphabetically order the DropDownList
    ''' </summary>
    ''' <param name="objDDL"></param>
    ''' <remarks></remarks>
    Private Sub SortDDL(ByRef objDDL As DropDownList)
        Dim textList As ArrayList = New ArrayList()
        Dim valueList As ArrayList = New ArrayList()

        For Each li As ListItem In objDDL.Items

            textList.Add(li.Text)
        Next
        textList.Sort()

        For Each item As Object In textList

            Dim value As String = objDDL.Items.FindByText(item.ToString()).Value
            valueList.Add(value)
        Next
        objDDL.Items.Clear()

        For i As Integer = 0 To textList.Count - 1 Step 1
            Dim objItem As ListItem = New ListItem(textList(i).ToString(), valueList(i).ToString())
            objDDL.Items.Add(objItem)
        Next
    End Sub

    Public Sub resetSession()
        If PlaceHolder1 IsNot Nothing AndAlso Session.Count > 0 Then
            For Each Control As Control In PlaceHolder1.Controls
                Select Case Control.TemplateControl.ToString
                    Case ucTemplateControlPath & "_txtusercontrol_ascx"
                        Dim ucTextBox As txtUserControl = TryCast(Control, txtUserControl)
                        Session.Remove(ucTextBox.SessionID)

                    Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                        Dim ucDropDownList As ddlUserControl = TryCast(Control, ddlUserControl)
                        Session.Remove(ucDropDownList.SessionID)

                    Case ucTemplateControlPath & "_cldusercontrol_ascx"
                        Dim ucCalendar As cldUserControl = TryCast(Control, cldUserControl)
                        Session.Remove(ucCalendar.SessionID)

                    Case ucTemplateControlPath & "_rbusercontrol_ascx"
                        Dim ucRadioButton As rbUserControl = TryCast(Control, rbUserControl)
                        Session.Remove(ucRadioButton.SessionID)
                End Select
            Next
        End If

        PlaceHolder1.Controls.Clear()
        consSessionID = 0

        Dim toRemove As New List(Of String)
        For Each item As String In Session.Keys
            If item.Contains(_ucTemplateControlPath) Then
                toRemove.Add(item)
            End If
        Next

        For Each item In toRemove
            Session.Remove(item)
        Next

    End Sub

End Class

''' <summary>
''' Friend class to store config options for filters
''' </summary>
''' <remarks></remarks>
Friend Class CriteriaSelection

    Property DataValueType As ucFiltro.DataValueType

    Private _Name As String = ""
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Source As Object = Nothing
    Public Property Source() As Object
        Get
            Return _Source
        End Get
        Set(ByVal value As Object)
            _Source = value

        End Set
    End Property
    Private _Type As ucFiltro.AcceptedControls
    Public Property Type As ucFiltro.AcceptedControls
        Get
            Return _Type
        End Get
        Set(ByVal value As ucFiltro.AcceptedControls)
            _Type = value
        End Set
    End Property

    Private _DataTextField As String
    Public Property DataTextField() As String
        Get
            Return _DataTextField
        End Get
        Set(ByVal value As String)
            _DataTextField = value
        End Set
    End Property

    Private _DataValueField As String
    Public Property DataValueField() As String
        Get
            Return _DataValueField
        End Get
        Set(ByVal value As String)
            _DataValueField = value
        End Set
    End Property
    Private _isCalendarSingle As Boolean
    Public Property isCalendarSingle() As Boolean
        Get
            Return _isCalendarSingle
        End Get
        Set(ByVal value As Boolean)
            _isCalendarSingle = value
        End Set
    End Property
    Private _isTextBoxLike As Boolean
    Public Property isTextBoxLike As Boolean
        Get
            Return _isTextBoxLike
        End Get
        Set(ByVal value As Boolean)
            _isTextBoxLike = value
        End Set
    End Property
    Private _isTextBoxRange As Boolean
    Public Property IsTextBoxRange() As Boolean
        Get
            Return _isTextBoxRange
        End Get
        Set(ByVal value As Boolean)
            _isTextBoxRange = value
        End Set
    End Property
    Private _isFixed As Boolean
    Public Property IsFixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(ByVal value As Boolean)
            _isFixed = value
        End Set
    End Property
    Private _initValue As Object
    Public Property InitValue() As Object
        Get
            Return _initValue
        End Get
        Set(ByVal value As Object)
            _initValue = value
        End Set
    End Property

End Class
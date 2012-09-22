Imports System.Data.SqlClient


Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            '------------------------------------------
            ' Load Dropdown options based on the filters declared
            '------------------------------------------
            ucFilter1.LoadDDL()
        End If

    End Sub

    Private Sub WebForm1_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not IsPostBack Then

            Dim ds As New DataSet
            Dim sql As String = String.Empty

            '-----------------------------------------
            '***Connection
            '***Important, modify this connection string to reflect your current DB settings.
            '***Using AdventureWorks2008R2 database as example.
            '-----------------------------------------
            Dim connStr As String = "Data Source=.\SQLEXPRESS;Initial Catalog=AdventureWorks2008R2;Integrated Security=SSPI;"
            Dim myConnection As New SqlConnection(connStr)
            If myConnection.State = ConnectionState.Closed Then myConnection.Open()

            '-----------------------------------------
            'Retreive Catalogs
            '-----------------------------------------
            sql = "SELECT [ShipMethodID], [Name] FROM [Purchasing].[ShipMethod]"
            Dim da As New SqlDataAdapter(sql, myConnection)
            da.Fill(ds, "ShipMethod")

            sql = "SELECT [TerritoryID], [Name] FROM [Sales].[SalesTerritory]"
            da = New SqlDataAdapter(sql, myConnection)
            da.Fill(ds, "SalesTerritory")

            myConnection.Close()

            '-----------------------------------------
            'Create a catalog manually
            '-----------------------------------------
            Dim tTable As New DataTable("Status")
            tTable.Columns.Add("Status", Type.GetType("System.Int32"))
            tTable.Columns.Add("Description", Type.GetType("System.String"))

            Dim row As DataRow

            row = tTable.NewRow

            row("Status") = 1
            row("Description") = "In Process"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("Status") = 2
            row("Description") = "Approved"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("Status") = 3
            row("Description") = "Back ordered"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("Status") = 4
            row("Description") = "Rejected"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("Status") = 5
            row("Description") = "Shipped"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("Status") = 6
            row("Description") = "Canceled"
            tTable.Rows.Add(row)

            ds.Tables.Add(tTable)

            tTable = New DataTable("OnlineOrder")
            tTable.Columns.Add("OnlineOrderFlag", Type.GetType("System.Int32"))
            tTable.Columns.Add("Description", Type.GetType("System.String"))

            row = tTable.NewRow
            row("OnlineOrderFlag") = 1
            row("Description") = "Yes"
            tTable.Rows.Add(row)

            row = tTable.NewRow
            row("OnlineOrderFlag") = 0
            row("Description") = "No"
            tTable.Rows.Add(row)

            ds.Tables.Add(tTable)

            '--------------------------------------------------------------------------------'
            ' * ucFilter *
            '--------------------------------------------------------------------------------'
            ucFilter1.resetSession()

            'This ID prefix of each control, it's the path separated by underscore starting with ASP. all in lowercase.
            ucFilter1.ucTemplateControlPath = "ASP.usercontrols_filters"
            ucFilter1.ucPath = "~/UserControls/filters/"
            ucFilter1.dateFormat = "MM/dd/yyyy"
            '**Try any of these
            'ucFilter1.dateFormat = "yyyy-MM-dd"
            'ucFilter1.dateFormat = "dd/MM/yyyy"
            'ucFilter1.dateFormat = "d, MMM yy"

            ucFilter1.AddCalendarFilter("Date", "OrderDate", True, True, False, New Date(2008, 7, 31))
            ucFilter1.AddDropDownFilter("Status", ds.Tables("Status"), "Description", "Status", ucFiltro.DataValueType.IntegerType, True, False, 5)
            ucFilter1.AddDropDownFilter("Ship Method", ds.Tables("ShipMethod"), "Name", "ShipMethodID", ucFiltro.DataValueType.IntegerType, False, False, Nothing)
            ucFilter1.AddDropDownFilter("Sales Territory", ds.Tables("SalesTerritory"), "Name", "TerritoryID", ucFiltro.DataValueType.IntegerType, False, False, Nothing)
            ucFilter1.AddRadioButtonFilter("Online Order", ds.Tables("OnlineOrder"), "Description", "OnlineOrderFlag", ucFiltro.DataValueType.IntegerType, True, False, 1)

            '***Uncomment the following if you want a free textbox input as part of the condition string, beware that if uncommented
            '***and example is run, it will throw an exception. It is good if you are testing/debugging and wonder what it does.

            'ucFilter1.AddTextboxFilter("Name", "ValueField", ucFiltro.DataValueType.StringType, True, False, False, False)
            ucFilter1.SelectionButton = Me.btnFilter.ClientID

        End If
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim strCondition As String = String.Empty

        Try

            Dim list As List(Of String) = ucFilter1.getFilterSelection
            For Each listItem As String In list
                strCondition &= listItem & " AND "
            Next

            '----------------------------------------------
            'Connection
            '----------------------------------------------
            Dim connStr As String = "Data Source=.\SQLEXPRESS;Initial Catalog=AdventureWorks2008R2;Integrated Security=SSPI;"
            Dim myConnection As New SqlConnection(connStr)
            If myConnection.State = ConnectionState.Closed Then myConnection.Open()

            '----------------------------------------------
            'Query
            '----------------------------------------------
            Dim sql As String = "SELECT [SalesOrderID], [OrderDate], [DueDate], [ShipDate], [SalesOrderNumber], [PurchaseOrderNumber], [AccountNumber], [CreditCardApprovalCode], [SubTotal], [TaxAmt], [Freight], [TotalDue] FROM [AdventureWorks2008R2].[Sales].[SalesOrderHeader]"

            Dim da As SqlDataAdapter = Nothing
            Dim dt As New DataTable

            If strCondition IsNot String.Empty Then
                strCondition = strCondition.Remove(strCondition.LastIndexOf(" AND "))
                sql &= " WHERE " & strCondition
            End If

            da = New SqlDataAdapter(sql, myConnection)
            da.Fill(dt)

            myConnection.Close()

            '----------------------------------------------
            'Bind Gridview
            '----------------------------------------------
            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.Visible = True

            lblError.Visible = False
        Catch ex As ApplicationException
            lblError.Visible = True
            lblError.Text = ex.Message
            GridView1.Visible = False
        End Try
    End Sub
End Class
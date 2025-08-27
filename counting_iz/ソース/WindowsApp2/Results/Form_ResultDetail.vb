Imports Common
Imports Common.ClsFunction
Public Class Form_ResultDetail
  '新規:１ 、変更:２
  Public InputMode As Integer

  'Public additionDateTxTValue As String
  'Public additionTimeTxTValue As String
  'Public terminalNumberTxTValue As String
  'Public callCodeTxTValue As String
  'Public itemNoTxTValue As String
  'Public itemNameTxTValue As String

  'Public packingTxTValue As Decimal
  'Public packingUnitTxTValue As String
  'Public packing1WeightTxTValue As Decimal
  'Public packing1WeightUnitTxTValue As String
  'Public packing2WeightTxTValue As Decimal
  'Public packing2WeightUnitTxTValue As String
  'Public packing2MultiplicationTxTValue As Decimal
  'Public packing1NumberTxTValue As String
  'Public packing1NameTxTValue As String
  'Public packing2NumberTxTValue As String
  'Public packing2NameTxTValue As String

  'Public free1NumberTxTValue As Integer
  'Public free1NameTxTValue As String
  'Public free2NumberTxTValue As Integer
  'Public free2NameTxTValue As String
  'Public free3NumberTxTValue As Integer
  'Public free3NameTxTValue As String
  'Public free4NumberTxTValue As Integer
  'Public free4NameTxTValue As String
  'Public free5NumberTxTValue As Integer
  'Public free5NameTxTValue As String

  'Public manufacturerCodeTxTValue As Integer
  'Public manufacturerNameTxTValue As String
  'Public staffNumberTxTValue As Integer
  'Public staffNameTxTValue As String
  'Public lot1TxTValue As String
  'Public lot2TxTValue As String
  'Public classificationTxTValue As String

  'Public weightTxTValue As Decimal
  'Public weightUnitTxTValue As String
  'Public grossWeightTxTValue As Decimal
  'Public grossWeightUnitTxTValue As String
  'Public temperatureTxTValue As String
  'Public temperatureUnitTxTValue As String

  'Public processingDateTxTValue As String
  'Public processingTimeTxTValue As String
  'Public effectiveDateTxTValue As String
  'Public effectiveTimeTxTValue As String
  'Public workOrderNumberTxTValue As String
  'Public detailNumberTxTValue As String
  'Public instructionQtyTxTValue As Integer
  'Public actualQtyTxTValue As Integer
  'Public workOrderNameTxTValue As String

  Public additionDateTxTValue As String
  Public additionTimeTxTValue As String
  Public terminalNumberTxTValue As String
  Public callCodeTxTValue As String
  Public itemNoTxTValue As String
  Public itemNameTxTValue As String

  Public packingTxTValue As String
  Public packingUnitTxTValue As String
  Public packing1WeightTxTValue As String
  Public packing1WeightUnitTxTValue As String
  Public packing2WeightTxTValue As String
  Public packing2WeightUnitTxTValue As String
  Public packing2MultiplicationTxTValue As String
  Public packing1NumberTxTValue As String
  Public packing1NameTxTValue As String
  Public packing2NumberTxTValue As String
  Public packing2NameTxTValue As String

  Public free1NumberTxTValue As String
  Public free1NameTxTValue As String
  Public free2NumberTxTValue As String
  Public free2NameTxTValue As String
  Public free3NumberTxTValue As String
  Public free3NameTxTValue As String
  Public free4NumberTxTValue As String
  Public free4NameTxTValue As String
  Public free5NumberTxTValue As String
  Public free5NameTxTValue As String

  Public manufacturerCodeTxTValue As String
  Public manufacturerNameTxTValue As String
  Public staffNumberTxTValue As String
  Public staffNameTxTValue As String
  Public lot1TxTValue As String
  Public lot2TxTValue As String
  Public classificationTxTValue As String

  Public weightTxTValue As String
  Public weightUnitTxTValue As String
  Public grossWeightTxTValue As String
  Public grossWeightUnitTxTValue As String
  Public temperatureTxTValue As String
  Public temperatureUnitTxTValue As String

  Public processingDateTxTValue As String
  Public processingTimeTxTValue As String
  Public effectiveDateTxTValue As String
  Public effectiveTimeTxTValue As String
  Public workOrderNumberTxTValue As String
  Public detailNumberTxTValue As String
  Public instructionQtyTxTValue As String
  Public actualQtyTxTValue As String
  Public workOrderNameTxTValue As String

  ReadOnly tmpDb As New ClsSqlServer
  ReadOnly tmpDt As New DataTable
  ' SQLサーバー操作オブジェクト
  Private _SqlServer As ClsSqlServer
  Private ReadOnly Property SqlServer As ClsSqlServer
    Get
      If _SqlServer Is Nothing Then
        _SqlServer = New ClsSqlServer
      End If
      Return _SqlServer
    End Get
  End Property
  Private Sub Form_ResultDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ' フォームの最大化ボタンを無効にする
    MaximizeBox = False

    ' アセンブリの最終更新日時を取得し、フォームのタイトルに表示するテキストを設定
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "実績詳細" & " ( " & updateTime & " ) "

    Me.KeyPreview = True

    FormBorderStyle = FormBorderStyle.FixedSingle

    SetMstComboBox()
    SetUnitComboBox()
    SetInitialProperty()
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub SetMstComboBox()
    callCodeComboBox.Items.Clear()
    packingComboBox.Items.Clear()
    terminalComboBox.Items.Clear()
    manufacturerCodeComboBox.Items.Clear()
    staffNumberComboBox.Items.Clear()
    free1NumberComboBox.Items.Clear()
    free2NumberComboBox.Items.Clear()
    free3NumberComboBox.Items.Clear()
    free4NumberComboBox.Items.Clear()
    free5NumberComboBox.Items.Clear()

    ' 商品マスタ
    Dim ItemData As DataTable = GetMasterData("SELECT call_code FROM MST_Item ORDER BY call_code")
    If ItemData.Rows.Count = 0 Then
      MessageBox.Show("商品マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      For Each row As DataRow In ItemData.Rows
        callCodeComboBox.Items.Add(row(0))
      Next
    End If

    ' 風袋
    Dim PackingData As DataTable = GetMasterData("SELECT PackingWeight FROM MST_Packing ORDER BY PackingNo")
    If PackingData.Rows.Count = 0 Then
      MessageBox.Show("風袋マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      For Each row As DataRow In PackingData.Rows
        packingComboBox.Items.Add(row(0))
      Next
    End If

    ' 号機番号
    Dim TerminalData As DataTable = GetMasterData("SELECT unit_number FROM MST_Scale ORDER BY unit_number")
    If TerminalData.Rows.Count = 0 Then
      MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      For Each row As DataRow In TerminalData.Rows
        terminalComboBox.Items.Add(row(0))
      Next
    End If

    ' 製造者
    Dim manufacturerData As DataTable = GetMasterData("SELECT Manufacturer_Code FROM MST_Manufacturer ORDER BY Manufacturer_Code")
    If manufacturerData.Rows.Count = 0 Then
      MessageBox.Show("製造者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      For Each row As DataRow In manufacturerData.Rows
        manufacturerCodeComboBox.Items.Add(row(0))
      Next
    End If

    ' 製造者
    Dim staffData As DataTable = GetMasterData("SELECT staff_number FROM MST_Staff ORDER BY staff_number")
    If staffData.Rows.Count = 0 Then
      MessageBox.Show("担当者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      For Each row As DataRow In staffData.Rows
        staffNumberComboBox.Items.Add(row(0))
      Next
    End If

    ' フリー
    Dim comboBoxes() As ComboBox = {free1NumberComboBox, free2NumberComboBox, free3NumberComboBox, free4NumberComboBox, free5NumberComboBox}
    Dim tableNames() As String = {"MST_Free1", "MST_Free2", "MST_Free3", "MST_Free4", "MST_Free5"}

    For i As Integer = 0 To comboBoxes.Length - 1
      Dim freedata As DataTable = GetMasterData($"SELECT free{i + 1}_number FROM {tableNames(i)} ORDER BY free{i + 1}_number")
      If freedata.Rows.Count = 0 Then
        MessageBox.Show($"フリー{i + 1}マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        For Each row As DataRow In freedata.Rows
          comboBoxes(i).Items.Add(row(0))
        Next
      End If
    Next
  End Sub

  Private Function GetMasterData(query As String) As DataTable
    Dim data As New DataTable
    Try
      SqlServer.GetResult(data, query)
    Catch ex As Exception
      Call ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
    Return data
  End Function

  'Private Sub SetMstComboBox()
  '    Dim PackingData As DataTable = GetPackingMasterData()

  '    packingComboBox.Items.Clear()

  '    If PackingData.Rows.Count = 0 Then
  '        MessageBox.Show("風袋マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '    Else
  '        For Each row As DataRow In PackingData.Rows
  '            packingComboBox.Items.Add(row(0))
  '        Next
  '    End If

  '    Dim terminalData As DataTable = GetScaleMasterData()

  '    terminalComboBox.Items.Clear()

  '    If terminalData.Rows.Count = 0 Then
  '        MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '    Else
  '        For Each row As DataRow In terminalData.Rows
  '            terminalComboBox.Items.Add(row(0))
  '        Next
  '    End If
  'End Sub
  'Private Function GetPackingMasterData() As DataTable
  '    Dim PackingData As New DataTable
  '    Try
  '        With tmpDb
  '            SqlServer.GetResult(PackingData, GetSelectPackingMaster)
  '        End With
  '    Catch ex As Exception
  '        Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '        Throw New Exception(ex.Message)
  '    End Try

  '    Return PackingData
  'End Function
  'Private Function GetSelectPackingMaster() As String
  '    Dim sql As String = String.Empty
  '    sql &= " SELECT PackingWeight"
  '    sql &= " FROM MST_Packing "
  '    sql &= " ORDER BY PackingNo"
  '    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '    Return sql
  'End Function

  'Private Function GetScaleMasterData() As DataTable
  '    Dim ScaleData As New DataTable
  '    Try
  '        With tmpDb
  '            SqlServer.GetResult(ScaleData, GetSelectScaleMaster)
  '        End With
  '    Catch ex As Exception
  '        Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '        Throw New Exception(ex.Message)
  '    End Try

  '    Return ScaleData
  'End Function
  'Private Function GetSelectScaleMaster() As String
  '    Dim sql As String = String.Empty
  '    sql &= " SELECT unit_number"
  '    sql &= " FROM MST_Scale "
  '    sql &= " ORDER BY unit_number"
  '    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '    Return sql
  'End Function


  Private Sub SetUnitComboBox()
    ' 共通のアイテムリスト
    Dim items As String() = {"mg", "g", "kg", "t"}

    ' コンボボックスのアイテムをクリア
    weightComboBox.Items.Clear()
    packing1WeightUnitComboBox.Items.Clear()
    packing2WeightUnitComboBox.Items.Clear()
    grossWeightUnitComboBox.Items.Clear()
    'unitWeightUnitComboBox.Items.Clear()
    'upperWeightLimitUnitComboBox.Items.Clear()
    'lowerWeightLimitUnitComboBox.Items.Clear()

    ' アイテムをコンボボックスに追加
    For Each item As String In items
      weightComboBox.Items.Add(item)
      packing1WeightUnitComboBox.Items.Add(item)
      packing2WeightUnitComboBox.Items.Add(item)
      grossWeightUnitComboBox.Items.Add(item)
      'unitWeightUnitComboBox.Items.Add(item)
      'upperWeightLimitUnitComboBox.Items.Add(item)
      'lowerWeightLimitUnitComboBox.Items.Add(item)
    Next
  End Sub
  Private Sub SetPackingComboBox()

  End Sub
  Private Sub SetInitialProperty()
    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        additionDateText.Enabled = True
        additionTimeText.Enabled = True
        'terminalComboBox.Enabled = True
        'callCodeComboBox.Enabled = True
      Case 2
        additionDateText.Enabled = False
        additionTimeText.Enabled = False
        terminalComboBox.Enabled = False
        'callCodeComboBox.Enabled = False

        additionDateText.Text = additionDateTxTValue
        additionTimeText.Text = additionTimeTxTValue
        terminalComboBox.SelectedItem = terminalNumberTxTValue
        callCodeComboBox.SelectedItem = callCodeTxTValue
        itemNoText.Text = itemNoTxTValue
        itemNameText.Text = itemNameTxTValue
        weightText.Text = weightTxTValue.ToString()
        weightComboBox.SelectedItem = weightUnitTxTValue
        staffNumberComboBox.SelectedItem = staffNumberTxTValue
        staffNameText.Text = staffNameTxTValue
        packingComboBox.SelectedItem = packingTxTValue.ToString()
        packingUnitText.Text = packingUnitTxTValue
        packing1WeightText.Text = packing1WeightTxTValue.ToString()
        packing1WeightUnitComboBox.SelectedItem = packing1WeightUnitTxTValue
        packing2WeightText.Text = packing2WeightTxTValue.ToString()
        packing2WeightUnitComboBox.SelectedItem = packing2WeightUnitTxTValue
        packing2MultiplicationText.Text = packing2MultiplicationTxTValue.ToString()
        packing1NumberComboBox.SelectedItem = packing1NumberTxTValue
        packing1NameText.Text = packing1NameTxTValue
        packing2NumberComboBox.SelectedItem = packing2NumberTxTValue
        packing2NameText.Text = packing2NameTxTValue
        free1NumberComboBox.SelectedItem = free1NumberTxTValue
        free1NameTextValue.Text = free1NameTxTValue
        free2NumberComboBox.SelectedItem = free2NumberTxTValue
        free2NameTextValue.Text = free2NameTxTValue
        free3NumberComboBox.SelectedItem = free3NumberTxTValue
        free3NameTextValue.Text = free3NameTxTValue
        free4NumberComboBox.SelectedItem = free4NumberTxTValue
        free4NameTextValue.Text = free4NameTxTValue
        free5NumberComboBox.SelectedItem = free5NumberTxTValue
        free5NameTextValue.Text = free5NameTxTValue
        manufacturerCodeComboBox.SelectedItem = manufacturerCodeTxTValue.ToString()
        manufacturerNameTextValue.Text = manufacturerNameTxTValue
        lot1TextValue.Text = lot1TxTValue
        lot2TextValue.Text = lot2TxTValue
        classificationTextValue.Text = classificationTxTValue
        processingDateTextValue.Text = processingDateTxTValue
        processingTimeTextValue.Text = processingTimeTxTValue
        effectiveDateTextValue.Text = effectiveDateTxTValue
        effectiveTimeTextValue.Text = effectiveTimeTxTValue
        workOrderNumberTextValue.Text = workOrderNumberTxTValue
        detailNumberTextValue.Text = detailNumberTxTValue
        instructionQtyTextValue.Text = instructionQtyTxTValue.ToString()
        actualQtyTextValue.Text = actualQtyTxTValue.ToString()
        workOrderNameTextValue.Text = workOrderNameTxTValue

    End Select
  End Sub

    Private Sub staffNumberComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedOption As Integer = CInt(DirectCast(sender, ComboBox).SelectedItem)

        SetStaffName(selectedOption)
    End Sub

    Private Sub SetStaffName(selectedOption As Integer)
    Dim StaffData As DataTable = GetStaffName(selectedOption)

    If StaffData.Rows.Count = 0 Then
      MessageBox.Show("担当者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      Dim StaffName As String = StaffData.Rows(0)("staff_name").ToString()
      staffNameText.Text = StaffName
    End If
  End Sub

  Private Function GetStaffName(selectedOption As Integer) As DataTable
    Dim freeData As New DataTable
    Try
      SqlServer.GetResult(freeData, GetSelectStaffNameQuery(selectedOption))
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return freeData
  End Function

  Private Function GetSelectStaffNameQuery(selectedOption As Integer) As String
    Dim sql As String = String.Empty
    sql &= " SELECT staff_name "
    sql &= " FROM MST_Staff "
    sql &= " WHERE staff_number = '" & selectedOption & "'"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub freeNumberComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles free1NumberComboBox.SelectedIndexChanged, free2NumberComboBox.SelectedIndexChanged, free3NumberComboBox.SelectedIndexChanged, free4NumberComboBox.SelectedIndexChanged, free5NumberComboBox.SelectedIndexChanged
    Dim selectedOption As Integer = CInt(DirectCast(sender, ComboBox).SelectedItem)
    Dim freeType As Integer

    Select Case sender.Name
      Case "free1NumberComboBox"
        freeType = 1
      Case "free2NumberComboBox"
        freeType = 2
      Case "free3NumberComboBox"
        freeType = 3
      Case "free4NumberComboBox"
        freeType = 4
      Case "free5NumberComboBox"
        freeType = 5
    End Select

    SetFreeName(freeType, selectedOption)
  End Sub

  Private Sub SetFreeName(freeType As Integer, selectedOption As Integer)
    Dim freeData As DataTable = GetFreeName(freeType, selectedOption)
    Dim freeNameTextValue As TextBox = New TextBox()

    Select Case freeType
      Case 1
        freeNameTextValue = free1NameTextValue
      Case 2
        freeNameTextValue = free2NameTextValue
      Case 3
        freeNameTextValue = free3NameTextValue
      Case 4
        freeNameTextValue = free4NameTextValue
      Case 5
        freeNameTextValue = free5NameTextValue
    End Select

    If freeData.Rows.Count = 0 Then
      MessageBox.Show($"フリー{freeType}マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      Dim freeName As String = freeData.Rows(0)($"free{freeType}_name").ToString()
      freeNameTextValue.Text = freeName
    End If
  End Sub

  Private Function GetFreeName(freeType As Integer, selectedOption As Integer) As DataTable
    Dim freeData As New DataTable
    Try
      SqlServer.GetResult(freeData, GetSelectFreeNameQuery(freeType, selectedOption))
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return freeData
  End Function

  Private Function GetSelectFreeNameQuery(freeType As Integer, selectedOption As Integer) As String
    Dim sql As String = String.Empty
    sql &= $" SELECT free{freeType}_name "
    sql &= $" FROM MST_Free{freeType} "
    sql &= $" WHERE free{freeType}_number = {selectedOption} "
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    If Not ValidateInputs() Then
      Return
    End If

    Select Case InputMode
      Case 1
        ' 新規登録メソッド呼び出し
        'InsertResults()
      Case 2
        ' 更新メソッド呼び出し
        UpdateResults()
    End Select
  End Sub

  'Private Sub InsertResults()
  '  Dim sql As String = GetInsertSql()

  '  Dim confirmation As DialogResult
  '  confirmation = MessageBox.Show("登録します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

  '  If confirmation = DialogResult.Yes Then
  '    If ExecuteAndCommit(sql) Then
  '      MessageBox.Show("登録処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
  '      Form_ResultList.SelectResults()
  '      Close()
  '    Else
  '      MessageBox.Show("実績管理の登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '    End If
  '  End If
  'End Sub
  'Private Function GetInsertSql() As String
  '  Dim sql As String = String.Empty
  '  Dim Tmpdate As DateTime = CDate(ComGetProcTime())

  '  sql &= "INSERT INTO TRN_Results("
  '  sql &= "    addition_date,"
  '  sql &= "    addition_time,"
  '  sql &= "    terminal_number,"
  '  sql &= "    call_code,"
  '  sql &= "    item_number,"
  '  sql &= "    item_name,"
  '  sql &= "    unit_weight,"
  '  sql &= "    unit_weight_unit,"
  '  sql &= "    quantity,"
  '  sql &= "    packing,"
  '  sql &= "    packing_unit,"
  '  sql &= "    weight,"
  '  sql &= "    weight_unit,"
  '  sql &= "    manufacturer_code,"
  '  sql &= "    manufacturer_name,"
  '  sql &= "    lot1,"
  '  sql &= "    lot2,"
  '  sql &= "    classification,"
  '  sql &= "    upper_weight_limit,"
  '  sql &= "    upper_weight_unit,"
  '  sql &= "    lower_weight_limit,"
  '  sql &= "    lower_weight_unit,"
  '  sql &= "    packing1_weight,"
  '  sql &= "    packing1_weight_unit,"
  '  sql &= "    packing2_weight,"
  '  sql &= "    packing2_weight_unit,"
  '  sql &= "    packing2_multiplication,"
  '  sql &= "    packing1_number,"
  '  sql &= "    packing1_name,"
  '  sql &= "    packing2_number,"
  '  sql &= "    packing2_name,"
  '  sql &= "    staff_number,"
  '  sql &= "    staff_name,"
  '  sql &= "    free1_number,"
  '  sql &= "    free1_name,"
  '  sql &= "    free2_number,"
  '  sql &= "    free2_name,"
  '  sql &= "    free3_number,"
  '  sql &= "    free3_name,"
  '  sql &= "    free4_number,"
  '  sql &= "    free4_name,"
  '  sql &= "    free5_number,"
  '  sql &= "    free5_name,"
  '  sql &= "    processing_date,"
  '  sql &= "    processing_time,"
  '  sql &= "    effective_date,"
  '  sql &= "    effective_time,"
  '  sql &= "    work_order_number,"
  '  sql &= "    detail_number,"
  '  sql &= "    instruction_qty,"
  '  sql &= "    actual_qty,"
  '  sql &= "    work_order_name,"
  '  sql &= "    create_date,"
  '  sql &= "    update_date"
  '  sql &= ") VALUES ("
  '  sql &= "    '" & additionDateText.Text & "',"
  '  sql &= "    '" & additionTimeText.Text & "',"
  '  sql &= "    '" & terminalComboBox.SelectedItem & "',"
  '  sql &= "    '" & callCodeComboBox.SelectedItem & "',"
  '  sql &= "    '" & itemNoText.Text & "',"
  '  sql &= "    '" & itemNameText.Text & "',"
  '  sql &= "    " & unitWeightText.Text & ","                    ' ← 数値
  '  sql &= "    '" & unitWeightUnitComboBox.SelectedItem & "',"
  '  sql &= "    " & quantityText.Text & ","                      ' ← 数値
  '  sql &= "    '" & packingComboBox.SelectedItem & "',"
  '  sql &= "    '" & packingUnitText.Text & "',"
  '  sql &= "    " & weightText.Text & ","                        ' ← 数値
  '  sql &= "    '" & weightComboBox.SelectedItem & "',"
  '  sql &= "    '" & manufacturerCodeComboBox.SelectedItem & "',"
  '  sql &= "    '" & manufacturerNameTextValue.Text & "',"
  '  sql &= "    '" & lot1TextValue.Text & "',"
  '  sql &= "    '" & lot2TextValue.Text & "',"
  '  sql &= "    '" & classificationTextValue.Text & "',"
  '  sql &= "    " & upperWeightLimitText.Text & ","              ' ← 数値
  '  sql &= "    '" & upperWeightUnitComboBox.SelectedItem & "',"
  '  sql &= "    " & lowerWeightLimitText.Text & ","              ' ← 数値
  '  sql &= "    '" & lowerWeightUnitComboBox.SelectedItem & "',"
  '  sql &= "    " & packing1WeightText.Text & ","                ' ← 数値
  '  sql &= "    '" & packing1WeightUnitComboBox.SelectedItem & "',"
  '  sql &= "    " & packing2WeightText.Text & ","                ' ← 数値
  '  sql &= "    '" & packing2WeightUnitComboBox.SelectedItem & "',"
  '  sql &= "    " & packing2MultiplicationText.Text & ","        ' ← 乗数(数値)
  '  sql &= "    '" & packing1NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & packing1NameText.Text & "',"
  '  sql &= "    '" & packing2NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & packing2NameText.Text & "',"
  '  sql &= "    '" & staffNumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & staffNameText.Text & "',"
  '  sql &= "    '" & free1NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & free1NameTextValue.Text & "',"
  '  sql &= "    '" & free2NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & free2NameTextValue.Text & "',"
  '  sql &= "    '" & free3NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & free3NameTextValue.Text & "',"
  '  sql &= "    '" & free4NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & free4NameTextValue.Text & "',"
  '  sql &= "    '" & free5NumberComboBox.SelectedItem & "',"
  '  sql &= "    '" & free5NameTextValue.Text & "',"
  '  sql &= "    '" & processingDateTextValue.Text & "',"
  '  sql &= "    '" & processingTimeTextValue.Text & "',"
  '  sql &= "    '" & effectiveDateTextValue.Text & "',"
  '  sql &= "    '" & effectiveTimeTextValue.Text & "',"
  '  sql &= "    '" & workOrderNumberTextValue.Text & "',"
  '  sql &= "    '" & detailNumberTextValue.Text & "',"
  '  sql &= "    " & instructionQtyText.Text & ","                ' ← 数値
  '  sql &= "    " & actualQtyText.Text & ","                     ' ← 数値
  '  sql &= "    '" & workOrderNameTextValue.Text & "',"
  '  sql &= "    '" & Tmpdate & "',"
  '  sql &= "    '" & Tmpdate & "'"
  '  sql &= ")"
  '  ｚ

  '  Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '  Return sql
  'End Function

  Private Sub UpdateResults()
    Dim sql As String = String.Empty
    With tmpDb
      Try
        sql = GetUpdateSql()
        Dim confirmation As String
        confirmation = MessageBox.Show("更新します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show("更新処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Form_ResultList.SelectResults()
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("実績管理の更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
        Throw New Exception(ex.Message)
      End Try
    End With
  End Sub
  Private Function GetUpdateSql() As String
    Dim sql As String = String.Empty
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    sql &= "  UPDATE TRN_Results "
    sql &= "  SET "
    'sql &= "      weight = '" & weightText.Text & "'"
    'sql &= "     ,weight_unit = '" & weightComboBox.Text & "'"
    'sql &= "     ,manufacturer_code = '" & manufacturerCodeComboBox.Text & "'"
    'sql &= "     ,manufacturer_name = '" & manufacturerNameTextValue.Text & "'"
    sql &= "     call_code = '" & callCodeComboBox.SelectedItem.ToString() & "'"
    sql &= "     ,item_number = '" & itemNoText.Text & "'"
    sql &= "     ,item_name = '" & itemNameText.Text & "'"
    sql &= "     ,staff_number = '" & staffNumberComboBox.Text & "'"
    sql &= "     ,staff_name = '" & staffNameText.Text & "'"
    sql &= "     ,lot1 = '" & lot1TextValue.Text & "'"
    sql &= "     ,lot2 = '" & lot2TextValue.Text & "'"
    sql &= "     ,classification = '" & classificationTextValue.Text & "'"
    sql &= "     ,free1_number = '" & free1NumberComboBox.SelectedItem & "'"
    sql &= "     ,free1_name = '" & free1NameTextValue.Text & "'"
    sql &= "     ,free2_number = '" & free2NumberComboBox.SelectedItem & "'"
    sql &= "     ,free2_name = '" & free2NameTextValue.Text & "'"
    sql &= "     ,free3_number = '" & free3NumberComboBox.SelectedItem & "'"
    sql &= "     ,free3_name = '" & free3NameTextValue.Text & "'"
    sql &= "     ,free4_number = '" & free4NumberComboBox.SelectedItem & "'"
    sql &= "     ,free4_name = '" & free4NameTextValue.Text & "'"
    sql &= "     ,free5_number = '" & free5NumberComboBox.SelectedItem & "'"
    sql &= "     ,free5_name = '" & free5NameTextValue.Text & "'"
    sql &= "  WHERE "
    sql &= "      addition_date = '" & additionDateText.Text & "'"
    sql &= "      AND addition_time = '" & additionTimeText.Text & "'"
    sql &= "      AND terminal_number = '" & terminalComboBox.Text & "'"
    'sql &= "      AND call_code = '" & callCodeComboBox.Text & "'"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function ExecuteAndCommit(sql As String) As Boolean
    Try
      With tmpDb
        If .Execute(sql) = 1 Then
          .TrnCommit()
          Return True
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
    Return False
  End Function
  Private Function ValidateInputs() As Boolean
    Dim isValid As Boolean = True

    ' 日付チェック
    If String.IsNullOrWhiteSpace(additionDateText.Text) Then
      MessageBox.Show("日付を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      additionDateText.Focus()
      isValid = False
    End If

    ' 時刻チェック
    If String.IsNullOrWhiteSpace(additionTimeText.Text) Then
      MessageBox.Show("時刻を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      additionTimeText.Focus()
      isValid = False
    End If

    ' 端末機№チェック
    If terminalComboBox.SelectedIndex = -1 Then
      MessageBox.Show("端末機№を選択してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      terminalComboBox.Focus()
      isValid = False
    End If

    ' 呼出コードチェック
    If callCodeComboBox.SelectedIndex = -1 Then
      MessageBox.Show("呼出コードを選択してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      callCodeComboBox.Focus()
      isValid = False
    End If

    Return isValid
  End Function

  Private Sub callCodeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles callCodeComboBox.SelectedIndexChanged
    Dim selectedOption As String = DirectCast(sender, ComboBox).SelectedItem

    SetCallCodeData(selectedOption)
  End Sub

  Private Sub SetCallCodeData(selectedOption As String)
    Dim CallCodeData As DataTable = GetCallCode(selectedOption)

    If CallCodeData.Rows.Count = 0 Then
      MessageBox.Show("商品マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      Dim wItemNoText As String = CallCodeData.Rows(0)("item_number").ToString()
      Dim wItemNameText As String = CallCodeData.Rows(0)("item_name").ToString()
      itemNoText.Text = wItemNoText
      itemNameText.Text = wItemNameText
    End If
  End Sub

  Private Function GetCallCode(selectedOption As String) As DataTable
    Dim freeData As New DataTable
    Try
      SqlServer.GetResult(freeData, GetSelectCallCodeQuery(selectedOption))
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return freeData
  End Function

  Private Function GetSelectCallCodeQuery(selectedOption As String) As String
    Dim sql As String = String.Empty
    sql &= " SELECT item_number,item_name "
    sql &= " FROM MST_Item "
    sql &= " WHERE call_code = '" & selectedOption & "'"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub manufacturerCodeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles manufacturerCodeComboBox.SelectedIndexChanged
    Dim selectedOption As String = DirectCast(sender, ComboBox).SelectedItem

    SetManufacturerName(selectedOption)
  End Sub
  Private Sub SetManufacturerName(selectedOption As String)
    Dim ManufacturerData As DataTable = GetManufacturerName(selectedOption)

    If ManufacturerData.Rows.Count = 0 Then
      MessageBox.Show("製造者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      Dim ManufacturerName As String = ManufacturerData.Rows(0)("Manufacturer_Name").ToString()
      manufacturerNameTextValue.Text = ManufacturerName
    End If
  End Sub
  Private Function GetManufacturerName(selectedOption As String) As DataTable
    Dim freeData As New DataTable
    Try
      SqlServer.GetResult(freeData, GetSelectManufacturerQuery(selectedOption))
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return freeData
  End Function

  Private Function GetSelectManufacturerQuery(selectedOption As String) As String
    Dim sql As String = String.Empty
    sql &= " SELECT Manufacturer_Name "
    sql &= " FROM MST_Manufacturer "
    sql &= " WHERE Manufacturer_Code = '" & selectedOption & "'"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub Form_ResultDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

  Private Sub staffNumberComboBox_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles staffNumberComboBox.SelectedIndexChanged

  End Sub
  'Private Sub packingComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles packingComboBox.SelectedIndexChanged
  '    Dim selectedOption As Integer = CInt(packingComboBox.SelectedItem)
  '    SetPackingUnit(selectedOption)
  'End Sub

  'Function SetPackingUnit(selectedOption As Integer)
  '    Dim PackingData As DataTable = PackingUnit(selectedOption)

  '    If PackingData.Rows.Count = 0 Then
  '        MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '        Return String.Empty
  '    Else
  '        Dim packingUnit As String = PackingData.Rows(0)("UNIT_NUMBER").ToString()

  '        packingUnitText.Text = packingUnit
  '        Return packingUnit
  '    End If
  'End Function
  'Private Function PackingUnit(selectedOption As Integer) As DataTable
  '    Dim scaleData As New DataTable
  '    Try
  '        With tmpDb
  '            SqlServer.GetResult(scaleData, GetSelectPackingUnit(selectedOption))
  '        End With
  '    Catch ex As Exception
  '        Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '        Throw New Exception(ex.Message)
  '    End Try

  '    Return scaleData
  'End Function

  'Private Function GetSelectPackingUnit(selectedOption As Integer) As String
  '    Dim sql As String = String.Empty
  '    sql &= " SELECT PackingWeightUnit "
  '    sql &= " FROM MST_Packing "
  '    sql &= " WHERE DELETE_FLG = 0 "
  '    sql &= " ORDER BY UNIT_NUMBER"
  '    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '    Return sql
  'End Function
End Class
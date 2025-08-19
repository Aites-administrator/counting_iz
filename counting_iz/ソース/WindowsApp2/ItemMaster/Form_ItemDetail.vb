Imports Common
Imports Common.ClsFunction
Public Class Form_ItemDetail

  Public CallCodeDigits As Integer = ReadSettingIniFile("ITEM_DIGITS", "VALUE")

  '新規:１ 、変更:２
  Public InputMode As Integer

  Public codeTextValue As String
  Public itemCodeTextValue As String
  Public itemNameTextValue As String
  Public weightTextValue As Decimal
  Public weightUnitComboValue As String
  Public safetyFactorTextValue As Decimal
  Public targetQtyTextValue As Decimal
  Public tareComboValue As String
  Public tareUnitComboValue As String
  Public maxTextValue As Decimal
  Public standardTextValue As Decimal
  Public minTextValue As Decimal
  Public subtotalTargetTextValue As Decimal
  Public subtotalCountTextValue As Decimal
  Public weightMaxTextValue As Decimal
  Public weightMaxUnitComboValue As String
  Public weightMinTextValue As Decimal
  Public weightMinUnitComboValue As String

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
  Private Sub Form_ItemDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "商品マスタ詳細" & " ( " & updateTime & " ) "

    Me.KeyPreview = True

    FormBorderStyle = FormBorderStyle.FixedSingle

    SetPackingComboBox()
    SetUnitComboBox()

    SetInitialProperty()
  End Sub
  Private Sub SetPackingComboBox()
    Try
      ' 風袋マスタからデータを取得
      Dim PackingData As DataTable = GetPackingMasterData()

      ' 風袋マスタからデータが取得できなかった場合
      If PackingData.Rows.Count = 0 Then
        ' エラーメッセージを表示して終了
        MessageBox.Show("風袋マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        ' ComboBoxのアイテムをクリア
        tareCombo.Items.Clear()

        ' 空の項目をComboBoxに追加
        tareCombo.Items.Add("")

        ' 風袋マスタから取得したデータをComboBoxに追加
        For Each row As DataRow In PackingData.Rows
          tareCombo.Items.Add(row(0).ToString())
        Next
      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
  End Sub


  Private Function GetPackingMasterData() As DataTable
    Dim PackingData As New DataTable
    Try
      With tmpDb
        SqlServer.GetResult(PackingData, GetSelectPackingMaster)
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return PackingData
  End Function
  Private Function GetSelectPackingMaster() As String
    Dim sql As String = String.Empty
    sql &= " SELECT CONVERT(VARCHAR, PackingNo) + ' ' + PackingName AS DisplayText "
    sql &= " FROM MST_Packing "
    sql &= " ORDER BY PackingNo"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub SetUnitComboBox()
    ' 共通のアイテムリスト
    Dim items As String() = {"mg", "g", "kg", "t"}

    ' コンボボックスのアイテムをクリア
    weightUnitCombo.Items.Clear()
    weightMaxUnitCombo.Items.Clear()
    weightMinUnitCombo.Items.Clear()

    ' アイテムをコンボボックスに追加
    For Each item As String In items
      weightUnitCombo.Items.Add(item)
      weightMaxUnitCombo.Items.Add(item)
      weightMinUnitCombo.Items.Add(item)
    Next

    ' 初期値を "g" に設定
    weightUnitCombo.SelectedItem = "g"
    weightMaxUnitCombo.SelectedItem = "g"
    weightMinUnitCombo.SelectedItem = "g"

    'PackingComboBox.Items.Add(10.0)
  End Sub

  Private Sub SetInitialProperty()
    weightUnitCombo.Enabled = True
    weightMaxUnitCombo.Enabled = True
    weightMinUnitCombo.Enabled = True
    tareCombo.Enabled = True
    tareCombo.Enabled = True

    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        codeText.Enabled = True

      Case 2
        codeText.Enabled = False

        codeText.Text = codeTextValue
        itemCodeText.Text = itemCodeTextValue
        itemNameText.Text = itemNameTextValue

        weightText.Text = weightTextValue.ToString()
        weightUnitCombo.SelectedItem = weightUnitComboValue

        safetyFactorText.Text = safetyFactorTextValue.ToString()
        targetQtyText.Text = targetQtyTextValue.ToString()

        tareCombo.SelectedItem = tareComboValue

        maxText.Text = maxTextValue.ToString()
        standardText.Text = standardTextValue.ToString()
        minText.Text = minTextValue.ToString()

        subtotalTargetText.Text = subtotalTargetTextValue.ToString()
        subtotalCountText.Text = subtotalCountTextValue.ToString()

        weightMaxText.Text = weightMaxTextValue.ToString()
        weightMaxUnitCombo.SelectedItem = weightMaxUnitComboValue

        weightMinText.Text = weightMinTextValue.ToString()
        weightMinUnitCombo.SelectedItem = weightMinUnitComboValue

    End Select
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    Select Case InputMode
      Case 1
        If CheckValue() = False Then
          Exit Sub
        End If
        '新規登録メソッド呼出し
        InsertItemMaster()
      Case 2
        '更新メソッド呼出し
        UpdateItemMaster()
    End Select
  End Sub

  Function CheckValue() As Boolean
    Dim errorMessages As New List(Of String)
    Dim firstErrorControl As Control = Nothing

    If String.IsNullOrEmpty(codeText.Text) Then
      errorMessages.Add("呼出コードを入力してください。")
      If firstErrorControl Is Nothing Then firstErrorControl = codeText
    ElseIf codeText.Text = "000000" Then
      errorMessages.Add("呼出コードに000000は登録できません。")
      If firstErrorControl Is Nothing Then firstErrorControl = codeText
    End If

    'If String.IsNullOrEmpty(itemCodeText.Text) Then
    '  errorMessages.Add("品番を入力してください。")
    '  If firstErrorControl Is Nothing Then firstErrorControl = itemCodeText
    'End If

    If String.IsNullOrEmpty(itemNameText.Text) Then
      errorMessages.Add("品名を入力してください。")
      If firstErrorControl Is Nothing Then firstErrorControl = itemNameText
    End If

    'If String.IsNullOrEmpty(weightText.Text) Then
    '  errorMessages.Add("単重を入力してください。")
    '  If firstErrorControl Is Nothing Then firstErrorControl = weightText
    'End If

    'If String.IsNullOrWhiteSpace(weightUnitCombo.Text) Then
    '  errorMessages.Add("単重単位を選択してください。")
    '  If firstErrorControl Is Nothing Then firstErrorControl = weightUnitCombo
    'End If

    'If String.IsNullOrWhiteSpace(targetQtyText.Text) Then
    '  errorMessages.Add("目標個数を入力してください")
    '  If firstErrorControl Is Nothing Then firstErrorControl = targetQtyText
    'End If

    ' エラー表示とフォーカス
    If errorMessages.Count > 0 Then
      MessageBox.Show(String.Join(vbCrLf, errorMessages), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      If firstErrorControl IsNot Nothing Then firstErrorControl.Focus()
      Return False
    End If

    ' 重複チェック
    For Each row As DataGridViewRow In Form_ItemList.ItemDetail.Rows
      If codeText.Text.Equals(row.Cells(0).Value?.ToString()) Then
        MessageBox.Show("既に登録されている商品コードです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        codeText.Focus()
        Return False
      End If
    Next

    Return True
  End Function


  Private Sub InsertItemMaster()
    Dim sql As String = String.Empty
    With tmpDb
      Try
        sql = GetInsertSql()
        Dim confirmation As String
        confirmation = MessageBox.Show("登録します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show("登録処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' 一覧画面データ更新
            Form_ItemList.SelectItemMaster()
            ' 一覧画面件数更新
            'Form_ItemList.GetRowCount(Form_ItemList.DeletedDisplayCheckBox.Checked)

            Close()
          Else
            ' 登録失敗
            MessageBox.Show("商品マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

  Private Function GetInsertSql() As String
    Dim sql As String = String.Empty
    Dim callCode As String = codeText.Text
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    ' 各フィールドの型に応じた変換
    Dim itemNoTxt As String = itemCodeText.Text
    Dim itemNameTxt As String = itemNameText.Text
    Dim unitWeightInt As Integer = If(IsNumeric(weightText.Text), CInt(weightText.Text), 0)
    Dim unitWeightUnit As String = weightUnitCombo.SelectedItem.ToString()

    ' 数値変換：空白や非数値は 0 に

    Dim safetyFactorInt As Integer = If(IsNumeric(safetyFactorText.Text), CInt(safetyFactorText.Text), 0)
    Dim targetQtyInt As Integer = If(IsNumeric(targetQtyText.Text), CInt(targetQtyText.Text), 0)

    Dim packingCode As String = ""

    If Not String.IsNullOrWhiteSpace(tareCombo.Text) Then
      Dim parts = tareCombo.Text.Split(" "c)
      If parts.Length > 0 Then
        packingCode = parts(0)
      End If
    End If

    Dim packingDecimal As Decimal = If(Decimal.TryParse(tareWeightTextBox.Text, Nothing), CDec(tareWeightTextBox.Text), 0D)

    Dim upperLimitInt As Integer = If(IsNumeric(maxText.Text), CInt(maxText.Text), 0)
    Dim standardValueInt As Integer = If(IsNumeric(standardText.Text), CInt(standardText.Text), 0)
    Dim lowerLimitInt As Integer = If(IsNumeric(minText.Text), CInt(minText.Text), 0)

    Dim subtotalTargetQtyInt As Integer = If(IsNumeric(subtotalTargetText.Text), CInt(subtotalTargetText.Text), 0)
    Dim subtotalTargetCntInt As Integer = If(IsNumeric(subtotalCountText.Text), CInt(subtotalCountText.Text), 0)

    Dim upperUnitWeightDecimal As Decimal = If(IsNumeric(weightMaxText.Text), CDec(weightMaxText.Text), 0D)
    Dim lowerUnitWeightDecimal As Decimal = If(IsNumeric(weightMinText.Text), CDec(weightMinText.Text), 0D)

    ' 変換不要の文字列はそのまま
    Dim packingUnit As String = tareWeightUnitTextBox.Text
    Dim upperUnitWeightUnit As String = If(weightMaxUnitCombo.SelectedItem IsNot Nothing, weightMaxUnitCombo.SelectedItem.ToString(), "")
    Dim lowerUnitWeightUnit As String = If(weightMinUnitCombo.SelectedItem IsNot Nothing, weightMinUnitCombo.SelectedItem.ToString(), "")


    sql &= " INSERT INTO MST_Item("
    sql &= "     call_code,"
    sql &= "     item_number,"
    sql &= "     item_name,"
    sql &= "     upper_limit,"
    sql &= "     standard_value,"
    sql &= "     lower_limit,"
    sql &= "     subtotal_target_qty,"
    sql &= "     subtotal_target_cnt,"
    sql &= "     safety_factor,"
    sql &= "     target_qty,"
    sql &= "     unit_weight,"
    sql &= "     upper_unit_weight,"
    sql &= "     lower_unit_weight,"
    sql &= "     unit_weight_unit,"
    sql &= "     upper_unit_weight_unit,"
    sql &= "     lower_unit_weight_unit,"
    sql &= "     packing_code,"
    sql &= "     packing,"
    sql &= "     packing_unit,"
    sql &= "     create_date,"
    sql &= "     update_date"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & callCode & "',"
    sql &= "     '" & itemNoTxt & "',"
    sql &= "     '" & itemNameTxt & "',"
    sql &= "     " & upperLimitInt & ","
    sql &= "     " & standardValueInt & ","
    sql &= "     " & lowerLimitInt & ","
    sql &= "     " & subtotalTargetQtyInt & ","
    sql &= "     " & subtotalTargetCntInt & ","
    sql &= "     " & safetyFactorInt & ","
    sql &= "     " & targetQtyInt & ","
    sql &= "     " & unitWeightInt & ","
    sql &= "     " & upperUnitWeightDecimal & ","
    sql &= "     " & lowerUnitWeightDecimal & ","
    sql &= "     '" & unitWeightUnit & "',"
    sql &= "     '" & upperUnitWeightUnit & "',"
    sql &= "     '" & lowerUnitWeightUnit & "',"
    sql &= "     '" & packingCode & "',"
    sql &= "     " & packingDecimal & ","
    sql &= "     '" & packingUnit & "',"
    sql &= "     '" & tmpdate.ToString("yyyy-MM-dd HH:mm:ss") & "',"
    sql &= "     '" & tmpdate.ToString("yyyy-MM-dd HH:mm:ss") & "'"
    sql &= " )"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function


  ' NULL許容の値をフォーマット
  Private Function FormatValue(value As Object) As String
    If value Is Nothing OrElse value.ToString() = "" Then
      Return "NULL"
    ElseIf TypeOf value Is String Then
      Return "'" & value & "'"
    Else
      Return value.ToString()
    End If
  End Function


  Private Sub UpdateItemMaster()
    Dim sql As String = String.Empty
    'Dim rowSelectionCode As String = String.Empty
    'rowSelectionCode = CodeText.Text
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
            Dim CurrentRow As Integer = Form_ItemList.ItemDetail.CurrentRow.Index
            Form_ItemList.SelectItemMaster()
            Form_ItemList.ItemDetail.Rows(CurrentRow).Selected = True
            Form_ItemList.ItemDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            '選択している行の行番号の取得
            Form_ItemList.ItemDetail.CurrentCell = Form_ItemList.ItemDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("商品マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    Dim callCode As String = codeText.Text
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    ' 各フィールドの型に応じた変換（安全な変換処理）
    Dim itemNoTxt As String = itemCodeText.Text
    Dim itemNameTxt As String = itemNameText.Text

    ' Integer.TryParseを使用した安全な変換
    Dim upperLimitInt As Integer = If(Integer.TryParse(maxText.Text, Nothing), CInt(maxText.Text), 0)
    Dim standardValueInt As Integer = If(Integer.TryParse(standardText.Text, Nothing), CInt(standardText.Text), 0)
    Dim lowerLimitInt As Integer = If(Integer.TryParse(minText.Text, Nothing), CInt(minText.Text), 0)
    Dim subtotalTargetQtyInt As Integer = If(Integer.TryParse(subtotalTargetText.Text, Nothing), CInt(subtotalTargetText.Text), 0)
    Dim subtotalTargetCntInt As Integer = If(Integer.TryParse(subtotalCountText.Text, Nothing), CInt(subtotalCountText.Text), 0)
    Dim safetyFactorInt As Integer = If(Integer.TryParse(safetyFactorText.Text, Nothing), CInt(safetyFactorText.Text), 0)
    Dim targetQtyInt As Integer = If(Integer.TryParse(targetQtyText.Text, Nothing), CInt(targetQtyText.Text), 0)
    Dim unitWeightInt As Integer = If(Integer.TryParse(weightText.Text, Nothing), CInt(weightText.Text), 0)

    ' Decimalの場合も同様に
    Dim upperUnitWeightDecimal As Decimal = If(Decimal.TryParse(weightMaxText.Text, Nothing), CDec(weightMaxText.Text), 0D)
    Dim lowerUnitWeightDecimal As Decimal = If(Decimal.TryParse(weightMinText.Text, Nothing), CDec(weightMinText.Text), 0D)

    Dim packingCode As String = ""
    If Not String.IsNullOrWhiteSpace(tareCombo.Text) Then
      Dim parts = tareCombo.Text.Split(" "c)
      If parts.Length > 0 Then
        packingCode = parts(0)
      End If
    End If

    Dim packingDecimal As Decimal = If(Decimal.TryParse(tareWeightTextBox.Text, Nothing), CDec(tareWeightTextBox.Text), 0D)
    Dim packingUnit As String = tareWeightUnitTextBox.Text
    Dim unitWeightUnit As String = weightUnitCombo.SelectedItem.ToString()
    Dim upperUnitWeightUnit As String = weightMaxUnitCombo.SelectedItem.ToString()
    Dim lowerUnitWeightUnit As String = weightMinUnitCombo.SelectedItem.ToString()

    '' 各フィールドの型に応じた変換
    'Dim itemNoTxt As String = itemCodeText.Text
    'Dim itemNameTxt As String = itemNameText.Text
    'Dim upperLimitInt As Integer = CInt(maxText.Text)
    'Dim standardValueInt As Integer = CInt(standardText.Text)
    'Dim lowerLimitInt As Integer = CInt(minText.Text)
    'Dim subtotalTargetQtyInt As Integer = CInt(subtotalTargetText.Text)
    'Dim subtotalTargetCntInt As Integer = CInt(subtotalCountText.Text)
    'Dim safetyFactorInt As Integer = CInt(safetyFactorText.Text)
    'Dim targetQtyInt As Integer = CInt(targetQtyText.Text)
    'Dim unitWeightInt As Integer = CInt(weightText.Text)
    'Dim upperUnitWeightDecimal As Decimal = CDec(weightMaxText.Text)
    'Dim lowerUnitWeightDecimal As Decimal = CDec(weightMinText.Text)

    'Dim packingCode As String = ""

    'If Not String.IsNullOrWhiteSpace(tareCombo.Text) Then
    '  Dim parts = tareCombo.Text.Split(" "c)
    '  If parts.Length > 0 Then
    '    packingCode = parts(0)
    '  End If
    'End If

    'Dim packingDecimal As Decimal = If(Decimal.TryParse(tareWeightTextBox.Text, Nothing), CDec(tareWeightTextBox.Text), 0D)
    'Dim packingUnit As String = tareWeightUnitTextBox.Text
    'Dim unitWeightUnit As String = weightUnitCombo.SelectedItem.ToString()
    'Dim upperUnitWeightUnit As String = weightMaxUnitCombo.SelectedItem.ToString()
    'Dim lowerUnitWeightUnit As String = weightMinUnitCombo.SelectedItem.ToString()


    sql &= " UPDATE MST_Item"
    sql &= "    SET item_number = '" & itemNoTxt & "',"
    sql &= "        item_name = '" & itemNameTxt & "',"
    sql &= "        upper_limit = " & upperLimitInt & ","
    sql &= "        standard_value = " & standardValueInt & ","
    sql &= "        lower_limit = " & lowerLimitInt & ","
    sql &= "        subtotal_target_qty = " & subtotalTargetQtyInt & ","
    sql &= "        subtotal_target_cnt = " & subtotalTargetCntInt & ","
    sql &= "        safety_factor = " & safetyFactorInt & ","
    sql &= "        target_qty = " & targetQtyInt & ","
    sql &= "        packing_code = '" & packingCode & "',"
    sql &= "        packing = " & packingDecimal & ","
    sql &= "        packing_unit = '" & packingUnit & "',"
    sql &= "        unit_weight = " & unitWeightInt & ","
    sql &= "        upper_unit_weight = " & upperUnitWeightDecimal & ","
    sql &= "        lower_unit_weight = " & lowerUnitWeightDecimal & ","
    sql &= "        unit_weight_unit = '" & unitWeightUnit & "',"
    sql &= "        upper_unit_weight_unit = '" & upperUnitWeightUnit & "',"
    sql &= "        lower_unit_weight_unit = '" & lowerUnitWeightUnit & "',"

    sql &= "        update_date = '" & tmpdate.ToString("yyyy-MM-dd HH:mm:ss") & "'"
    sql &= " WHERE call_code = '" & callCode & "'"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function


  Private Sub UpperUnitWeightText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles weightMaxText.KeyPress
    ' テキストボックスに入力された文字が数字、小数点、バックスペースでない場合は入力を無効化する
    If Not (Char.IsDigit(e.KeyChar) OrElse e.KeyChar = "." OrElse Char.IsControl(e.KeyChar)) Then
      e.Handled = True
    End If

    ' 小数点の場合、既に小数点が含まれているか、すでに小数点が含まれていて、小数点が2つ以上入力された場合は無効化する
    If e.KeyChar = "." Then
      If weightMaxText.Text.Contains(".") OrElse weightMaxText.Text.Length = 0 Then
        e.Handled = True
      End If
    End If

    ' 入力された文字数が7桁以上の場合は無効化する
    If weightMaxText.Text.Replace(".", "").Length >= 7 AndAlso e.KeyChar <> ControlChars.Back Then
      e.Handled = True
    End If
  End Sub

  Private Sub LowerUnitWeightText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles weightMinText.KeyPress
    ' テキストボックスに入力された文字が数字、小数点、バックスペースでない場合は入力を無効化する
    If Not (Char.IsDigit(e.KeyChar) OrElse e.KeyChar = "." OrElse Char.IsControl(e.KeyChar)) Then
      e.Handled = True
    End If

    ' 小数点の場合、既に小数点が含まれているか、すでに小数点が含まれていて、小数点が2つ以上入力された場合は無効化する
    If e.KeyChar = "." Then
      If weightMaxText.Text.Contains(".") OrElse weightMaxText.Text.Length = 0 Then
        e.Handled = True
      End If
    End If

    ' 入力された文字数が7桁以上の場合は無効化する
    If weightMaxText.Text.Replace(".", "").Length >= 7 AndAlso e.KeyChar <> ControlChars.Back Then
      e.Handled = True
    End If
  End Sub

  Private Sub CallCodeText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles codeText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub UnitWeightText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles weightText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub SafetyFactorText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub TargetQtyText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub UpperLimitText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub StandardText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub LowerLimitText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub SubtotalTargetQtyText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles safetyFactorText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub SubtotalTargetCntText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles targetQtyText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub CallCodeText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles codeText.Validating
    'If String.IsNullOrEmpty(codeText.Text) = False Then
    '  codeText.Text = codeText.Text.PadLeft(CallCodeDigits, "0"c)
    'End If
  End Sub

  Private Sub Form_ItemDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

  Private Sub tareCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tareCombo.SelectedIndexChanged
    Dim combo As ComboBox = DirectCast(sender, ComboBox)

    If combo.SelectedItem Is Nothing OrElse String.IsNullOrWhiteSpace(combo.SelectedItem.ToString()) Then
      ' 空白が選ばれた場合はテキストボックスを空にする
      tareWeightTextBox.Text = ""
      tareWeightUnitTextBox.Text = ""
      Return
    End If

    Dim selectedText As String = combo.SelectedItem.ToString()
    Dim selectedParts As String() = selectedText.Split(" "c)

    If selectedParts.Length > 0 AndAlso Integer.TryParse(selectedParts(0), Nothing) Then
      Dim selectedOption As Integer = Integer.Parse(selectedParts(0))
      SetPackingData(selectedOption)
    Else
      tareWeightTextBox.Text = ""
      tareWeightUnitTextBox.Text = ""
    End If
  End Sub



  Private Sub SetPackingData(selectedOption As String)
    Dim StaffData As DataTable = GetPackingData(selectedOption)

    If StaffData.Rows.Count = 0 Then
      MessageBox.Show("風袋マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      Dim wkPackingWeight As String = StaffData.Rows(0)("PackingWeight").ToString()
      Dim wkPackingWeightUnit As String = StaffData.Rows(0)("PackingWeightUnit").ToString()

      tareWeightTextBox.Text = wkPackingWeight
      tareWeightUnitTextBox.Text = wkPackingWeightUnit
    End If
  End Sub
  Private Function GetPackingData(selectedOption As String) As DataTable
    Dim freeData As New DataTable
    Try
      SqlServer.GetResult(freeData, GetSelectPackingDataQuery(selectedOption))
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return freeData
  End Function

  Private Function GetSelectPackingDataQuery(selectedOption As String) As String
    Dim sql As String = String.Empty
    sql &= " SELECT PackingWeight,PackingWeightUnit "
    sql &= " FROM MST_Packing "
    sql &= " WHERE PackingNo = '" & selectedOption & "'"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub tareCombo_DropDown(sender As Object, e As EventArgs) Handles tareCombo.DropDown
    AdjustDropDownWidth(tareCombo)
  End Sub
  Private Sub AdjustDropDownWidth(cb As ComboBox)
    Dim maxItemWidth As Integer = 0
    Using g As Graphics = cb.CreateGraphics()
      Dim font As Font = cb.Font
      For Each item In cb.Items
        Dim itemWidth As Integer = CInt(g.MeasureString(item.ToString(), font).Width)
        If itemWidth > maxItemWidth Then
          maxItemWidth = itemWidth
        End If
      Next
    End Using

    ' 現在の幅より狭いときだけ拡張する（）
    If maxItemWidth > cb.Width Then
      cb.DropDownWidth = maxItemWidth + 10
    Else
      cb.DropDownWidth = cb.Width
    End If
  End Sub

  Private Sub maxText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles maxText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub standardText_KeyPress_1(sender As Object, e As KeyPressEventArgs) Handles standardText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub minText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles minText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub subtotalTargetText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles subtotalTargetText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub subtotalCountText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles subtotalCountText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
End Class
Imports Common
Imports Common.ClsFunction
Public Class Form_PackingDetail
  Public InputMode As Integer
  Public CodeTextValue As String
  Public NameTextValue As String
  Public WeightTextValue As String
  Public weightUnitComboValue As String

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
  Private Sub Form_PackingDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "風袋マスタ詳細" & " ( " & updateTime & " ) "
    Me.KeyPreview = True

    FormBorderStyle = FormBorderStyle.FixedSingle
    MarkRequiredLabels(CodeLabel, NameLabel, WeightLabel, Label12)
    AlignDetailActionButtons(Me, OkButton, CloseButton)

    SetUnitComboBox()
    SetInitialProperty()

  End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    If Not CheckValue() Then Return
    Select Case InputMode
      Case 1
        '新規登録メソッド呼出し
        InsertPackingMaster()
      Case 2
        '更新メソッド呼出し
        UpdatePackingMaster()
    End Select
  End Sub

  Private Sub SetUnitComboBox()
    ' 共通のアイテムリスト
    Dim items As String() = {"mg", "g", "kg", "t"}

    ' コンボボックスのアイテムをクリア
    UnitComboBox.Items.Clear()

    ' アイテムをコンボボックスに追加
    For Each item As String In items
      UnitComboBox.Items.Add(item)
    Next

    ' 初期値を "g" に設定
    UnitComboBox.SelectedItem = "g"

    'PackingComboBox.Items.Add(10.0)
  End Sub

  Private Sub CodeText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CodeText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub WeightText_TextChanged(sender As Object, e As EventArgs) Handles WeightText.TextChanged
    If IsNumeric(WeightText.Text) = True Then
      If CDec(WeightText.Text) * 100 < Math.Ceiling(CDec(WeightText.Text) * 100) Then
        WeightText.Text = WeightText.Text.Remove(WeightText.Text.Length - 1, 1)
        WeightText.SelectionStart = WeightText.Text.Length
      End If
    End If
    If CountChar(WeightText.Text, ".") > 1 Then
      WeightText.Text = WeightText.Text.Remove(WeightText.Text.Length - 1, 1)
      WeightText.SelectionStart = WeightText.Text.Length
    End If
  End Sub
  Private Sub WeightText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles WeightText.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "WeightText" AndAlso ActiveControl.Name <> "CloseButton" Then
      If String.IsNullOrEmpty(WeightText.Text) = False Then
        Dim wkWeight As Decimal
        If IsValidWeight(WeightText.Text, wkWeight) Then
          WeightText.Text = wkWeight.ToString("0.00")
        Else
          MessageBox.Show("重量は0.00～9999.99の範囲で、小数点以下2桁まで入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
          Common.ClsFunction.FocusInputError(WeightText)
          e.Cancel = True
        End If
      End If
    End If
  End Sub
  Private Sub WeightText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles WeightText.KeyPress
    If e.KeyChar = ControlChars.Back Then
      Return
    End If

    If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
      e.Handled = True
      Return
    End If

    Dim proposed As String = WeightText.Text.Remove(WeightText.SelectionStart, WeightText.SelectionLength).
      Insert(WeightText.SelectionStart, e.KeyChar.ToString())
    Dim parts() As String = proposed.Split("."c)
    If proposed.StartsWith(".") OrElse parts.Length > 2 OrElse parts(0).Length > 4 OrElse
       (parts.Length = 2 AndAlso parts(1).Length > 2) Then
      e.Handled = True
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Sub SetInitialProperty()
    CodeText.Text = CodeTextValue
    NameText.Text = NameTextValue
    WeightText.Text = WeightTextValue
    UnitComboBox.SelectedItem = weightUnitComboValue

    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        'コード入力不可
        CodeText.Enabled = True

      Case 2
        'コード入力不可
        CodeText.Enabled = False
        '名称入力不可
        NameText.Enabled = True
    End Select
  End Sub
  Private Function CheckValue() As Boolean
    Dim errorMessages As New List(Of String)
    Dim packingCode As Integer
    Dim packingWeight As Decimal

    If Not Integer.TryParse(CodeText.Text, packingCode) OrElse packingCode < 1 OrElse packingCode > 9999 Then
      errorMessages.Add("風袋番号は1～9999の整数で入力してください。")
    End If

    If String.IsNullOrWhiteSpace(NameText.Text) Then
      errorMessages.Add("名称を入力してください。")
    ElseIf NameText.Text.Length > 20 Then
      errorMessages.Add("名称は20文字以内で入力してください。")
    End If

    If Not IsValidWeight(WeightText.Text, packingWeight) Then
      errorMessages.Add("重量は0.00～9999.99の範囲で、小数点以下2桁まで入力してください。")
    End If

    Dim isUnchangedLegacyUnit As Boolean =
      InputMode = 2 AndAlso Not String.IsNullOrWhiteSpace(weightUnitComboValue) AndAlso UnitComboBox.Text = weightUnitComboValue
    If Not New String() {"mg", "g", "kg", "t"}.Contains(UnitComboBox.Text) AndAlso Not isUnchangedLegacyUnit Then
      errorMessages.Add("単位はmg・g・kg・tから選択してください。")
    End If

    If InputMode = 1 AndAlso packingCode >= 1 AndAlso packingCode <= 9999 Then
      For Each row As DataGridViewRow In Form_PackingList.PackingDetail.Rows
        Dim registeredCode As Integer
        If Integer.TryParse(Convert.ToString(row.Cells(0).Value), registeredCode) AndAlso registeredCode = packingCode Then
          errorMessages.Add("指定した風袋番号は既に登録されています。")
          Exit For
        End If
      Next
    End If

      If errorMessages.Count > 0 Then
        MessageBox.Show(String.Join(vbCrLf, errorMessages), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Dim invalidCode As Integer
        If Not Integer.TryParse(CodeText.Text, invalidCode) OrElse invalidCode < 1 OrElse invalidCode > 9999 Then
          Common.ClsFunction.FocusInputError(CodeText)
        ElseIf String.IsNullOrWhiteSpace(NameText.Text) OrElse NameText.Text.Length > 20 Then
          Common.ClsFunction.FocusInputError(NameText)
        ElseIf Not IsValidWeight(WeightText.Text, packingWeight) Then
          Common.ClsFunction.FocusInputError(WeightText)
        Else
          Common.ClsFunction.FocusInputError(UnitComboBox)
        End If
        Return False
    End If

    Return True
  End Function
  Private Sub InsertPackingMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = CodeText.Text
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
            Form_PackingList.SelectPackingMaster()
            Close()
          Else
            ' 登録失敗
            MessageBox.Show("風袋マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
        Throw
      End Try
    End With
  End Sub
  Private Function GetInsertSql() As String
    Dim sql As String = String.Empty
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim upperUnitWeightUnit As String = UnitComboBox.SelectedItem.ToString()

    sql &= " INSERT INTO MST_PACKING("
    sql &= "     PackingNo,"
    sql &= "     PackingName,"
    sql &= "     PackingWeight,"
    sql &= "     PackingWeightUnit,"
    sql &= "     create_date,"
    sql &= "     update_date"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & EscapeSqlLiteral(CodeText.Text) & "',"
    sql &= "     '" & EscapeSqlLiteral(NameText.Text) & "',"
    sql &= "     '" & EscapeSqlLiteral(WeightText.Text) & "',"
    sql &= "     '" & EscapeSqlLiteral(upperUnitWeightUnit) & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub UpdatePackingMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = CodeText.Text
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
            Dim CurrentRow As Integer = Form_PackingList.PackingDetail.CurrentRow.Index
            Form_PackingList.SelectPackingMaster()
            Form_PackingList.PackingDetail.Rows(CurrentRow).Selected = True
            Form_PackingList.PackingDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            Form_PackingList.PackingDetail.CurrentCell = Form_PackingList.PackingDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 削除失敗
            MessageBox.Show("風袋マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
        Throw
      End Try
    End With
  End Sub
  Private Function GetUpdateSql() As String
    Dim sql As String = String.Empty
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim upperUnitWeightUnit As String = UnitComboBox.SelectedItem.ToString()

    sql &= " UPDATE MST_PACKING"
    sql &= "    SET PackingNo = '" & EscapeSqlLiteral(CodeText.Text) & "'"
    sql &= "       ,PackingName = '" & EscapeSqlLiteral(NameText.Text) & "'"
    sql &= "       ,PackingWeight = '" & EscapeSqlLiteral(WeightText.Text) & "'"
    sql &= "       ,PackingWeightUnit = '" & EscapeSqlLiteral(upperUnitWeightUnit) & "'"
    sql &= "       ,update_date = '" & tmpdate & "'"
    sql &= " WHERE PackingNo = '" & EscapeSqlLiteral(CodeText.Text) & "' "
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub Form_PackingDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
  Private Sub ApplyCommonLayout_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    Common.ClsFunction.ApplyScreenLayout(Me)
  End Sub
  Private Function IsValidWeight(value As String, ByRef weight As Decimal) As Boolean
    If Not System.Text.RegularExpressions.Regex.IsMatch(value, "^\d{1,4}(\.\d{1,2})?$") Then
      Return False
    End If
    Return Decimal.TryParse(value, weight) AndAlso weight >= 0D AndAlso weight <= 9999.99D
  End Function
End Class

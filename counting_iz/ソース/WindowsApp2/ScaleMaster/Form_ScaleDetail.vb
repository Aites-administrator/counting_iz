Imports Common
Imports Common.ClsFunction
Imports System.Text.RegularExpressions

Public Class Form_ScaleDetail
  Public InputMode As Integer
  Public UnitNumberTextValue As String
  Public IpAddressTextValue As String
  Public MemoTextValue As String

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
  Private Sub Form_ScaleDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "計量器マスタ詳細" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    MarkRequiredLabels(CodeLabel, NameLabel)
    AlignDetailActionButtons(Me, OkButton, CloseButton)
    SetInitialProperty()
    FormBorderStyle = FormBorderStyle.FixedSingle
  End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    If Not CheckValue() Then Exit Sub

    Select Case InputMode
      Case 1
        '新規登録メソッド呼出し
        InsertScaleMaster()
      Case 2
        '更新メソッド呼出し
        UpdateScaleMaster()
    End Select
  End Sub
  Private Sub UnitNumberText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UnitNumberText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub IpAddressText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles IpAddressText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back Or e.KeyChar = ".") Then
      e.Handled = True
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Dispose()
  End Sub

  Private Sub SetInitialProperty()
    UnitNumberText.Text = UnitNumberTextValue
    IpAddressText.Text = IpAddressTextValue

    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        'コード入力不可
        UnitNumberText.Enabled = True
        Common.ClsFunction.FocusInputError(UnitNumberText)
      Case 2
        'コード入力不可
        UnitNumberText.Enabled = False
        '名称入力不可
        IpAddressText.Enabled = True
        Common.ClsFunction.FocusInputError(IpAddressText)
    End Select
  End Sub
  Function CheckValue() As Boolean
    Dim unitNumber As Integer
    Dim unitText As String = UnitNumberText.Text.Trim()
    Dim ipAddress As String = IpAddressText.Text.Trim()

    ' === 必須項目チェック①：号機番号 ===
    If Not Integer.TryParse(unitText, unitNumber) OrElse unitNumber < 1 OrElse unitNumber > 99 Then
      MessageBox.Show("号機番号は1～99の数字で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Common.ClsFunction.FocusInputError(UnitNumberText)
      Return False
    End If
    UnitNumberText.Text = unitNumber.ToString("D2")

    ' === 必須項目チェック②：IPアドレス ===
    If String.IsNullOrEmpty(ipAddress) Then
      MessageBox.Show("IPアドレスを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Common.ClsFunction.FocusInputError(IpAddressText)
      Return False
    ElseIf Not IsValidIpAddress(ipAddress) Then
      MessageBox.Show("IPアドレスの形式が正しくありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Common.ClsFunction.FocusInputError(IpAddressText)
      Return False
    ElseIf ipAddress = "127.0.0.1" Then
      MessageBox.Show("127.0.0.1は登録できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Common.ClsFunction.FocusInputError(IpAddressText)
      Return False
    End If
    IpAddressText.Text = ipAddress

    ' === 重複チェック：号機番号 & IPアドレス ===
    If Form_ScaleList.ScaleDetail.Rows.Count > 0 Then
      For i As Integer = 0 To Form_ScaleList.ScaleDetail.Rows.Count - 1
        Dim existingUnit As String = Convert.ToString(Form_ScaleList.ScaleDetail.Rows(i).Cells(0).Value)
        Dim isCurrentRecord As Boolean = InputMode = 2 AndAlso existingUnit = UnitNumberTextValue
        If InputMode = 1 AndAlso UnitNumberText.Text = existingUnit Then
          MessageBox.Show("既に登録されている号機番号です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          Common.ClsFunction.FocusInputError(UnitNumberText)
          Return False
        End If

        If Not isCurrentRecord AndAlso IpAddressText.Text = Convert.ToString(Form_ScaleList.ScaleDetail.Rows(i).Cells(1).Value) Then
          MessageBox.Show("既に登録されているIPアドレスです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          Common.ClsFunction.FocusInputError(IpAddressText)
          Return False
        End If
      Next
    End If

    Return True
  End Function

  ' === IPアドレス形式チェック用の関数 ===
  Function IsValidIpAddress(ip As String) As Boolean
    Dim pattern As String = "^(\d{1,3}\.){3}\d{1,3}$"
    If Not Regex.IsMatch(ip, pattern) Then
      Return False
    End If

    Dim octets As String() = ip.Split("."c)
    For Each octet As String In octets
      Dim value As Integer
      If Not Integer.TryParse(octet, value) OrElse value < 0 OrElse value > 255 Then
        Return False
      End If
    Next
    Return True
  End Function

  Private Sub InsertScaleMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = UnitNumberText.Text
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
            Form_ScaleList.SelectScaleMaster()
            Close()
          Else
            ' 登録失敗
            MessageBox.Show("計量器マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    sql &= " INSERT INTO MST_SCALE("
    sql &= "     UNIT_NUMBER,"
    sql &= "     IP_ADDRESS,"
    sql &= "     CREATE_DATE,"
    sql &= "     UPDATE_DATE"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & EscapeSqlLiteral(UnitNumberText.Text) & "',"
    sql &= "     '" & EscapeSqlLiteral(IpAddressText.Text) & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub UpdateScaleMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = UnitNumberText.Text
    With tmpDb
      Try
        sql = GetUpdateSql()
        ' SQL実行結果が1件か？
        Dim confirmation As String
        confirmation = MessageBox.Show("更新します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show("更新処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Dim CurrentRow As Integer = Form_ScaleList.ScaleDetail.CurrentRow.Index
            Form_ScaleList.SelectScaleMaster()
            Form_ScaleList.ScaleDetail.Rows(CurrentRow).Selected = True
            Form_ScaleList.ScaleDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            Form_ScaleList.ScaleDetail.CurrentCell = Form_ScaleList.ScaleDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("計量器マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    sql &= " UPDATE MST_SCALE"
    sql &= "    SET UNIT_NUMBER = '" & EscapeSqlLiteral(UnitNumberText.Text) & "'"
    sql &= "       ,IP_ADDRESS = '" & EscapeSqlLiteral(IpAddressText.Text) & "'"
    'sql &= "       ,DOWN_PROCESSING_TIME = '" & DateTimePicker_DownLoad.Value & "'"
    'sql &= "       ,UPLOAD_PROCESSING_TIME = '" & DateTimePicker_Upload.Value & "'"
    sql &= "       ,UPDATE_DATE = '" & tmpdate & "'"
    sql &= " WHERE UNIT_NUMBER = '" & EscapeSqlLiteral(UnitNumberTextValue) & "' "
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub UnitNumberText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles UnitNumberText.Validating
    If String.IsNullOrEmpty(UnitNumberText.Text) = False Then
      UnitNumberText.Text = UnitNumberText.Text.PadLeft(2, "0"c)
    End If
  End Sub

  Private Sub Form_ScaleDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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
End Class

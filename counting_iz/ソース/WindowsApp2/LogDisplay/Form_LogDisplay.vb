Imports Common
Imports Common.ClsFunction
Public Class Form_LogDisplay
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
  Private Sub Form_LogDisplay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "送受信ログ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    Dim tmpDb As New ClsSqlServer
    Dim tmpDt As New DataTable
    StartPosition = FormStartPosition.CenterScreen
    FormBorderStyle = FormBorderStyle.FixedSingle
    LogDetail.RowHeadersVisible = False

    Dim todayDate As Date = Date.Today
    DateTime.Text = todayDate
    'ユーザーからのデータ追加を不可にしておく
    LogDetail.AllowUserToAddRows = False
    LogDetail.ColumnCount = 5
    LogDetail.Columns(0).HeaderText = "日付"
    LogDetail.Columns(1).HeaderText = "計量器"
    LogDetail.Columns(2).HeaderText = "処理時刻"
    LogDetail.Columns(3).HeaderText = "ファイル"
    LogDetail.Columns(4).HeaderText = "備考"

    'カラムの幅指定
    LogDetail.Columns(0).Width = 100
    LogDetail.Columns(1).Width = 90
    LogDetail.Columns(2).Width = 120
    LogDetail.Columns(3).Width = 120
    LogDetail.Columns(4).Width = 600

    'カラムの整列設定
    LogDetail.Columns(0).DefaultCellStyle.Alignment =
     DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(1).DefaultCellStyle.Alignment =
     DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(2).DefaultCellStyle.Alignment =
     DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(3).DefaultCellStyle.Alignment =
     DataGridViewContentAlignment.MiddleCenter

    'ヘッダーの整列設定
    LogDetail.Columns(0).HeaderCell.Style.Alignment =
    DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(1).HeaderCell.Style.Alignment =
     DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(2).HeaderCell.Style.Alignment =
     DataGridViewContentAlignment.MiddleCenter
    LogDetail.Columns(3).HeaderCell.Style.Alignment =
     DataGridViewContentAlignment.MiddleCenter

    'マルチ選択不可
    LogDetail.MultiSelect = False

    SetScaleComboBox()

    '選択モード設定(全カラム)
    LogDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, GetAllSelectSql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("通信ログにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, LogDetail)
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try

    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更
  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With LogDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Private Sub ScaleText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub DateText_KeyPress(sender As Object, e As KeyPressEventArgs)
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/") Then
      e.Handled = True
    End If
  End Sub
  Private Sub Scale_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Scale_ComboBox.SelectedIndexChanged
    If LogDetail.Rows.Count > 0 Then
      For i As Integer = 0 To LogDetail.Rows.Count - 1
        LogDetail.Rows(i).Visible = True
      Next
    End If
    If LogDetail.Rows.Count > 0 Then
      If Scale_ComboBox.Text <> "" Then
        For i As Integer = 0 To LogDetail.Rows.Count - 1
          If LogDetail.Rows(i).Cells(1).Value <> Scale_ComboBox.Text Then
            LogDetail.Rows(i).Visible = False
          End If
        Next
      End If
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Sub DateTime_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTime.Validating
    If ActiveControl.Name <> "DateTime" And ActiveControl.Name <> "CloseButton" Then
      Dim inputText As String = DateTime.Text.Replace("/", "").Trim()
      If DateTypeCheck(inputText) Then
        DateTime.Text = DateTxt2DateTxt(inputText)
        MoveTargetRow()
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTime.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub
  Private Sub DateTime_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTime.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/"c) Then
      e.Handled = True
    End If
  End Sub

  Private Sub SetScaleComboBox()
    Dim tmpDb As New ClsSqlServer
    Dim tmpDt As New DataTable

    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, GetSelectScaleMaster)

        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          Scale_ComboBox.Items.Add("")
          For i As Integer = 0 To tmpDt.Rows.Count - 1
            Scale_ComboBox.Items.Add(tmpDt.Rows(i)(0))
          Next
        End If

      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try
  End Sub
  Private Function GetSelectScaleMaster() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     UNIT_NUMBER"
    sql &= " FROM"
    sql &= "     MST_SCALE"
    sql &= " WHERE"
    sql &= "     DELETE_FLG = 0"
    sql &= " ORDER BY  "
    sql &= "     UNIT_NUMBER"
    Call WriteExecuteLog([GetType]().Name, Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetAllSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     PROCESS_DATE,"
    sql &= "     MACHINE_NO,"
    sql &= "     PROCESS_TIME,"
    sql &= "     FILE_NAME,"
    sql &= "     NOTE"
    sql &= " FROM"
    sql &= "     TRN_LOG"
    sql &= " ORDER BY  "
    sql &= "     PROCESS_DATE DESC, PROCESS_TIME DESC, MACHINE_NO "
    Call WriteExecuteLog([GetType]().Name, Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub MoveTargetRow()
    Dim CurrentRow As Integer
    Dim wkDate As String
    wkDate = DateTime.Text.Replace("/", "-")

    If LogDetail.Rows.Count > 0 Then
      For i As Integer = 0 To LogDetail.Rows.Count - 1
        If LogDetail.Rows(i).Cells(0).Value = wkDate Then
          CurrentRow = i
          Exit For
        End If
      Next
      LogDetail.Rows(CurrentRow).Selected = True
      LogDetail.FirstDisplayedScrollingRowIndex = CurrentRow
      LogDetail.CurrentCell = LogDetail.Rows(CurrentRow).Cells(0)
    End If
  End Sub

  Private Sub Form_LogDisplay_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class

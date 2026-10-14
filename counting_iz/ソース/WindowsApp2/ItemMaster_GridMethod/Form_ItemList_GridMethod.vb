Imports System.Data.SqlClient
Imports Common
Imports Common.ClsFunction

Public Class Form_ItemList_GridMethod
  Private dt As DataTable
  Private adapter As SqlDataAdapter
  Private CallCodeDigits As Integer
  Private isClosing As Boolean = False
  Private isUserLimitUnitChange As Boolean = False
  Private isRevertingLimitUnit As Boolean = False
  Private previousLimitUnit As String = String.Empty

  ' ClsSqlServer のインスタンスを共有
  Private _SqlServer As ClsSqlServer
  Private ReadOnly Property SqlServer As ClsSqlServer
    Get
      If _SqlServer Is Nothing Then
        _SqlServer = New ClsSqlServer
      End If
      Return _SqlServer
    End Get
  End Property

  ' フォームロード時にデータを取得して表示
  Private Sub Form_ItemList_GridMethod_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    ' アセンブリの最終更新日時を取得し、フォームのタイトルに表示するテキストを設定
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "商品マスタ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    ' 終了操作では編集中セルの入力チェックを実行しない
    CloseButton.CausesValidation = False

    If Not TryLoadCallCodeDigits() Then
      ForceClose()
      Return
    End If

    LoadData()
    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

    Me.FormBorderStyle = FormBorderStyle.FixedDialog ' ウィンドウのサイズ変更不可
    Me.ControlBox = False ' ×（閉じる）、最大化、最小化ボタンを非表示

    NumberCheckBox.Checked = True
    AddRowTextBox.Text = 1

    ItemDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
  End Sub

  Private Function TryLoadCallCodeDigits() As Boolean
    Try
      Dim settingValue As String = ReadSettingIniFile("ITEM_DIGITS", "VALUE")
      If Not Integer.TryParse(settingValue, CallCodeDigits) OrElse
         CallCodeDigits < 1 OrElse CallCodeDigits > 9 Then
        MessageBox.Show("呼出コードの桁数設定が正しくありません。" & vbCrLf &
                        "ITEM_DIGITS は1～9で設定してください。",
                        "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return False
      End If

      Return True
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      MessageBox.Show("呼出コードの桁数設定を読み込めませんでした。" & vbCrLf & ex.Message,
                      "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return False
    End Try
  End Function

  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With ItemDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo UI", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub

  ' データベースからデータを取得して DataGridView に表示
  Private Sub LoadData()
    Try
      ' ClsSqlServer を使用して接続情報を取得
      Dim connectionString As String = SqlServer.GetConnectionString()

      ' SQL クエリ
      Dim query As String =
         "SELECT
			        call_code,
			        item_number,
			        item_name,
			        packing_bag,
			        packing_bag_unit,
			        upper_limit,
			        upper_limit_unit,
			        reference_value,
			        reference_value_unit,
			        lower_limit,
			        lower_limit_unit,
			        subtotal_target_value,
			        subtotal_target_value_unit,
			        subtotal_target_count,
			        create_date,
			        update_date
			    FROM
			        MST_Item
			    ORDER BY
			        call_code"


      ' DataTable を作成
      dt = New DataTable()

      ' SQL Server へ接続してデータ取得
      Using connection As New SqlConnection(connectionString)
        adapter = New SqlDataAdapter(query, connection)
        adapter.Fill(dt)
        Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, query)
      End Using

      ' DataGridView にデータをバインド
      ItemDetail.DataSource = dt

      ' 重量単位は通常の商品マスタ詳細と同じ選択肢のコンボボックスにする

      ' ヘッダー名変更 & 不要なカラム非表示
      With ItemDetail
        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        .MultiSelect = True
        .Columns("call_code").HeaderText = "呼出コード"
        .Columns("item_number").HeaderText = "品番"
        .Columns("item_name").HeaderText = "品名"
        .Columns("call_code").HeaderCell.Style.ForeColor = Color.Firebrick
        .Columns("item_name").HeaderCell.Style.ForeColor = Color.Firebrick

        ' item_number と item_name の幅を広げる
        .Columns("item_number").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        .Columns("item_name").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        ' 通常の商品マスタ詳細と同じ項目だけを一覧表示する
        .Columns("packing_bag").Visible = False
        .Columns("packing_bag_unit").Visible = False
        .Columns("upper_limit").HeaderText = "上限値"
        .Columns("upper_limit_unit").HeaderText = "上限値単位"
        .Columns("reference_value").HeaderText = "基準値"
        .Columns("reference_value_unit").HeaderText = "基準値単位"
        .Columns("lower_limit").HeaderText = "下限値"
        .Columns("lower_limit_unit").HeaderText = "下限値単位"
        .Columns("subtotal_target_value").Visible = False
        .Columns("subtotal_target_value_unit").Visible = False
        .Columns("subtotal_target_count").Visible = False

        ' 作成日・更新日は非表示
        .Columns("create_date").Visible = False
        .Columns("update_date").Visible = False
      End With

      ' ✅ 1行目を選択状態にする（データがある場合）
      If dt.Rows.Count > 0 Then
        ItemDetail.ClearSelection()
        ItemDetail.Rows(0).Selected = True
        ItemDetail.CurrentCell = ItemDetail.Rows(0).Cells(0)

        ' 検索後にセルの内容に合わせて列幅を自動調整
        ItemDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)

        ' 改行を防ぐ（必要なら再適用）
        ItemDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        ItemDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        ItemDetail.ColumnHeadersHeight = 30
      End If

      SetEditControlsEnabled(True)

    Catch ex As Exception
      ItemDetail.DataSource = Nothing
      SetEditControlsEnabled(False)
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      MessageBox.Show("データの読み込みに失敗しました: " & ex.Message,
                      "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub

  Private Sub SetEditControlsEnabled(enabled As Boolean)
    ItemDetail.Enabled = enabled
    AddRowButton.Enabled = enabled
    CopyRowButton.Enabled = enabled
    OkButton.Enabled = enabled
    DeleteButton.Enabled = enabled
    NumberCheckBox.Enabled = enabled
    AddRowTextBox.Enabled = enabled
  End Sub


  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    If dt IsNot Nothing AndAlso dt.GetChanges() IsNot Nothing Then
      Dim message As String = "現在、編集中のデータがあります。" & vbCrLf &
                              "このまま閉じると変更が失われますが、" & vbCrLf &
                              "よろしいですか？"

      Dim result As DialogResult = MessageBox.Show(message, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)

      If result = DialogResult.OK Then
        ForceClose()
      End If
    Else
      ForceClose()
    End If
  End Sub

  ' 入力途中や入力エラーのセルがあっても、終了時は編集を破棄して画面を閉じる
  Private Sub ForceClose()
    isClosing = True
    Me.AutoValidate = AutoValidate.Disable

    Try
      If ItemDetail.IsCurrentCellInEditMode Then
        ItemDetail.CancelEdit()
      End If
    Finally
      Me.Close()
    End Try
  End Sub

  ' ボタン操作前に編集中セルを確定し、検証エラー中の状態変更を防ぐ
  Private Function TryCommitPendingEdit() As Boolean
    Try
      If Not Me.Validate() Then Return False

      If ItemDetail.IsCurrentCellInEditMode AndAlso Not ItemDetail.EndEdit() Then
        MessageBox.Show("編集中のセルを確定できません。入力内容を確認してください。",
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return False
      End If

      Dim currencyManager As CurrencyManager =
        TryCast(BindingContext(ItemDetail.DataSource), CurrencyManager)
      If currencyManager IsNot Nothing Then currencyManager.EndCurrentEdit()
      Return True
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      MessageBox.Show("入力内容を確定できません。編集中のセルを確認してください。" & vbCrLf & ex.Message,
                      "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return False
    End Try
  End Function

  Private Sub AddRowButton_Click(sender As Object, e As EventArgs) Handles AddRowButton.Click
    If dt Is Nothing OrElse Not TryCommitPendingEdit() Then Return

    ' 入力された行数を取得
    Dim rowCount As Integer
    If Not Integer.TryParse(AddRowTextBox.Text, rowCount) OrElse rowCount <= 0 Then
      MessageBox.Show("追加する行数を正しく入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    Try
      Dim availableCallCodes As List(Of String) = Nothing
      If NumberCheckBox.Checked Then
        availableCallCodes = GetAvailableCallCodes(rowCount)
        If availableCallCodes Is Nothing Then Return
      End If

      ' DataTable に新規行を追加
      For i As Integer = 1 To rowCount
        Dim newRow As DataRow = dt.NewRow() ' 空の新規行を作成

        ' NumberCheckBox がチェックされている場合は、INI設定の桁数で call_code を採番
        If NumberCheckBox.Checked Then
          newRow("call_code") = availableCallCodes(i - 1)
        Else
          newRow("call_code") = DBNull.Value ' チェックされていない場合は空
        End If

        ' 追加する行のセルを空にする
        newRow("item_number") = DBNull.Value
        newRow("item_name") = DBNull.Value
        newRow("packing_bag") = DBNull.Value
        newRow("packing_bag_unit") = DBNull.Value
        newRow("upper_limit") = DBNull.Value
        newRow("upper_limit_unit") = DBNull.Value
        newRow("reference_value") = DBNull.Value
        newRow("reference_value_unit") = DBNull.Value
        newRow("lower_limit") = DBNull.Value
        newRow("lower_limit_unit") = DBNull.Value
        newRow("subtotal_target_value") = DBNull.Value
        newRow("subtotal_target_value_unit") = DBNull.Value
        newRow("subtotal_target_count") = DBNull.Value
        newRow("create_date") = DBNull.Value
        newRow("update_date") = DBNull.Value

        dt.Rows.Add(newRow) ' DataTable に追加
      Next

      ' 最後に追加した行へフォーカスを移動
      If dt.Rows.Count > 0 Then
        ItemDetail.ClearSelection()
        ItemDetail.Rows(dt.Rows.Count - 1).Selected = True
        ItemDetail.CurrentCell = ItemDetail.Rows(dt.Rows.Count - 1).Cells(0)
      End If

    Catch ex As Exception
      MessageBox.Show("行の追加に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
    End Try
  End Sub
  Private Sub CopyRowButton_Click(sender As Object, e As EventArgs) Handles CopyRowButton.Click
    If dt Is Nothing OrElse Not TryCommitPendingEdit() Then Return

    ' 選択された行があるか確認
    If ItemDetail.SelectedRows.Count = 0 Then
      MessageBox.Show("コピーする行を選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    Try
      Dim availableCallCodes As List(Of String) = Nothing
      If NumberCheckBox.Checked Then
        availableCallCodes = GetAvailableCallCodes(ItemDetail.SelectedRows.Count)
        If availableCallCodes Is Nothing Then Return
      End If

      ' 選択された行をコピーして新規追加
      Dim copyIndex As Integer = 0
      For Each row As DataGridViewRow In ItemDetail.SelectedRows
        ' DataRow を作成
        Dim newRow As DataRow = dt.NewRow()

        ' call_code の採番
        If NumberCheckBox.Checked Then
          newRow("call_code") = availableCallCodes(copyIndex)
          copyIndex += 1
        Else
          newRow("call_code") = DBNull.Value ' チェックされていない場合は空
        End If

        ' item_number, item_name は空にする
        newRow("item_number") = DBNull.Value
        newRow("item_name") = DBNull.Value

        ' 他のカラムはコピー
        newRow("packing_bag") = row.Cells("packing_bag").Value
        newRow("packing_bag_unit") = row.Cells("packing_bag_unit").Value
        newRow("upper_limit") = row.Cells("upper_limit").Value
        newRow("upper_limit_unit") = row.Cells("upper_limit_unit").Value
        newRow("reference_value") = row.Cells("reference_value").Value
        newRow("reference_value_unit") = row.Cells("reference_value_unit").Value
        newRow("lower_limit") = row.Cells("lower_limit").Value
        newRow("lower_limit_unit") = row.Cells("lower_limit_unit").Value
        newRow("subtotal_target_value") = row.Cells("subtotal_target_value").Value
        newRow("subtotal_target_value_unit") = row.Cells("subtotal_target_value_unit").Value
        newRow("subtotal_target_count") = row.Cells("subtotal_target_count").Value
        newRow("create_date") = DBNull.Value
        newRow("update_date") = DBNull.Value

        ' DataTable に追加
        dt.Rows.Add(newRow)
      Next

      ' 最後に追加した行へフォーカスを移動
      If dt.Rows.Count > 0 Then
        ItemDetail.ClearSelection()
        ItemDetail.Rows(dt.Rows.Count - 1).Selected = True
        ItemDetail.CurrentCell = ItemDetail.Rows(dt.Rows.Count - 1).Cells(0)
      End If

    Catch ex As Exception
      MessageBox.Show("行のコピーに失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
    End Try
  End Sub

  Private Function GetAvailableCallCodes(requiredCount As Integer) As List(Of String)
    Dim maxCallCode As Integer
    Dim maxCallCodeText As String = New String("9"c, CallCodeDigits)

    If Not Integer.TryParse(maxCallCodeText, maxCallCode) Then
      MessageBox.Show("呼出コードの桁数設定が正しくありません。" & vbCrLf &
                      "管理者へお問い合わせください。",
                      "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return Nothing
    End If

    Dim usedCallCodes As New HashSet(Of Integer)()
    For Each row As DataRow In dt.Rows
      If row.RowState = DataRowState.Deleted OrElse row.IsNull("call_code") Then Continue For

      Dim callCode As Integer
      If Integer.TryParse(row("call_code").ToString(), callCode) AndAlso
         callCode >= 1 AndAlso callCode <= maxCallCode Then
        usedCallCodes.Add(callCode)
      End If
    Next

    Dim availableCallCodes As New List(Of String)()
    Dim startCode As Integer = If(usedCallCodes.Count > 0, usedCallCodes.Min(), 1)

    ' 現在使われている番号帯から空きを探し、上限到達後は先頭側も探す
    For callCode As Integer = startCode To maxCallCode
      If Not usedCallCodes.Contains(callCode) Then
        availableCallCodes.Add(callCode.ToString("D" & CallCodeDigits.ToString()))
        If availableCallCodes.Count = requiredCount Then Return availableCallCodes
      End If
    Next

    For callCode As Integer = 1 To startCode - 1
      If Not usedCallCodes.Contains(callCode) Then
        availableCallCodes.Add(callCode.ToString("D" & CallCodeDigits.ToString()))
        If availableCallCodes.Count = requiredCount Then Return availableCallCodes
      End If
    Next

    If availableCallCodes.Count < requiredCount Then
      MessageBox.Show("呼出コードは最大" & CallCodeDigits.ToString() & "桁です。" & vbCrLf &
                      "使用できる空き番号が不足しているため、" & vbCrLf &
                      "行を追加できません。" & vbCrLf &
                      "管理者へお問い合わせください。",
                      "採番上限", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return Nothing
    End If

    Return availableCallCodes
  End Function


  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    If dt Is Nothing Then Return

    ' 入力エラー中の新規行も取り消せるよう、削除時は編集中セルだけを破棄する
    If ItemDetail.IsCurrentCellInEditMode Then ItemDetail.CancelEdit()

    ' 選択された行があるか確認
    If ItemDetail.SelectedRows.Count = 0 Then
      MessageBox.Show("削除する行を選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    Dim selectedDataRows As New List(Of DataRow)()
    For Each gridRow As DataGridViewRow In ItemDetail.SelectedRows
      Dim rowView As DataRowView = TryCast(gridRow.DataBoundItem, DataRowView)
      If rowView IsNot Nothing AndAlso Not selectedDataRows.Contains(rowView.Row) Then
        selectedDataRows.Add(rowView.Row)
      End If
    Next
    If selectedDataRows.Count = 0 Then Return

    Dim persistedCount As Integer = selectedDataRows.Where(
      Function(row) row.RowState <> DataRowState.Added AndAlso
                    row.RowState <> DataRowState.Detached).Count()
    Dim message As String = If(persistedCount > 0,
      "選択された登録済みデータを削除します。" & vbCrLf & "よろしいですか？",
      "選択された未登録行を取り消しますか？")
    If MessageBox.Show(message, "確認", MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question) <> DialogResult.OK Then Return

    Try
      If persistedCount > 0 Then
        Dim connectionString As String = SqlServer.GetConnectionString()
        Using connection As New SqlConnection(connectionString)
          connection.Open()
          Using transaction As SqlTransaction = connection.BeginTransaction()
            Try
              For Each row As DataRow In selectedDataRows
                If row.RowState = DataRowState.Added OrElse row.RowState = DataRowState.Detached Then Continue For

                Dim callCode As String = Convert.ToString(row("call_code", DataRowVersion.Original))
                Using command As New SqlCommand(
                    "DELETE FROM MST_Item WHERE call_code = @call_code", connection, transaction)
                  command.Parameters.AddWithValue("@call_code", callCode)
                  Call WriteExecuteLog([GetType]().Name,
                                       System.Reflection.MethodBase.GetCurrentMethod().Name,
                                       BuildCommandTextWithParams(command))
                  If command.ExecuteNonQuery() <> 1 Then
                    Throw New DBConcurrencyException(
                      "ほかの端末で更新または削除された商品があります。画面を開き直してください。")
                  End If
                End Using
              Next
              transaction.Commit()
            Catch
              Try
                transaction.Rollback()
              Catch rollbackException As Exception
                Call ComWriteErrLog([GetType]().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name, rollbackException)
              End Try
              Throw
            End Try
          End Using
        End Using
      End If

      ' DB再読込を行わず、対象行だけを外す。ほかの未登録・編集中データは保持する。
      For Each row As DataRow In selectedDataRows
        If row.RowState = DataRowState.Added Then
          dt.Rows.Remove(row)
        ElseIf row.RowState <> DataRowState.Detached Then
          row.Delete()
          row.AcceptChanges()
        End If
      Next

      MessageBox.Show(If(persistedCount > 0, "削除が完了しました。", "未登録行を取り消しました。"),
                      "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
    Catch ex As Exception
      MessageBox.Show("削除に失敗しました: " & ex.Message, "エラー",
                      MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
    End Try
  End Sub
  'Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
  '  Try
  '    ' ClsSqlServer を使用して接続情報を取得
  '    Dim connectionString As String = SqlServer.GetConnectionString()

  '    Using connection As New SqlConnection(connectionString)
  '      adapter = New SqlDataAdapter("SELECT * FROM MST_Item ORDER BY call_code", connection)
  '      Dim commandBuilder As New SqlCommandBuilder(adapter)

  '      ' DataTable の変更を確認
  '      If dt.GetChanges() IsNot Nothing Then
  '        ' データ保存前の重複チェック
  '        For Each row As DataRow In dt.Rows
  '          If row.RowState = DataRowState.Added Or row.RowState = DataRowState.Modified Then
  '            Dim callCode As String = If(row("call_code") IsNot DBNull.Value, row("call_code").ToString(), "")
  '            Dim itemNumber As String = If(row("item_number") IsNot DBNull.Value, row("item_number").ToString(), "")
  '            Dim itemName As String = If(row("item_name") IsNot DBNull.Value, row("item_name").ToString(), "")

  '            ' **現在の行（新規登録 or 編集中）を特定**
  '            Dim currentRowIndex As Integer = dt.Rows.IndexOf(row)

  '            ' **どれか1つでも重複していたらエラーを出す**
  '            Dim duplicateCallCode = dt.AsEnumerable().Where(Function(r) r("call_code").ToString() = callCode AndAlso Not r Is row).Any()
  '            Dim duplicateItemNumber = dt.AsEnumerable().Where(Function(r) r("item_number").ToString() = itemNumber AndAlso Not r Is row).Any()
  '            Dim duplicateItemName = dt.AsEnumerable().Where(Function(r) r("item_name").ToString() = itemName AndAlso Not r Is row).Any()

  '            Dim duplicateFields As New List(Of String)
  '            Dim duplicateColumn As String = ""

  '            If duplicateCallCode Then
  '              duplicateFields.Add("呼出コード")
  '              duplicateColumn = "call_code"
  '            End If
  '            If duplicateItemNumber Then
  '              duplicateFields.Add("品番")
  '              duplicateColumn = "item_number"
  '            End If
  '            If duplicateItemName Then
  '              duplicateFields.Add("品名")
  '              duplicateColumn = "item_name"
  '            End If

  '            If duplicateFields.Count > 0 Then
  '              ' **エラーメッセージを表示**
  '              MessageBox.Show(String.Join("、", duplicateFields) & " が既に存在しています。別の値を入力してください。", "重複エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)

  '              ' **現在編集中の行（新規登録 or 既存編集）にフォーカス**
  '              If currentRowIndex >= 0 AndAlso Not String.IsNullOrEmpty(duplicateColumn) Then
  '                ItemDetail.CurrentCell = ItemDetail.Rows(currentRowIndex).Cells(duplicateColumn)
  '              End If

  '              Return
  '            End If
  '          End If
  '        Next

  '        ' 更新された行の `update_date` を現在の日時に設定
  '        Dim currentDateTime As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
  '        For Each row As DataRow In dt.Rows
  '          If row.RowState = DataRowState.Modified Then
  '            row("update_date") = currentDateTime
  '          ElseIf row.RowState = DataRowState.Added Then
  '            ' 新規追加のデータは `create_date` と `update_date` を設定
  '            row("create_date") = currentDateTime
  '            row("update_date") = currentDateTime
  '          End If
  '        Next

  '        ' 🔽この位置にログ処理を追加
  '        For Each row As DataRow In dt.Rows
  '          If row.RowState = DataRowState.Added Or row.RowState = DataRowState.Modified Then
  '            Dim callCode As String = QuoteOrNull(row("call_code"))
  '            Dim itemNumber As String = QuoteOrNull(row("item_number"))
  '            Dim itemName As String = QuoteOrNull(row("item_name"))
  '            Dim packingBag As String = NumberOrNull(row("packing_bag"))
  '            Dim packingBagUnit As String = QuoteOrNull(row("packing_bag_unit"))
  '            Dim upperLimit As String = NumberOrNull(row("upper_limit"))
  '            Dim upperLimitUnit As String = QuoteOrNull(row("upper_limit_unit"))
  '            Dim referenceValue As String = NumberOrNull(row("reference_value"))
  '            Dim referenceValueUnit As String = QuoteOrNull(row("reference_value_unit"))
  '            Dim lowerLimit As String = NumberOrNull(row("lower_limit"))
  '            Dim lowerLimitUnit As String = QuoteOrNull(row("lower_limit_unit"))
  '            Dim subtotalTargetValue As String = NumberOrNull(row("subtotal_target_value"))
  '            Dim subtotalTargetValueUnit As String = QuoteOrNull(row("subtotal_target_value_unit"))
  '            Dim subtotalTargetCount As String = NumberOrNull(row("subtotal_target_count"))
  '            Dim createDate As String = If(row.RowState = DataRowState.Added, QuoteOrNull(currentDateTime), "NULL")
  '            Dim updateDate As String = QuoteOrNull(currentDateTime)

  '            Dim sql As String = ""

  '            If row.RowState = DataRowState.Added Then
  '              sql = "INSERT INTO MST_Item (call_code, item_number, item_name, packing_bag, packing_bag_unit, upper_limit, upper_limit_unit, reference_value, reference_value_unit, lower_limit, lower_limit_unit, subtotal_target_value, subtotal_target_value_unit, subtotal_target_count, create_date, update_date) VALUES (" &
  '          String.Join(", ", {callCode, itemNumber, itemName, packingBag, packingBagUnit, upperLimit, upperLimitUnit, referenceValue, referenceValueUnit, lowerLimit, lowerLimitUnit, subtotalTargetValue, subtotalTargetValueUnit, subtotalTargetCount, createDate, updateDate}) & ")"
  '            Else
  '              sql = "UPDATE MST_Item SET " &
  '          "item_number = " & itemNumber & ", " &
  '          "item_name = " & itemName & ", " &
  '          "packing_bag = " & packingBag & ", " &
  '          "packing_bag_unit = " & packingBagUnit & ", " &
  '          "upper_limit = " & upperLimit & ", " &
  '          "upper_limit_unit = " & upperLimitUnit & ", " &
  '          "reference_value = " & referenceValue & ", " &
  '          "reference_value_unit = " & referenceValueUnit & ", " &
  '          "lower_limit = " & lowerLimit & ", " &
  '          "lower_limit_unit = " & lowerLimitUnit & ", " &
  '          "subtotal_target_value = " & subtotalTargetValue & ", " &
  '          "subtotal_target_value_unit = " & subtotalTargetValueUnit & ", " &
  '          "subtotal_target_count = " & subtotalTargetCount & ", " &
  '          "update_date = " & updateDate & " " &
  '          "WHERE call_code = " & callCode
  '            End If

  '            Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '          End If
  '        Next
  '        ' 🔼ログ処理ここまで

  '        ' データベースへ更新
  '        adapter.Update(dt)

  '        MessageBox.Show("データを保存しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)

  '        ' 再読み込み
  '        LoadData()
  '      Else
  '        MessageBox.Show("変更されたデータがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
  '      End If
  '    End Using

  '  Catch ex As Exception
  '    MessageBox.Show("データの保存に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '    Call ComWriteErrLog([GetType]().Name,
  '                System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
  '  End Try
  'End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    If dt Is Nothing Then
      MessageBox.Show("商品データが読み込まれていません。再度画面を開いてください。", "エラー",
                      MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return
    End If

    If Not TryCommitPendingEdit() Then Return

    If dt.GetChanges() Is Nothing Then
      MessageBox.Show("変更されたデータがありません。", "情報",
                      MessageBoxButtons.OK, MessageBoxIcon.Information)
      Return
    End If

    If Not ValidateChangedRows() Then Return

    If MessageBox.Show("変更内容を登録します。" & vbCrLf & "よろしいですか？",
                       "確認", MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question) <> DialogResult.Yes Then Return

    Try
      Dim connectionString As String = SqlServer.GetConnectionString()
      Using connection As New SqlConnection(connectionString)
        connection.Open()
        Using transaction As SqlTransaction = connection.BeginTransaction()
          Try
            Dim currentDateTime As DateTime = DateTime.Now

            For Each row As DataRow In dt.Rows
              If row.RowState <> DataRowState.Modified AndAlso row.RowState <> DataRowState.Added Then Continue For

              ' 通常の商品マスタ詳細と同じく、未入力の数値項目は登録時に0として扱う
              ' 未入力値は計数版の従来仕様どおりDBNullを維持する。

              Dim isAdded As Boolean = row.RowState = DataRowState.Added
              row("update_date") = currentDateTime
              If isAdded Then row("create_date") = currentDateTime

              Using command As New SqlCommand()
                command.Connection = connection
                command.Transaction = transaction
                If isAdded Then
                  command.CommandText =
                    "INSERT INTO MST_Item (call_code, item_number, item_name, packing_bag, packing_bag_unit, upper_limit, upper_limit_unit, reference_value, reference_value_unit, lower_limit, lower_limit_unit, subtotal_target_value, subtotal_target_value_unit, subtotal_target_count, create_date, update_date) " &
                    "VALUES (@call_code, @item_number, @item_name, @packing_bag, @packing_bag_unit, @upper_limit, @upper_limit_unit, @reference_value, @reference_value_unit, @lower_limit, @lower_limit_unit, @subtotal_target_value, @subtotal_target_value_unit, @subtotal_target_count, @create_date, @update_date)"
                Else
                  command.CommandText =
                    "UPDATE MST_Item SET call_code = @call_code, item_number = @item_number, item_name = @item_name, packing_bag = @packing_bag, packing_bag_unit = @packing_bag_unit, upper_limit = @upper_limit, upper_limit_unit = @upper_limit_unit, reference_value = @reference_value, reference_value_unit = @reference_value_unit, lower_limit = @lower_limit, lower_limit_unit = @lower_limit_unit, subtotal_target_value = @subtotal_target_value, subtotal_target_value_unit = @subtotal_target_value_unit, subtotal_target_count = @subtotal_target_count, update_date = @update_date WHERE call_code = @original_call_code"
                  command.Parameters.AddWithValue("@original_call_code",
                                                  row("call_code", DataRowVersion.Original))
                End If

                command.Parameters.AddWithValue("@call_code", row("call_code"))
                command.Parameters.AddWithValue("@item_number", row("item_number"))
                command.Parameters.AddWithValue("@item_name", row("item_name"))
                command.Parameters.AddWithValue("@packing_bag", row("packing_bag"))
                command.Parameters.AddWithValue("@packing_bag_unit", row("packing_bag_unit"))
                command.Parameters.AddWithValue("@upper_limit", row("upper_limit"))
                command.Parameters.AddWithValue("@upper_limit_unit", row("upper_limit_unit"))
                command.Parameters.AddWithValue("@reference_value", row("reference_value"))
                command.Parameters.AddWithValue("@reference_value_unit", row("reference_value_unit"))
                command.Parameters.AddWithValue("@lower_limit", row("lower_limit"))
                command.Parameters.AddWithValue("@lower_limit_unit", row("lower_limit_unit"))
                command.Parameters.AddWithValue("@subtotal_target_value", row("subtotal_target_value"))
                command.Parameters.AddWithValue("@subtotal_target_value_unit", row("subtotal_target_value_unit"))
                command.Parameters.AddWithValue("@subtotal_target_count", row("subtotal_target_count"))
                command.Parameters.AddWithValue("@update_date", row("update_date"))
                If isAdded Then command.Parameters.AddWithValue("@create_date", row("create_date"))

                Call WriteExecuteLog([GetType]().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                                     BuildCommandTextWithParams(command))
                If command.ExecuteNonQuery() <> 1 Then
                  Throw New DBConcurrencyException(
                    "ほかの端末で更新または削除された商品があります。画面を開き直してください。")
                End If
              End Using
            Next

            transaction.Commit()
          Catch
            Try
              transaction.Rollback()
            Catch rollbackException As Exception
              Call ComWriteErrLog([GetType]().Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name, rollbackException)
            End Try
            Throw
          End Try
        End Using
      End Using

      MessageBox.Show("データを保存しました。", "成功",
                      MessageBoxButtons.OK, MessageBoxIcon.Information)
      LoadData()
    Catch ex As Exception
      MessageBox.Show("データの保存に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
    End Try
  End Sub

  Private Function ValidateChangedRows() As Boolean
    For Each row As DataRow In dt.Rows
      If row.RowState <> DataRowState.Added AndAlso row.RowState <> DataRowState.Modified Then Continue For

      Dim callCode As String = Convert.ToString(row("call_code")).Trim()
      Dim callCodeNumber As Integer
      If String.IsNullOrEmpty(callCode) OrElse
         callCode.Length > CallCodeDigits OrElse
         Not Integer.TryParse(callCode, callCodeNumber) OrElse callCodeNumber <= 0 Then
        Return ShowRowValidationError(row, "call_code",
          "呼出コードは1～" & CallCodeDigits.ToString() & "桁の数字で入力してください。" & vbCrLf &
          "全桁0は登録できません。")
      End If
      callCode = callCodeNumber.ToString("D" & CallCodeDigits.ToString())
      row("call_code") = callCode

      Dim itemNumber As String = Convert.ToString(row("item_number")).Trim()
      Dim itemName As String = Convert.ToString(row("item_name")).Trim()
      If itemNumber.Length > 20 Then
        Return ShowRowValidationError(row, "item_number", "品番は20文字以内で入力してください。")
      End If
      If itemName.Length > 50 Then
        Return ShowRowValidationError(row, "item_name", "品名は50文字以内で入力してください。")
      End If

      If Not ValidateDecimalValue(row, "upper_limit", "上限値", "upper_limit_unit") OrElse
         Not ValidateDecimalValue(row, "reference_value", "基準値") OrElse
         Not ValidateDecimalValue(row, "lower_limit", "下限値", "lower_limit_unit") Then
        Return False
      End If

      Dim subtotalCount As Integer
      If Not row.IsNull("subtotal_target_count") AndAlso
         (Not Integer.TryParse(Convert.ToString(row("subtotal_target_count")), subtotalCount) OrElse
          subtotalCount < 0 OrElse subtotalCount > 999999) Then
        Return ShowRowValidationError(row, "call_code",
          "小計目標回数は0～999999の範囲で入力してください。")
      End If

      For Each columnName In New String() {"packing_bag_unit", "upper_limit_unit", "reference_value_unit", "lower_limit_unit", "subtotal_target_value_unit"}
        If Convert.ToString(row(columnName)).Length > 3 Then
          Return ShowRowValidationError(row, columnName, "単位は3文字以内で入力してください。")
        End If
      Next

      row("item_number") = itemNumber
      row("item_name") = itemName

      For Each otherRow As DataRow In dt.Rows
        If otherRow Is row OrElse otherRow.RowState = DataRowState.Deleted Then Continue For

        Dim otherCallCode As String = NormalizeCallCode(Convert.ToString(otherRow("call_code")))
        If String.Equals(otherCallCode, callCode, StringComparison.Ordinal) Then
          Return ShowRowValidationError(row, "call_code",
            "呼出コードは既に存在しています。別の値を入力してください。")
        End If

      Next
    Next

    Return True
  End Function

  Private Function ValidateDecimalValue(row As DataRow, columnName As String,
                                        displayName As String,
                                        Optional unitColumnName As String = Nothing) As Boolean
    If row.IsNull(columnName) OrElse String.IsNullOrWhiteSpace(Convert.ToString(row(columnName))) Then
      Return True
    End If

    Dim value As Decimal
    If Not Decimal.TryParse(Convert.ToString(row(columnName)), value) OrElse
       value < 0D OrElse value > 99999.99D OrElse Decimal.Round(value, 2) <> value Then
      Return ShowRowValidationError(row, columnName,
        displayName & "は0～99999.99の範囲で、小数点以下2桁まで入力してください。")
    End If


    Return True
  End Function

  Private Function NormalizeCallCode(value As String) As String
    Dim number As Integer
    value = If(value, String.Empty).Trim()
    If Integer.TryParse(value, number) AndAlso number >= 0 Then
      Return number.ToString("D" & CallCodeDigits.ToString())
    End If
    Return value
  End Function

  Private Function ShowRowValidationError(row As DataRow, columnName As String,
                                          message As String) As Boolean
    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    Dim rowIndex As Integer = dt.Rows.IndexOf(row)
    If rowIndex >= 0 AndAlso rowIndex < ItemDetail.Rows.Count Then
      ItemDetail.ClearSelection()
      ItemDetail.Rows(rowIndex).Selected = True
      ItemDetail.CurrentCell = ItemDetail.Rows(rowIndex).Cells(columnName)
      ItemDetail.CurrentCell.Style.BackColor = Color.MistyRose
      ItemDetail.CurrentCell.Style.SelectionBackColor = Color.Maroon
      ItemDetail.BeginEdit(True)
    End If
    Return False
  End Function


  Private Function BuildCommandTextWithParams(cmd As SqlCommand) As String
    Dim sql As String = cmd.CommandText

    ' パラメータ名が長い順に並べ替え（部分一致バグ対策）
    Dim sortedParams = cmd.Parameters.Cast(Of SqlParameter)().OrderByDescending(Function(p) p.ParameterName.Length)

    For Each p As SqlParameter In sortedParams
      Dim value As String

      If p.Value Is DBNull.Value OrElse p.Value Is Nothing Then
        value = "NULL"
      ElseIf TypeOf p.Value Is String OrElse TypeOf p.Value Is DateTime Then
        value = "'" & p.Value.ToString().Replace("'", "''") & "'"
      Else
        value = p.Value.ToString()
      End If

      sql = sql.Replace(p.ParameterName, value)
    Next

    Return sql
  End Function



  Private Function QuoteOrNull(value As Object) As String
    If value Is DBNull.Value OrElse value Is Nothing Then
      Return "NULL"
    Else
      Return "'" & value.ToString().Replace("'", "''") & "'"
    End If
  End Function

  Private Function NumberOrNull(value As Object) As String
    If value Is DBNull.Value OrElse value Is Nothing Then
      Return "NULL"
    Else
      Return value.ToString()
    End If
  End Function


  Private Sub ItemDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles ItemDetail.KeyDown
    ' DELETE キーが押された場合、DeleteButton のクリックイベントを呼び出す
    If e.KeyCode = Keys.Delete AndAlso Not ItemDetail.IsCurrentCellInEditMode Then
      e.SuppressKeyPress = True ' デフォルトの削除動作を防止
      DeleteButton_Click(DeleteButton, EventArgs.Empty) ' 削除ボタンの動作を実行
    End If
  End Sub

  Private Sub ItemDetail_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles ItemDetail.DataError
    e.ThrowException = False
    If e.Exception IsNot Nothing Then
      Call ComWriteErrLog([GetType]().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, e.Exception)
    End If

    If (e.Context And DataGridViewDataErrorContexts.Commit) <> 0 OrElse
       (e.Context And DataGridViewDataErrorContexts.Parsing) <> 0 Then
      MessageBox.Show("入力値を確定できません。選択肢または数値形式を確認してください。",
                      "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End If
  End Sub

  ' DataGridView の CellValidating イベントを処理
  Private Sub ItemDetail_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles ItemDetail.CellValidating

    If isClosing Then Return

    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name
    Dim headerText As String = ItemDetail.Columns(e.ColumnIndex).HeaderText ' ヘッダー名を取得
    Dim cellValue As String = Convert.ToString(e.FormattedValue).Trim()

    ' 通常の商品マスタ詳細と同じ必須項目
    Dim requiredColumns As String() = {"call_code"}
    If requiredColumns.Contains(columnName) AndAlso String.IsNullOrEmpty(cellValue) Then
      MessageBox.Show(headerText & " は必須入力です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      e.Cancel = True
      Return
    End If

    If columnName = "call_code" Then
      Dim callCodeNumber As Integer
      If cellValue.Length > CallCodeDigits OrElse
         Not Integer.TryParse(cellValue, callCodeNumber) OrElse callCodeNumber <= 0 Then
        MessageBox.Show("呼出コードは1～" & CallCodeDigits.ToString() &
                        "桁の数字で入力してください。" & vbCrLf &
                        "全桁0は登録できません。",
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If
    End If

    ' 呼出コード・入力済みの品番・品名は一覧内で重複させない
    If columnName = "call_code" Then
      Dim comparisonValue As String = If(columnName = "call_code", NormalizeCallCode(cellValue), cellValue)
      Dim duplicateCount As Integer = 0
      For Each row As DataGridViewRow In ItemDetail.Rows
        If Not row.IsNewRow AndAlso row.Index <> e.RowIndex Then ' 現在編集している行を除外
          Dim otherValue As String = Convert.ToString(row.Cells(columnName).Value).Trim()
          If columnName = "call_code" Then otherValue = NormalizeCallCode(otherValue)
          If Not String.IsNullOrEmpty(comparisonValue) AndAlso
             String.Equals(otherValue, comparisonValue, StringComparison.OrdinalIgnoreCase) Then
            duplicateCount += 1
          End If
        End If
      Next
      If duplicateCount > 0 Then
        MessageBox.Show(headerText & " は既に存在しています。別の値を入力してください。", "重複エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If
    End If

    ' **文字列長チェック（varchar）**
    Dim maxLengths As New Dictionary(Of String, Integer) From {
        {"call_code", CallCodeDigits}, {"item_number", 20}, {"item_name", 50},
        {"packing_bag_unit", 3}, {"upper_limit_unit", 3}, {"reference_value_unit", 3},
        {"lower_limit_unit", 3}, {"subtotal_target_value_unit", 3}
    }
    If maxLengths.ContainsKey(columnName) AndAlso cellValue.Length > maxLengths(columnName) Then
      MessageBox.Show(headerText & " の最大桁数は " & maxLengths(columnName) & " 文字です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      e.Cancel = True
      Return
    End If

    ' **小数チェック（decimal(7,2)）**
    Dim decimalColumns As String() = {"packing_bag", "upper_limit", "reference_value", "lower_limit", "subtotal_target_value"}
    If decimalColumns.Contains(columnName) Then
      ' 空欄は登録時に0へ変換するため、入力チェックでは許可する
      If String.IsNullOrEmpty(cellValue) Then Return

      Dim value As Decimal
      If Not Decimal.TryParse(cellValue, value) OrElse value < 0 OrElse value > 99999.99D Then
        MessageBox.Show(headerText & " は 0～99999.99 の範囲で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If

      ' **小数点以下2桁チェック**
      If (value * 100) Mod 1 <> 0 Then
        MessageBox.Show(headerText & " は小数点以下2桁まで入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If


    End If

    ' **整数チェック（subtotal_target_count のみ）**
    If columnName = "subtotal_target_count" Then
      ' 空欄は登録時に0へ変換するため、入力チェックでは許可する
      If String.IsNullOrEmpty(cellValue) Then Return

      Dim intValue As Integer
      If Not Integer.TryParse(cellValue, intValue) OrElse intValue < 0 OrElse intValue > 999999 Then
        MessageBox.Show(headerText & " は 0～999999 の範囲で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If
    End If
  End Sub

  Private Sub ItemDetail_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles ItemDetail.CellBeginEdit
    If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name
    If columnName = "upper_limit_unit" OrElse columnName = "lower_limit_unit" Then
      previousLimitUnit = Convert.ToString(ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
    End If
  End Sub

  Private Sub ItemDetail_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles ItemDetail.CurrentCellDirtyStateChanged
    If Not ItemDetail.IsCurrentCellDirty OrElse ItemDetail.CurrentCell Is Nothing Then Return

    Dim columnName As String = ItemDetail.Columns(ItemDetail.CurrentCell.ColumnIndex).Name
    If columnName = "upper_limit_unit" OrElse columnName = "lower_limit_unit" Then
      isUserLimitUnitChange = True
      ItemDetail.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End If
  End Sub

  Private Sub ItemDetail_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles ItemDetail.CellValueChanged
    If isRevertingLimitUnit Then Return
    If Not isUserLimitUnitChange OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return

    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name
    If columnName <> "upper_limit_unit" AndAlso columnName <> "lower_limit_unit" Then Return

    isUserLimitUnitChange = False
    Dim newUnit As String = Convert.ToString(ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
    If String.Equals(previousLimitUnit, newUnit, StringComparison.Ordinal) Then Return

    Dim valueColumnName As String = If(columnName = "upper_limit_unit", "upper_limit", "lower_limit")
    Dim currentValue As Decimal
    Decimal.TryParse(Convert.ToString(ItemDetail.Rows(e.RowIndex).Cells(valueColumnName).Value), currentValue)

    If currentValue <> 0D Then
      Dim result As DialogResult = MessageBox.Show(
        ItemDetail.Columns(valueColumnName).HeaderText & "の単位を変更すると、現在値 " &
        currentValue.ToString("0.##") & " は0に戻ります。" & vbCrLf & "変更しますか？",
        "単位変更の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
      If result <> DialogResult.Yes Then
        isRevertingLimitUnit = True
        Try
          ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = previousLimitUnit
        Finally
          isRevertingLimitUnit = False
        End Try
        Return
      End If
    End If

    ItemDetail.Rows(e.RowIndex).Cells(valueColumnName).Value = 0D
  End Sub


  ' DataGridView の CellEndEdit イベントを処理（INI設定の桁数で call_code をゼロ埋め）
  Private Sub ItemDetail_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles ItemDetail.CellEndEdit
    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name

    ' **call_code のゼロ埋め処理**
    If columnName = "call_code" Then
      Dim rawValue As Object = ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
      If rawValue Is Nothing OrElse rawValue Is DBNull.Value Then Return
      Dim cellValue As String = rawValue.ToString().Trim()
      Dim intValue As Integer

      ' 数値に変換できる場合のみゼロ埋め
      If Integer.TryParse(cellValue, intValue) Then
        ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = intValue.ToString("D" & CallCodeDigits.ToString())
      End If
    End If
  End Sub

  ' DataGridView の EditingControlShowing イベントを処理
  Private Sub ItemDetail_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles ItemDetail.EditingControlShowing
    Dim columnName As String = ItemDetail.Columns(ItemDetail.CurrentCell.ColumnIndex).Name
    Dim textBox As TextBox = TryCast(e.Control, TextBox)

    If textBox IsNot Nothing Then
      ' 既存のイベントハンドラを削除（複数回追加されるのを防ぐ）
      RemoveHandler textBox.KeyPress, AddressOf NumericOnly_KeyPress
      RemoveHandler textBox.KeyPress, AddressOf DecimalOnly_KeyPress
      RemoveHandler textBox.KeyPress, AddressOf StringOnly_KeyPress

      ' **桁数制限の設定**
      Dim maxLengths As New Dictionary(Of String, Integer) From {
            {"call_code", CallCodeDigits}, {"subtotal_target_count", 6},
            {"item_number", 20}, {"item_name", 50},
            {"packing_bag_unit", 3}, {"upper_limit_unit", 3}, {"reference_value_unit", 3},
            {"lower_limit_unit", 3}, {"subtotal_target_value_unit", 3}
        }
      If maxLengths.ContainsKey(columnName) Then
        textBox.MaxLength = maxLengths(columnName) ' 最大桁数を設定
      ElseIf columnName = "upper_limit" OrElse columnName = "lower_limit" Then
        Dim unitColumnName As String = If(columnName = "upper_limit", "upper_limit_unit", "lower_limit_unit")
        Dim unitValue As String = Convert.ToString(ItemDetail.CurrentRow.Cells(unitColumnName).Value)
        textBox.MaxLength = If(unitValue = "%", 5, 8)
      ElseIf {"packing_bag", "reference_value", "subtotal_target_value"}.Contains(columnName) Then
        textBox.MaxLength = 8
      Else
        textBox.MaxLength = 0 ' デフォルト（制限なし）
      End If

      ' **数値のみ許可**
      Dim numericColumns As String() = {"call_code", "subtotal_target_count"}
      If numericColumns.Contains(columnName) Then
        AddHandler textBox.KeyPress, AddressOf NumericOnly_KeyPress
      End If

      ' **小数のみ許可（decimal(7,2)）**
      Dim decimalColumns As String() = {"packing_bag", "upper_limit", "reference_value", "lower_limit", "subtotal_target_value"}
      If decimalColumns.Contains(columnName) Then
        AddHandler textBox.KeyPress, AddressOf DecimalOnly_KeyPress
      End If

      ' **文字列のみ許可（varchar）**
      Dim varcharColumns As String() = {"item_number", "packing_bag_unit", "upper_limit_unit", "reference_value_unit", "lower_limit_unit", "subtotal_target_value_unit"}
      If varcharColumns.Contains(columnName) Then
        AddHandler textBox.KeyPress, AddressOf StringOnly_KeyPress
      End If

      ' **item_name は日本語 OK**
      If columnName = "item_name" Then
        RemoveHandler textBox.KeyPress, AddressOf StringOnly_KeyPress ' 制限を解除
      End If
    End If
  End Sub


  ' **整数のみ許可（call_code, subtotal_target_count）**
  Private Sub NumericOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
    Dim textBox As TextBox = TryCast(sender, TextBox)
    If textBox Is Nothing Then Return

    ' **数字、バックスペース、削除キーのみ許可**
    If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
      e.Handled = True ' 入力を禁止
    End If

    ' **最大桁数チェック（選択中の文字は入力時に置換される）**
    Dim remainingLength As Integer = textBox.Text.Length - textBox.SelectionLength
    If textBox.MaxLength > 0 AndAlso remainingLength >= textBox.MaxLength AndAlso
       Not Char.IsControl(e.KeyChar) Then
      e.Handled = True
    End If
  End Sub



  ' **小数のみ許可（decimal(7,2) 型: packing_bag, upper_limit など）**
  Private Sub DecimalOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
    Dim textBox As TextBox = TryCast(sender, TextBox)
    If textBox Is Nothing OrElse Char.IsControl(e.KeyChar) Then Return

    If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
      e.Handled = True
      Return
    End If

    Dim proposedText As String = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength).
      Insert(textBox.SelectionStart, e.KeyChar.ToString())
    Dim parts As String() = proposedText.Split("."c)
    Dim maxIntegerDigits As Integer = If(textBox.MaxLength = 5, 2, 5)

    If parts.Length > 2 OrElse
       parts(0).Length > maxIntegerDigits OrElse
       (parts.Length = 2 AndAlso (parts(0).Length = 0 OrElse parts(1).Length > 2)) Then
      e.Handled = True
    End If
  End Sub

  ' **文字列のみ許可（varchar 型）**
  Private Sub StringOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
    Dim textBox As TextBox = TryCast(sender, TextBox)
    If textBox Is Nothing Then Return

    ' **最大桁数チェック（選択中の文字は入力時に置換される）**
    Dim remainingLength As Integer = textBox.Text.Length - textBox.SelectionLength
    If textBox.MaxLength > 0 AndAlso remainingLength >= textBox.MaxLength AndAlso
       Not Char.IsControl(e.KeyChar) Then
      e.Handled = True
    End If
  End Sub

  Private Sub Form_ItemList_GridMethod_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        AddRowButton.PerformClick()
      Case Keys.F2
        CopyRowButton.PerformClick()
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.F6
        DeleteButton.PerformClick()
      Case Keys.Escape
        e.SuppressKeyPress = True
        CloseButton.PerformClick()
    End Select
  End Sub
  Private Sub ApplyCommonLayout_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    Common.ClsFunction.ApplyScreenLayout(Me)
  End Sub
End Class

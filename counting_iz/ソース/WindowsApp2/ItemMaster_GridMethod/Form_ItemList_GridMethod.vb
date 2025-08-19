Imports System.Data.SqlClient
Imports Common
Imports Common.ClsFunction

Public Class Form_ItemList_GridMethod
  Private dt As DataTable
  Private adapter As SqlDataAdapter

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
    LoadData()
    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

    Me.FormBorderStyle = FormBorderStyle.FixedDialog ' ウィンドウのサイズ変更不可
    Me.ControlBox = False ' ×（閉じる）、最大化、最小化ボタンを非表示

    NumberCheckBox.Checked = True
    AddRowTextBox.Text = 1
  End Sub

  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With ItemDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
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

      ' ヘッダー名変更 & 不要なカラム非表示
      With ItemDetail
        .Columns("call_code").HeaderText = "呼出番号"
        .Columns("item_number").HeaderText = "品番"
        .Columns("item_name").HeaderText = "品名"

        ' item_number と item_name の幅を広げる
        .Columns("item_number").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        .Columns("item_name").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        .Columns("packing_bag").HeaderText = "風袋"
        .Columns("packing_bag_unit").HeaderText = "単位"
        .Columns("upper_limit").HeaderText = "上限値"
        .Columns("upper_limit_unit").HeaderText = "単位"
        .Columns("reference_value").HeaderText = "基準値"
        .Columns("reference_value_unit").HeaderText = "単位"
        .Columns("lower_limit").HeaderText = "下限値"
        .Columns("lower_limit_unit").HeaderText = "単位"
        .Columns("subtotal_target_value").HeaderText = "小計目標値"
        ' subtotal_target_value の幅を広げる
        .Columns("subtotal_target_value").Width = 110

        .Columns("subtotal_target_value_unit").HeaderText = "単位"
        .Columns("subtotal_target_count").HeaderText = "回数"

        ' 作成日・更新日は非表示
        .Columns("create_date").Visible = False
        .Columns("update_date").Visible = False
      End With

      ' ✅ 1行目を選択状態にする（データがある場合）
      If dt.Rows.Count > 0 Then
        ItemDetail.ClearSelection()
        ItemDetail.Rows(0).Selected = True
        ItemDetail.CurrentCell = ItemDetail.Rows(0).Cells(0)
      End If

    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      MessageBox.Show("データの読み込みに失敗しました: " & ex.Message)
    End Try
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    If dt.GetChanges() IsNot Nothing Then
      Dim message As String = "現在、編集中のデータがあります。" & vbCrLf &
                              "このまま閉じると変更が失われますが、" & vbCrLf &
                              "よろしいですか？"

      Dim result As DialogResult = MessageBox.Show(message, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)

      If result = DialogResult.OK Then
        Me.Close() ' フォームを閉じる
      End If
    Else
      Me.Close()
    End If
  End Sub

  Private Sub AddRowButton_Click(sender As Object, e As EventArgs) Handles AddRowButton.Click
    ' 入力された行数を取得
    Dim rowCount As Integer
    If Not Integer.TryParse(AddRowTextBox.Text, rowCount) OrElse rowCount <= 0 Then
      MessageBox.Show("追加する行数を正しく入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    Try
      ' call_code の最大値を取得（NumberCheckBox がチェックされている場合のみ）
      Dim maxCallCode As Integer = 0
      If NumberCheckBox.Checked AndAlso dt.Rows.Count > 0 Then
        '' call_code の最大値を取得（NULLでないデータのみ対象）
        'Dim existingCallCodes = dt.AsEnumerable().
        '        Where(Function(row) Not IsDBNull(row("call_code")) AndAlso Not String.IsNullOrWhiteSpace(row("call_code").ToString())).
        '        Select(Function(row) Convert.ToInt32(row("call_code")))

        'If existingCallCodes.Any() Then
        '  maxCallCode = existingCallCodes.Max()
        'End
        '
        '上記のLINQをComputeに変更
        Dim maxValue = dt.Compute("MAX(call_code)", "call_code IS NOT NULL")
        maxCallCode = If(IsDBNull(maxValue), 0, Convert.ToInt32(maxValue))
      End If

      ' DataTable に新規行を追加
      For i As Integer = 1 To rowCount
        Dim newRow As DataRow = dt.NewRow() ' 空の新規行を作成

        ' NumberCheckBox がチェックされている場合は call_code を採番（6桁のゼロ埋め）
        If NumberCheckBox.Checked Then
          maxCallCode += 1
          newRow("call_code") = maxCallCode.ToString("D6") ' 6桁のゼロ埋め
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

      ' DataGridView を更新
      ItemDetail.DataSource = dt

      ' 最後に追加した行へフォーカスを移動
      If dt.Rows.Count > 0 Then
        ItemDetail.CurrentCell = ItemDetail.Rows(dt.Rows.Count - 1).Cells(0)
      End If

    Catch ex As Exception
      MessageBox.Show("行の追加に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
    End Try
  End Sub
  Private Sub CopyRowButton_Click(sender As Object, e As EventArgs) Handles CopyRowButton.Click
    ' 選択された行があるか確認
    If ItemDetail.SelectedRows.Count = 0 Then
      MessageBox.Show("コピーする行を選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    Try
      ' call_code の最大値を取得（NumberCheckBox がチェックされている場合のみ）
      Dim maxCallCode As Integer = 0
      If NumberCheckBox.Checked AndAlso dt.Rows.Count > 0 Then
        ' call_code の最大値を取得（NULLでないデータのみ対象）
        'Dim existingCallCodes = dt.AsEnumerable().
        '        Where(Function(row) Not IsDBNull(row("call_code")) AndAlso Not String.IsNullOrWhiteSpace(row("call_code").ToString())).
        '        Select(Function(row) Convert.ToInt32(row("call_code")))

        'If existingCallCodes.Any() Then
        '  maxCallCode = existingCallCodes.Max()
        'End If

        '上記のLINQをComputeに変更
        Dim maxValue = dt.Compute("MAX(call_code)", "call_code IS NOT NULL")
        maxCallCode = If(IsDBNull(maxValue), 0, Convert.ToInt32(maxValue))

      End If

      ' 選択された行をコピーして新規追加
      For Each row As DataGridViewRow In ItemDetail.SelectedRows
        ' DataRow を作成
        Dim newRow As DataRow = dt.NewRow()

        ' call_code の採番
        If NumberCheckBox.Checked Then
          maxCallCode += 1
          newRow("call_code") = maxCallCode.ToString("D6") ' 6桁のゼロ埋め
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

      ' DataGridView を更新
      ItemDetail.DataSource = dt

      ' 最後に追加した行へフォーカスを移動
      If dt.Rows.Count > 0 Then
        ItemDetail.CurrentCell = ItemDetail.Rows(dt.Rows.Count - 1).Cells(0)
      End If

    Catch ex As Exception
      MessageBox.Show("行のコピーに失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
    End Try
  End Sub


  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    ' 選択された行があるか確認
    If ItemDetail.SelectedRows.Count = 0 Then
      MessageBox.Show("削除する行を選択してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    ' 削除確認メッセージ
    Dim result As DialogResult = MessageBox.Show("選択された行を削除しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

    ' OK が押された場合
    If result = DialogResult.OK Then
      Try
        ' ClsSqlServer を使用して接続情報を取得
        Dim connectionString As String = SqlServer.GetConnectionString()

        Using connection As New SqlConnection(connectionString)
          connection.Open()

          ' トランザクションを開始
          Using transaction As SqlTransaction = connection.BeginTransaction()
            Try
              ' 選択された行をループ処理
              For Each row As DataGridViewRow In ItemDetail.SelectedRows
                ' call_code（主キー）を取得
                Dim callCode As String = row.Cells("call_code").Value.ToString()

                ' SQL DELETE コマンド
                Dim query As String = "DELETE FROM MST_Item WHERE call_code = @call_code"

                ' ログ用にパラメータ展開したSQL文字列を生成（文字列連結）
                Dim logQuery As String = "DELETE FROM MST_Item WHERE call_code = '" & callCode.Replace("'", "''") & "'"

                Using command As New SqlCommand(query, connection, transaction)
                  command.Parameters.AddWithValue("@call_code", callCode)
                  command.ExecuteNonQuery()
                  Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, logQuery)
                End Using
              Next

              ' トランザクションをコミット
              transaction.Commit()

              ' 削除成功メッセージ
              MessageBox.Show("削除が完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
              ' エラーが発生した場合はロールバック
              transaction.Rollback()
              MessageBox.Show("削除に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
              Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
            End Try
          End Using
        End Using

        ' DataGridView を再読み込み
        LoadData()

      Catch ex As Exception
        MessageBox.Show("データベース接続エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Call ComWriteErrLog([GetType]().Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      End Try
    End If
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
  '                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '  End Try
  'End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    Try
      Dim connectionString As String = SqlServer.GetConnectionString()
      Using connection As New SqlConnection(connectionString)
        connection.Open()
        Dim transaction = connection.BeginTransaction()

        ' 変更のある行を確認
        If dt.GetChanges() IsNot Nothing Then

          For Each row As DataRow In dt.Rows
            If row.RowState = DataRowState.Added Or row.RowState = DataRowState.Modified Then
              Dim callCode As String = row("call_code").ToString()
              Dim itemNumber As String = row("item_number").ToString()
              Dim itemName As String = row("item_name").ToString()

              Dim currentRowIndex As Integer = dt.Rows.IndexOf(row)

              Dim duplicateCallCode = dt.AsEnumerable().Where(Function(r) r("call_code").ToString() = callCode AndAlso Not r Is row).Any()
              Dim duplicateItemNumber = dt.AsEnumerable().Where(Function(r) r("item_number").ToString() = itemNumber AndAlso Not r Is row).Any()
              Dim duplicateItemName = dt.AsEnumerable().Where(Function(r) r("item_name").ToString() = itemName AndAlso Not r Is row).Any()

              Dim duplicateFields As New List(Of String)
              Dim duplicateColumn As String = ""

              If duplicateCallCode Then
                duplicateFields.Add("呼出コード")
                duplicateColumn = "call_code"
              End If
              If duplicateItemNumber Then
                duplicateFields.Add("品番")
                duplicateColumn = "item_number"
              End If
              If duplicateItemName Then
                duplicateFields.Add("品名")
                duplicateColumn = "item_name"
              End If

              If duplicateFields.Count > 0 Then
                MessageBox.Show(String.Join("、", duplicateFields) & " が既に存在しています。別の値を入力してください。", "重複エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                If currentRowIndex >= 0 AndAlso Not String.IsNullOrEmpty(duplicateColumn) Then
                  ItemDetail.CurrentCell = ItemDetail.Rows(currentRowIndex).Cells(duplicateColumn)
                End If
                transaction.Rollback()
                Return
              End If
            End If
          Next

          ' 明示的なDB更新
          Dim currentDateTime As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

          For Each row As DataRow In dt.Rows
            If row.RowState = DataRowState.Modified Or row.RowState = DataRowState.Added Then

              Dim commandText As String = ""
              Dim command As New SqlCommand()
              command.Connection = connection
              command.Transaction = transaction

              If row.RowState = DataRowState.Added Then
                row("create_date") = currentDateTime
                row("update_date") = currentDateTime
                commandText = "INSERT INTO MST_Item (call_code, item_number, item_name, packing_bag, packing_bag_unit, upper_limit, upper_limit_unit, reference_value, reference_value_unit, lower_limit, lower_limit_unit, subtotal_target_value, subtotal_target_value_unit, subtotal_target_count, create_date, update_date) " &
                                          "VALUES (@call_code, @item_number, @item_name, @packing_bag, @packing_bag_unit, @upper_limit, @upper_limit_unit, @reference_value, @reference_value_unit, @lower_limit, @lower_limit_unit, @subtotal_target_value, @subtotal_target_value_unit, @subtotal_target_count, @create_date, @update_date)"
              ElseIf row.RowState = DataRowState.Modified Then
                row("update_date") = currentDateTime
                commandText = "UPDATE MST_Item SET item_number = @item_number, item_name = @item_name, packing_bag = @packing_bag, packing_bag_unit = @packing_bag_unit, upper_limit = @upper_limit, upper_limit_unit = @upper_limit_unit, reference_value = @reference_value, reference_value_unit = @reference_value_unit, lower_limit = @lower_limit, lower_limit_unit = @lower_limit_unit, subtotal_target_value = @subtotal_target_value, subtotal_target_value_unit = @subtotal_target_value_unit, subtotal_target_count = @subtotal_target_count, update_date = @update_date WHERE call_code = @call_code"
              End If

              command.CommandText = commandText

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
              command.Parameters.AddWithValue("@create_date", row("create_date"))
              command.Parameters.AddWithValue("@update_date", row("update_date"))

              command.ExecuteNonQuery()

              ' SQLログ出力
              Dim sqlForLog As String = BuildCommandTextWithParams(command)
              Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sqlForLog)
            End If
          Next

          transaction.Commit()
          MessageBox.Show("データを保存しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
          LoadData()
        Else
          MessageBox.Show("変更されたデータがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
      End Using
    Catch ex As Exception
      MessageBox.Show("データの保存に失敗しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
    End Try
  End Sub

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
    If e.KeyCode = Keys.Delete Then
      e.SuppressKeyPress = True ' デフォルトの削除動作を防止
      DeleteButton_Click(DeleteButton, EventArgs.Empty) ' 削除ボタンの動作を実行
    End If
  End Sub
  ' DataGridView の CellValidating イベントを処理
  Private Sub ItemDetail_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles ItemDetail.CellValidating

    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name
    Dim headerText As String = ItemDetail.Columns(e.ColumnIndex).HeaderText ' ヘッダー名を取得
    Dim cellValue As String = e.FormattedValue.ToString().Trim()

    '' **必須チェック（call_code, item_number, item_name のみ）**
    'Dim requiredColumns As String() = {"call_code", "item_number", "item_name"}

    '**必須チェック（call_codeのみ）**
    Dim requiredColumns As String() = {"call_code"}
    If requiredColumns.Contains(columnName) AndAlso String.IsNullOrEmpty(cellValue) Then
      MessageBox.Show(headerText & " は必須入力です。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      e.Cancel = True
      Return
    End If

    ' **重複チェック（call_code, item_number）**
    'If columnName = "call_code" OrElse columnName = "item_number" Then
    If columnName = "call_code" Then
      Dim duplicateCount As Integer = 0
      For Each row As DataGridViewRow In ItemDetail.Rows
        If Not row.IsNewRow AndAlso row.Index <> e.RowIndex Then ' 現在編集している行を除外
          If row.Cells(columnName).Value IsNot Nothing AndAlso row.Cells(columnName).Value.ToString().Trim() = cellValue Then
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
        {"call_code", 6}, {"item_number", 20}, {"item_name", 50},
        {"packing_bag_unit", 3}, {"upper_limit_unit", 3}, {"reference_value_unit", 3},
        {"lower_limit_unit", 3}, {"subtotal_target_value_unit", 3}
    }
    If maxLengths.ContainsKey(columnName) AndAlso cellValue.Length > maxLengths(columnName) Then
      MessageBox.Show(headerText & " の最大桁数は " & maxLengths(columnName) & " 文字です。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      e.Cancel = True
      Return
    End If

    ' **小数チェック（decimal(7,2)）**
    Dim decimalColumns As String() = {"packing_bag", "upper_limit", "reference_value", "lower_limit", "subtotal_target_value"}
    If decimalColumns.Contains(columnName) Then
      Dim value As Decimal
      If Not Decimal.TryParse(cellValue, value) OrElse value < 0 Then
        MessageBox.Show(headerText & " は 0 以上の数値を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If

      ' **小数点以下2桁チェック**
      If (value * 100) Mod 1 <> 0 Then
        MessageBox.Show(headerText & " は小数点以下2桁まで入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If
    End If

    ' **整数チェック（subtotal_target_count のみ）**
    If columnName = "subtotal_target_count" Then
      Dim intValue As Integer
      If Not Integer.TryParse(cellValue, intValue) OrElse intValue < 0 Then
        MessageBox.Show(headerText & " は 0 以上の整数を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.Cancel = True
        Return
      End If
    End If
  End Sub


  ' DataGridView の CellEndEdit イベントを処理（call_code の 6桁ゼロ埋め処理）
  Private Sub ItemDetail_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles ItemDetail.CellEndEdit
    Dim columnName As String = ItemDetail.Columns(e.ColumnIndex).Name

    ' **call_code のゼロ埋め処理**
    If columnName = "call_code" Then
      Dim cellValue As String = ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString().Trim()
      Dim intValue As Integer

      ' 数値に変換できる場合のみゼロ埋め
      If Integer.TryParse(cellValue, intValue) Then
        ItemDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = intValue.ToString("D6")
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
            {"call_code", 6}, {"subtotal_target_count", 10}, ' subtotal_target_count の最大桁数を10に設定
            {"item_number", 20}, {"item_name", 50},
            {"packing_bag_unit", 3}, {"upper_limit_unit", 3}, {"reference_value_unit", 3},
            {"lower_limit_unit", 3}, {"subtotal_target_value_unit", 3}
        }
      If maxLengths.ContainsKey(columnName) Then
        textBox.MaxLength = maxLengths(columnName) ' 最大桁数を設定
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

    ' **数字、バックスペース、削除キーのみ許可**
    If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
      e.Handled = True ' 入力を禁止
    End If

    ' **最大桁数チェック**
    If textBox.MaxLength > 0 AndAlso textBox.Text.Length >= textBox.MaxLength AndAlso Not Char.IsControl(e.KeyChar) Then
      e.Handled = True ' ただし、バックスペースや削除キーは許可
    End If
  End Sub



  ' **小数のみ許可（decimal(7,2) 型: packing_bag, upper_limit など）**
  Private Sub DecimalOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
    Dim textBox As TextBox = TryCast(sender, TextBox)
    If textBox IsNot Nothing Then
      ' **数字、小数点、バックスペースのみ許可**
      If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
        e.Handled = True
      End If

      ' **小数点は1回のみ許可**
      If e.KeyChar = "."c AndAlso textBox.Text.Contains(".") Then
        e.Handled = True
      End If

      ' **バックスペースが有効になるようにする**
      If e.KeyChar = ChrW(Keys.Back) Then
        e.Handled = False ' バックスペースを許可
        Return
      End If

      ' **桁数制限（整数5桁 + 小数2桁 = 最大8桁）**
      If textBox.Text.Contains(".") Then
        Dim parts() As String = textBox.Text.Split("."c)
        If parts(0).Length > 5 AndAlso textBox.SelectionStart <= parts(0).Length Then
          e.Handled = True ' 整数部の桁数制限
        End If
        If parts.Length > 1 AndAlso parts(1).Length >= 2 AndAlso textBox.SelectionStart > parts(0).Length Then
          e.Handled = True ' 小数部の桁数制限
        End If
      ElseIf textBox.Text.Length >= 5 AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "."c Then
        e.Handled = True ' 整数部の桁数制限
      End If
    End If
  End Sub

  ' **文字列のみ許可（varchar 型）**
  Private Sub StringOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
    Dim textBox As TextBox = TryCast(sender, TextBox)
    ' **最大桁数チェック**
    If textBox.Text.Length >= textBox.MaxLength AndAlso Not Char.IsControl(e.KeyChar) Then
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
        Me.Close()
    End Select
  End Sub
End Class

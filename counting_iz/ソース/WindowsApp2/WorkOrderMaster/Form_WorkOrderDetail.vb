Imports Common
Imports Common.ClsFunction
Public Class Form_WorkOrderDetail
  '新規:１ 、変更:２
  Public InputMode As Integer

  Public WorkOrderIDTextValue As String
  Public DetailIDTextValue As String
  Public WorkOrderWorkOrderNameTextValue As String
  Public IsDetailSubtotalComboValue As String
  Public ProductIDTextValue As String
  Public ProductNameTextValue As String
  Public OrderQuantityTextValue As String

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

  Private Sub Form_WorkOrderDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "作業指示マスタ詳細" & " ( " & updateTime & " ) "
    FormBorderStyle = FormBorderStyle.FixedSingle

    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True

    SetIsDetailSubtotalComboBox()

    SetItemComboBox()
    SetInitialProperty()
  End Sub

  Private Sub SetIsDetailSubtotalComboBox()
    ' 共通のアイテムリスト
    Dim items As String() = {"する", "しない"}

    ' コンボボックスのアイテムをクリア
    IsDetailSubtotalCombo.Items.Clear()

    ' 空の項目をComboBoxに追加
    IsDetailSubtotalCombo.Items.Add("")

    ' アイテムをコンボボックスに追加
    For Each item As String In items
      IsDetailSubtotalCombo.Items.Add(item)
    Next
  End Sub


  Private Sub SetItemComboBox()
    Try
      ' 商品マスタからデータを取得
      Dim ItemData As DataTable = GetItemMasterData()

      ' 商品マスタからデータが取得できなかった場合
      If ItemData.Rows.Count = 0 Then
        ' エラーメッセージを表示して終了
        MessageBox.Show("商品マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        ' ComboBoxのアイテムをクリア
        ProductIDCombo.Items.Clear()

        ' 空の項目をComboBoxに追加
        ProductIDCombo.Items.Add("")

        ' 商品マスタから取得したデータをComboBoxに追加
        For Each row As DataRow In ItemData.Rows
          ProductIDCombo.Items.Add(row(0).ToString())
        Next
      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
  End Sub


  Private Function GetItemMasterData() As DataTable
    Dim PackingData As New DataTable
    Try
      With tmpDb
        SqlServer.GetResult(PackingData, GetSelectItemMaster)
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try

    Return PackingData
  End Function
  Private Function GetSelectItemMaster() As String
    Dim sql As String = String.Empty
    sql &= " SELECT CONVERT(VARCHAR, call_code) + ' ' + item_name AS DisplayText "
    sql &= " FROM MST_Item "
    sql &= " ORDER BY call_code"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function


  Private Sub SetInitialProperty()
    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        WorkOrderIDText.Enabled = True
      Case 2
        WorkOrderIDText.Enabled = False
        DetailIDText.Enabled = False
        WorkOrderIDText.Text = WorkOrderIDTextValue
        DetailIDText.Text = DetailIDTextValue
        WorkOrderNameText.Text = WorkOrderWorkOrderNameTextValue
        IsDetailSubtotalCombo.Text = IsDetailSubtotalComboValue

        ProductIDCombo.Text = ProductIDTextValue
        OrderQuantityText.Text = OrderQuantityTextValue

    End Select
  End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    Select Case InputMode
      Case 1 ' 新規
        If CheckValue(False) = False Then
          Exit Sub
        End If
        InsertWorkOrderMaster()

      Case 2 ' 更新
        If CheckValue(True) = False Then
          Exit Sub
        End If
        UpdateWorkOrderMaster()
    End Select
  End Sub
  ''' <summary>
  ''' 入力チェック処理
  ''' </summary>
  ''' <param name="isUpdate">更新モードかどうか。True = 更新時, False = 新規登録時</param>
  ''' <returns>エラーがあれば False</returns>
  Function CheckValue(Optional isUpdate As Boolean = False) As Boolean
    Dim errorMessages As New List(Of String)

    ' === 必須チェック ===
    If String.IsNullOrEmpty(WorkOrderIDText.Text) Then
      errorMessages.Add("作業指示№を入力してください。")
    ElseIf WorkOrderIDText.Text = "0" Then
      errorMessages.Add("作業指示№に0は登録できません。")
    End If

    If String.IsNullOrEmpty(DetailIDText.Text) Then
      errorMessages.Add("明細番号を入力してください。")
    End If

    If String.IsNullOrWhiteSpace(WorkOrderNameText.Text) Then
      errorMessages.Add("作業指示名を入力してください。")
    End If

    If String.IsNullOrWhiteSpace(IsDetailSubtotalCombo.Text) Then
      errorMessages.Add("明細小計有無を選択してください。")
    End If

    If String.IsNullOrWhiteSpace(ProductIDCombo.Text) Then
      errorMessages.Add("製品を選択してください。")
    End If

    If String.IsNullOrWhiteSpace(OrderQuantityText.Text) Then
      errorMessages.Add("指示数を入力してください。")
    Else
      Dim quantity As Integer
      If Not Integer.TryParse(OrderQuantityText.Text, quantity) Then
        errorMessages.Add("指示数は数値で入力してください。")
      End If
    End If

    ' === エラー表示 ===
    If errorMessages.Count > 0 Then
      MessageBox.Show(String.Join(vbCrLf, errorMessages), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)

      ' 最初のエラー項目にフォーカスを当てる
      If String.IsNullOrEmpty(WorkOrderIDText.Text) OrElse WorkOrderIDText.Text = "0" Then
        WorkOrderIDText.Focus()
      ElseIf String.IsNullOrEmpty(DetailIDText.Text) Then
        DetailIDText.Focus()
      ElseIf String.IsNullOrWhiteSpace(WorkOrderNameText.Text) Then
        WorkOrderNameText.Focus()
      ElseIf String.IsNullOrWhiteSpace(IsDetailSubtotalCombo.Text) Then
        IsDetailSubtotalCombo.Focus()
      ElseIf String.IsNullOrWhiteSpace(ProductIDCombo.Text) Then
        ProductIDCombo.Focus()
      ElseIf String.IsNullOrWhiteSpace(OrderQuantityText.Text) Then
        OrderQuantityText.Focus()
      End If

      Return False
    End If

    ' === 重複チェック（作業指示№ + 明細№）: 新規時のみ ===
    If Not isUpdate AndAlso Form_WorkOrderList.WorkOrderDetail.Rows.Count > 0 Then
      For Each row As DataGridViewRow In Form_WorkOrderList.WorkOrderDetail.Rows
        If WorkOrderIDText.Text.Equals(row.Cells(0).Value?.ToString()) AndAlso
               DetailIDText.Text.Equals(row.Cells(1).Value?.ToString()) Then

          MessageBox.Show("既に登録されている作業指示№と明細番号の組み合わせです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          WorkOrderIDText.Focus()
          Return False
        End If
      Next
    End If

    Return True
  End Function


  Private Sub InsertWorkOrderMaster()
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
            Form_WorkOrderList.SelectWorkOrderMaster()

            Close()
          Else
            ' 登録失敗
            MessageBox.Show("作業指示マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    ' 値の取得・変換
    Dim workOrderID As Integer = Integer.Parse(WorkOrderIDText.Text)
    Dim detailID As Integer = Integer.Parse(DetailIDText.Text)
    Dim workOrderName As String = WorkOrderNameText.Text
    Dim isSubtotalBit As Integer = If(IsDetailSubtotalCombo.Text = "する", 1, 0)
    Dim orderQuantity As Integer = Integer.Parse(OrderQuantityText.Text)
    Dim tmpDate As DateTime = CDate(ComGetProcTime())

    Dim itemCode As Integer = 0
    If Not String.IsNullOrWhiteSpace(ProductIDCombo.Text) Then
      Dim parts = ProductIDCombo.Text.Split(" "c)
      If parts.Length > 0 Then
        Integer.TryParse(parts(0), itemCode)
      End If
    End If

    ' SQL組み立て
    sql &= "INSERT INTO MST_WorkOrder ("
    sql &= "WorkOrderID, DetailID, WorkOrderName, IsDetailSubtotal, ProductID, OrderQuantity, CreateDate, UpdateDate"
    sql &= ") VALUES ("
    sql &= workOrderID & ", "
    sql &= detailID & ", "
    sql &= "'" & workOrderName & "', "
    sql &= isSubtotalBit & ", "
    sql &= itemCode & ", "
    sql &= orderQuantity & ", "
    sql &= "'" & tmpDate & "', "
    sql &= "'" & tmpDate & "'"
    sql &= ")"

    ' ログ出力
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)

    Return sql
  End Function


  Private Sub UpdateWorkOrderMaster()
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
            Dim CurrentRow As Integer = Form_WorkOrderList.WorkOrderDetail.CurrentRow.Index
            Form_WorkOrderList.SelectWorkOrderMaster()
            Form_WorkOrderList.WorkOrderDetail.Rows(CurrentRow).Selected = True
            Form_WorkOrderList.WorkOrderDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            '選択している行の行番号の取得
            Form_WorkOrderList.WorkOrderDetail.CurrentCell = Form_WorkOrderList.WorkOrderDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("作業指示4マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    ' 値の取得・変換
    Dim workOrderID As Integer = Integer.Parse(WorkOrderIDText.Text)
    Dim detailID As Integer = Integer.Parse(DetailIDText.Text)
    Dim workOrderName As String = WorkOrderNameText.Text
    Dim isSubtotalBit As Integer = If(IsDetailSubtotalCombo.Text = "する", 1, 0)
    Dim orderQuantity As Integer = Integer.Parse(OrderQuantityText.Text)
    Dim tmpDate As DateTime = CDate(ComGetProcTime())

    ' ProductIDの抽出（例: "123 商品名" → 123）
    Dim itemCode As Integer = 0
    If Not String.IsNullOrWhiteSpace(ProductIDCombo.Text) Then
      Dim parts = ProductIDCombo.Text.Split(" "c)
      If parts.Length > 0 Then
        Integer.TryParse(parts(0), itemCode)
      End If
    End If

    ' SQL文構築
    sql &= "UPDATE MST_WorkOrder SET "
    sql &= "WorkOrderName = '" & workOrderName & "', "
    sql &= "IsDetailSubtotal = " & isSubtotalBit & ", "
    sql &= "ProductID = " & itemCode & ", "
    sql &= "OrderQuantity = " & orderQuantity & ", "
    sql &= "UpdateDate = '" & tmpDate & "' "
    sql &= "WHERE WorkOrderID = " & workOrderID & " AND DetailID = " & detailID

    ' ログ出力
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)

    Return sql
  End Function

  Private Sub WorkOrderIDText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles WorkOrderIDText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Dispose()
  End Sub

  Private Sub Form_WorkOrderDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

  Private Sub DetailIDText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DetailIDText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub OrderQuantityText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles OrderQuantityText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
End Class

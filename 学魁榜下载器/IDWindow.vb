Public Class IDWindow
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For Each i In ListBox1.Items
            Main.FLinks.Add("https://www.xkb365.com/app/video/" & i)
        Next
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If IsNumeric(TextBox1.Text) And TextBox1.Text <> "" Then
            ListBox1.Items.Add(TextBox1.Text)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If IsNumeric(TextBox2.Text) And IsNumeric(TextBox3.Text) Then
            For i = CInt(TextBox2.Text) To CInt(TextBox3.Text)
                ListBox1.Items.Add(i)
            Next
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If IsNumeric(TextBox1.Text) And TextBox1.Text <> "" Then
                ListBox1.Items.Add(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub TextBox3_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox3.KeyDown
        If e.KeyCode = Keys.Enter Then
            If IsNumeric(TextBox2.Text) And IsNumeric(TextBox3.Text) Then
                For i = CInt(TextBox2.Text) To CInt(TextBox3.Text)
                    ListBox1.Items.Add(i)
                Next
            End If
        End If
    End Sub

    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
    End Sub
End Class
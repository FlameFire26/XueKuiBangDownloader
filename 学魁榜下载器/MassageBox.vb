Public Class MassageBox
    Private Sub MassageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Main.ProcessRate = 100 Then
            Close()
        End If
        Label1.Text = "已完成 " & Main.ProcessRate & " %"
        If Not Main.Nowing = Nothing Then
            Label2.Text = Main.Nowing
        Else
            Label2.Text = "当前：..."
        End If
    End Sub

    Private Sub Downloading_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Timer1.Enabled = False
    End Sub
End Class
Imports System.Net
Imports System.Text.RegularExpressions

Public Class Main

    Public ProcessRate As Short = 0
    Public Nowing As String
    Public FLinks As New List(Of String)
    Public SLinks As New List(Of String)
    Public Items As New Dictionary(Of String, String)

    '''<summary>获取网页源码</summary>
    Function GetWebCode(ByVal URL As String, Optional ByVal IsGb2312 As Boolean = False) As String
        On Error Resume Next
        Dim XmlHttp As Object
        XmlHttp = CreateObject("Microsoft.XMLHttp")
        XmlHttp.Open("POST", URL, False)
        XmlHttp.Send()
        Dim WebContent As Object
        Dim Str_WebContent As String
        If IsGb2312 Then
            WebContent = XmlHttp.ResponseBody
            Str_WebContent = System.Text.Encoding.Default.GetString(WebContent)
        Else
            WebContent = XmlHttp.ResponseText
            Str_WebContent = WebContent.ToString
        End If
        XmlHttp = Nothing
        Return Str_WebContent
    End Function

    Public Sub HandleLinks()
        For i = 0 To FLinks.Count - 1
            Dim Content = GetWebCode(FLinks.Item(i))
            Dim MatchMade As Match
            Dim MyRegex As New Regex("https://www\.xkb365\.com/index\.php\?app=classroom&mod=Video&act=watch&id=\d\d\d\d?&s_id=\d\d\d\d?")
            MatchMade = MyRegex.Match(Content, 0)
            Do While MatchMade.Success
                SLinks.Add(MatchMade.Value)
                MatchMade = MatchMade.NextMatch()
            Loop
            ProcessRate = Int(i * 50 / FLinks.Count)
        Next
        For i = 0 To SLinks.Count - 1
            Dim Content = GetWebCode(SLinks.Item(i))
            Dim HZMatch, LJMatch As Match
            Dim HZRegex As New Regex("【(数学|语文|英语|政治|历史|地理|物理|化学|生物)】([\u4e00-\u9fa5]|（|）|—)+")
            Dim LJRegex As New Regex("https://video\.(xkb365|xuekuibang)\.com/\d{11}\?e=\d{10}&token=[A-Za-z0-9_:+/=-]+")
            HZMatch = HZRegex.Match(Content, 0)
            LJMatch = LJRegex.Match(Content, 0)
            If InDictKey(HZMatch.Value, Items) Then
                If Not (InDictValue(LJMatch.Value, Items) Or InDictKey(HZMatch.Value & "2", Items)) Then
                    Items.Add(HZMatch.Value & "2", LJMatch.Value)
                End If
            Else
                Items.Add(HZMatch.Value, LJMatch.Value)
            End If
            ProcessRate = Int(i * 50 / SLinks.Count)
            ProcessRate += 50
            Nowing = "当前：" & HZMatch.Value
        Next
        ProcessRate = 100
        Nowing = Nothing
        MsgBox("完成，共获得" & Items.Count & "个链接")
    End Sub

    Public Sub Download()
        Dim p As Short = 0
        Dim Downloader As New WebClient
        For i = 0 To ListBox2.Items.Count - 1
            Nowing = "当前：" & ListBox1.Items.Item(i)
            Downloader.DownloadFile(ListBox2.Items.Item(i), ListBox1.Items.Item(i) & ".mp4")
            p += 1
            ProcessRate = Int(p * 100 / ListBox2.Items.Count)
        Next
        ProcessRate = 100
        MsgBox("下载完成")
        If CheckBox1.Checked Then
            Select Case ComboBox1.SelectedIndex
                Case 0
                    Shell("cmd.exe /c shutdown -s -t 30", vbHide)
                Case 1
                    Shell("cmd.exe /c shutdown -s -r", vbHide)
                Case 2
                    Shell("cmd.exe /c rundll32.exe powrprof.dll,SetSuspendState 0,1,0", vbHide)
                Case 3
                    Shell("cmd.exe logoff", vbHide)
            End Select
        End If
    End Sub

    Public Function InDictKey(Finding As String, Dict As Dictionary(Of String, String)) As Boolean
        For Each Content As KeyValuePair(Of String, String) In Dict
            If Finding = Content.Key Then Return True
        Next
        Return False
    End Function

    Public Function InDictValue(Finding As String, Dict As Dictionary(Of String, String)) As Boolean
        For Each Content As KeyValuePair(Of String, String) In Dict
            If Finding = Content.Value Then Return True
        Next
        Return False
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tHL As New Threading.Thread(AddressOf HandleLinks)
        Button1.Enabled = False
        ProcessRate = Nothing
        tHL.Start()
        MassageBox.ShowDialog()
        For Each i As KeyValuePair(Of String, String) In Items
            ListBox1.Items.Add(i.Key)
            ListBox2.Items.Add(i.Value)
        Next
        Button2.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim tDL As New Threading.Thread(AddressOf Download)
        TableLayoutPanel3.Enabled = False
        Button3.Enabled = True
        Button4.Enabled = True
        ProcessRate = Nothing
        tDL.Start()
        MassageBox.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TextWindow.TextBox1.Text = Nothing
        For Each i In ListBox2.Items
            TextWindow.TextBox1.Text &= i & vbCrLf
        Next
        TextWindow.ShowDialog()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextWindow.TextBox1.Text = Nothing
        For Each i In ListBox1.Items
            TextWindow.TextBox1.Text &= i & vbCrLf
        Next
        TextWindow.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        IDWindow.ShowDialog()
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
    End Sub

    Private Sub ListBox2_DoubleClick(sender As Object, e As EventArgs) Handles ListBox2.DoubleClick
        ListBox2.Items.RemoveAt(ListBox2.SelectedIndex)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        ComboBox1.Enabled = CheckBox1.Checked
    End Sub
End Class
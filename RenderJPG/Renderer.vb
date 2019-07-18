Imports System.Drawing

Public Class Renderer
    Private Width As Integer
    Private Height As Integer
    Private Types As Integer
    Private fileLoc As String
    Private s As String = vbCrLf
    Private tempArray() As Char = s.ToCharArray
    Private charArray() As Char
    Private binList() As Integer
    Private NewLineChar1 As Char = tempArray(0)
    Private NewLineChar2 As Char = tempArray(1)

    Public Sub New(_width As Integer, _height As Integer, _types As Integer, _fileLoc As String)
        Width = _width
        Height = _height
        Types = _types
        fileLoc = _fileLoc
        binList = New Integer(Width * Height) {}
        ReadFile()
        GetTypes()
    End Sub

    Private Sub ReadFile()
        Dim fileReader As String
        fileReader = My.Computer.FileSystem.ReadAllText(fileLoc)
        charArray = fileReader.ToCharArray()
    End Sub

    Private Function ConvertAddress(tail As String) As String
        Return fileLoc & tail
    End Function

    Public Sub RenderImage(ByVal toFileLoc As String)
        Dim binCounter As Integer = 0
        Dim imageTest = New Bitmap(Width, Height)
        For x = 0 To imageTest.Width - 1
            For y = 0 To imageTest.Height - 1
                Dim nColor As PixelRGB = ColorPicker(binList(binCounter))

                Dim newColor As Color = Color.FromArgb(nColor.R, nColor.G, nColor.B)
                imageTest.SetPixel(x, y, newColor)
                binCounter = binCounter + 1
            Next
        Next
        imageTest.Save(toFileLoc)
    End Sub

    Private Sub GetTypes()
        Dim binCounter As Integer = 0
        'Dim nonNewLineCounter As Integer = 0
        For i As Integer = 0 To charArray.Count - 1

            If charArray(i) = NewLineChar1 Then  '' If charArray(i) = new line char
                Dim int As Integer
                If charArray(i - 2) = "," Then
                    int = Convert.ToInt32(charArray(i - 1).ToString)
                ElseIf charArray(i - 3) = "," Then
                    int = Convert.ToInt32(charArray(i - 2).ToString) * 10 + Convert.ToInt32(charArray(i - 1).ToString)
                ElseIf charArray(i - 4) = "," Then
                    int = Convert.ToInt32(charArray(i - 3).ToString) * 100 + Convert.ToInt32(charArray(i - 2).ToString) * 10 + Convert.ToInt32(charArray(i - 1).ToString)
                Else
                    int = 0
                End If
                binList(binCounter) = int
                binCounter += 1
                'ElseIf charArray(i) = NewLineChar2 Then

            Else
                'Console.WriteLine(charArray(i))
                'nonNewLineCounter += 1
            End If
            If i > 0 Then
                'Console.WriteLine($"charArray({i}) = {charArray(i)}? -> binList({binCounter}) = {charArray(i - 1)}")
            End If
        Next
    End Sub

    Private Function ColorPicker(ByVal group As Integer) As PixelRGB
        Dim assignPool As Integer = 1020 / Types * group
        Dim R As Integer
        Dim G As Integer
        Dim B As Integer
        If assignPool <= 255 And assignPool > 0 Then
            R = 255
            G = assignPool
            B = 0
            'Console.Write("R")
        ElseIf assignPool > 255 And assignPool <= 510 Then
            R = 255
            G = 255
            B = assignPool - 255
            'Console.Write("Y")
        ElseIf assignPool > 510 And assignPool <= 765 Then
            R = 255 - (assignPool - 510)
            G = 255
            B = 255
            'Console.Write("B")
        ElseIf assignPool > 765 And assignPool <= 1020 Then
            R = assignPool - 765
            G = 0
            B = 255
            'Console.Write("P")
        Else
            R = 0
            G = 0
            B = 0
            'Console.WriteLine(group)
            'Console.WriteLine("else case")
        End If

        Dim color As New PixelRGB(R, G, B)
        Return color
    End Function

    Private Class PixelRGB
        Public R
        Public G
        Public B
        Public Sub New(ByVal _r As Integer, ByVal _g As Integer, ByVal _b As Integer)
            R = _r
            G = _g
            B = _b
        End Sub
    End Class
End Class
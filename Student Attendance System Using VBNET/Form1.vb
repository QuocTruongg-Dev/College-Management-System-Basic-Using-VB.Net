Imports System.IO
Imports Microsoft.Data.SqlClient

Public Class Form1
    Private dataHandler As StudentDataHandler
    Private connectionString As String = "Data Source=LAPTOP-H9PTCLCF\SQLEXPRESS;Initial Catalog=studentdb;Integrated Security=True;Trust Server Certificate=True"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dataHandler = New StudentDataHandler(connectionString)
        LoadData()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse String.IsNullOrWhiteSpace(TextBox2.Text) OrElse String.IsNullOrWhiteSpace(TextBox3.Text) OrElse String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If
        Dim StudentId As Integer
        If Not Integer.TryParse(TextBox1.Text, StudentId) Then
            MessageBox.Show("Student ID must be a number.") 'check id
            Return
        End If
        'lấy dữ liệu ...
        Dim StudentName As String = TextBox2.Text
        Dim RollNo As String = TextBox3.Text
        Dim Status As String = TextBox4.Text
        If dataHandler.InsertStudent(StudentId, StudentName, RollNo, Status) Then
            MessageBox.Show("Record Saved Successfully")
            LoadData()
        Else
            MessageBox.Show("Error saving record.")
        End If
    End Sub

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        LoadData()
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse String.IsNullOrWhiteSpace(TextBox2.Text) OrElse String.IsNullOrWhiteSpace(TextBox3.Text) OrElse String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If
        Dim StudentId As Integer
        If Not Integer.TryParse(TextBox1.Text, StudentId) Then
            MessageBox.Show("Student ID must be a number.")
            Return
        End If
        Dim StudentName As String = TextBox2.Text
        Dim RollNo As String = TextBox3.Text
        Dim Status As String = TextBox4.Text

        If dataHandler.UpdateStudent(StudentId, StudentName, RollNo, Status) Then
            MessageBox.Show("Record Updated Successfully")
            LoadData()
        Else
            MessageBox.Show("Error updating record.")
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click ' Delete
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Please enter Student ID to delete.")
            Return
        End If
        Dim StudentId As Integer
        If Not Integer.TryParse(TextBox1.Text, StudentId) Then
            MessageBox.Show("Student ID must be a number.")
            Return
        End If

        If dataHandler.DeleteStudent(StudentId) Then
            MessageBox.Show("Record Deleted Successfully")
            LoadData()
        Else
            MessageBox.Show("Error deleting record.")
        End If
    End Sub

    Private Sub LoadData()
        DataGridView1.DataSource = dataHandler.GetAllStudents()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim defaultFilePath As String = "D:\VB.NET\student_data.csv"
        Try
            Using writer As New StreamWriter(defaultFilePath)
                writer.WriteLine("StudentId,StudentName,RollNo,Status")

                ' Ghi dữ liệu từ DataGridView
                For Each row As DataGridViewRow In DataGridView1.Rows
                    If Not row.IsNewRow Then ' Bỏ qua dòng trống cuối cùng
                        writer.WriteLine($"{row.Cells("StudentId").Value},{row.Cells("StudentName").Value},{row.Cells("RollNo").Value},{row.Cells("Status").Value}")
                    End If
                Next
            End Using
            MessageBox.Show("Data saved to CSV successfully.")
        Catch ex As Exception
            MessageBox.Show($"Error saving data to CSV: {ex.Message}")
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim defaultFilePath As String = "D:\VB.NET\student_data.csv"

        Try
            Dim table As New DataTable()
            table.Columns.Add("StudentId", GetType(Integer))
            table.Columns.Add("StudentName", GetType(String))
            table.Columns.Add("RollNo", GetType(String))
            table.Columns.Add("Status", GetType(String))

            Using reader As New StreamReader(defaultFilePath)
                reader.ReadLine()

                ' Đọc dữ liệu
                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    Dim values As String() = line.Split(",")
                    table.Rows.Add(values(0), values(1), values(2), values(3))
                End While
            End Using

            DataGridView1.DataSource = table
            MessageBox.Show("Data loaded from CSV successfully.")

            ' Mở file bằng ứng dụng mặc định
            System.Diagnostics.Process.Start(defaultFilePath)

        Catch ex As Exception
            MessageBox.Show($"Error loading data from CSV: {ex.Message}")
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex >= 0 Then ' Kiểm tra xem có hàng nào được chọn hay không
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            TextBox1.Text = row.Cells("StudentId").Value.ToString()
            TextBox2.Text = row.Cells("StudentName").Value.ToString()
            TextBox3.Text = row.Cells("RollNo").Value.ToString()
            TextBox4.Text = row.Cells("Status").Value.ToString()
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim searchId As Integer
        If Integer.TryParse(txtSearchId.Text, searchId) Then ' Kiểm tra ID hợp lệ
            Try
                Dim resultTable As DataTable = dataHandler.SearchStudentById(searchId)

                If resultTable IsNot Nothing AndAlso resultTable.Rows.Count > 0 Then
                    DataGridView1.DataSource = resultTable
                Else
                    DataGridView1.DataSource = Nothing ' Xóa dữ liệu cũ
                    MessageBox.Show("Không tìm thấy sinh viên có ID này.")
                End If
            Catch ex As Exception
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}")
            End Try
        Else
            DataGridView1.DataSource = Nothing ' Xóa dữ liệu cũ
            MessageBox.Show("ID không hợp lệ.")
        End If
    End Sub

End Class
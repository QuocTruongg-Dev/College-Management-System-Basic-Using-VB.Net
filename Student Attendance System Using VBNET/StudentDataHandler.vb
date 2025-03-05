Imports Microsoft.Data.SqlClient
Public Class StudentDataHandler
    Private connectionString As String

    Public Sub New(connectionString As String)
        Me.connectionString = connectionString
    End Sub

    Public Function InsertStudent(studentId As Integer, studentName As String, rollNo As String, status As String) As Boolean
        Try
            Using con As SqlConnection = New SqlConnection(connectionString)
                con.Open() 'mở connection
                Using cmd As SqlCommand = New SqlCommand("InsertStudent", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@studentid", studentId)
                    cmd.Parameters.AddWithValue("@studentname", studentName)
                    cmd.Parameters.AddWithValue("@rollno", rollNo)
                    cmd.Parameters.AddWithValue("@status", status)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function UpdateStudent(studentId As Integer, studentName As String, rollNo As String, status As String) As Boolean
        Try
            Using con As SqlConnection = New SqlConnection(connectionString)
                con.Open()
                Using cmd As SqlCommand = New SqlCommand("UpdateStudent", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@studentid", studentId)
                    cmd.Parameters.AddWithValue("@studentname", studentName)
                    cmd.Parameters.AddWithValue("@rollno", rollNo)
                    cmd.Parameters.AddWithValue("@status", status)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function DeleteStudent(studentId As Integer) As Boolean
        Try
            Using con As SqlConnection = New SqlConnection(connectionString)
                con.Open()
                Using cmd As SqlCommand = New SqlCommand("DeleteStudent", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@studentid", studentId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetAllStudents() As DataTable
        Try
            Dim table As New DataTable()
            Using con As SqlConnection = New SqlConnection(connectionString)
                con.Open()
                Using cmd As SqlCommand = New SqlCommand("GetAllStudents", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(table) ' đổ dữ liệu ra table
                    End Using
                End Using
            End Using
            Return table
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function SearchStudentById(studentId As Integer) As DataTable
        Try
            Dim table As New DataTable()
            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand("SearchStudentById", con) ' Thay "SearchStudentById" bằng tên stored procedure
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@StudentId", studentId)

                    Using da As New SqlDataAdapter(cmd)
                        con.Open()
                        da.Fill(table)
                    End Using
                End Using
            End Using
            Return table
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
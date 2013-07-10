Imports System.IO
Imports System.Text
Imports fotelo
Imports System.Web.Configuration.WebConfigurationManager

Partial Public Class Main
   Inherits System.Web.UI.Page

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

   End Sub

   Protected Sub LoadDataRedirect(ByVal sender As System.Object, ByVal e As System.EventArgs) _
             Handles btnParseFile.Click

      LoadDataFile()

   End Sub

   Protected Sub ChangePage(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) _
             Handles gvResults.PageIndexChanging

      gvResults.PageIndex = e.NewPageIndex
      LoadDataFile()

   End Sub

   Private Sub LoadDataFile()

      Dim strSourceFile As StreamReader
      Dim strFileContents As StringBuilder
      Dim ftlLoader As TextLoader
      Dim strSampleFilePath As String = ""
      Dim strSampleLayoutPath As String = ""
      Dim strSampleFileURL As String = ""

      ' Load the appropriate files based on the drop option specified.
      Try

         ' Create an instance of StreamReader and read the file into a string. Put these contents into the data 
         ' textbox. Then parse file and load it into a datatable.
         Select Case ddlConversionType.SelectedValue

            Case "Position"

               strSampleFilePath = AppSettings("SampleFilePath") & "SamplePosition.txt"
               strSampleLayoutPath = AppSettings("SampleFilePath") & "SamplePositionLayout.xml"
               strSampleFileURL = "SamplePositionLayout.xml"

            Case "Delimited"

               strSampleFilePath = AppSettings("SampleFilePath") & "SampleCSV.csv"
               strSampleLayoutPath = AppSettings("SampleFilePath") & "SampleCSVLayout.xml"
               strSampleFileURL = "SampleCSVLayout.xml"

            Case "Regex"

               strSampleFilePath = AppSettings("SampleFilePath") & "SampleRegex.txt"
               strSampleLayoutPath = AppSettings("SampleFilePath") & "SampleRegexLayout.xml"
               strSampleFileURL = "SampleRegexLayout.xml"

         End Select

         strFileContents = New StringBuilder
         strSourceFile = New StreamReader(strSampleFilePath)
         strFileContents.Append(strSourceFile.ReadToEnd())
         strSourceFile.Close()

         txtRawData.Text = strFileContents.ToString
         lnkViewControlFile.NavigateUrl = strSampleFileURL

         ftlLoader = New TextLoader(strSampleFilePath, strSampleLayoutPath)

         txtControlFileDetails.Text = ftlLoader.Layout.ToString
         gvResults.DataSource = ftlLoader.LoadDataTable()
         gvResults.DataBind()

      Catch ex As Exception

         lblError.Text = "Error Using Text Loader:<br/>" & ex.ToString
         lblError.Visible = True

      End Try

   End Sub

End Class
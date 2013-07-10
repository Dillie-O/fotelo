Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
'''   This module is designed to load text files with a set pattern into a data table for use within an 
'''   application. This is especially useful when creating web acessible front ends to application log files
'''   that output to text in a fixed format. The data within the text file can be patterened by a character 
'''   delimiter, character position, or by a regular expression formula. When the data table is created by 
'''   fotelo, it is created into a strongly typed data table. The specifications for the file and the columns to be 
'''   created are located in a supplementary XML file.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Sean Patterson]	6/14/2004	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class TextLoader

#Region "Private Members"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The layout of the data table.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private ftlLayout As TextLayout

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The path to the file containing raw import data.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private strSourceFile As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The strongly typed data table containing the imported results.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private dtTextData As DataTable

#End Region

#Region "Public Properties"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''    Gets the data table layout information.
   ''' </summary>
   ''' <value>Layout returns the ftlLayout data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------     
   Public ReadOnly Property Layout() As TextLayout
      Get
         Return ftlLayout
      End Get
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Gets/Sets the source file infomation.
   ''' </summary>
   ''' <value>SourceFile returns the strSourceFile data memeber.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Property SourceFile() As String
      Get
         Return strSourceFile
      End Get
      Set(ByVal Value As String)
         If Value Is Nothing Or Value = "" Then
            Throw New ArgumentNullException("Source file must be specified.")
         Else
            strSourceFile = Value
         End If
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''    Gets the imported data table.
   ''' </summary>
   ''' <value>TextData returns the dtTextData data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public ReadOnly Property TextData() As DataTable
      Get
         Return dtTextData
      End Get
   End Property

#End Region

#Region "Constructors"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''    Constructor creates a new TextLoader object using the the file path and text layout specified.
   ''' </summary>
   ''' <param name="SourceFile">The path to the file containing raw data to be imported.</param>
   ''' <param name="TableStructure">The layout of the data table once the data is imported.</param>
   ''' <remarks>
   '''   The constructor saves the source file name and sets the layout to the TextLayout object provided.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub New(ByVal SourceFile As String, ByVal TableStructure As TextLayout)

      ' Check for a file name('Nothing'is not allowed) and valid table structure before continuing
      If SourceFile Is Nothing Or SourceFile = "" Then
         Throw New ArgumentNullException("Source file must be specified.")
      Else
         strSourceFile = SourceFile
      End If

      ' If table structure is valid, then assign the TextLayout to the class
      If Not TableStructure.IsValid Then
         Throw New Exception("Table structure is invalid.")
      Else
         ftlLayout = TableStructure
      End If

   End Sub

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Constructor creates a new TextLoader object using the the file paths specified.
   ''' </summary>
   ''' <param name="SourceFile">The path to the file containing raw data to be imported.</param>
   ''' <param name="LayoutFile">The path to the file containing the layout of the data table.</param>
   ''' <remarks>
   '''   The constructor saves the source file name and creates a new TextLayout object using the XML file provided.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub New(ByVal SourceFile As String, ByVal LayoutFile As String)

      ' Check for a file name('Nothing' is not allowed) and valid table structure before continuing
      If SourceFile Is Nothing Or SourceFile = "" Then
         Throw New ArgumentNullException("Source file must be specified.")
      Else
         strSourceFile = SourceFile
      End If

      ' Try to create a text layout using the layout file specified, if not, throw an exception
      Try
         ftlLayout = New TextLayout(LayoutFile)
      Catch ex As Exception
         Throw New Exception("Error Creating Loader Layout: " & ex.ToString, ex)
      End Try

   End Sub

#End Region

#Region "Private Methods"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method creates a data table using the file name, delimiter, and columns specifed.
   ''' </summary>
   ''' <param name="strFileName">The path of the source file containing raw data.</param>
   ''' <param name="intSkipRows">Number of "header" rows to skip before importing data.</param>
   ''' <param name="strDelimiter">The delimiter separating data elements within the raw data.</param>
   ''' <param name="alColumns">The collection of column information in the data table.</param>
   ''' <returns>DataTable containing the imported data.</returns>
   ''' <remarks>
   '''   The file specified is loaded into an array, with each line of text as a new value in the array. If any rows
   '''   are to be skipped before processing, those rows are eliminated. The parser then iterates through each line 
   '''   of text, splitting the line based off of the delimiter and building the data table with each column value.
   '''   The resulting data table is returned after the process is completed. This method throws a custom exception 
   '''   if an error occurs during processing.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private Function ParseDelimitedText(ByVal strFileName As String, ByVal intSkipRows As Integer, _
                                       ByVal strDelimiter As String, ByVal alColumns As ArrayList) As DataTable

      ' Build the data table based on the column information
      Dim dtResults As New DataTable
      Dim drResults As DataRow
      Dim alFileContents As New ArrayList
      Dim astrSplitData() As String

      Try
         dtResults = BuildTableStructure(alColumns)
      Catch ex As Exception
         Throw New Exception("Error Building Table Structure: " & ex.ToString, ex)
      End Try

      Try
         alFileContents = LoadFileIntoArrayList(strFileName)
      Catch ex As Exception
         Throw New Exception("Error Loading File Contents: " & ex.ToString, ex)
      End Try

      ' If there is a designated amount of rows to skip, remove them from the ArrayList before proceeding
      If intSkipRows > 0 Then
         alFileContents.RemoveRange(0, intSkipRows)
      End If

      ' Iterate through each line of the loaded data, split the data into its individual fields based on the
      ' delimiter, and load into the data table.
      Try
         For Each DataLine As String In alFileContents
            astrSplitData = DataLine.Split(CType(strDelimiter, Char))

            ' Check the number of split items. If the number does not match the number of columns specified, throw an 
            ' exception since it will be unsafe to load the data into their respective columns.
            If astrSplitData.Length <> dtResults.Columns.Count Then
               Throw New Exception("Number of delimited items does not match column count. Import aborted.")
            End If

            ' Generate a new data row using the values specified. Add the row to the data table. If a value is 
            ' empty, change it to DBNull for insertion into the data table.
            drResults = dtResults.NewRow()
            For i As Integer = 0 To astrSplitData.Length - 1
               If astrSplitData(i) = "" Then
                  drResults(i) = System.DBNull.Value
               Else
                  ' Trim space from data if specified
                  If ftlLayout.TrimSpace Then
                     drResults(i) = astrSplitData(i).Trim
                  Else
                     drResults(i) = astrSplitData(i)
                  End If
               End If
            Next

            dtResults.Rows.Add(drResults)
         Next

      Catch ex As Exception
         Throw New Exception("Error loading delimited data: " & ex.ToString, ex)
      End Try

      ' After loading is complete, return the resulting data table
      Return dtResults

   End Function

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method creates a data table using the file name and columns specifed.
   ''' </summary>
   ''' <param name="strFileName">The path of the source file containing raw data.</param>
   ''' <param name="intSkipRows">Number of "header" rows to skip before importing data.</param>
   ''' <param name="alColumns">The collection of column information in the data table.</param>
   ''' <returns>DataTable containing the imported data.</returns>
   ''' <remarks>
   '''   The file specified is loaded into an array, with each line of text as a new value in the array. If any rows 
   '''   are to be skipped before processing, those rows are eliminated. The parser then iterates through each line 
   '''   of text, gathering and trimming the data based off of the position lengths specified. Each column in the 
   '''   data table stores its retrieved value. The resulting data table is returned after the process is completed.
   '''   This method throws a custom exception if an error occurs during processing.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private Function ParsePositionText(ByVal strFileName As String, ByVal intSkipRows As Integer, _
                                      ByVal alColumns As ArrayList) As DataTable

      ' Build the data table based on the column information
      Dim dtResults As New DataTable
      Dim drResults As DataRow
      Dim alFileContents As New ArrayList
      Dim strColumnData As String
      Dim intColumnCount As Integer

      Try
         dtResults = BuildTableStructure(alColumns)
      Catch ex As Exception
         Throw New Exception("Error Building Table Structure: " & ex.ToString, ex)
      End Try

      Try
         alFileContents = LoadFileIntoArrayList(strFileName)
      Catch ex As Exception
         Throw New Exception("Error Loading File Contents: " & ex.ToString, ex)
      End Try

      ' If there is a designated amount of rows to skip, remove them from the ArrayList before proceeding
      If intSkipRows > 0 Then
         alFileContents.RemoveRange(0, intSkipRows)
      End If

      ' Iterate through each line of the loaded data, use the data position and length parameters to gather the
      ' data, trim it, and insert it into the data column specified. Once the columns have been parsed, insert the
      ' row into the data table. If a value is empty, change it to DBNull for insertion into the data table.
      Try
         For Each DataLine As String In alFileContents

            drResults = dtResults.NewRow()

            intColumnCount = 0
            For Each ColumnInfo As Column In alColumns
               strColumnData = DataLine.Substring(ColumnInfo.StartingPosition, ColumnInfo.Length)

               ' Trim space from data if specified
               If ftlLayout.TrimSpace Then
                  strColumnData = strColumnData.Trim()
               Else
                  strColumnData = strColumnData
               End If

               If strColumnData = "" Then
                  drResults(intColumnCount) = System.DBNull.Value
               Else
                  drResults(intColumnCount) = strColumnData
               End If

               intColumnCount += 1
            Next

            dtResults.Rows.Add(drResults)
         Next

      Catch ex As Exception
         Throw New Exception("Error loading position data: " & ex.ToString, ex)
      End Try

      ' After loading is complete, return the resulting data table
      Return dtResults

   End Function

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method creates a data table using the file name, regular expression formula, and columns specifed.
   ''' </summary>
   ''' <param name="strFileName">The path of the source file containing raw data.</param>
   ''' <param name="intSkipRows">Number of "header" rows to skip before importing data.</param>
   ''' <param name="strFormula">The regular expression formula for the row of data.</param>
   ''' <param name="alColumns">The collection of column information in the data table.</param>
   ''' <returns>DataTable containing the imported data.</returns>
   ''' <remarks>
   '''   The source file is loaded into an array, with each line of text as a new value in the array. If any rows 
   '''   are to be skipped before processing, those rows are eliminated. The parser then iterates through each 
   '''   line of text, applying the table's regular expression formula against it to split the text into its 
   '''   individual groups. Each group is saved into its corresponding data column and the row is added to the data
   '''   table. The resulting data table is returned after the process is completed. This method throws a custom 
   '''   exception if an error occurs during processing.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private Function ParseRegexText(ByVal strFileName As String, ByVal intSkipRows As Integer, _
                                   ByVal strFormula As String, ByVal alColumns As ArrayList) As DataTable

      ' Build the data table based on the column information
      Dim dtResults As New DataTable
      Dim drResults As DataRow
      Dim alFileContents As New ArrayList
      Dim regexTableFormula As Regex
      Dim matchTableMatch As Match

      Try
         dtResults = BuildTableStructure(alColumns)
      Catch ex As Exception
         Throw New Exception("Error Building Table Structure: " & ex.ToString, ex)
      End Try

      Try
         alFileContents = LoadFileIntoArrayList(strFileName)
      Catch ex As Exception
         Throw New Exception("Error Loading File Contents: " & ex.ToString, ex)
      End Try

      ' If there is a designated amount of rows to skip, remove them from the ArrayList before proceeding
      If intSkipRows > 0 Then
         alFileContents.RemoveRange(0, intSkipRows)
      End If

      ' Iterate through each line of the loaded data, split the data into its individual fields based on the
      ' regular expression formula, and load into the data table.
      Try
         For Each strDataLine As String In alFileContents

            regexTableFormula = New Regex(strFormula)
            matchTableMatch = regexTableFormula.Match(strDataLine)

            ' Check for a valid match before continuing
            If matchTableMatch.Success = False Then
               Throw New Exception("Regular Expression match failed. Import aborted.")
            End If

            ' Check the number of match gruops. If the number does not match the number of columns specified, 
            ' throw an exception since it will be unsafe to load the data into their respective columns. The Match
            ' class includes an additional group item which is the entire matched value. For this reason the group
            ' count needs to be reduced by 1 for comparison
            If matchTableMatch.Groups.Count - 1 <> dtResults.Columns.Count Then
               Throw New Exception("Number of match groups does not match column count. Import aborted.")
            End If

            ' Generate a new data row using the values specified. Add the row to the data table. If a value is 
            ' empty, change it to DBNull for insertion into the data table.
            drResults = dtResults.NewRow()
            For i As Integer = 1 To matchTableMatch.Groups.Count - 1
               If matchTableMatch.Groups(i).Value = "" Then
                  drResults(i - 1) = System.DBNull.Value
               Else
                  ' Trim space from data if specified
                  If ftlLayout.TrimSpace Then
                     drResults(i - 1) = matchTableMatch.Groups(i).Value.Trim
                  Else
                     drResults(i - 1) = matchTableMatch.Groups(i).Value
                  End If

               End If

            Next

            dtResults.Rows.Add(drResults)
         Next

      Catch ex As Exception
         Throw New Exception("Error loading delimited data: " & ex.ToString, ex)
      End Try

      ' After loading is complete, return the resulting data table
      Return dtResults

   End Function

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method loads the data from the file name specifed into an ArrayList.
   ''' </summary>
   ''' <param name="strFileName">The path of the source file containing raw data.</param>
   ''' <returns>ArrayList containing the contents of the file.</returns>
   ''' <remarks>
   '''   Each entry in the ArrayList reflects one line of data in the file. The CR/LF characters are removed from 
   '''   each line of data before insertion into the ArrayList. If any errors are encountered during the loading 
   '''   process, a custom exception is thrown.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private Function LoadFileIntoArrayList(ByVal strFileName As String) As ArrayList

      Dim srSourceFile As StreamReader
      Dim strFileData As String
      Dim alResults As New ArrayList

      ' Next open the file specified and load its contents into an ArrayList. Since the text loader will not 
      ' manipulate the file contents, set the filesharing to ReadWrite to allow other applications to continue to use
      ' the file.
      srSourceFile = New StreamReader(strFileName)

      Try
         ' Read and save the lines from the file until the end of the file is reached. Clear out the newline
         ' characters from each line before adding it to the array
         strFileData = srSourceFile.ReadLine()
         While Not strFileData Is Nothing
            strFileData = strFileData.Replace(vbCr, "")
            strFileData = strFileData.Replace(vbLf, "")
            alResults.Add(strFileData)
            strFileData = srSourceFile.ReadLine()
         End While

      Catch fnfEx As FileNotFoundException
         Throw New Exception("Delimited parsing file not found: " & fnfEx.ToString(), fnfEx)

      Catch flEx As FileLoadException
         Throw New Exception("Error loading delimited parsing file: " & flEx.ToString(), flEx)

      Catch ex As Exception
         Throw New Exception("General Error parsing delimited file: " & ex.ToString, ex)

      Finally
         srSourceFile.Close()
         srSourceFile = Nothing

      End Try

      Return alResults

   End Function

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method builds an empty data table using the column definitions specifed.
   ''' </summary>
   ''' <param name="alColumnStructure">The collection of column information in the data table.</param>
   ''' <returns>A strongly-typed, empty DataTable based on the column information specified.</returns>
   ''' <remarks>
   '''   The ArrayList contains a collection of TextLoader.Column structures. If any errors are encountered during the 
   '''   loading process, a custom exception is thrown.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private Function BuildTableStructure(ByVal alColumnStructure As ArrayList) As DataTable

      Dim dtResults As New DataTable
      Dim dcNewColumn As DataColumn

      Try

         For Each TempColumn As Column In alColumnStructure

            ' When building column type, use additional parameters in the GetType function to throw an exception if
            ' the type doesn't exist and to perform a case insensitive search for the type. In addition, apppend the
            ' "System." prefix in front of the data type to search for the framework system types
            dcNewColumn = New DataColumn(TempColumn.ColumnName)
            dcNewColumn.DataType = Type.GetType("System." & TempColumn.ColumnType, True, True)
            dtResults.Columns.Add(dcNewColumn)

         Next

      Catch ex As Exception

         Throw New Exception("Error generating data table:" & ex.ToString(), ex)

      End Try

      Return dtResults

   End Function

#End Region

#Region "Public Methods"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method loads the data table based on the table type.
   ''' </summary>
   ''' <returns>DataTable containing loaded data.</returns>
   ''' <remarks>
   '''   If an exception is thrown by the corresponding parse method, the exception is passed up to the calling
   '''   method.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Function LoadDataTable() As DataTable

      ' Initialize internal data table to store results to be available through the dtData property
      dtTextData = New DataTable

      Try
         Select Case ftlLayout.TableType

            Case TextLayout.TableTypes.Delimeted
               dtTextData = ParseDelimitedText(strSourceFile, ftlLayout.SkipRows, ftlLayout.TableDelimiter, _
                                                ftlLayout.Columns)

            Case TextLayout.TableTypes.Position
               dtTextData = ParsePositionText(strSourceFile, ftlLayout.SkipRows, ftlLayout.Columns)

            Case TextLayout.TableTypes.Regex
               dtTextData = ParseRegexText(strSourceFile, ftlLayout.SkipRows, ftlLayout.TableFormula, _
                                            ftlLayout.Columns)

         End Select

         ' Return data table results.
         Return dtTextData

      Catch ex As Exception
         Throw New Exception("Error loading data table: " & ex.ToString, ex)
      End Try

   End Function

#End Region

End Class
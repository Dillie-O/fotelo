Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization

''' -----------------------------------------------------------------------------
''' Project	 : Fotelo
''' Class	 : TextLayout
''' 
''' -----------------------------------------------------------------------------
''' <summary>
'''   This module is designed to specify the data table configuration for a file being imported through the text 
'''   loader. The TextLayout class is serializable so that the layout can be configured programmatically or imported
'''   from a properly formatted XML file.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Sean Patterson]	6/14/2004	Created
''' </history>
''' -----------------------------------------------------------------------------
<XmlTypeAttribute(Namespace:="http://tempuri.org/TextLayout"), _
XmlRootAttribute("TextLayout", Namespace:="http://tempuri.org/TextLayout", IsNullable:=False)> _
Public Class TextLayout

#Region "Enumerations"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Acceptable type definitions for a table
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Enum TableTypes

      ''' -----------------------------------------------------------------------------
      ''' <summary>
      '''   Table that has a fixed width between each field value.
      ''' </summary>
      ''' <remarks>
      ''' </remarks>
      ''' <history>
      ''' 	[Sean Patterson]	6/14/2004	Created
      ''' </history>
      ''' -----------------------------------------------------------------------------
      <XmlEnum("Position")> Position = 1

      ''' -----------------------------------------------------------------------------
      ''' <summary>
      '''   Table that uses a specific character between each field value.
      ''' </summary>
      ''' <remarks>
      ''' </remarks>
      ''' <history>
      ''' 	[Sean Patterson]	6/14/2004	Created
      ''' </history>
      ''' -----------------------------------------------------------------------------
      <XmlEnum("Delimited")> Delimeted = 2

      ''' -----------------------------------------------------------------------------
      ''' <summary>
      '''   Table that uses a regular expression formula to group each field value.
      ''' </summary>
      ''' <remarks>
      ''' </remarks>
      ''' <history>
      ''' 	[Sean Patterson]	6/14/2004	Created
      ''' </history>
      ''' -----------------------------------------------------------------------------
      <XmlEnum("Regex")> Regex = 3

   End Enum

#End Region

#Region "Private Members"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The name of the data table.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private strTableName As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The type of table.
   ''' </summary>
   ''' <remarks>
   '''   Accepted types are Position, Delimted, and Regex
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private enumTableType As TableTypes

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The number of rows to skip before processing data during loading.
   ''' </summary>
   ''' <remarks>
   '''   This property is used to skip any header information that may exist in the file
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private intSkipRows As Integer

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The delimiter used for parsing delimited data.
   ''' </summary>
   ''' <remarks>
   '''   This property is used for Delimited type formats only.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private strTableDelimiter As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The formula used for parsing Regex data.
   ''' </summary>
   ''' <remarks>
   '''   This property is used for Regex type formats only.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private strTableFormula As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The collection of column information.
   ''' </summary>
   ''' <remarks>
   '''   The elements in the ArrayList are of type FormattedTextLoader.Column
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private alColumns As ArrayList

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Indicates whether or not extra spaces should be trimmed from the parsed data.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[c07884]	9/11/2006	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Private bTrimSpace As Boolean

#End Region

#Region "Public Properties"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the table name.
   ''' </summary>
   ''' <value>TableName returns the strTableName data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlElement(ElementName:="TableName")> _
   Public Property TableName() As String
      Get
         Return strTableName
      End Get
      Set(ByVal Value As String)
         strTableName = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the table type.
   ''' </summary>
   ''' <value>TableType returns the enumTableType data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlElement(ElementName:="TableType")> _
   Public Property TableType() As TableTypes
      Get
         Return enumTableType
      End Get
      Set(ByVal Value As TableTypes)
         enumTableType = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the number of rows to skip.
   ''' </summary>
   ''' <value>SkipRows returns the intSkipRows data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlElement(ElementName:="SkipRows")> _
   Public Property SkipRows() As Integer
      Get
         Return intSkipRows
      End Get
      Set(ByVal Value As Integer)
         intSkipRows = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the table delimiter.
   ''' </summary>
   ''' <value>TableDelimiter returns the strTableDelimiter data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlElement(ElementName:="TableDelimiter")> _
   Public Property TableDelimiter() As String
      Get
         Return strTableDelimiter
      End Get
      Set(ByVal Value As String)
         strTableDelimiter = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the table formula.
   ''' </summary>
   ''' <value>TableFormula returns the strTableFormula data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   '''-----------------------------------------------------------------------------
   <XmlElement(ElementName:="TableFormula")> _
   Public Property TableFormula() As String
      Get
         Return strTableFormula
      End Get
      Set(ByVal Value As String)
         strTableFormula = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the table column collection
   ''' </summary>
   ''' <value>Columns returns the alColumns data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlArray("Columns"), XmlArrayItem(GetType(Column))> _
   Public Property Columns() As ArrayList
      Get
         Return alColumns
      End Get
      Set(ByVal Value As ArrayList)
         alColumns = Value
      End Set
   End Property

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''     Gets/Sets the number the trim space flag.
   ''' </summary>
   ''' <value>TrimSpace returns the bTrimSpace data member.</value>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   <XmlElement(ElementName:="TrimSpace")> _
   Public Property TrimSpace() As Boolean
      Get
         Return bTrimSpace
      End Get
      Set(ByVal Value As Boolean)
         bTrimSpace = Value
      End Set
   End Property

#End Region

#Region "Constructors"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Creates an empty TextLayout object.
   ''' </summary>
   ''' <remarks>
   '''   The constructor initializes the column collection for storage.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub New()

      Columns = New ArrayList

   End Sub

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Creates a TextLayout object using the information file specified.
   ''' </summary>
   ''' <param name="FileName">The file path containing table information in XML format.</param>
   ''' <remarks>
   '''   The constructor uses the ImportControlFile method to create the object using the data within the file.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub New(ByVal FileName As String)

      ImportControlFile(FileName)

   End Sub

#End Region

#Region "Public Methods"

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   This method generates a string with the details of all the values in the object. 
   ''' </summary>
   ''' <returns>String formatted to contain the data members and their values of the object.</returns>
   ''' <remarks>
   '''   Useful for debugging the object.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Overrides Function ToString() As String

      Dim sbResults As New StringBuilder

      sbResults.Append("Text Layout Details:" & vbCrLf)
      sbResults.Append("Table Name: " & strTableName & vbCrLf)
      sbResults.Append("Table Type: " & enumTableType.ToString & vbCrLf)
      sbResults.Append("Table Delimiter: " & strTableDelimiter & vbCrLf)
      sbResults.Append("Table Formula: " & strTableFormula & vbCrLf)
      sbResults.Append("Trim Space: " & bTrimSpace.ToString & vbCrLf)
      sbResults.Append("Skip Rows: " & intSkipRows & vbCrLf & vbCrLf)
      sbResults.Append("Columns: " & Columns.Count & vbCrLf)
      For Each Column As Column In alColumns
         sbResults.Append("Column Name: " & Column.ColumnName & vbCrLf)
         sbResults.Append("       Type: " & Column.ColumnType & vbCrLf)
         sbResults.Append("   Position: " & Column.StartingPosition & vbCrLf)
         sbResults.Append("     Length: " & Column.Length & vbCrLf & vbCrLf)
      Next

      Return sbResults.ToString

   End Function

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Serializes TextLayout object into an XML control file.
   ''' </summary>
   ''' <param name="strFileName">The file path of the resulting control file.</param>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub ExportControlFile(ByRef strFileName As String)

      ' Initialize serializer, stream writer and serialize self to XML
      Dim xsSerializer As XmlSerializer = New XmlSerializer(GetType(TextLayout))
      Dim swWriter As StreamWriter = New StreamWriter(strFileName)

      xsSerializer.Serialize(swWriter, Me)

   End Sub

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Deserializes XML control file into TextLayout object.
   ''' </summary>
   ''' <param name="strFileName">The file path of the control file.</param>
   ''' <remarks>
   '''   This method deserializes the filename specified into the class structure. The control file is XML 
   '''   formatted. Once the file has been deserialized, a few condition checks are made to ensure that the 
   '''   class will function properly. If these conditions are met, the values are passed into the class.
   '''   Otherwise, and exception is thrown.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Sub ImportControlFile(ByRef strFileName As String)

      ' Initialize serializer, stream reader, XML reader, XML Validator and text layout object
      Dim xsSerializer As XmlSerializer = New XmlSerializer(GetType(fotelo.TextLayout))
      Dim xtrXmlReader As XmlReader
      Dim xrsXmlReaderSettings As XmlReaderSettings = New XmlReaderSettings()
      Dim ftlPendingLayout As New TextLayout
      Dim strAppPath As String

      strAppPath = System.IO.Path.GetDirectoryName( _
                   System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)

      ' Setup XML Reader Settings to be a validating reader and create a new XML Reader accordingly
      xrsXmlReaderSettings.ValidationType = ValidationType.Schema
      xrsXmlReaderSettings.Schemas.Add("http://tempuri.org/TextLayout", strAppPath & "/VerificationLayout.xsd")
      xtrXmlReader = XmlReader.Create(strFileName, xrsXmlReaderSettings)

      ' Load XML file into "holding bin". Use the XMLValidator to verify proper XML structure
      Try
         ftlPendingLayout = CType(xsSerializer.Deserialize(xtrXmlReader), TextLayout)
      Catch ex As Exception
         Throw New Exception("Error validating table layout: " & ex.ToString, ex)
      Finally
         xtrXmlReader.Close()
         xtrXmlReader = Nothing
      End Try

      ' In order for the class to function properly, the following conditions must exist:
      '   Table Name exists
      '   Table Type exists
      '   Number of rows to skip cannot be < 0
      '   At least 1 column exists in the Columns collection
      '   Table Formula if Regex type table is specified
      '   Table Delimiter if Delimiter type table is specified
      '   Starting Position and Position Length values for each column if Position type table is specified

      ' The XSD used for validating the configuration file will throw an exception if any of the required elements 
      ' are missing (Name, Type, Columns Collection). However, the XSD will not check for the supplemental 
      ' information required for each table type. Check for these conditions before continuing.

      If ftlPendingLayout.SkipRows < 0 Then
         Throw New Exception("Error Creating Layout. The number of rows to skip cannot be less than 0")
      End If

      If ftlPendingLayout.TableType = TableTypes.Delimeted And ftlPendingLayout.TableDelimiter Is Nothing Then
         Throw New Exception("Error Creating Layout. Delimiter must be spefified for Delimiter type table.")
      End If

      If ftlPendingLayout.TableType = TableTypes.Regex And ftlPendingLayout.TableFormula Is Nothing Then
         Throw New Exception("Error Creating Layout. Formula must be speficied for Regex type table.")
      End If

      ' For Position type layouts, iterate through each column to make sure a starting position and length exists.
      ' The deserializing function will automatically set the starting position and length to 0 if nothing is 
      ' specified. This will be the test for missing data, since a starting position and length of 0 would be invalid
      ' for a column.
      If ftlPendingLayout.TableType = TableTypes.Position Then
         For Each TestColumn As Column In ftlPendingLayout.Columns
            If TestColumn.StartingPosition = 0 And TestColumn.Length = 0 Then
               Throw New ArgumentNullException("Column", "Starting Position or Length missing in Position " & _
                                                       "type table column.")
            End If
         Next
      End If

      ' If the import has reached this point, the "holding bin" contains a valid TextLayout class structure.
      strTableName = ftlPendingLayout.TableName
      enumTableType = ftlPendingLayout.TableType
      intSkipRows = ftlPendingLayout.SkipRows
      strTableDelimiter = ftlPendingLayout.TableDelimiter
      strTableFormula = ftlPendingLayout.TableFormula
      bTrimSpace = ftlPendingLayout.TrimSpace
      alColumns = New ArrayList(ftlPendingLayout.Columns)

   End Sub

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Performs sanity check on object.
   ''' </summary>
   ''' <returns>Boolean value indicating if class is valid.</returns>
   ''' <remarks>
   '''   Since the class can be created through manual programming or file deserialization and modified,
   '''   this method provides a quick sanity check to make sure all the appropriate elements exist in the
   '''   text layout.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Function IsValid() As Boolean

      ' Assume a valid text layout at the beginning.
      Dim bResults As Boolean = True

      ' Check for conditions that would cause the class to function improperly. In order for the class to 
      ' function properly, the following conditions must exist:
      '   Table Name exists
      '   Table Type is valid
      '   At least 1 column exists in the Columns collection
      '   Table Regex if Regex type table is specified
      '   Table Delimiter if Delimiter type table is specified
      '   Starting Position and Position Length values for each column if Position type table is specified

      If strTableName Is Nothing Then
         bResults = False
      End If

      If intSkipRows < 0 Then
         bResults = False
      End If

      If enumTableType <> TableTypes.Delimeted AndAlso enumTableType <> TableTypes.Position _
         AndAlso enumTableType <> TableTypes.Regex Then
         bResults = False
      End If

      If alColumns Is Nothing Or alColumns.Count = 0 Then
         bResults = False
      End If

      If enumTableType = TableTypes.Delimeted And strTableDelimiter Is Nothing Then
         bResults = False
      End If

      If enumTableType = TableTypes.Regex And strTableFormula Is Nothing Then
         bResults = False
      End If

      ' For Position type layouts, iterate through each column to make sure a starting position and length 
      ' exists. The deserializing function will automatically set the starting position and length to 0 if 
      ' nothing is specified. This will be the test for missing data, since a starting position and length of 0
      ' would be invalid for a column.
      If enumTableType = TableTypes.Position Then
         For Each TestColumn As Column In alColumns
            If TestColumn.StartingPosition = Nothing Or TestColumn.Length = Nothing Or _
               (TestColumn.StartingPosition = 0 And TestColumn.Length = 0) Then
               bResults = False
            End If
         Next
      End If

      ' After all checks have been made, return results
      Return bResults

   End Function

#End Region

End Class
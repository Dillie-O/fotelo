''' -----------------------------------------------------------------------------
''' Project	 : FormattedTextLoader
''' Struct	 : Column
''' 
''' -----------------------------------------------------------------------------
''' <summary>
'''   The structure of a column in a data table.
''' </summary>
''' <remarks>
'''   The Name and Type values are the only required in the structure.
''' </remarks>
''' <history>
''' 	[Sean Patterson]	6/14/2004	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Structure Column
   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Name of the column.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public ColumnName As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Value Type to be stored in column.
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public ColumnType As String

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   Starting position in a line of text for this column's value.
   '''   <note>This value is only used in Position type tables.</note>
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public StartingPosition As Integer

   ''' -----------------------------------------------------------------------------
   ''' <summary>
   '''   The number of characters in a line of text this column uses in the data file.
   '''   <note>This value is only used in Position type tables.</note>
   ''' </summary>
   ''' <remarks>
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	6/14/2004	Created
   ''' </history>
   ''' -----------------------------------------------------------------------------
   Public Length As Integer

End Structure
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="FTL_Demo.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Fotelo Demo/Test</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <div style="text-align: center">
          <table border="1" cellpadding="0" cellspacing="0" width="750">
             <tr>
                <td style="background-color: #6b696b">
                   <span style="font-size: 13pt; color: #ffffff; font-family: Verdana">Fotelo Demo/Test</span></td>
             </tr>
             <tr>
                <td align="left">
                   <asp:Label ID="lblError" runat="server" Font-Bold="True"
                      Font-Names="Verdana" Font-Size="X-Small" ForeColor="Red" Visible="False">Error</asp:Label><span
                         style="font-size: 10pt; font-family: Verdana">&nbsp;</span></td>
             </tr>
             <tr style="font-size: 10pt; font-family: Verdana">
                <td style="background-color: #f7f7de">
                   <span style="FONT-SIZE: 10pt; COLOR: #000000; FONT-FAMILY: Verdana"><strong>Test Criteria</strong></span></td>
             </tr>
             <tr style="font-size: 12pt; font-family: Times New Roman">
                <td align="left">
                   <span style="font-family:Verdana; font-size:10pt;">Specify Conversion Type:</span>
                   <asp:DropDownList ID="ddlConversionType" runat="server">
                      <asp:ListItem Selected="True" Value="Position">Position Dependent</asp:ListItem>
                      <asp:ListItem Value="Delimited">Delimited</asp:ListItem>
                      <asp:ListItem Value="Regex">Regular Expression</asp:ListItem>
                   </asp:DropDownList>
                   <asp:Button ID="btnParseFile" runat="server" Text="Go" />
                </td>
             </tr>
             <tr>
                <td style="background-color: #f7f7de">
                   <strong><span style="font-size: 10pt; color: #000000; font-family: Verdana">Control
                      File Details</span></strong></td>
             </tr>
             <tr>
                <td align="left" style="text-align: center">
                   <asp:TextBox ID="txtControlFileDetails" runat="server" Columns="90" Rows="10" TextMode="MultiLine"
                      Wrap="False"></asp:TextBox><br />
                   <asp:HyperLink ID="lnkViewControlFile" runat="server" Font-Names="Verdana" Font-Size="X-Small"
                      Target="_blank">View Control File (XML)</asp:HyperLink></td>
             </tr>
             <tr>
                <td style="background-color: #f7f7de">
                   <strong><span style="font-size: 10pt; color: #000000; font-family: Verdana">Raw Data
                      Text</span></strong></td>
             </tr>
             <tr>
                <td align="left">
                   <asp:TextBox ID="txtRawData" runat="server" Columns="90" Rows="10" TextMode="MultiLine"
                      Wrap="False"></asp:TextBox></td>
             </tr>
             <tr>
                <td style="background-color: #f7f7de">
                   <strong><span style="font-size: 10pt; color: #000000; font-family: Verdana">Resulting
                      Data Table</span></strong></td>
             </tr>
             <tr>
                <td>
                   <asp:GridView ID="gvResults" runat="server" AllowPaging="True" BackColor="White"
                      BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Names="Verdana"
                      Font-Size="XX-Small" ForeColor="Black" GridLines="Vertical" PageSize="25">
                      <FooterStyle BackColor="#CCCC99" />
                      <RowStyle BackColor="#F7F7DE" />
                      <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                      <PagerStyle BackColor="#6B696B" ForeColor="White" HorizontalAlign="Right" />
                      <HeaderStyle BackColor="#6B696B" Font-Bold="True" Font-Size="X-Small" ForeColor="White" />
                      <AlternatingRowStyle BackColor="White" />
                   </asp:GridView>
                </td>
             </tr>
          </table>
       </div>
    
    </div>
    </form>
</body>
</html>

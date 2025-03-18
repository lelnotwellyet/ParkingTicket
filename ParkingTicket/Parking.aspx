<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Parking.aspx.cs" Inherits="ParkingTicket.Parking" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Parking Fare Calculator</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="txtCarNumber">Car Number:</label>
            <asp:TextBox ID="txtCarNumber" runat="server"></asp:TextBox>
            <br />

            <label for="txtInTime">In-Time:</label>
            <asp:TextBox ID="txtInTime" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
            <br />

            <label for="txtOutTime">Out-Time:</label>
            <asp:TextBox ID="txtOutTime" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
            <br />

            <asp:Button ID="btnCalculate" runat="server" Text="Calculate Fare" OnClick="btnCalculate_Click" />
            <br />

            <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
            <br />

            <asp:Button ID="btnLoadRecords" runat="server" Text="Load Parking Records" OnClick="btnLoadRecords_Click" />
            <asp:Button ID="btnSearch" runat="server" Text="Search by Car Number" OnClick="btnSearch_Click" />
            <asp:TextBox ID="txtSearchCarNumber" runat="server" Placeholder="Enter Car Number"></asp:TextBox>
            <br />

            <asp:GridView ID="gvParkingRecords" runat="server" AutoGenerateColumns="False" BorderWidth="1" 
                OnRowCommand="gvParkingRecords_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CarNumber" HeaderText="Car Number" />
                    <asp:BoundField DataField="InTime" HeaderText="In Time" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="OutTime" HeaderText="Out Time" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="Duration" HeaderText="Duration (hrs)" />
                    <asp:BoundField DataField="Fare" HeaderText="Fare (₹)" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure?');"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>

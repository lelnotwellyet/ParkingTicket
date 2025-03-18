<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditParking.aspx.cs" Inherits="ParkingTicket.EditParking" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Parking Record</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Edit Parking Record</h2>
            <label for="txtCarNumber">Car Number:</label>
            <asp:TextBox ID="txtCarNumber" runat="server"></asp:TextBox>
            <br />

            <label for="txtInTime">In-Time:</label>
            <asp:TextBox ID="txtInTime" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
            <br />

            <label for="txtOutTime">Out-Time:</label>
            <asp:TextBox ID="txtOutTime" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
            <br />

            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
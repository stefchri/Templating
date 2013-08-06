<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template.aspx.cs" Inherits="Template" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TestParsing templates</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <p runat="server" id="template"></p>
        <p>__________________________________________________________</p>
        <h2>Parsed</h2>
        <p runat="server" id="parsed"></p>
    </div>
    </form>
</body>
</html>

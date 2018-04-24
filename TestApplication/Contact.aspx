<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="StatusPage.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3 style="color:white">Information Services Contact.</h3>
    <address style="color:white">
        University of Portland<br />
        5000 N. Willamette Blvd.<br />
        Portland, Oregon 97203-5798<br />
        <abbr title="Phone">P:</abbr>
        503.943.7000
    </address>

    <address>
        <strong style="color:white">Need Help? Email: </strong>   <a href="mailto:help@up.edu">help@up.edu</a><br />
    </address>
</asp:Content>

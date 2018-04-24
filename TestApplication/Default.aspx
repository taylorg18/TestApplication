<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StatusPage._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1><font color="purple">University of</font></h1>
        <h1><font color="purple">Portland Services Status</font></h1>
        <p class="lead">
            <asp:Label ID="Label2" runat="server" Text="Label" ForeColor="Purple" Font-Size="Medium"></asp:Label>
        </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <p>
                &nbsp;</p>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers >
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="label1" runat="server" ForeColor="White"></asp:Label>
                    <asp:GridView ID="GridView1" runat="server" Width="1159px">
                    </asp:GridView>
                   <asp:Timer ID="Timer1" runat="server" Interval="300000" OnTick="Timer1_Tick">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
           
           
        </div>
    </div>

</asp:Content>

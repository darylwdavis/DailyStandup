<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyStandup.aspx.cs" Inherits="DWD_DailyStandup.Main.DailyStandup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style4
    {
        width: 205px;
    }
        .style5
        {
            height: 138px;
            width: 199px;
        }
        .style7
        {
            height: 138px;
            width: 200px;
        }
        .style8
        {
            width: 200px;
        }
        .style11
        {
            width: 199px;
        }
        .style12
        {
            width: 201px;
        }
        .style13
        {
            height: 138px;
            width: 201px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style1">
    <tr>
        <td class="style4">
            <asp:Calendar ID="Calendar1" runat="server" 
                onselectionchanged="Calendar1_SelectionChanged" BackColor="White" 
                BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" 
                ForeColor="Black" Height="123px" NextPrevFormat="FullMonth" Width="228px">
                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" 
                    VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#999999" />
                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" 
                    Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                <TodayDayStyle BackColor="#CCCCCC" />
            </asp:Calendar>
        </td>
        <td>
            <table class="style1" style="height: 55px; width: 99%">
                <tr>
                    <td class="style8">
                        <strong>What I did Yesterday</strong></td>
                    <td class="style11">
                        <strong>What I will do Today</strong></td>
                    <td class="style12">
                        <strong>What Impedes me</strong></td>
                </tr>
                <tr>
                    <td class="style7">
                        <asp:TextBox ID="txtYesterday" runat="server" Height="128px" 
                            TextMode="MultiLine" Width="195px">Yesterday I </asp:TextBox>
                    </td>
                    <td class="style5">
                        <asp:TextBox ID="txtToday" runat="server" Height="128px" TextMode="MultiLine" 
                            Width="195px">Today I </asp:TextBox>
                    </td>
                    <td class="style13">
                        <asp:TextBox ID="txtYesterday0" runat="server" Height="128px" 
                            TextMode="MultiLine" Width="195px">I&#39;m impeded by </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="style4">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>

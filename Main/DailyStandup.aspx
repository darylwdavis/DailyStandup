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
            height: 6px;
        }
        .style11
        {
            width: 199px;
            height: 6px;
        }
        .style12
        {
            width: 201px;
            height: 6px;
        }
        .style13
        {
            height: 138px;
            width: 201px;
        }
        .style14
        {
            width: 205px;
            height: 174px;
        }
        .style15
        {
            height: 174px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style1">
    <tr>
        <td class="style14">
            <asp:Calendar ID="Calendar1" runat="server" 
                onselectionchanged="Calendar1_SelectionChanged" BackColor="White" 
                BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
                ForeColor="Black" Height="180px" Width="200px" CellPadding="4" 
                DayNameFormat="Shortest">
                <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                <NextPrevStyle 
                    VerticalAlign="Bottom" />
                <OtherMonthDayStyle ForeColor="#808080" />
                <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                <SelectorStyle BackColor="#CCCCCC" />
                <TitleStyle BackColor="#999999" BorderColor="Black" 
                    Font-Bold="True" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <WeekendDayStyle BackColor="#FFFFCC" />
            </asp:Calendar>
        </td>
        <td class="style15">
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
                        <asp:TextBox ID="txtYesterday" runat="server" Height="167px" 
                            TextMode="MultiLine" Width="200px" Font-Names="Tahoma">Yesterday I </asp:TextBox>
                    </td>
                    <td class="style5">
                        <asp:TextBox ID="txtToday" runat="server" Height="164px" TextMode="MultiLine" 
                            Width="200px" Font-Names="Tahoma">Today I </asp:TextBox>
                    </td>
                    <td class="style13">
                        <asp:TextBox ID="txtImpediments" runat="server" Height="166px" 
                            TextMode="MultiLine" Width="200px" Font-Names="Tahoma">I&#39;m impeded by </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="style4">
            &nbsp;</td>
        <td>
            <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" 
                Text="Add Your Stand-up" />
        </td>
    </tr>
</table>
</asp:Content>

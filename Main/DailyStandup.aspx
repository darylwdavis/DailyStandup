<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyStandup.aspx.cs" Inherits="DWD_DailyStandup.Main.DailyStandup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="shortcut icon" href="~/favicon.ico" type="image/x-icon"/>
    <link rel="icon" href="~/favicon.ico" type="image/x-icon"/>
    <style type="text/css">
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
            width: 192px;
            height: 174px;
        }
        .style15
        {
            height: 174px;
        }
        .style16
        {
            width: 192px;
            height: 17px;
        }
        .style17
        {
            height: 17px;
        }
        .style18
        {
            width: 130px;
        }
        .style20
        {
            width: 135px;
        }
        .style26
        {
            height: 78px;
        }
        .style31
        {
            width: 115px;
        }
        .style32
        {
            width: 57px;
        }
        .style33
        {
            height: 21px;
        }
        </style>  
</asp:Content>   
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style1">
    <tr>
        <td class="style16">
            <table class="style1">
                <tr>
                    <td class="style18">
            <asp:SqlDataSource ID="sdsProjects" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                SelectCommand="SELECT [Project], [ProjectID] FROM [tblProjects] ORDER BY [Project]" 
                            onselecting="sdsProjects_Selecting"></asp:SqlDataSource>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
        <td class="style17">
            &nbsp;<asp:Label ID="lblProjectDesc" runat="server" Font-Size="Large" 
                Text="Project"></asp:Label>
&nbsp;
            <asp:DropDownList ID="ddlProjects" runat="server" DataSourceID="sdsProjects" 
                DataTextField="Project" DataValueField="ProjectID" AutoPostBack="True"
                 onDataBound="ddlProjects_DataBound"
                onselectedindexchanged="ddlProjects_SelectedIndexChanged" Height="30px" 
                Width="451px">
            </asp:DropDownList>
            
        &nbsp;</td>
    </tr>
    <tr>
        <td class="style14">
            <table class="style1">
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
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
            <table class="style1" style="height: 205px; width: 100%">
                <tr>
                    <td class="style8">
                        <table class="style1" style="width: 117%">
                            <tr>
                                <td class="style20">
                        <strong>What I did Yesterday</strong></td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td class="style11">
                        <table class="style1" style="width: 120%">
                            <tr>
                                <td class="style20">
                        <strong>What I will do Today</strong></td>
                                <td>
                        <strong>
                                    <asp:Label ID="lblDate" runat="server" Text="yyyy-mm-dd"></asp:Label>
                                    </strong>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="style12">
                        <table class="style1" style="width: 117%">
                            <tr>
                                <td class="style20">
                        <strong>What Impedes me</strong></td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style7">
                        <asp:TextBox ID="txtYesterday" runat="server" Height="170px" 
                            TextMode="MultiLine" Width="200px" Font-Names="Tahoma">Yesterday I </asp:TextBox>
                    </td>
                    <td class="style5">
                        <asp:TextBox ID="txtToday" runat="server" Height="170px" TextMode="MultiLine" 
                            Width="200px" Font-Names="Tahoma">Today I </asp:TextBox>
                    </td>
                    <td class="style13">
                        <asp:TextBox ID="txtImpediments" runat="server" Height="170px" 
                            TextMode="MultiLine" Width="200px" Font-Names="Tahoma">My impediments are </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
    <table class="style1" style="height: 232px">
        <tr>
            <td class="style26">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <table class="style1">
                            <tr>
                                <td>
                                    <asp:Button ID="btnNewProject" runat="server" onclick="btnNewProject_Click" 
                                        style="text-align: right" Text="New Project" />
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click" 
                                        Text="Add Your Stand-up" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <table class="style1" style="border-style: groove; height: 124px" 
                            frame="border">
                            <tr>
                                <td class="style31">
                                    <strong style="text-align: right">New Project</strong></td>
                                <td class="style32">
                                    <strong>Name:</strong></td>
                                <td>
                                    <asp:TextBox ID="txtNewProject" runat="server" style="margin-left: 0px" 
                                        Width="736px" Font-Names="Times New Roman"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style31">
                                    <asp:Button ID="btnAddProject" runat="server" onclick="btnAddProject_Click" 
                                        Text="Add Project" />
                                </td>
                                <td class="style32">
                                    <strong>Details:</strong></td>
                                <td>
                                    <asp:TextBox ID="txtNewProjectDetails" runat="server" Height="53px" 
                                        TextMode="MultiLine" Width="738px" Font-Names="Times New Roman"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style31">
                                    <asp:Button ID="btnCancelAdd" runat="server" onclick="btnCancelAdd_Click" 
                                        Text="Cancel" />
                                </td>
                                <td class="style32">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
</asp:Content>

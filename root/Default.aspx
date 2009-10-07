<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/Templates/Default.Master"
    CodeBehind="Default.aspx.cs" Inherits="Apprenda.Taskr.Web._Default" %>

<%@ Import Namespace="Apprenda.SaaSGrid" %>
<%@ Import Namespace="Apprenda.SaaSGrid.Subscription" %>

<script runat="server">
    bool priorityEnabled = SubscriptionContext.Instance.IsToggleEnabled("Priority Management").Value; 
</script>

<asp:Content ContentPlaceHolderID="BodyContent" runat="server">

    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(GiveFocus);        
        function GiveFocus()
        {           
            document.getElementById('<%= SubjectField.ClientID %>').focus();
            return;
        }            
        window.onload = function() { GiveFocus(); }        
    </script>
    
    <asp:UpdatePanel ID="TaskUpdater" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="90%" cellpadding="0" cellspacing="20" style="margin: auto; text-align: center;">
                <tr>
                    <td valign="top" align="center" style="width: 25%;">
                        <div class="section">
                            <h3 class="title">
                                New Task</h3>
                            <asp:Panel ID="NewTaskPanel" runat="server" DefaultButton="SaveTaskButton">
                                <table width="85%" style="text-align: left;" cellpadding="8" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <p>
                                                Subject:
                                                <br />
                                                <asp:TextBox ID="SubjectField" runat="server" Width="100%"></asp:TextBox>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <p>
                                                Description:
                                                <br />
                                                <asp:TextBox ID="DescriptionField" runat="server" TextMode="MultiLine" Rows="8" Width="100%"></asp:TextBox>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <p>
                                                Due Date:
                                                <br />
                                                <asp:TextBox runat="server" Width="100%" ID="DueDate" />
                                        </td>
                                        <td style="width: 50%;">
                                            <p>
                                                <% 
                                                    if (priorityEnabled)
                                                    {
                                                %>
                                                Priority:
                                                <br />
                                                <asp:DropDownList ID="PriorityField" runat="server" Width="100%">
                                                </asp:DropDownList>
                                                <%
                                                    }
                                                %>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <p>
                                                Tags:<br />
                                                <asp:ListBox ID="TagList" runat="server" Width="100%" SelectionMode="Multiple"></asp:ListBox>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <p>
                                                <asp:Button ID="SaveTaskButton" runat="server" OnCommand="SaveTask" Text="Create Task" />
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <div class="section-shadow">
                        </div>
                    </td>
                    <td valign="top" align="left">
                        <div class="section">
                            <h3 class="title">
                                Task List</h3>
                            <asp:GridView ID="TaskList" runat="server" DataKeyNames="Id">
                                <EmptyDataTemplate>
                                    <br />
                                    <p style="text-align: center;">
                                        <asp:Label ID="EmptyListLabel" runat="server">
                                                You don't have any tasks.
                                        </asp:Label>
                                    </p>
                                    <br />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField DataField="Id" Visible="false" ShowHeader="false" />
                                    <asp:TemplateField ItemStyle-CssClass="datagrid-item">
                                        <ItemTemplate>
                                            <strong>
                                                <asp:HyperLink ID="TaskLink" runat="server" Text='<%# Eval("Subject") %>' NavigateUrl='<%# String.Format(@"EditTask.aspx?taskId={0}", Eval("Id")) %>'></asp:HyperLink>
                                            </strong><font color="#999999"><em>
                                                <asp:Label ID="DescriptionField" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </em></font>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="datagrid-item" HeaderText="Due">
                                        <ItemTemplate>
                                            <asp:Label ID="DueDate" runat="server" Text='<%# Eval("DueDate", "{0:M/dd/yyyy}") %>'
                                                CssClass='<%# ((DateTime)Eval("DueDate")).Date < DateTime.Now.Date ? "overdue" : "" %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                        
                                    <asp:TemplateField ItemStyle-CssClass="datagrid-item" HeaderText="Priority" >
                                        <ItemTemplate>                                            
                                            <span class="priority-<%# Eval("Priority") %>">
                                                <asp:Label ID="Priority" runat="server" Text='<%# Eval("Priority") %>'></asp:Label>
                                            </span>                                            
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="datagrid-item">
                                        <ItemTemplate>
                                            <asp:Button ID="DeleteButton" runat="server" OnCommand="DeleteTask" CommandArgument='<%# Eval("Id") %>'
                                                Text="Delete" CssClass="small-button" Visible='<%# UserContext.Instance.IsAuthorized("Delete Tasks") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

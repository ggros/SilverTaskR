<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="EditTask.aspx.cs" 
    MasterPageFile="~/Templates/Default.Master"
    Inherits="Apprenda.Taskr.Web.EditTask" %>

<%@ Import Namespace="Apprenda.SaaSGrid.Subscription" %>    
    
<asp:Content ContentPlaceHolderID="BodyContent" runat="server">

    <script language="javascript" type="text/javascript">        
        function GiveFocus()
        {           
            document.getElementById('<%= SubjectField.ClientID %>').focus();
            return;
        }            
        window.onload = function() { GiveFocus(); }        
    </script>

<asp:HiddenField ID="TaskIdField" runat="server" />

                            <div class="section" style="width: 600px; margin: 0 auto;">
                                <h3 class="title">
                                    Edit Task</h3>
                                    <asp:Panel ID="NewTaskPanel" runat="server" DefaultButton="SaveTaskButton">
                                    <table width="90%" style="text-align: left; margin: 0 auto;" cellpadding="8" cellspacing="0">
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
                                             <asp:TextBox ID="DueDate" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        <td style="width: 50%;">
                                            <p>
                                                <% 
                                                    if (SubscriptionContext.Instance.IsToggleEnabled("Priority Management").Value)
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
                                                <asp:ListBox ID="TagList" runat="server" Width="100%"
                                                    SelectionMode="Multiple"></asp:ListBox>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <p>
                                                <asp:Button ID="SaveTaskButton" runat="server" Text="Save Task" OnClick="SaveTask" />
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                        </div>    

</asp:Content>
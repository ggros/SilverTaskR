<%@ Page Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/Templates/Default.Master"
    CodeBehind="Admin.aspx.cs" 
    Inherits="Apprenda.Taskr.Web.Admin" %>

<%@ Import Namespace="Apprenda.SaaSGrid" %>

<asp:Content ContentPlaceHolderID="BodyContent" runat="server">

    <script language="javascript" type="text/javascript">        
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(GiveFocus);
        function GiveFocus()
        {                                   
            document.getElementById('<%= TagField.ClientID %>').focus();
            return;
        }            
        window.onload = function() { GiveFocus(); }        
    </script>


    <asp:UpdatePanel ID="NewTagUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="600" cellpadding="0" cellspacing="20" style="margin: 10px auto; text-align: center;">
                <tr>
                    <td>
                        <asp:Panel ID="NewTagPanel" runat="server" DefaultButton="SaveTagButton">
                        
                            <% 
                                if (UserContext.Instance.IsAuthorized("Modify Tags"))
                                {
                            %>
                            <p>
                                <asp:TextBox ID="TagField" runat="server" Width="200"></asp:TextBox>
                                <asp:Button ID="SaveTagButton" runat="server" Text='Add Tag' OnCommand="SaveTag" />
                            </p>
                            <%
                                }
                            %>
                            <hr />
                            <asp:GridView ID="TagList" runat="server">
                                <EmptyDataTemplate>
                                    <br />
                                    <p style="text-align: center;">
                                        <asp:Label ID="EmptyListLabel" runat="server">
                                                There aren't any tags.
                                        </asp:Label>
                                    </p>
                                    <br />
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField DataField="label" ItemStyle-CssClass="datagrid-item" />
                                    <asp:TemplateField ItemStyle-CssClass="datagrid-item" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Button ID="DeleteButton" runat="server" 
                                                Text="Delete" OnCommand="DeleteTag" CommandArgument='<%# Eval("Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
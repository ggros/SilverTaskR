<%@ Page Language="C#" 
         AutoEventWireup="true" 
         MasterPageFile="~/Templates/Default.Master"
         CodeBehind="Subscription.aspx.cs" 
         Inherits="Apprenda.Taskr.Web.Subscription"  %>

<%@ Import Namespace="Apprenda.SaaSGrid" %>
<%@ Import Namespace="Apprenda.SaaSGrid.Subscription" %>

<asp:content contentplaceholderid="BodyContent" runat="server">    
            
        <table width="70%" cellpadding="0" cellspacing="4" style="margin: 20px auto;">
            <tr>
                <td colspan="2">
                    <h3>User Profile</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <p>
                        <strong>                                          
                            Name:           
                        </strong>
                        <%= UserData.FirstName %> <%= UserData.LastName %>
                    </p>
                </td>
            </tr>
            <tr>            
                <td>
                    <p>
                        <strong>
                            Email address: 
                        </strong>
                        <%= UserData.Email %></p>
                </td>                
            </tr>
            <% if (null != UserData.PrimaryLocation)
               { %>
            <tr>           
                <td>
                    <p>
                        <strong>Location</strong><br />                
                        <%= UserData.PrimaryLocation.Address%><br />
                        <%= UserData.PrimaryLocation.City%>, <%= UserData.PrimaryLocation.State%> <%= UserData.PrimaryLocation.Zip%>                
                    </p>
                </td>
            </tr>
            <% } %>
        </table>    
            
        <table width="70%" cellpadding="0" cellspacing="4" style="margin: 20px auto;">
            <tr>
                <td colspan="2">
                    <h3>Taskr Subscription Info</h3>                    
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Tasks Allowed: </strong>
                        <%= CurrentSubscription.GetLimit("Number of Tasks").Value %>
                    </p>     
                </td>                           
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Current Tasks: </strong>
                        <%= CurrentSubscription.GetLimitCount("Number of Tasks").Value %>                    
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Priority Management: </strong>
                        <%= true == CurrentSubscription.IsToggleEnabled("Priority Management").Value ? "Enabled" : "<font color='red'>Not Subscribed</font>" %>                    
                    </p>
                </td>
            </tr>
        </table>
        
        <table width="70%" cellpadding="0" cellspacing="4" style="margin: 20px auto;">
            <tr>
                <td colspan="2">
                    <h3>Taskr Permissions</h3>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Delete Tasks: </strong>
                        <%= CurrentUser.IsAuthorized("Delete Tasks") %>
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Change Task Priority: </strong>
                        <%= CurrentUser.IsAuthorized("Change Priority") %>
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <strong>Manage Tags: </strong>
                        <%= CurrentUser.IsAuthorized("Modify Tags") %>
                    </p>
                </td>                                
            </tr>
        </table>
   
</asp:content>

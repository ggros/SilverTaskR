<%@ Page Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/Templates/Default.Master"
    CodeBehind="Unauthorized.aspx.cs" 
    Inherits="Taskr.Web.Errors.Unauthorized" %>

<asp:Content ContentPlaceHolderID="BodyContent" runat="server">

    <p class="status-error" style="width:50%; margin: 0 auto;">
        You are not authorized to access this location.
    </p>

    <br /><br />

</asp:Content>
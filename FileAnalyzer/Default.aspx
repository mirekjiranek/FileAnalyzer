<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FileAnalyzer._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <h1>File Analyzer</h1>
        <div>
            <asp:Label ID="DirectoryLabel" runat="server" AssociatedControlID="DirectoryTextBox">Directory path:</asp:Label>
            <asp:TextBox ID="DirectoryTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DirectoryValidator" runat="server" ControlToValidate="DirectoryTextBox"
                ErrorMessage="Please, insert directory path" ValidationGroup="Validation"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Button ID="AnalyzeButton" runat="server" Text="Analyze" OnClick="AnalyzeButton_Click" ValidationGroup="Validation" />
        </div>
        <div>
            <asp:Label ID="ResultLabel" runat="server" EnableViewState="false"></asp:Label>
        </div>
    </asp:Panel>
</asp:Content>

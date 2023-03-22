<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FileAnalyzer._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <h1>Detekce změn v adresáři</h1>
        <div>
            <asp:Label ID="DirectoryLabel" runat="server" AssociatedControlID="DirectoryTextBox">Cesta k adresáři:</asp:Label>
            <asp:TextBox ID="DirectoryTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DirectoryValidator" runat="server" ControlToValidate="DirectoryTextBox"
                ErrorMessage="Zadejte prosím cestu k adresáři." ValidationGroup="Validation"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Button ID="AnalyzeButton" runat="server" Text="Analyzovat" OnClick="AnalyzeButton_Click" ValidationGroup="Validation" />
        </div>
        <div>
            <asp:Label ID="ResultLabel" runat="server" EnableViewState="false"></asp:Label>
        </div>
    </asp:Panel>
</asp:Content>

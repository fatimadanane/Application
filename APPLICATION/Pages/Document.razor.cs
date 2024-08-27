using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using APPLICATION.Models.ARCHIVE;
using System.Diagnostics;
namespace APPLICATION.Pages
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public partial class Document
    {
        [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected SecurityService Security { get; set; }

    [Inject]
    protected DocumentService DocumentService { get; set; }

    List<TDocument> Documents;

    protected override async Task OnInitializedAsync()
    {
        Documents = await DocumentService.GetAllDocumentsAsync();
    }

    protected async Task DataGridCellClick(Radzen.DataGridCellMouseEventArgs<TDocument> args)
    {
    }

    protected async Task DataGridCellDoubleClick(Radzen.DataGridCellMouseEventArgs<TDocument> args)
    {
    }

    private async Task ConfirmAndDelete(TDocument document)
    {
        var result = await DialogService.Confirm("Confirmer la suppression ?", "Confirmation", new ConfirmOptions() { OkButtonText = "Confirmer", CancelButtonText = "Annuler" });
        
        if (result == true)
        {
            // Call the delete operation
            await DocumentService.DeleteDocumentAsync(document.id);
            NotificationService.Notify(NotificationSeverity.Success, "Success", "Un document a été supprimé avec succès.");
            await OnInitializedAsync();
        }
    }

    private async Task OpenAdd()
    {
        await DialogService.OpenAsync<AddDocument>("");
    }
private async void EditDocument(TDocument document)
{
    int documentId = document.id;

    DialogService.Open<EditDocument>("Edit Document", new Dictionary<string, object>() { { "DocumentId", documentId } });

    Documents = await DocumentService.GetAllDocumentsAsync();
}

  public async Task OpenDocument(TDocument DocumentData)
{
    await DialogService.OpenAsync<ViewDocument>("",
               new Dictionary<string, object>() { { "DocumentData", DocumentData } },
               new DialogOptions() { });
}

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
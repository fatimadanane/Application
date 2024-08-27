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
namespace APPLICATION.Pages
{
    public partial class Societe
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
        protected SocieteService SocieteService { get; set; }

         List<TSociete> Societes;

    protected override async Task OnInitializedAsync()
    {
       
        Societes = await SocieteService.GetSocietes();
    }


        protected async System.Threading.Tasks.Task DataGrid0CellClick(Radzen.DataGridCellMouseEventArgs<TSociete> args)
        {
        }

        protected async System.Threading.Tasks.Task DataGrid0CellDoubleClick(Radzen.DataGridCellMouseEventArgs<TSociete> args)
        {
        }
          private async Task ConfirmAndDelete(TSociete societe)
    {
        var result = await DialogService.Confirm("Confirmer la suppression ?", "Confirmation", new ConfirmOptions() { OkButtonText = "Confirmer", CancelButtonText = "Annuler" });
        
        if (result == true)
        {
            // Call the delete operation
            await SocieteService.DeleteSociete(societe.id);
             NotificationService.Notify(NotificationSeverity.Success, "Success", "Une societé a éte supprimé avec succés.");
            await OnInitializedAsync();
        }
    }

private async Task OpenAdd(){
     await DialogService.OpenAsync<AddSociete>("");

}
    private async void EditSociete(TSociete societe)
{
    // Open the modal popup with the form for editing the selected row's information
    DialogService.Open<EditSocieteModal>("Edit Societe", new Dictionary<string, object>() { { "Societe", societe } });
      
        Societes = await SocieteService.GetSocietes();
}


 public async Task OpenSociete(TSociete societe)
{
    await DialogService.OpenAsync<ViewSociete>("",
               new Dictionary<string, object>() { { "Societe", societe } },
               new DialogOptions() { });
}




}
}
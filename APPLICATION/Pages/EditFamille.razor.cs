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
    public partial class EditFamille
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
        protected FamilleService FamilleService { get; set; }

        [Parameter]
        public TFamille  famille{ get; set; }
           private TFamille editedFamille = new TFamille();

               protected override void OnParametersSet()
    {
        
        editedFamille = famille;
    }
     private async Task SubmitForm()
    {
      
        await FamilleService.UpdateFamille(editedFamille);

     
        DialogService.Close(); 
      
        NavigationManager.NavigateTo("/famille", true);
    }

    }
}
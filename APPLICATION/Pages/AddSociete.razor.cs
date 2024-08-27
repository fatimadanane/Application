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
    public partial class AddSociete
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
            private TSociete addedSociete;
          [Parameter]
    public TSociete Societe { get; set; }
    protected override void OnInitialized()
{
    addedSociete = new TSociete();
}


       private async Task SubmitForm()
    {
      
       if (addedSociete != null)
    {
        // Call the service method to add the new societe
        await SocieteService.AddSociete(addedSociete);
 NotificationService.Notify(NotificationSeverity.Success, "Success", "Une societé a éte ajouté avec succés.");
        // Close the dialog after saving
        DialogService.Close();

        // Navigate to the "/societe" page
        NavigationManager.NavigateTo("/societe", true);
    }
    else
    {
        // Handle error or display a message indicating that the societe object is not properly initialized
    }
    }

    }
}
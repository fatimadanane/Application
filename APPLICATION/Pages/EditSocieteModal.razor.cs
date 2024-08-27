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
    public partial class EditSocieteModal
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
        protected SocieteService SocieteService{ get; set; }
        
          private TSociete editedSociete;
          [Parameter]
    public TSociete Societe { get; set; }

    // This method will be called when the component is initialized
    protected override void OnInitialized()
    {
        // Initialize the editedSociete object
        editedSociete = new TSociete();
    }

    // This method will be called when the component is parameterized
    protected override void OnParametersSet()
    {
        // Update the editedSociete object when the parameter value changes
        editedSociete = Societe;
    }

    // This method will be called when the form is submitted
    private async Task SubmitForm()
    {
      
        await SocieteService.UpdateSociete(editedSociete);

     
        DialogService.Close(); 
      
        NavigationManager.NavigateTo("/societe", true);
    }



    
    }
}
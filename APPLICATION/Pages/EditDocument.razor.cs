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
    public partial class EditDocument
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
        
        [Inject]
        protected FamilleService familleService { get; set; }
        
        [Parameter]
        public int DocumentId { get; set; }
        
        private TDocument editedDocument = new TDocument();
        private List<TFamille> parentFamilies;
        private TFamille selectedParentFamily;

    protected override async Task OnInitializedAsync()
{
    parentFamilies = await familleService.GetGrandParentsAsync();
    
 
    selectedParentFamily = parentFamilies.FirstOrDefault(f => f.intitule == editedDocument.Famille_Un);
}
        
     protected override async Task OnParametersSetAsync()
{
    if (DocumentId != 0)
    {
        editedDocument = await DocumentService.GetDocumentByIdAsync(DocumentId);
       
        selectedParentFamily = parentFamilies.FirstOrDefault(f => f.intitule == editedDocument.Famille_Un);
    }
}
        private void OnFamilleUnChange(TFamille selectedFamille)
        {
            // Update the editedDocument.Famille_Un based on the selected parent family
            editedDocument.Famille_Un = selectedFamille.intitule;
        }
        
        private async Task SubmitForm()
        {     
            await DocumentService.UpdateDocumentAsync(editedDocument);     
            DialogService.Close();       
            NavigationManager.NavigateTo("/document", true);
        }
    }
}
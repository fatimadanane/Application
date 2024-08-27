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
    public partial class AddDocument
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
    private TDocument addedDocument;

    [Parameter]
    public TDocument Document { get; set; }

List<TFamille> subChildren = new List<TFamille>();
// Families 
 List<TFamille> parentFamilies;
List<TFamille> childFamilies = new List<TFamille>(); // Initialize childFamilies

protected override async Task OnInitializedAsync()
{
    parentFamilies = await familleService.GetGrandParentsAsync();
}
public async Task OnParentFamilySelected(object value)
{
    string selectedFamilyIntitule = (string)value;

    // Find the TFamille object with matching intitule
    TFamille selectedFamily = parentFamilies.FirstOrDefault(f => f.intitule == selectedFamilyIntitule);

    if (selectedFamily != null)
    {
        addedDocument.Famille_Un = selectedFamily.intitule;
        int selectedParentId = selectedFamily.id;
        childFamilies = await familleService.GetChildrenByParentIdAsync(selectedParentId);
        addedDocument.Famille_Deux = null; // Reset the selected child family in addedDocument
    }
}
public async Task OnChildFamilySelected(object value)
{
    string selectedFamilyIntitule = (string)value;

    // Find the TFamille object with matching intitule
    TFamille selectedFamily = childFamilies.FirstOrDefault(f => f.intitule == selectedFamilyIntitule);

    if (selectedFamily != null)
    {
        addedDocument.Famille_Deux = selectedFamily.intitule;
        int selectedParentId = selectedFamily.id;
        subChildren = await familleService.GetSubChildrenByParentIdAsync(selectedParentId);
        addedDocument.Famille_Trois = null; // Reset the selected child family in addedDocument
    }
   }

    protected override void OnInitialized()
    {
        addedDocument = new TDocument();
        addedDocument.date = DateTime.Today;
        addedDocument.Date_Alerte = DateTime.Today;
        addedDocument.Date_Exp = DateTime.Today;
    }

    private async Task SubmitForm()
    {
        if (addedDocument != null)
        {
           
            await DocumentService.AddDocumentAsync(addedDocument);
            NotificationService.Notify(NotificationSeverity.Success, "Success", "Un document a été ajouté avec succès.");

          
            DialogService.Close();

            
            NavigationManager.NavigateTo("/document", true);
        }
        else
        {
            
        }
    }
}

}
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
    public partial class Famille
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
   
    List<TFamille> grandParents;
    List<TFamille> children;
    List<TFamille> subChildren;
    bool showAddChildButton = false;
    TFamille selectedGrandParent;
    TFamille selectedChild;
    TFamille newGrandParent = new TFamille();
    bool showAddGrandParentDialog = false;
    public string newSubChildIntitule;
public string newGrandParentIntitule;
public string newChildIntitule;
    RadzenDialog addGrandParentDialog;
    protected override async Task OnInitializedAsync()
    {
        grandParents = await FamilleService.GetGrandParentsAsync();
    }
    
    async Task SelectGrandParent(TFamille row)
    {
     selectedGrandParent = row;
        children = await FamilleService.GetChildrenByParentIdAsync(row.id);
        selectedChild = null;
        subChildren = null;
        showAddChildButton = true;
       
    }
    
    async Task SelectChild(TFamille row)
    {
        selectedChild = row;
        subChildren = await FamilleService.GetSubChildrenByParentIdAsync(row.id);
    }
   async Task DeleteGrandParent(TFamille grandParent)
{
    // Delete the grandparent
    await FamilleService.DeleteGrandParentAsync(grandParent);

    // Delete children of the grandparent
    var childrenToDelete = grandParents.Where(child => child.parent == grandParent.id).ToList();
    foreach (var child in childrenToDelete)
    {
        await DeleteChildWithSubChildren(child);
    }

    NavigationManager.NavigateTo("/famille", true);
    await OnInitializedAsync();
}

async Task DeleteChild(TFamille child)
{
    // Delete the child
    await FamilleService.DeleteChildAsync(child);

    NavigationManager.NavigateTo("/famille", true);
    await OnInitializedAsync();
}

async Task DeleteSubChild(TFamille subChild)
{
    // Delete the subchild
    await FamilleService.DeleteSubChildAsync(subChild);

    NavigationManager.NavigateTo("/famille", true);
    await OnInitializedAsync();
}

async Task DeleteChildWithSubChildren(TFamille child)
{
    // Delete the child
    await FamilleService.DeleteChildAsync(child);

    // Delete subchildren of the child
    var subChildrenToDelete = grandParents.Where(subChild => subChild.parent == child.id).ToList();
    foreach (var subChild in subChildrenToDelete)
    {
        await DeleteSubChild(subChild);
    }
}

private async void EditFamille(TFamille famille)
{
    // Open the modal popup with the form for editing the selected row's information
    DialogService.Open<EditFamille>("Edit Famille", new Dictionary<string, object>() { { "famille", famille } });
      
}
 Task ShowAddGrandParentModal()
    {
        newGrandParent = new TFamille
        {
            niveau = 0,
            parent = null
        };

        showAddGrandParentDialog = true;

        return Task.CompletedTask;
    }

 

     Task CancelAddGrandParent()
    {
        showAddGrandParentDialog = false;

        return Task.CompletedTask;
    }
      bool showAddParentForm = false;

    void ToggleAddParentForm()
    {
        showAddParentForm = !showAddParentForm;
    }
        bool showAddChildForm = false;

    void ToggleAddChildForm()
    {
        showAddChildForm = !showAddChildForm;
    }
        bool showAddSubChildForm = false;

    void ToggleAddSubChildForm()
    {
        showAddSubChildForm = !showAddSubChildForm;
    }


    private async Task AddGrandParent()
{
    // Validate the input or perform any necessary checks

    // Create a new instance of TFamille for the new grandparent
    TFamille grandParent = new TFamille
    {
        intitule = newGrandParentIntitule,
        niveau = 0,
        parent = null
    };

    // Add the new grandparent to the database using your FamilleService or DbContext
    await FamilleService.AddFamille(grandParent);

    // Refresh the list of grandparent items or update the data source
    grandParents = await FamilleService.GetGrandParentsAsync();

    // Reset any form-related variables or clear input fields
    newGrandParentIntitule = string.Empty;

    // Optionally, display a success message or perform any other actions
}

private async Task AddChild()
{
    // Create a new child entity
    var child = new TFamille
    {
        intitule = newChildIntitule,
        niveau = 1,
        parent = selectedGrandParent.id // Set the parent ID
    };

    // Save the new child to the database
    
         await FamilleService.AddFamille(child);
      
     children = await FamilleService.GetChildrenByParentIdAsync(selectedGrandParent.id);

    
    newChildIntitule = string.Empty;

}


private async Task AddSubChild()
{
    
    var child = new TFamille
    {
        intitule = newSubChildIntitule,
        niveau = 2,
        parent = selectedChild.id 
    };  await FamilleService.AddFamille(child);
      
     subChildren = await FamilleService.GetSubChildrenByParentIdAsync(selectedChild.id);

    
    newSubChildIntitule = string.Empty;

}
}}
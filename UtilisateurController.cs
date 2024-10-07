<div class="form-group">
    <label>Utilisateurs</label>
    @Html.DropDownListFor(model => model.SelectedUtilisateurs, 
        new MultiSelectList(
            utilisateursWithService, 
            "Value", 
            "Text", 
            Model.GroupUtilisateurs.Select(gu => gu.UtilisateurId.ToString())), 
        new { @class = "form-control", multiple = "multiple" })
</div>

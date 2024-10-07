@model NotifApps.Models.Group

@{
    ViewData["Title"] = "Modifier le groupe";
    var utilisateursWithService = ViewBag.UtilisateursWithService as List<SelectListItem>;
}

<div class="container mt-5">
    <div class="card">
        <div class="card-header bg-primary text-white">
            <h2>@ViewData["Title"]</h2>
        </div>
        <div class="card-body">
            <form asp-action="GroupEdit">
                <div class="form-group">
                    <label asp-for="GroupNom" class="control-label"></label>
                    <input asp-for="GroupNom" class="form-control" placeholder="Nom du groupe" />
                    <span asp-validation-for="GroupNom" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label for="serviceInput" class="control-label">Service</label>
                    <input type="text" id="serviceInput" class="form-control" placeholder="Filtrer par service" />
                </div>
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
                <div class="form-group">
                    <input type="submit" value="Enregistrer" class="btn btn-primary" />
                </div>
            </form>
            <form asp-action="GroupDelete" method="post" class="mt-3">
                <input type="hidden" asp-for="GroupId" />
                <input type="submit" value="Supprimer" class="btn btn-danger" />
            </form>
        </div>
        <div class="card-footer">
            <a asp-action="GroupList" class="btn btn-secondary">Retour à la liste</a>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
    <script>
        // Script pour filtrer les utilisateurs en fonction du service
        document.getElementById('serviceInput').addEventListener('input', function () {
            var service = this.value.toLowerCase();
            var options = document.querySelectorAll('select[name="SelectedUtilisateurs"] option');
            options.forEach(function (option) {
                if (option.getAttribute('data-service').toLowerCase().includes(service)) {
                    option.style.display = '';
                } else {
                    option.style.display = 'none';
                }
            });
        });
    </script>
}

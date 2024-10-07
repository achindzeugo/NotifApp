@model IEnumerable<NotifApps.Models.Group>

<div class="container mt-5">
    <h2>Liste des groupes</h2>
    <table class="table">
        <thead>
            <tr>
                <th>Nom du groupe</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var group in Model)
            {
                <tr>
                    <td>@group.GroupNom</td>
                    <td>
                        <!-- Lien pour modifier le groupe -->
                        <a asp-action="GroupEdit" asp-route-id="@group.GroupId" class="btn btn-primary">Modifier</a>

                        <!-- Lien pour voir les détails du groupe (optionnel si vous avez l'action GroupDetails) -->
                        <a asp-action="GroupDetails" asp-route-id="@group.GroupId" class="btn btn-info">Détails</a>

                        <!-- Formulaire pour supprimer le groupe -->
                        <form asp-action="GroupDelete" asp-route-id="@group.GroupId" method="post" style="display:inline;">
                            <input type="submit" class="btn btn-danger" value="Supprimer" />
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>
    <a asp-action="GroupCreate" class="btn btn-success">Créer un nouveau groupe</a>
</div>

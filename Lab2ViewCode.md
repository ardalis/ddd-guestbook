## Lab 2 View Sample Code ##
```
<form asp-controller="Home" asp-action="Index" method="post" class="form-horizontal">
    <h4>Sign the Guestbook</h4>
    <hr />
    <div asp-validation-summary="All" class="text-danger"></div>
    <div class="form-group">
        <label asp-for="NewEntry.EmailAddress" class="col-md-2 control-label"></label>
        <div class="col-md-10">
            <input asp-for="NewEntry.EmailAddress" class="form-control" />
            <span asp-validation-for="NewEntry.EmailAddress" class="text-danger"></span>
        </div>
    </div>
    <div class="form-group">
        <label asp-for="NewEntry.Message" class="col-md-2 control-label"></label>
        <div class="col-md-10">
            <input asp-for="NewEntry.Message" class="form-control" />
            <span asp-validation-for="NewEntry.Message" class="text-danger"></span>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <button type="submit" class="btn btn-default">Save</button>
        </div>
    </div>
</form>
```
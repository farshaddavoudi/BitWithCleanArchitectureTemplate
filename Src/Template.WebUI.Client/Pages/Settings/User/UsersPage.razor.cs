using ATABit.Shared;
using Bit.Http.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using Template.Domain.Shared;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Enums;
using Template.WebUI.Client.Extensions;

namespace Template.WebUI.Client.Pages.Settings.User;

[Authorize(Claims.Settings_Permissions_View)]
public partial class UsersPage : IDisposable
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Filter;

    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }

    private TelerikGrid<UserDto> UserGridRef { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public string SearchTerm { get; set; }
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    // Injects
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }

    // Life Cycles
    protected override Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return base.OnInitializedAsync(cancellationToken);
    }

    // Methods

    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        return UserGridRef.SetState(UserGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        IsLoading = true;

        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                UserGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            //args.Data = await System.Net.Http.HttpClient.User().GetUsersHavingRole(SearchTerm, context, cancellationToken);

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    private void SearchSubscriber()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(t => string.IsNullOrEmpty(t) is false && t!.Length > 2 || string.IsNullOrEmpty(t))
            .Subscribe(async _ =>
            {
                try
                {
                    _searchCancellationTokenSource.Cancel();

                    _searchCancellationTokenSource.Dispose();

                    _searchCancellationTokenSource = new CancellationTokenSource();

                    await RebindGrid(true, true);
                }
                catch (Exception exp)
                {
                    ExceptionHandler.OnExceptionReceived(exp);
                }
                finally
                {
                    StateHasChanged();
                }
            });
    }

    public void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (SearchTerm != searchTerm)
            SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private ValueTask OpenSecurityToManageRole()
    {
        return JsRuntime.OpenInNewTabAsync("http://security.app.ataair.ir/pages/group/grouplist.aspx");
    }
}
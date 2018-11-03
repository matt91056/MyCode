using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
public class PagerViewComponent:ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
    {
       return Task.FromResult( (IViewComponentResult)View(result));
    }
}
using Microsoft.AspNetCore.Mvc;
using OneTWebApp.Client.Models;
using OneTWebApp.Services;

namespace OneTWebApp;

[ApiController]
[Route("api/[controller]")]
public class InstanceController(KubService kubService) : ControllerBase {
    private const string LabelSelectorDocs = "app.kubernetes.io/name=docs";
    private const string LabelSelectorMeet = "app.kubernetes.io/name=meet";

    [HttpGet]
    public async Task<IActionResult> Index() {
        kubService.CreateClient();
        var docsInstances = await kubService.ListRunningInstances(LabelSelectorDocs);

        var docs = docsInstances.Select(instance => new InstanceModelDTO {
            DeployState = DeployState.Running,
            AppType = AppType.Docs,
            Name = instance.Name,
            Url = kubService.GetURL(instance.Pods)
        });

        var meetInstances = await kubService.ListRunningInstances(LabelSelectorMeet);
        var meets = meetInstances.Select(instance => new InstanceModelDTO {
            DeployState = DeployState.Running,
            AppType = AppType.Meet,
            Name = instance.Name,
            Url = kubService.GetURL(instance.Pods)
        });

        var res = docs.Concat(meets);
        return Ok(res);
    }
}
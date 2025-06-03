namespace OneTWebApp.Client.Models;

public class InstanceModelDTO {
    public string Name { get; set; }

    public AppType AppType { get; set; }

    public DeployState DeployState { get; set; }

    public string Url { get; set; }
}
using k8s.Models;

namespace OneTWebApp.Models;

public class InstanceModel {
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public List<V1Pod> Pods { get; set; } = [];
}
using k8s;
using k8s.Models;

namespace OneTWebApp.Services;

public class KubService {
    private Kubernetes _client;

    public void CreateClient() {
        // Load from the default kubeconfig on the machine.
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();

        // Use the config object to create a client.
        _client = new Kubernetes(config);
    }

    public async Task ListNamespaces() {
        Console.WriteLine(_client.BaseUri);
        var namespaces = await _client.ListNodeAsync();
        foreach (var name in namespaces) {
            Console.WriteLine(name.Name());
        }
    }
}
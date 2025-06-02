using k8s;

namespace OneTWebApp.Services;

public class KubService {
    private Kubernetes _client;

    public void CreateClient() {
        // Load from the default kubeconfig on the machine.
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();

        // Use the config object to create a client.
        _client = new Kubernetes(config);
        
        Console.WriteLine("Creating client");
    }

    public void ListNamespaces() {
        var namespaces = _client.CoreV1.ListNamespace();
        foreach (var ns in namespaces.Items) {
            Console.WriteLine(ns.Metadata.Name);
            var list = _client.CoreV1.ListNamespacedPod(ns.Metadata.Name);
            foreach (var item in list.Items) {
                Console.WriteLine(item.Metadata.Name);
            }
        }
    }
}
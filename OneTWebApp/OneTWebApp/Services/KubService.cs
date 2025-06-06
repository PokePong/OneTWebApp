using k8s;
using k8s.Models;
using OneTWebApp.Models;

namespace OneTWebApp.Services;

public class KubService {
    private Kubernetes _client;

    public void CreateClient() {
        // Load from the default kubeconfig on the machine.
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();

        // Use the config object to create a client.
        _client = new Kubernetes(config);
    }

    public async Task<List<InstanceModel>> ListRunningInstances(string labelSelector) {
        var pods = await _client.CoreV1.ListPodForAllNamespacesAsync(labelSelector: labelSelector);

        // Filtrer les pods en phase Running
        var runningPods = pods.Items
            .Where(p => p.Status.Phase == "Running")
            .ToList();

        // Grouper par instance
        var groupedByInstance = runningPods
            .Where(p => p.Metadata.Labels.ContainsKey("app.kubernetes.io/instance"))
            .GroupBy(p => p.Metadata.Labels["app.kubernetes.io/instance"])
            .ToDictionary(g => g.Key, g => g.ToList());


        return groupedByInstance.Select(kvp => new InstanceModel { Name = kvp.Key, Pods = kvp.Value }).ToList();
    }

    public string GetURL(List<V1Pod> pods) {
        const string envKey = "LOGIN_REDIRECT_URL";
        foreach (var url in from pod in pods
                 where pod.Spec.Containers.Count > 0
                 select pod.Spec.Containers[0]
                 into container
                 select container.Env.FirstOrDefault<V1EnvVar>(var => var.Name == envKey)?.Value
                 into url
                 where !string.IsNullOrEmpty(url)
                 select url) {
            return url;
        }

        return string.Empty;
    }

    public async Task DeleteInstances(string labelSelector, string name) {
        // var pods = await _client.CoreV1.ListPodForAllNamespacesAsync(labelSelector: labelSelector);
        // var pods = pods.Items
        //     .Where(p => p.Metadata.Labels.ContainsKey("app.kubernetes.io/instance"))
        //     .Where(p => p.Metadata.Labels["app.kubernetes.io/instance"] == name)
        //     .ToList();
        //
        // foreach (var pod in pods) {
        //     _client.DeleteNamespacedPodAsync(pod.Metadata.Namespace(), pod.pa)
        // }
    }

    private void PrintInstance(List<InstanceModel> instances) {
        Console.WriteLine($"Nombre d'instances différentes en cours d'exécution : {instances.Count}");
        foreach (var instance in instances) {
            Console.WriteLine($"Instance: {instance.Name} ({instance.Pods.Count} pod(s))");
            foreach (var pod in instance.Pods) {
                Console.WriteLine($" - Pod: {pod.Metadata.Name} | IP: {pod.Status.PodIP} | Node: {pod.Spec.NodeName}");
            }

            foreach (var pod in instance.Pods) {
                var podIP = pod.Status.PodIP;
                var port = pod.Spec.Containers
                    .SelectMany(c => c.Ports ?? new List<V1ContainerPort>())
                    .FirstOrDefault()?.ContainerPort ?? 80;

                var url = $"http://{podIP}:{port}";

                Console.WriteLine($" - Pod: {pod.Metadata.Name} | URL: {url}");
            }
        }
    }
}
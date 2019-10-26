/// <summary>
/// A Singleton for accessing global systems.
/// Source: https://csharpindepth.com/Articles/Singleton
/// </summary>
public sealed class G {

    #region Fields
    private static readonly G instance = new G();
    public static G Instance { get => instance; }

    public PipelineManager Pipeline { get; }
    #endregion

    static G() {}

    private G() {
        Pipeline = new PipelineManager();
    }

    public void Update(float deltaTime) {
        Pipeline.Update(deltaTime);
    }
}
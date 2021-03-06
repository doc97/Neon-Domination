/// <summary>
/// A Singleton for accessing global systems.
/// Source: https://csharpindepth.com/Articles/Singleton
/// </summary>
public sealed class G {

    #region Fields
    private static readonly G instance = new G();
    public static G Instance { get => instance; }

    public PipelineManager Pipeline { get; }
    public RoundManager Round { get; }
    public NDSceneManager Scene { get; set; } // set in GObject.Awake()
    #endregion

    static G() {}

    private G() {
        Pipeline = new PipelineManager();
        Round = new RoundManager();
    }

    public void Update(float deltaTime) {
        Pipeline.Update(deltaTime);
    }
}
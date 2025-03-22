namespace CompileAndConquer.Content;

public enum BehaviorType
{
    SpawnAlliesWhenAlone,
    DodgeBullets
}
public class AIComponent
{
    public BehaviorType Behavior { get; set; }

    public AIComponent(BehaviorType behavior)
    {
        Behavior = behavior;
    }
}

public class ShieldsModule : Module {
    public float SpawnRateBuff = 5;
    protected override void UpdateEffects(bool active) {
        if (active) {
            BotSpawner.Instance.SpawnRateBuff = SpawnRateBuff;
        }
        else {
            BotSpawner.Instance.SpawnRateBuff = 0f;
        }
    }
}

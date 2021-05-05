public class ShieldsModule : Module {
    protected override void UpdateEffects(bool active) {
        if (active) {
            BotSpawner.Instance.SpawnRateBuff = 10f;
        }
        else {
            BotSpawner.Instance.SpawnRateBuff = 0f;
        }
    }
}

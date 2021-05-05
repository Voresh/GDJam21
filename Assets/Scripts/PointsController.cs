using System;

public class PointsController : JamBase<PointsController> {
    public int EnemyDeadReward = 5;
    private int _Points;
    public int Points {
        get => _Points;
        set {
            var lastPoints = _Points;
            _Points = value;
            if (lastPoints != _Points)
                onPointsUpdated.Invoke(_Points);
        }
    }

    public event Action<int> onPointsUpdated = _ => { };

    private void Start() {
        BotSpawner.Instance.onBotSpawned += OnBotSpawned;
    }

    private void OnBotSpawned(Bot bot) {
        bot.Health.onDeadStatusUpdated += OnBotDeadStatusUpdated;
    }

    private void OnBotDeadStatusUpdated(bool dead) {
        if (!dead)
            return;
        Points += EnemyDeadReward;
    }
}

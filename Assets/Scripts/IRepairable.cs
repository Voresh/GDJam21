using UnityEngine;

public interface IRepairable {
    Vector3 RepairablePosition { get; }
    bool Repaired { get; }
    int RepairPrice { get; }
    void Repair();
}

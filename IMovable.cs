using UnityEngine;

public interface IMovable {
    void SetDirection(Vector2 dir);
    public void MovePosition(Vector2 targetPos);
    bool ReachedDestination(Vector2 target);
    bool ReachedDestination(Vector2 target, float stopDistance);
}
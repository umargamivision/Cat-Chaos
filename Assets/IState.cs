public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate(); // For physics-based updates
    void Exit();
}
public interface ISpawner
{
    void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ);
    void UpdateSpeed(float scrollSpeed);
    void StopScroll();
    void EnableScroll();
}

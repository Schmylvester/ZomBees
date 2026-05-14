using UnityEngine;

public class ConflictResolutionManager : MonoBehaviour
{
    [SerializeField] float m_speedWeight;
    [SerializeField] float m_valueWeight;
    [SerializeField] float m_distanceWeight;

    public bool resolveEnemyConflict(Enemy _object, Enemy _subject)
    {
        var speedDiff = _object.stats.moveSpeed - _subject.stats.moveSpeed;
        var valueDiff = _object.stats.yield - _subject.stats.yield;
        // lower distance is better, so do the subtraction in reverse
        var distanceDiff = _subject.getDistanceToCurrentTargetCell() - _object.getDistanceToCurrentTargetCell();

        // normalize the values and multiply them by their weight
        float maxAbs = 0;
        maxAbs = Mathf.Max(maxAbs, speedDiff, valueDiff, distanceDiff);

        var speedRelativeValue = (speedDiff / maxAbs) * m_speedWeight;
        var valueRelativeValue = (valueDiff / maxAbs) * m_valueWeight;
        var distanceRelativeValue = (distanceDiff / maxAbs) * m_distanceWeight;

        var weight = speedRelativeValue + valueRelativeValue + distanceRelativeValue;
        
        if (weight > 0)
        {
            return true;
        }
        if (weight < 0)
        {
            return false;
        }

        return _object.gameObject.name.CompareTo(_subject.gameObject.name) > 0;
    }
}

using UnityEngine;

namespace IronSight.Room
{
    public sealed class RoomReadinessController : MonoBehaviour
    {
        [SerializeField] private bool simulatePreparedRoom;

        public bool HasPreparedRoom()
        {
            return simulatePreparedRoom;
        }

        public string GetCreateNewSetupMessage()
        {
            return "Launch Meta Space Setup. After completing it, return to IronSight and re-check.";
        }
    }
}

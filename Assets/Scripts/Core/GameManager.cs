using System.Collections;
using IronSight.Room;
using IronSight.UI;
using UnityEngine;

namespace IronSight.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        private UIScreenRouter _screenRouter;
        private RoomReadinessController _roomReadinessController;

        public MainSceneState CurrentState { get; private set; }

        private void Awake()
        {
            _screenRouter = GetComponent<UIScreenRouter>();
            if (_screenRouter == null)
            {
                _screenRouter = gameObject.AddComponent<UIScreenRouter>();
            }

            _roomReadinessController = GetComponent<RoomReadinessController>();
            if (_roomReadinessController == null)
            {
                _roomReadinessController = gameObject.AddComponent<RoomReadinessController>();
            }

            _screenRouter.Initialize(this);
        }

        private void Start()
        {
            EnterDashboard();
        }

        public void EnterDashboard()
        {
            CurrentState = MainSceneState.Dashboard;
            _screenRouter.ShowDashboard();
            _screenRouter.SetStatus("Room Status: Unknown");
        }

        public void OpenRoomPreparation()
        {
            CurrentState = MainSceneState.RoomPreparation;
            _screenRouter.ShowRoomPreparation();
            _screenRouter.SetStatus("Select an existing room setup or create a new one.");
        }

        public void UseExistingRoomSetup()
        {
            StopAllCoroutines();
            StartCoroutine(CheckExistingRoomSetupRoutine());
        }

        public void CreateNewRoomSetup()
        {
            CurrentState = MainSceneState.RoomNotReady;
            _screenRouter.ShowRoomPreparation();
            _screenRouter.SetStatus(_roomReadinessController.GetCreateNewSetupMessage());
        }

        public void QuitApplication()
        {
            Debug.Log("IronSight quit requested.");

#if UNITY_EDITOR
            return;
#endif

            Application.Quit();
        }

        private IEnumerator CheckExistingRoomSetupRoutine()
        {
            CurrentState = MainSceneState.CheckingRoom;
            _screenRouter.ShowRoomPreparation();
            _screenRouter.SetStatus("Checking existing room setup...");

            yield return new WaitForSecondsRealtime(0.75f);

            if (_roomReadinessController.HasPreparedRoom())
            {
                CurrentState = MainSceneState.RoomReady;
                _screenRouter.SetStatus("Prepared room found. Room is ready.");
                yield break;
            }

            CurrentState = MainSceneState.RoomNotReady;
            _screenRouter.SetStatus("No prepared room found. Create a new room setup.");
        }
    }
}

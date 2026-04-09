using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Popups;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Zenject;

namespace ArmyCommander.Modules.Level
{
    public class LevelManager : MonoBehaviour
    {
        private const float PlayerSpawnRadius = 2f;
        private const float FullCircleRadians = Mathf.PI * 2f;
        private const float NavMeshSampleDistance = 1f;

        private const float LootSpawnRadius = 5f;
        private const int InitialLootCount = 10;
        
        [SerializeField] private UnitConfig _playerUnitConfig;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _initialRallyPoint;
        [SerializeField] private int _startingUnitsCount = 3;
        
        [Header("UI")]
        [SerializeField] private GameResultPopup _resultPopup;
        
        private bool _isLevelFinished = false;

        [Inject] private UnitFactory _unitFactory;
        [Inject] private UnitManager _unitManager;
        [Inject] private MoneyTagSpawner _moneySpawner;

        private void Start()
        {
            _unitManager.RallyPoint = _initialRallyPoint.position;

            SpawnInitialPlayerSquad();
            SpawnStartingLoot();
        }
        
        public void FinishLevel(bool isWin)
        {
            if (_isLevelFinished) return;
            _isLevelFinished = true;

            Time.timeScale = 0f;

            if (isWin)
            {
                _resultPopup.Show("VICTORY!", "NEXT LEVEL", LoadNextLevel);
            }
            else
            {
                _resultPopup.Show("GAME OVER", "RETRY", RestartLevel);
            }
        }

        private void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void LoadNextLevel()
        {
            Time.timeScale = 1f;
            Debug.Log("Loading next level data...");
            RestartLevel(); 
        }

        private void SpawnInitialPlayerSquad()
        {
            for (int i = 0; i < _startingUnitsCount; i++)
            {
                float angle = i * FullCircleRadians / _startingUnitsCount;
        
                Vector3 newPos = _playerSpawnPoint.position + new Vector3(
                    Mathf.Cos(angle) * PlayerSpawnRadius, 
                    0, 
                    Mathf.Sin(angle) * PlayerSpawnRadius);

                if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, NavMeshSampleDistance, NavMesh.AllAreas))
                {
                    var unit = _unitFactory.Create(_playerUnitConfig, hit.position);
                    _unitManager.RegisterUnit(unit, FactionType.Player);
                }
            }
        }

        private void SpawnStartingLoot()
        {
            for (int i = 0; i < InitialLootCount; i++)
            {
                Vector2 randomPoint = Random.insideUnitCircle * LootSpawnRadius;
                Vector3 pos = _playerSpawnPoint.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                _moneySpawner.Spawn(pos, CurrencyType.Silver);
            }
        }
    }
}
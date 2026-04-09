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
            float spawnRadius = 2f;

            for (int i = 0; i < _startingUnitsCount; i++)
            {
                float angle = i * Mathf.PI * 2f / _startingUnitsCount;
                Vector3 newPos = _playerSpawnPoint.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);

                if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    var unit = _unitFactory.Create(_playerUnitConfig, hit.position);
                    _unitManager.RegisterUnit(unit, FactionType.Player);
                }
            }
        }

        private void SpawnStartingLoot()
        {
            float lootRadius = 5f;

            for (int i = 0; i < 10; i++)
            {
                Vector2 randomPoint = Random.insideUnitCircle * lootRadius;
                Vector3 pos = _playerSpawnPoint.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                _moneySpawner.Spawn(pos, CurrencyType.Silver);
            }
        }
    }
}
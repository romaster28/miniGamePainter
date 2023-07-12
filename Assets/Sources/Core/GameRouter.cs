using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sources.Core.Map;
using Sources.Core.Roller;
using Sources.Core.UserControl;
using Sources.Sound;
using Sources.UserInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Core
{
    public class GameRouter : MonoBehaviour
    {
        [SerializeField] private Vector2Int _startRollerPosition;

        [SerializeField] private Field _field;

        [SerializeField] private Mover _mover;

        [SerializeField] private Rotator _rotator;

        [SerializeField] private CellDrawer _cellDrawer;

        [SerializeField] private DirectionChooser _directionChooser;

        [SerializeField] private GameScreen _gameScreen;

        [SerializeField] private WinGameScreen _winGameScreen;

        [SerializeField] private SoundPlayer _soundPlayer;

        private bool CheckCellFree(Cell cell)
        {
            return !cell.BlockedOnStart && !_cellDrawer.IsPainted(cell);
        }

        private void MoveToDirection(Direction direction)
        {
            _directionChooser.HideAll();

            _rotator.RotateToDirection(direction, delegate
            {
                _soundPlayer.StartPlayMove();
                
                Cell playerCell = _field.GetMostNearCell(_mover.CurrentPosition);

                IEnumerable<Cell> path =
                    _field.GetPathForDirection(_field.GetPositionOfCell(playerCell), direction, CheckCellFree);

                _mover.MoveOnPathAsync(path.Select(x => x.MiddlePoint), OnMoveCompleted, OnStepCompleted);
            });
        }

        private void OnStepCompleted(Vector2 position)
        {
            var cell = _field.GetMostNearCell(position);
            
            if (CheckCellFree(cell))
                _cellDrawer.DrawOnCell(cell);
        }
        
        private void OnMoveCompleted()
        {
            ShowSelectForCurrentPosition();
            
            _soundPlayer.StopPlayMove();
            
            if (_directionChooser.ShowedCount == 0)
                EndGameAsync();
        }

        private void ShowSelectForCurrentPosition()
        {
            _directionChooser.ShowFor(_mover.CurrentPosition,
                _field.GetNearCells(_mover.CurrentPosition).Where(CheckCellFree));
        }

        private async void EndGameAsync()
        {
            int freeCellsCount = _field.GetAllCells().Count(CheckCellFree);

            bool isWin = freeCellsCount == 0;
            
            Debug.Log(freeCellsCount);

            if (isWin)
            {
                OnWin();
            }
            else
            {
                _soundPlayer.PlayLose();
                
                await Task.Delay(3000);
                
                ResetGame();
            }
        }

        private void OnWin()
        {
            _winGameScreen.gameObject.SetActive(true);
        }
        
        private void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Start()
        {
            _winGameScreen.gameObject.SetActive(false);
            
            _field.Initialize();

            _directionChooser.Initialize();

            _mover.SetPosition(_field[_startRollerPosition].MiddlePoint);

            _directionChooser.HideAll();

            ShowSelectForCurrentPosition();
        }

        private void OnEnable()
        {
            _directionChooser.Chosen += MoveToDirection;
            _directionChooser.Chosen += delegate
            {
                _soundPlayer.PlayClick();
            };
            _gameScreen.OnResetClicked += ResetGame;
            _winGameScreen.OnRestartClicked += ResetGame;
            _gameScreen.OnSkipClicked += OnWin;
        }

        private void OnDisable()
        {
            _directionChooser.Chosen -= MoveToDirection;
            _gameScreen.OnResetClicked -= ResetGame;
            _winGameScreen.OnRestartClicked -= ResetGame;
            _gameScreen.OnSkipClicked -= OnWin;
        }
    }
}
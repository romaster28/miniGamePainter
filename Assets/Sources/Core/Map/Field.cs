using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Core.Map
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Cell[] _cells;

        [SerializeField] private Vector2Int _size;

        private readonly List<Cell> _nearCells = new List<Cell>(6);
        
        private readonly List<Cell> _pathCells = new List<Cell>(10);
        
        private Cell[,] _field;

        public Cell this[int x, int y] => _field[y, x];

        public Cell this[Vector2Int position] => this[position.x, position.y];

        public Cell GetMostNearCell(Vector2 position)
        {
            return _cells.OrderBy(x => Vector2.Distance(x.MiddlePoint, position)).First();
        }

        public IEnumerable<Cell> GetAllCells() => _cells;
        
        public IEnumerable<Cell> GetPathForDirection(Vector2Int start, Direction direction, Func<Cell, bool> checkNotBlocked)
        {
            _pathCells.Clear();

            Vector2Int current = start;

            _pathCells.Add(this[current]);
                
            MoveNext(ref current, direction);
            
            while (CheckPosition(current) && checkNotBlocked?.Invoke(this[current]) == true)
            {
                _pathCells.Add(this[current]);
                
                MoveNext(ref current, direction);
            }

            return _pathCells;
        }
        
        public IEnumerable<Cell> GetNearCells(Vector2 position)
        {
            Cell middle = GetMostNearCell(position);

            return GetNearCells(GetPositionOfCell(middle));
        }

        public IEnumerable<Cell> GetNearCells(Vector2Int position)
        {
            int addToDiagonal = position.y % 2 == 0 ? -1 : 1; 
            
            _nearCells.Clear();
            
            TryAddNearCell(position.x - 1, position.y);

            TryAddNearCell(position.x + addToDiagonal, position.y - 1);
            
            TryAddNearCell(position.x, position.y - 1);
            
            TryAddNearCell(position.x + 1, position.y);
            
            TryAddNearCell(position.x, position.y + 1);
            
            TryAddNearCell(position.x + addToDiagonal, position.y + 1);
            
            return _nearCells;
        }

        public Vector2Int GetPositionOfCell(Cell cell)
        {
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    if (this[x, y] == cell)
                        return new Vector2Int(x, y);
                }
            }
            
            throw new ArgumentException($"Cant find {cell.name} on field");
        }
        
        public void Initialize()
        {
            _field = new Cell[_size.y, _size.x];

            int cellsIndex = 0;
            
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    _field[y, x] = _cells[cellsIndex];

                    cellsIndex++;
                }
            }
        }

        private void TryAddNearCell(int x, int y)
        {
            TryAddNearCell(new Vector2Int(x, y));
        }
        
        private void TryAddNearCell(Vector2Int position)
        {
            if (CheckPosition(position))
                _nearCells.Add(this[position]);
        }

        private bool CheckPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < _size.x && position.y >= 0 && position.y < _size.y;
        }
        
        private static void MoveNext(ref Vector2Int current, Direction direction)
        {
            bool isEven = current.y % 2 == 0;
            
            int addToDiagonal = isEven ? -1 : 0;

            switch (direction)
            {
                case Direction.Left:
                    current.x -= 1;
                    break;
                case Direction.LeftUp:
                    current.x += addToDiagonal;
                    current.y -= 1;
                    break;
                case Direction.RightUp:
                    current.y -= 1;
                    if (!isEven)
                        current.x += 1;
                    break;
                case Direction.Right:
                    current.x += 1;
                    break;
                case Direction.RightDown:
                    current.y += 1;
                    if (!isEven)
                        current.x += 1;
                    break;
                case Direction.LeftDown:
                    current.y += 1;
                    current.x += addToDiagonal;
                    break;
            }
        }
    }
}

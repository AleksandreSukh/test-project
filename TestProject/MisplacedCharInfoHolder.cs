using System;
using System.Linq;

namespace TestProject
{
    public class MisplacedCharInfoHolder
    {
        private Action<int> _beforeAdding;
        private int[] _misplacedCharInfo;

        public MisplacedCharInfoHolder(Action<int> beforeAdding)
        {
            _beforeAdding = beforeAdding;
            _misplacedCharInfo = new int[] { };
        }

        public int[] MisplacedCharInfo => _misplacedCharInfo;

        public bool Contains(int i)
        {
            return _misplacedCharInfo.Contains(i);
        }

        public void Remove(int i)
        {
            var newMisplacedCharIndexes = _misplacedCharInfo.ToList();
            newMisplacedCharIndexes.Remove(i);
            _misplacedCharInfo = newMisplacedCharIndexes.ToArray();
        }

        public void AddIfNotAlreadyExists(int i)
        {
            if (_misplacedCharInfo.All(ind => ind != i))
            {
                var newMisplacedCharIndexes = _misplacedCharInfo.ToList();
                _beforeAdding?.Invoke(i);
                newMisplacedCharIndexes.Add(i);
                _misplacedCharInfo = newMisplacedCharIndexes.ToArray();
            }
        }

        public void RemoveIfContains(int i)
        {
            if (Contains(i))
                Remove(i);
        }
    }
}
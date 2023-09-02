using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ZombieWaves : Resource
{
    [Export]
    private Array<ZombieWave> waves;

    public Iterator<ZombieWave> Iterator()
    {
        return new IteratorImpl<ZombieWave>(waves);
    }

    public class IteratorImpl<[MustBeVariant] T> : Iterator<T>
    {
        private Array<T> _items;
        private int _position = 0;

        public IteratorImpl(Array<T> items)
        {
            _items = items;
        }

        public bool HasNext()
        {
            return _position < _items.Count;
        }

        public T Next()
        {
            T item = _items[_position];
            _position++;
            return item;
        }
    }
}

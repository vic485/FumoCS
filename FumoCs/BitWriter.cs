using System.Collections.Generic;

namespace FumoCs
{
    public class BitWriter
    {
        private int _value = 0;
        private int _bit = 7;
        private readonly List<byte> _bytes = new List<byte>();

        public bool Flushable => _value != 0 && _bit != 7;

        public void Write(int value)
        {
            _bit--;
            if (_bit < 0)
                Flush();

            if (value == 1)
                _value |= 1 << _bit;
        }

        public void Flush()
        {
            _bytes.Add((byte)_value);
            _value = 0;
            _bit = 7;
        }

        public byte[] GetBytes() => _bytes.ToArray();
    }
}

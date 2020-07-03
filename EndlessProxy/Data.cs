namespace EndlessProxy
{
    static class EONumber
    {
        const uint MAX1 = 253;
        const uint MAX2 = 64009;
        const uint MAX3 = 16194277;

        public static byte[] Encode(int number, int size)
        {
            return Encode((uint)number, size);
        }

        public static byte[] Encode(uint number, int size)
        {
            var bytes = new byte[size];
            var original = number;
            for (var i = 0; i < size; ++i)
            {
                bytes[i] = 254;
            }

            if (original >= MAX3)
            {
                bytes[3] = (byte)((number / MAX3) + 1);
                number = number % MAX3;
            }

            if (original >= MAX2)
            {
                bytes[2] = (byte)((number / MAX2) + 1);
                number = number % MAX2;
            }

            if (original >= MAX1)
            {
                bytes[1] = (byte)((number / MAX1) + 1);
                number = number % MAX1;
            }

            bytes[0] = (byte)(number + 1);

            return bytes;
        }
        
        public static uint Decode(byte[] buffer)
        {
            var data = new byte[4];
            for (var i = 0; i < 4; ++i)
            {
                data[i] = 1;

                if (buffer.Length > i)
                {
                    data[i] = buffer[i];
                }

                if (data[i] == 254)
                {
                    data[i] = 1;
                }

                if (data[i] == 0)
                {
                    data[i] = 128;
                }

                data[i] -= 1;
            }

            return (data[3] * MAX3) + (data[2] * MAX2) + (data[1] * MAX1) + data[0];
        }
    }
}

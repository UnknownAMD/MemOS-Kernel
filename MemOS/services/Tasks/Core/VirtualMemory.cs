using System;
using System.Collections.Generic;

namespace MemOS.Emulation
{
    public class Stack<T>
    {
        public List<T> Data { get; private set; }
        public Stack()
        {
            Data = new List<T>();
        }


        // Push one single element to the stack

        // <param name="element"></param>
        // <returns></returns>
        public void Push(T element)
        {
            Data.Add(element);
        }


        // Gather the last element in the stack

        // <returns></returns>
        public bool Try_Pop(ref T val)
        {
            if (Data.Count == 0) return false;
            val = Data[Data.Count - 1];
            List<T> dat = new List<T>();
            if (Data.Count - 1 != -1)
                for (int i = 0; i < Data.Count - 1; i++)
                    dat.Add(Data[i]);

            Data = dat;

            dat = null;

            return true;
        }


        // Clears entire stack

        public void Clear() { Data.Clear(); }
    }
    public class VirtualMemory
    {
        //public static readonly int Size; 
        public List<int> Data { get; private set; }
        public Stack<int> Stack { get; private set; }

        public VirtualMemory()
        {
            Data = new List<int>();
            Stack = new Stack<int>();
        }


        // Clears entire memory

        public void Clear() { Data.Clear(); }


        // Add single value to ram

        // <param name="data"></param>
        // <returns></returns>
        public bool AddInt(int data = 0)
        {
            Data.Add(data);
            return true;
        }


        // Add single 8-bit value to ram

        // <param name="data"></param>
        // <returns></returns>
        public bool AddChar(byte data = 0)
        {
            List<byte> toAdd = new List<byte>();

            toAdd.Add(data);
            toAdd.Add(0x00);
            toAdd.Add(0x00);
            toAdd.Add(0x00);

            Data.Add(BitConverter.ToInt32(toAdd.ToArray(), 0));
            return true;
        }


        // Add single 8-bit value to ram

        // <param name="data"></param>
        // <returns></returns>
        public bool AddChar(char data = '\0')
        {
            List<byte> toAdd = new List<byte>();

            toAdd.Add((byte)data);
            toAdd.Add(0x00);
            toAdd.Add(0x00);
            toAdd.Add(0x00);

            Data.Add(BitConverter.ToInt32(toAdd.ToArray(), 0));
            return true;
        }


        // Add array to memory

        // <param name="dataArray"></param>
        // <returns></returns>
        public bool AddArray(List<int> dataArray)
        {
            foreach (int b in dataArray)
            {
                Data.Add(b);
            }
            return true;
        }


        // Add specified amount of integers to memory

        // <param name="addr"></param>
        // <returns></returns>
        public bool AddAmount(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Data.Add(0x00);
            }
            return true;
        }


        // Read single 8-bit value from memory

        // <param name="addr"></param>
        // <returns></returns>
        public byte ReadChar(int addr)
        {
            List<byte> data = new List<byte>();
            foreach (byte c in BitConverter.GetBytes(Data[addr]))
            {
                data.Add(c);
            }
            return data[0];
        }
        
        public short ReadShort(int addr)
        {
            List<byte> data = new List<byte>();
            foreach (byte b in BitConverter.GetBytes(Data[addr]))
            {
                data.Add(b);
            }
            return (short)((data[0] << 8) | data[1]);
        }


        // Read single value from memory

        // <param name="addr"></param>
        // <returns></returns>
        public int ReadInt32(int addr)
        {
            List<byte> bdata = new List<byte>();
            foreach(byte b in BitConverter.GetBytes(Data[addr]))
            {
                bdata.Add(b);
            }
            bdata.Reverse();
            int data = BitConverter.ToInt32(bdata.ToArray(), 0);
            return data;
        }


        // Read a range of bytes from memory

        // <param name="addr"></param> <param name="len"></param>
        // <returns></returns>
        public List<int> ReadRange(int addr, int len)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < len; i++) { output.Add(Data[addr + i]); }
            return output;
        }


        // Insert a single value into a location in memory

        // <param name="addr"></param> <param name="data"></param>
        // <returns></returns>
        public bool InsertInt32(int addr, int data)
        {
            if (addr > Data.Count) { return false; }
            Data.Insert(addr, data);
            return true;
        }


        // Write a single value into a location in memory

        // <param name="addr"></param> <param name="data"></param>
        // <returns></returns>
        public bool WriteInt32(int addr, int data)
        {
            List<byte> dat = new List<byte>();
            foreach (byte b in BitConverter.GetBytes(data))
            {
                dat.Add(b);
            }
            dat.Reverse();
            data = BitConverter.ToInt32(dat.ToArray(), 0);
            Data[addr] = data;
            return true;
        }


        // Write a range of data into memory

        // <param name="addr"></param> <param name="length"></param> <param name="data"></param>
        // <returns></returns>
        public bool WriteRange(int addr, int length, List<int> data)
        {
            for (int i = 0; i < length; i++)
            {
                WriteInt32(addr + i, data[i]);
            }
            return true;
        }


        // Write character value to memory

        // <param name="addr"></param> <param name="character"></param>
        // <returns></returns>
        public bool WriteChar(int addr, char c)
        {
            Data[addr] = (byte)c;
            return true;
        }


        // Write string(character array) to memory

        // <param name="addr"></param> <param name="text"></param>
        // <returns></returns>
        public bool WriteString(int addr, string txt)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                Data[addr + i] = txt[i];
            }
            return true;
        }

        // Clears The specified address in memory

        // <param name="addr"></param>
        // <returns></returns>
        public bool ClearInt32(int addr)
        {
            Data[addr] = 0;
            return true;
        }
    }
}

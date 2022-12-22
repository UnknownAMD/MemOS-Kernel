using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSE.Assembly
{
    public abstract class DataContainer
    {
        public byte[] prefix { get; internal set; }
        public byte[] data { get; internal set; }
        public DataContainer(uint prefix, int data)
        {
            List<byte> temp = BitConverter.GetBytes(prefix).ToList();
            this.prefix = temp.ToArray();
            temp = BitConverter.GetBytes(data).ToList();
            this.data = temp.ToArray();
        }
        public virtual byte[] GetOpInBytes()
        {
            List<byte> temp = new List<byte>();
            temp.AddRange(prefix);
            temp.AddRange(data);
            return temp.ToArray();
        }
        public virtual int[] GetOpInInts()
        {
            int[] output = null;
            if (data.Length > 4)
                output = new int[] { BitConverter.ToInt32(prefix, 0), BitConverter.ToInt32(data, 0), BitConverter.ToInt32(data, 4) };
            else
                output = new int[] { BitConverter.ToInt32(prefix, 0), BitConverter.ToInt32(data, 0) };
            return output;
        }
    }
    public class Register: DataContainer
    {
        public Register(byte address): base(0xFE001AFA, BitConverter.ToInt32(new byte[] { 0, 0, 0, address }))
        {
        }
    }
    public class Data: DataContainer
    {
        public Data(int data): base(0xFE001AFB, data)
        {
        }
    }
    public class Pointer: DataContainer
    {
        DataContainer _data;
        public Pointer(DataContainer data): base(0xFE001AFC, 0x00)
        {
            this._data = data;
        }
        public override int[] GetOpInInts()
        {
            List<int> output = new List<int>();
            output.Add(BitConverter.ToInt32(prefix, 0));
            foreach(int cont in _data.GetOpInInts())
            {
                output.Add(cont);
            }
            return output.ToArray();
        }
    }
    public class SegmentPlaceHolder: DataContainer
    {
        public string name;
        public SegmentPlaceHolder(string name, int num): base(0xFF001AFA, num)
        {
            this.name = name;
        }
        public bool Is(string name)
        {
            return name == this.name;
        }
    }
    public enum Arch : byte
    {
        _8bit = 1,
        _16bit = 2,
        _32bit = 3
    }
    public class Assembler
    {
        public List<byte> assembled = new List<byte>();
        public List<int> VRam = new List<int>();
        public List<int> opCode = new List<int>();
        public byte Architecture;

        public Assembler(Arch arch)
        {
            Architecture = (byte)arch;
            for (int i = 0; i < 17; i++)
                VRam.Add(0x00);
        }

        public byte[] Compile(int extendMemory = 0)
        {
            for (int i = 0; i < extendMemory; i++)
                VRam.Add(0x00);
            assembled.Add(Architecture);
            assembled.Add(0x02);
            List<byte> temp;
            foreach (int data in VRam)
            {
                temp = BitConverter.GetBytes(data).ToList();
                temp.Reverse();
                assembled.AddRange(temp);
            }    
            assembled.Add(0xFF);
            assembled.Add(0xFA);
            assembled.Add(0xFF);
            assembled.Add(0xFA);
            assembled.Add(0xFB);
            foreach (int data in opCode)
            {
                temp = BitConverter.GetBytes(data).ToList();
                temp.Reverse();
                assembled.AddRange(temp);
            }

            return assembled.ToArray();
        }

        private protected void iterationProcess(DataContainer from, DataContainer to)
        {
            opCode.AddRange(from.GetOpInInts());
            opCode.AddRange(to.GetOpInInts());
        }
        public byte NonOP()
        {
            opCode.Add(0x00);
            return (byte)(opCode.ToArray().Length - 1);
        }
        public void Move(DataContainer from, DataContainer to)
        {
            opCode.Add(1);
            iterationProcess(from, to);
        }
        public void Set(DataContainer from, DataContainer to)
        {
            opCode.Add(2);
            iterationProcess(from, to);
        }
        public void Sum(DataContainer from, DataContainer to)
        {
            opCode.Add(3);
            iterationProcess(from, to);
        }
        public void Subtract(DataContainer from, DataContainer to)
        {
            opCode.Add(4);
            iterationProcess(from, to);
        }
        public void Divide(DataContainer from, DataContainer to)
        {
            opCode.Add(5);
            iterationProcess(from, to);
        }
        public void Multiply(DataContainer from, DataContainer to)
        {
            opCode.Add(6);
            iterationProcess(from, to);
        }
        public void JumpToSegment(DataContainer segment)
        {
            opCode.Add(7);
            opCode.AddRange(segment.GetOpInInts());
        }
        public void CallSegment(DataContainer segment)
        {
            opCode.Add(8);
            opCode.AddRange(segment.GetOpInInts());
        }
        public void Return()
        {
            opCode.Add(9);
        }
        public void SystemCall(DataContainer to)
        {
            opCode.Add(10);
            opCode.AddRange(to.GetOpInInts());
        }

        public void End()
        {
            opCode.Add(11);
        }

        public void Compare(DataContainer arg1, DataContainer arg2)
        {
            opCode.Add(12);
            opCode.AddRange(arg1.GetOpInInts());
            opCode.AddRange(arg2.GetOpInInts());
        }
        public void Equal(DataContainer arg1, DataContainer arg2)
        {
            opCode.Add(13);
            opCode.AddRange(arg1.GetOpInInts());
            opCode.AddRange(arg2.GetOpInInts());
        }
        public void JumpIf(DataContainer to)
        {
            opCode.Add(14);
            opCode.AddRange(to.GetOpInInts());
        }
        public void JumpIfNot(DataContainer to)
        {
            opCode.Add(15);
            opCode.AddRange(to.GetOpInInts());
        }
        public void CallIf(DataContainer to)
        {
            opCode.Add(16);
            opCode.AddRange(to.GetOpInInts());
        }
        public void CallIfNot(DataContainer to)
        {
            opCode.Add(17);
            opCode.AddRange(to.GetOpInInts());
        }
        public void SystemCallIf(DataContainer syscall)
        {
            opCode.Add(18);
            opCode.AddRange(syscall.GetOpInInts());
        }
        public void SystemCallIfNot(DataContainer syscall)
        {
            opCode.Add(19);
            opCode.AddRange(syscall.GetOpInInts());
        }
        public void ReturnIf()
        {
            opCode.Add(20);
        }
        public void ReturnIfNot()
        {
            opCode.Add(21);
        }
        public void EndIf()
        {
            opCode.Add(22);
        }
        public void EndIfNot()
        {
            opCode.Add(23);
        }
        public void Push(DataContainer data)
        {
            opCode.Add(24);
            opCode.AddRange(data.GetOpInInts());
        }
        public void Pop(DataContainer pos)
        {
            opCode.Add(25);
            opCode.AddRange(pos.GetOpInInts());
        }
        public int AddOnRAM(int data)
        {
            VRam.Add(data);
            return VRam.ToArray().Length - 1;
        }
        public int AddOnRAM(int[] datas)
        {
            int pointerRes = VRam.ToArray().Length;
            VRam.AddRange(datas);
            return pointerRes;
        }
        public int AddOnRAM(short data)
        {
            byte[] dat = BitConverter.GetBytes(data);
            return AddOnRAM(BitConverter.ToInt32(new byte[] { 0, 0, dat[0], dat[1] }));
        }
        public int AddOnRAM(short[] datas)
        {
            int addr = AddOnRAM(datas[0]);
            for (int i = 1; i < datas.Length; i++)
                AddOnRAM(datas[i]);
            return addr;
        }
        public int AddOnRAM(byte data)
        {
            return AddOnRAM(BitConverter.ToInt32(new byte[] { 0, 0, 0, data }));
        }
        public int AddOnRAM(byte[] datas)
        {
            int addr = AddOnRAM(datas[0]);
            for (int i = 1; i < datas.Length; i++)
                AddOnRAM(datas[i]);
            return addr;
        }
    }
}

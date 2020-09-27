using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ChipEight
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream("IBM Logo.ch8", FileMode.Open)))
            {
                CPU cpu = new CPU();
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var opcode = (ushort)(reader.ReadByte() << 8 | reader.ReadByte());
                    try
                    {
                        cpu.ExecuteOpcode(opcode);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                
            }
        }
    }
    public class CPU
    {
        public byte[] RAM = new byte[4096];
        public byte[] Registers = new byte[16];
        public ushort I = default(ushort);
        //public ushort[] Stack = new ushort[24];
        public Stack<ushort> Stack = new Stack<ushort>();
        public byte DelayTimer;
        public byte SoundTimer;
        public byte KeyBoard;
        public byte[] Display = new byte[64 * 32];

        public void ExecuteOpcode(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000);
            switch (nibble)
            {
                case 0x0000:
                    if (opcode == 0x00E0)
                    {
                        for (int i = 0; i < Display.Length; ++i)
                        {
                            Display[i] = 0;
                        }
                    }
                    else if (opcode == 0x00EE)
                    {
                        I = Stack.Pop();
                    }
                    else
                    {
                        throw new Exception($"Unsuported Opcode {opcode.ToString("x4")}");
                    }
                    break;
                case 0x1000:
                    I = (ushort)(opcode & 0x0FFF);
                    break;
                default:
                    throw new Exception($"Unsuported Opcode {opcode.ToString("x4")}");
            }


        }
    }
}

using System;

namespace ModbusLib.Protocols
{
    public class ModbusCodecReadCustom : ModbusCommandCodec
    {
        #region Client codec

        public override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            ModbusCodecBase.PushRequestHeader(
                command,
                body);
        }


        public override void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            var count = body.ReadByte() / 2;
            command.Data = new ushort[count];
            for (int i = 0; i < count; i++)
                command.Data[i] = body.ReadUInt16BE();
        }

        #endregion



        #region Server codec

        private byte _device;
        private int _address;
        private int _dayIndex;
        private int _category;

        public override void ServerEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteByte(_device);
            var count = command.Count;
            body.WriteByte((byte)(count));
            body.WriteByte(0xFF);
            body.WriteByte(0xFF);
            body.WriteInt32BE(_address);

            if (_category == 0)
            {
                for (int i = 0; i < count/2; i++)
                {
                    UInt16 v = (UInt16) GetRandomNumber(0, 25);
                    byte h = (byte)(v >> 8);
                    byte l = (byte)(v &0x00FF);
                    UInt16 t = (UInt16)((l << 8) + h);
                    body.WriteUInt16BE(t);
                }
            }
            else if (_category == 2)
            {
                for (int i = 0; i < count / 2; i++)
                {
                    body.WriteUInt16BE((UInt16)GetRandomNumber(0, 5));
                }

            }
            else 
            {
                for (int i = 0; i < count / 2; i++)
                {
                    body.WriteUInt16BE((UInt16)GetRandomNumber(0, 50));
                }

            }

        }

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        public override void ServerDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {

            _device = body.ReadByte();
            var data_len = body.ReadByte();
            body.ReadByte();
            body.ReadByte();
            _address = body.ReadInt32BE();
            _dayIndex = _address & 0x03FF;
            _category = (_address & 0xFC00) >> 10;
            command.Count = data_len;
            command.Data = new ushort[command.Count];
        }

        #endregion
    }
}
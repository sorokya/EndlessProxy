using System.Text;

namespace EndlessProxy
{
    public enum Action : byte
    {
        Request = 1,
        Accept = 2,
        Reply = 3,
        Remove = 4,
        Agree = 5,
        Create = 6,
        Add = 7,
        Player = 8,
        Take = 9,
        Use = 10,
        Buy = 11,
        Sell = 12,
        Open = 13,
        Close = 14,
        Message = 15,
        Spec = 16,
        Admin = 17,
        List = 18,
        Tell = 20,
        Report = 21,
        Announce = 22,
        Server = 23,
        Drop = 24,
        Junk = 25,
        Obtain = 26,
        Get = 27,
        Kick = 28,
        Rank = 29,
        TargetSelf = 30,
        TargetOther = 31,
        TargetGroup = 32,
        Exp = 33,
        Dialog = 34,
        Ping = 240,
        Pong = 241,
        Net3 = 242,
        Init = 255
    }

    public enum Family : byte
    {
        Connection = 1,
        Account = 2,
        Character = 3,
        Login = 4,
        Welcome = 5,
        Walk = 6,
        Face = 7,
        Chair = 8,
        Emote = 9,
        Attack = 11,
        Spell = 12,
        Shop = 13,
        Item = 14,
        StatSkill = 16,
        Global = 17,
        Talk = 18,
        Warp = 19,
        JukeBox = 21,
        Players = 22,
        Avatar = 23,
        Party = 24,
        Refresh = 25,
        NPC = 26,
        AutoRefresh = 27,
        AutoRefresh2 = 28,
        Appear = 29,
        PaperDoll = 30,
        Effect = 31,
        Trade = 32,
        Chest = 33,
        Door = 34,
        Message = 35,
        Bank = 36,
        Locker = 37,
        Barber = 38,
        Guild = 39,
        Music = 40,
        Sit = 41,
        Recover = 42,
        Board = 43,
        Cast = 44,
        Arena = 45,
        Priest = 46,
        Marriage = 47,
        AdminInteract = 48,
        Citizen = 49,
        Quest = 50,
        Book = 51,
        Init = 255
    }

    public class Packet
    {
        public Action Action;
        public Family Family;
        public byte[] Data;

        public Packet(byte[] data)
        {
            Action = (Action)data[0];
            Family = (Family)data[1];
            this.Data = data;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Family: {0}\tAction: {1}\n[", Family.ToString(), Action.ToString());
            foreach (byte b in Data)
            {
                stringBuilder.AppendFormat("{0},", b);
            }
            stringBuilder.Length -= 1;
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
}

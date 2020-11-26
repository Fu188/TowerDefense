using UnityEngine;

namespace ooad
{

    public class RoomCard
    {
        /**
         * [sustech:ip:port] or [cn:appId] or [asia:appid]
         */
        [SerializeField]
        public string regionAndKey;
        [SerializeField]
        public string roomName;
        [SerializeField]
        public byte currentPlayerNum;
        [SerializeField]
        public byte maxPlayerNum;

        public RoomCard()
        {

        }

        public RoomCard(string regionAndKey, string roomName, byte maxPlayerNum)
        {
            this.regionAndKey = regionAndKey;
            this.roomName = roomName;
            this.maxPlayerNum = maxPlayerNum;
        }

        public RoomCard(string regionAndKey, string roomName, byte currentPlayerNum, byte maxPlayerNum)
        {
            this.regionAndKey = regionAndKey;
            this.roomName = roomName;
            this.currentPlayerNum = currentPlayerNum;
            this.maxPlayerNum = maxPlayerNum;
        }

        public string GetregionAndKey()
        {
            return regionAndKey;
        }

        public RoomCard SetregionAndKey(string regionAndKey)
        {
            this.regionAndKey = regionAndKey;
            return this;
        }

        public string GetRoomNhame()
        {
            return roomName;
        }

        public RoomCard SetRoomName(string roomName)
        {
            this.roomName = roomName;
            return this;
        }

        public byte GetCurrentPlayerNum()
        {
            return currentPlayerNum;
        }

        public RoomCard SetCurrentPlayerNum(byte currentPlayerNum)
        {
            this.currentPlayerNum = currentPlayerNum;
            return this;
        }

        public byte GetMaxPlayerNum()
        {
            return maxPlayerNum;
        }

        public RoomCard SetMaxPlayerNum(byte maxPlayerNum)
        {
            this.maxPlayerNum = maxPlayerNum;
            return this;
        }


        public string Tostring()
        {
            return "RoomCard{" +
                    ", regionAndKey='" + regionAndKey + '\'' +
                    ", roomName='" + roomName + '\'' +
                    ", currentPlayerNum=" + currentPlayerNum +
                    ", maxPlayerNum=" + maxPlayerNum +
                    '}';
        }



        /*
        [SerializeField]
        public string regionAndKey { get; }
        [SerializeField]
        public string RoomName { get; set; }
        [SerializeField]
        public byte CurrentPlayerNum { get; set; }
        [SerializeField]
        public byte MaxPlayerNum { get; set; }

        public RoomCard2()
        {

        }
        public RoomCard2(string regionAndKey, string roomName, byte maxPlayerNum)
        {
            regionAndKey = regionAndKey;
            RoomName = roomName;
            MaxPlayerNum = maxPlayerNum;
        }


        public string Tostring()
        {
            return "RoomCard{" +
                    ", regionAndKey='" + regionAndKey + '\'' +
                    ", RoomName='" + RoomName + '\'' +
                    ", CurrentPlayerNum=" + CurrentPlayerNum +
                    ", MaxPlayerNum=" + MaxPlayerNum +
                    '}';
        }*/
    }
}
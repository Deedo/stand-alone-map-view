/*

Copyright 2014 Daniel Kinsman.

This file is part of Stand Alone Map View.

Stand Alone Map View is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Stand Alone Map View is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Stand Alone Map View.  If not, see <http://www.gnu.org/licenses/>.

*/

using ProtoBuf;
using System;
using System.IO;

namespace StandAloneMapView.comms
{
    [ProtoContract]
    public class Packet
    {
        [ProtoMember(1)]
        public Time Time { get; set; }

        [ProtoMember(2)]
        public Vessel Vessel { get; set; }

        [ProtoMember(3)]
        public ManeuverList ManeuverList { get; set; }

        [ProtoMember(4)]
        public Target Target { get; set; }

        public byte[] Make()
        {
            return Make<Packet>(this);
        }

        public static Packet Read(byte[] buffer)
        {
            return Read<Packet>(buffer);
        }

        public static byte[] Make<T>(T obj)
        {
            using(var stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, obj);
                return stream.ToArray();
            }
        }

        public static T Read<T>(byte[] buffer)
        {
            using(var stream = new MemoryStream(buffer))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
    }

    [ProtoContract]
    public class ClientPacket
    {
        [ProtoMember(1)]
        public ManeuverList ManeuverList { get; set; }

        [ProtoMember(2)]
        public Target Target;

        public byte[] Make()
        {
            return Packet.Make<ClientPacket>(this);
        }

        public static ClientPacket Read(byte[] buffer)
        {
            return Packet.Read<ClientPacket>(buffer);
        }
    }
}
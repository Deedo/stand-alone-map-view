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

using KSP;
using ProtoBuf;
using System;
using UnityEngine;


namespace StandAloneMapView.comms
{
    [ProtoContract]
    public class Vessel
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public Orbit Orbit { get; set; } 

        [ProtoMember(4)]
        public float Height;

        [ProtoMember(5)]
        public bool SAS { get; set; }

        [ProtoMember(6)]
        public bool RCS { get; set; }

        [ProtoMember(7)]
        public bool Light { get; set; }

        [ProtoMember(8)]
        public bool Brakes { get; set; }

        [ProtoMember(9)]
        public bool Gear { get; set; }

        [ProtoMember(10)]
        public bool Abort { get; set; }

        [ProtoMember(11)]
        public float Throttle { get; set; }

        [ProtoMember(12)]
        public float pitch { get; set; }

        [ProtoMember(13)]
        public float yaw { get; set; }

        [ProtoMember(14)]
        public float roll { get; set; }

        public Vessel()
        {
        }

        public Vessel(global::Vessel kspVessel)
        {
            this.Id = kspVessel.id;
            this.Name = kspVessel.vesselName;
            this.Orbit = new Orbit(kspVessel.orbit);
            this.Height = Math.Min(kspVessel.GetHeightFromTerrain(), (float)kspVessel.altitude);
            this.SAS = (bool)kspVessel.ActionGroups[KSPActionGroup.SAS];
            this.RCS = (bool)kspVessel.ActionGroups[KSPActionGroup.RCS];
            this.Light = (bool)kspVessel.ActionGroups[KSPActionGroup.Light];
            this.Brakes = (bool)kspVessel.ActionGroups[KSPActionGroup.Brakes];
            this.Gear = (bool)kspVessel.ActionGroups[KSPActionGroup.Gear];
            this.Abort = (bool)kspVessel.ActionGroups[KSPActionGroup.Abort];
            this.Throttle = (float)kspVessel.ctrlState.mainThrottle;
            this.pitch = (float)kspVessel.ctrlState.pitch;
            this.yaw = (float)kspVessel.ctrlState.;
            this.roll = (float)kspVessel.ctrlState.roll;

        }
    }
}
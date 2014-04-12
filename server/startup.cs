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


#if DEBUG

using KSP;
using UnityEngine;

namespace StandAloneMapView
{
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class Startup : utils.MonoBehaviourExtended
	{
		public Startup()
		{
			this.LogPrefix = "samv server";
		}

		public override void Start()
		{
			HighLogic.SaveFolder = "default";
			var game = GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, true, false);
			game.startScene = GameScenes.SPACECENTER;
			game.Start();
		}
	}
}

#endif
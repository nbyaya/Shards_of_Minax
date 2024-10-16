using System;
using Server.Mobiles;

namespace Server
{
    public class SpawnClockmakerQuest
    {
        public static void Initialize()
        {
            // Spawn Clockmaker Felix at coordinates X=1000, Y=1000, Z=0 in Map=0
            ClockmakerFelix felix = new ClockmakerFelix();
            felix.MoveToWorld(new Point3D(1000, 1000, 0), Map.Felucca);

            // Spawn Eccentric Gear Maker Gizmo at coordinates X=1010, Y=1000, Z=0 in Map=0
            EccentricGearMakerGizmo gizmo = new EccentricGearMakerGizmo();
            gizmo.MoveToWorld(new Point3D(1010, 1000, 0), Map.Felucca);
        }
    }
}

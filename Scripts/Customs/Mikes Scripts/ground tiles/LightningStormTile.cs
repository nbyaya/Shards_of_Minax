using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class LightningStormTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int LightningDamage { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public LightningStormTile() : base(0x1AE4) // Use an appropriate storm cloud-like tile ID
        {
            Movable = false;
            Name = "a lightning storm";
            Hue = 1001; // Dark stormy color

            LightningDamage = 10; // Damage per lightning strike
            AutoDelete = TimeSpan.FromSeconds(30); // Duration of the storm

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), StrikeLightning);
            StartDeleteTimer();
        }

		private void StrikeLightning()
		{
			if (Deleted) return;

			List<Point3D> affectedTiles = new List<Point3D>
			{
				Location,
				new Point3D(X - 1, Y - 1, Z),
				new Point3D(X, Y - 1, Z),
				new Point3D(X + 1, Y - 1, Z),
				new Point3D(X - 1, Y, Z),
				new Point3D(X + 1, Y, Z),
				new Point3D(X - 1, Y + 1, Z),
				new Point3D(X, Y + 1, Z),
				new Point3D(X + 1, Y + 1, Z)
			};

			foreach (Point3D tile in affectedTiles)
			{
				IPooledEnumerable<Mobile> eable = Map.GetMobilesInRange(tile, 0); // Get mobiles exactly at this tile
				foreach (Mobile m in eable)
				{
					if (m.Alive)
					{
						m.SendLocalizedMessage(500935); // You are struck by lightning!
						m.Damage(LightningDamage, null);
						Effects.SendBoltEffect(m, true, 0);
					}
				}
				eable.Free();

				Effects.SendLocationParticles(EffectItem.Create(tile, Map, EffectItem.DefaultDuration), 
					0x3818, 1, 30, 9501, 3, 9501, 0);
			}

			Effects.PlaySound(Location, Map, 0x29);
		}



        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(AutoDelete, DeleteTimer);
        }

        private void DeleteTimer()
        {
            this.Delete();
        }

        public LightningStormTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(LightningDamage);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            LightningDamage = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), StrikeLightning);
            StartDeleteTimer();
        }
    }
}
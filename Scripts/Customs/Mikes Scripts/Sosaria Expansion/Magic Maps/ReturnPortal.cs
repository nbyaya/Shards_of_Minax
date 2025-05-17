using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.MiniChamps;

namespace Server.Items
{
    /// <summary>Portal at the encounter site – lets players go home.</summary>
    public class ReturnPortal : Item
    {
        //   • If the server is rebooted while this portal exists
        //     it will immediately (during world load) gather up
        //     any players in a 200-tile radius, send them to the
        //     “safe” spot, delete every *non-player* mobile in the
        //     same radius, and finally delete itself.
        //
        //   • While the world is running normally it behaves exactly
        //     like the old MagicPortal so that players can click it
        //     to leave early.

        public Point3D Destination    { get; set; }
        public Map     DestinationMap { get; set; }

        private int _hue;
        private int _sound;

        private static readonly Point3D _fallbackLoc = new Point3D(1325, 1624, 55);
        private static readonly Map     _fallbackMap = Map.Trammel;

        [Constructable]
        public ReturnPortal(int hue, int sound)
            : base(0x0DDA)                         // same art as gate
        {
            _hue   = hue;
            _sound = sound;

            Hue     = _hue;
            Name    = "Flickering Return Portal";
            Movable = false;
            Light   = LightType.Circle300;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this, 3))
            {
                from.SendLocalizedMessage(500446); // Too far away.
                return;
            }

            MoveOut(from); // pets first, player last
        }

        private void MoveOut(Mobile m)
        {
            // collect pets
            var pets = new System.Collections.Generic.List<BaseCreature>();
            var mount = m.Mount as BaseCreature;

            foreach (Mobile mob in m.Map.GetMobilesInRange(Location, 200))
            {
                if (mob is BaseCreature bc && bc != mount &&
                    ((bc.Controlled && bc.ControlMaster == m) ||
                     (bc.Summoned  && bc.SummonMaster  == m)))
                {
                    pets.Add(bc);
                }
            }

            // VFX / SFX
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                           0x3728, 10, 10, 2023);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                foreach (var pet in pets)
                {
                    if (pet is IMount pm && pm.Rider != null && pm.Rider != pet.ControlMaster)
                        pm.Rider = null;

                    pet.MoveToWorld(Destination, DestinationMap);
                }

                m.MoveToWorld(Destination, DestinationMap);
                Effects.SendLocationParticles(EffectItem.Create(Destination, DestinationMap, EffectItem.DefaultDuration),
                                               0x3728, 10, 10, 5023);
                Effects.PlaySound(Destination, DestinationMap, _sound);
            });
        }

        // ──────── (de)serialization ────────────────────────────────────────

        public ReturnPortal(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);              // version
            writer.Write(_hue);
            writer.Write(_sound);
            writer.Write(Destination);
            writer.Write(DestinationMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _hue        = reader.ReadInt();
            _sound      = reader.ReadInt();
            Destination = reader.ReadPoint3D();
            DestinationMap = reader.ReadMap();

            /*  ── THE IMPORTANT BIT ─────────────────────────────────────────
             *  When the world is loading, *all* items go through Deserialize
             *  before players are even able to log in.  So we do our cleanup
             *  right now.  That replaces the old “expiration timer” which
             *  obviously cannot survive a reboot.
             */
            Timer.DelayCall(TimeSpan.Zero, DoEmergencyCleanup);
        }

		private void DoEmergencyCleanup()
		{
			try
			{
				if (Deleted) return;

				// 1) Move players out, delete monsters
				foreach (Mobile mob in Map.GetMobilesInRange(Location, 200))
				{
					if (mob.Deleted) continue;

					if (mob.Player)
					{
						mob.MoveToWorld(_fallbackLoc, _fallbackMap);
					}
					else
					{
						mob.Delete();
					}
				}

				// 2) Delete leftover items
				foreach (Item item in Map.GetItemsInRange(Location, 200))
				{
					if (item.Deleted) continue;

					// Delete any MiniChamp controllers
					if (item is MiniChamp)
					{
						item.Delete();
					}
					// Delete any treasure map chests
					else if (item is TreasureMapChest)
					{
						item.Delete();
					}
					// Delete corpses (both player and creature corpses)
					else if (item is Corpse)
					{
						item.Delete();
					}
				}
			}
			finally
			{
				Delete(); // remove the portal itself
			}
		}

    }

    /// <summary>
    /// Portal left behind at the player’s original location.
    /// It has no work to do on a reboot – just disappear.
    /// </summary>
	public class OriginPortal : Item
	{
		public Point3D Destination { get; set; }
		public Map DestinationMap { get; set; }

		private int _hue;
		private int _sound;

		[Constructable]
		public OriginPortal(int hue, int sound) : base(0x0DDA)
		{
			_hue = hue;
			_sound = sound;

			Name = "Origin Portal";
			Hue = _hue;
			Movable = false;
			Light = LightType.Circle300;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this, 3))
			{
				from.SendLocalizedMessage(500446); // Too far.
				return;
			}

			Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
				0x3728, 10, 10, 2023);

			Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
			{
				from.MoveToWorld(Destination, DestinationMap);
				Effects.SendLocationParticles(EffectItem.Create(Destination, DestinationMap, EffectItem.DefaultDuration),
					0x3728, 10, 10, 5023);
				Effects.PlaySound(Destination, DestinationMap, _sound);
			});
		}

		public OriginPortal(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version
			writer.Write(_hue);
			writer.Write(_sound);
			writer.Write(Destination);
			writer.Write(DestinationMap);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			_hue = reader.ReadInt();
			_sound = reader.ReadInt();
			Destination = reader.ReadPoint3D();
			DestinationMap = reader.ReadMap();
			Delete();             // simply vanish after reboot

		}
	}

}

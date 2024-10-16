using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class TacticalRetreat : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tactical Retreat", "In Sanctum Retreat",
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public TacticalRetreat(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                // Play visual and sound effects
                Effects.SendLocationEffect(caster.Location, caster.Map, 0x3728, 10, 1);
                caster.PlaySound(0x1FE);

                // Create a decoy using the body ID of the caster
                Decoy decoy = new Decoy(caster.Body);
                decoy.MoveToWorld(caster.Location, caster.Map);

                // Teleport the caster a short distance away
                Point3D retreatLocation = GetRetreatLocation(caster);
                caster.MoveToWorld(retreatLocation, caster.Map);

                // Play another visual effect at the new location
                Effects.SendLocationEffect(retreatLocation, caster.Map, 0x3728, 10, 1);
                caster.PlaySound(0x1FE);
            }

            FinishSequence();
        }

        private Point3D GetRetreatLocation(Mobile caster)
        {
            // Calculate a location a few tiles away from the caster
            Point3D location = caster.Location;
            Map map = caster.Map;

            for (int i = 0; i < 10; i++)
            {
                int x = location.X + Utility.RandomMinMax(-3, 3);
                int y = location.Y + Utility.RandomMinMax(-3, 3);
                int z = map.GetAverageZ(x, y);

                if (map.CanFit(x, y, z, 16, false, false))
                    return new Point3D(x, y, z);
            }

            return location; // Fallback to the original location if no suitable location is found
        }

		private class Decoy : BaseCreature
		{
			private Timer m_Timer;

			// Parameterless constructor for serialization
			public Decoy() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
			{
				// Default properties
				Body = 0; // Default body ID, will be set later
				Blessed = true;
			}

			public Decoy(int bodyID) : this()
			{
				Body = bodyID;

				// Initialize stats for the decoy
				SetStr(50); // Set appropriate strength
				SetDex(50); // Set appropriate dexterity
				SetInt(10); // Set appropriate intelligence

				Hits = HitsMax; // Now HitsMax will be based on the initialized stats

				// Basic appearance and properties
				Hue = 0; // Default or set appropriate hue if needed
				Name = "Decoy";
				SpeechHue = 0; // Default or set appropriate speech hue if needed
				HairItemID = 0; // Default or set appropriate hair item ID if needed
				FacialHairItemID = 0; // Default or set appropriate facial hair item ID if needed

				// Start the timer to delete the decoy after a short duration
				m_Timer = new DecoyTimer(this);
				m_Timer.Start();
			}

			public override void OnDelete()
			{
				base.OnDelete();
				if (m_Timer != null)
					m_Timer.Stop();
			}

			private class DecoyTimer : Timer
			{
				private BaseCreature m_Decoy;

				public DecoyTimer(BaseCreature decoy) : base(TimeSpan.FromSeconds(10.0))
				{
					m_Decoy = decoy;
				}

				protected override void OnTick()
				{
					m_Decoy.Delete();
				}
			}
		}


    }
}

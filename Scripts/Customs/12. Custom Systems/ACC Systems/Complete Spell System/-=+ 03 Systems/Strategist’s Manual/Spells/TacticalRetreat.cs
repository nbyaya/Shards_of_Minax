using System;
using Server;
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

        public override SpellCircle Circle => SpellCircle.Fifth;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 20;

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

            return location; // fallback if no good spot found
        }

        private class Decoy : BaseCreature
        {
            private Timer m_Timer;

            // Parameterless constructor
            public Decoy() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
            {
                Body = 0; // will be set later
                Blessed = true;
            }

            public Decoy(int bodyID) : this()
            {
                Body = bodyID;
                SetStr(50);
                SetDex(50);
                SetInt(10);
                Hits = HitsMax;

                Hue = 0;
                Name = "Decoy";
                SpeechHue = 0;
                HairItemID = 0;
                FacialHairItemID = 0;

                m_Timer = new DecoyTimer(this);
                m_Timer.Start();
            }

            // Required for serialization
            public Decoy(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();

                // Restart timer when loaded if needed
                m_Timer = new DecoyTimer(this);
                m_Timer.Start();
            }

            public override void OnDelete()
            {
                base.OnDelete();
                m_Timer?.Stop();
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
                    m_Decoy?.Delete();
                }
            }
        }
    }
}

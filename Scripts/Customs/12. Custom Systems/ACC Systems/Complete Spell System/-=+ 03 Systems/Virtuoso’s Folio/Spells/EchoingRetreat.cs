using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class EchoingRetreat : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Echoing Retreat", "Retreatus Musica",
            21004,
            9300,
            false,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 95; } }

        public EchoingRetreat(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Perform a quick retreat and summon a musical decoy
            if (CheckSequence())
            {
                Point3D targetLocation = new Point3D(Caster.X + Utility.RandomMinMax(-2, 2), Caster.Y + Utility.RandomMinMax(-2, 2), Caster.Z);
                Caster.Location = targetLocation;
                Caster.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Waist);
                Caster.PlaySound(0x1FE); // Retreat sound

                // Create and summon the decoy
                MusicalDecoy decoy = new MusicalDecoy(Caster.Location, Caster.Map, Caster);
                Effects.PlaySound(decoy.Location, decoy.Map, 0x14D); // Musical note sound

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), decoy.Delete); // Decoy lasts for 10 seconds
            }

            FinishSequence();
        }

        private class MusicalDecoy : Item
        {
            private Mobile m_Caster;
            private Timer m_Timer;

            public MusicalDecoy(Point3D loc, Map map, Mobile caster) : base(0x1B72)
            {
                Movable = false;
                Visible = true;
                Name = "Musical Decoy";
                Hue = 0x482; // Give it a musical hue
                m_Caster = caster;

                MoveToWorld(loc, map);

                // Create a timer to apply the distraction effect
                m_Timer = new DecoyEffectTimer(this, m_Caster);
                m_Timer.Start();
            }

            // ✨ Serialization constructor — REQUIRED
            public MusicalDecoy(Serial serial) : base(serial)
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
            }

            private class DecoyEffectTimer : Timer
            {
                private MusicalDecoy m_Decoy;
                private Mobile m_Caster;

                public DecoyEffectTimer(MusicalDecoy decoy, Mobile caster)
                    : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
                {
                    m_Decoy = decoy;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Decoy.Deleted)
                    {
                        Stop();
                        return;
                    }

                    // Attract enemies and absorb some damage
                    foreach (Mobile mob in m_Decoy.GetMobilesInRange(5))
                    {
                        if (mob != m_Caster && mob.Alive && mob.Combatant == m_Caster)
                        {
                            mob.Combatant = null; // Distract enemy
                            mob.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                            mob.PlaySound(0x1F2); // Confusion sound

                            // Absorb some damage by the decoy
                            mob.Damage(Utility.RandomMinMax(5, 10), m_Caster);
                        }
                    }
                }
            }
        }
    }
}

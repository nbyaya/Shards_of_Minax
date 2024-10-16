using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class DissonantShield : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dissonant Shield", "Discordia Protectus",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 90; } }

        public DissonantShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You summon a shield of discordant sound!");
                Effects.PlaySound(Caster.Location, Caster.Map, 0x208); // Sound effect for shield activation

                Point3D loc = Caster.Location;
                Map map = Caster.Map;

                // Create the shield effect
                InternalItem shield = new InternalItem(loc, map, Caster);
                shield.MoveToWorld(loc, map);

                // Visual effect for shield activation
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008, 0, 9502, 0);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x375A)
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, caster, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();
            }

            public InternalItem(Serial serial) : base(serial)
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

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;
                private Mobile m_Caster;

                public InternalTimer(InternalItem item, Mobile caster, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                    Delay = duration;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                        return;

                    ArrayList toDamage = new ArrayList();

                    foreach (Mobile m in m_Item.GetMobilesInRange(3)) // Radius of 3 tiles
                    {
                        if (m != m_Caster && m is PlayerMobile && m.Alive && m.CanBeHarmful(m_Caster))
                        {
                            toDamage.Add(m);
                        }
                    }

                    foreach (Mobile m in toDamage)
                    {
                        m_Caster.DoHarmful(m);
                        m.PlaySound(0x5C3); // Sound for damage effect
                        m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Visual damage effect
                        m.Damage(Utility.RandomMinMax(20, 40), m_Caster); // Inflict damage
                    }

                    if (DateTime.Now >= this.Next)
                    {
                        m_Item.Delete();
                        Stop();
                    }
                }
            }
        }
    }
}

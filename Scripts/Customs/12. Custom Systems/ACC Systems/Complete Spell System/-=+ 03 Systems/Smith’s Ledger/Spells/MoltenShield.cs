using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class MoltenShieldSpell : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Molten Shield", "In Flama Scutum",
            21012,
            9308
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public MoltenShieldSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MoltenShieldSpell m_Spell;

            public InternalTarget(MoltenShieldSpell spell) : base(12, false, TargetFlags.Beneficial)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile targ = (Mobile)target;

                    if (m_Spell.CheckSequence())
                    {
                        // Create the shield effect
                        MoltenShield shield = new MoltenShield(targ);
                        shield.MoveToWorld(targ.Location, targ.Map);

                        // Play visual and sound effects
                        Effects.PlaySound(targ.Location, targ.Map, 0x208); // Fire sound
                        targ.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);

                        // Send message to the caster and target
                        from.SendMessage("You create a molten shield around your target!");
                        targ.SendMessage("A molten shield surrounds you, absorbing damage!");

                        // Start a timer to handle the shield duration and absorption
                        new MoltenShieldTimer(targ, shield).Start();
                    }

                    m_Spell.FinishSequence();
                }
                else
                {
                    from.SendMessage("You can only cast this spell on yourself or another player.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }

        private class MoltenShield : Item
        {
            private Mobile m_Target;
            private int m_AbsorbedDamage;

            public MoltenShield(Mobile target) : base(0x1F2F) // Shield item graphic
            {
                m_Target = target;
                m_AbsorbedDamage = 0;
                Movable = false;
                Visible = true;
                Name = "Molten Shield";
                Hue = 1358; // Red hue for molten effect
            }

            public MoltenShield(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
                writer.Write(m_Target);
                writer.Write(m_AbsorbedDamage);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                m_Target = reader.ReadMobile();
                m_AbsorbedDamage = reader.ReadInt();
            }

            public void AbsorbDamage(int amount)
            {
                m_AbsorbedDamage += amount;

                if (m_AbsorbedDamage >= 50) // Max damage the shield can absorb
                {
                    // Shield breaks
                    Effects.PlaySound(m_Target.Location, m_Target.Map, 0x1FB); // Shield break sound
                    m_Target.SendMessage("The molten shield shatters!");
                    Delete();
                }
            }
        }

        private class MoltenShieldTimer : Timer
        {
            private Mobile m_Target;
            private MoltenShield m_Shield;
            private DateTime m_End;

            public MoltenShieldTimer(Mobile target, MoltenShield shield) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_Shield = shield;
                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // 30-second duration
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Shield == null || m_Shield.Deleted || DateTime.Now >= m_End)
                {
                    // Shield duration ends
                    m_Shield?.Delete();
                    Stop();
                }
                else
                {
                    // Absorb some random damage for demonstration (can be replaced with actual damage logic)
                    int randomDamage = Utility.RandomMinMax(1, 5);
                    m_Shield.AbsorbDamage(randomDamage);
                }
            }
        }
    }
}

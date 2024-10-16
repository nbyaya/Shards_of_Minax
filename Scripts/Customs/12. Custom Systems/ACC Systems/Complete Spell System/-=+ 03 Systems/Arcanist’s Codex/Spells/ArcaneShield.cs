using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ArcaneShield : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Shield", "In Arcanus",
            21005,  // Effect ID for the spell casting
            9404    // Sound ID for the spell
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ArcaneShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Create the shield effect
                CreateShield();
            }

            FinishSequence();
        }

        private void CreateShield()
        {
            // Define the shield duration and damage absorption percentage
            TimeSpan duration = TimeSpan.FromSeconds(30);
            double absorptionPercentage = 0.5; // 50% damage absorption

            // Apply the visual and sound effects
            Effects.PlaySound(Caster.Location, Caster.Map, 0x20E); // Shield activation sound
            Effects.SendTargetEffect(Caster, 0x3779, 10, 16, 0x66, 0); // Shield visual effect

            // Create the shield item
            ShieldItem shield = new ShieldItem(Caster, duration, absorptionPercentage);
            shield.MoveToWorld(Caster.Location, Caster.Map);
        }

        private class ShieldItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;
            private double m_AbsorptionPercentage;

            public ShieldItem(Mobile caster, TimeSpan duration, double absorptionPercentage) : base(0x1F3F) // Shield item ID
            {
                m_Caster = caster;
                m_AbsorptionPercentage = absorptionPercentage;
                Visible = false; // Make the shield invisible
                Movable = false;

                // Timer to remove the shield after the duration
                m_Timer = new InternalTimer(this, duration);
                m_Timer.Start();
            }

            public ShieldItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version

                writer.Write(m_Caster);
                writer.Write(m_AbsorptionPercentage);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();

                m_Caster = reader.ReadMobile();
                m_AbsorptionPercentage = reader.ReadDouble();

                // Recreate timer
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30)); // Assume duration is 30 seconds, adjust as needed
                m_Timer.Start();
            }

            private class InternalTimer : Timer
            {
                private ShieldItem m_Shield;

                public InternalTimer(ShieldItem shield, TimeSpan duration) : base(duration)
                {
                    m_Shield = shield;
                }

                protected override void OnTick()
                {
                    m_Shield.Delete();
                }
            }
        }
    }
}

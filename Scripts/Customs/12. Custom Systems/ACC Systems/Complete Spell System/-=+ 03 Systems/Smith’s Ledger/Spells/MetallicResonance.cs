using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class MetallicResonance : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Metallic Resonance", "Resona Metallum",
                                                        21019,
                                                        9315
                                                       );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 2.5; // 2.5 seconds delay before casting
        public override double RequiredSkill => 50.0; // Requires 50.0 skill level to cast
        public override int RequiredMana => 20; // 20 mana cost

        public MetallicResonance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MetallicResonance m_Owner;

            public InternalTarget(MetallicResonance owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence() && target is Mobile)
            {
                // Check if the target is wearing plate armor
                if (IsWearingPlateArmor(target))
                {
                    SpellHelper.Turn(Caster, target);

                    // Play flashy effects and sounds
                    Effects.PlaySound(target.Location, target.Map, 0x229); // Sound effect for resonating metal
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 1, 15, 1153, 0, 0, 0); // Visual effect for resonance

                    // Apply stun effect
                    TimeSpan stunDuration = TimeSpan.FromSeconds(3.0);
                    target.Freeze(stunDuration);

                    Caster.SendMessage("You emit a resonant frequency that stuns your plate-armored foe!");
                    target.SendMessage("You are stunned by a resonant frequency!");
                }
                else
                {
                    Caster.SendMessage("Your target is not wearing any plate armor.");
                }
            }

            FinishSequence();
        }

        private bool IsWearingPlateArmor(Mobile target)
        {
            List<Item> equipment = new List<Item>
            {
                target.FindItemOnLayer(Layer.InnerTorso),
                target.FindItemOnLayer(Layer.MiddleTorso),
                target.FindItemOnLayer(Layer.OuterTorso),
                target.FindItemOnLayer(Layer.Pants),
                target.FindItemOnLayer(Layer.Helm),
                target.FindItemOnLayer(Layer.Neck),
                target.FindItemOnLayer(Layer.Gloves),
                target.FindItemOnLayer(Layer.Arms)
            };

            // Check if the target is wearing any specific plate armor
            foreach (Item item in equipment)
            {
                if (item is PlateChest || 
                    item is PlateLegs || 
                    item is PlateHelm || 
                    item is PlateArms || 
                    item is PlateGloves || 
                    item is PlateGorget)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Regions; // Add this if you need region-based effects

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class ArmorPiercer : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Armor Piercer", "Penetro Lorica",
            21006,
            9405
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public ArmorPiercer(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArmorPiercer m_Owner;

            public InternalTarget(ArmorPiercer owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile defender = (Mobile)target;

                    if (m_Owner.CheckHSequence(defender))
                    {
                        // Visual and sound effects for the ability
                        from.PlaySound(0x2A); // Play a piercing sound
                        defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist); // Visual effect on target

                        // Calculate armor penetration effect
                        int damageBonus = (int)(from.Skills[SkillName.Swords].Value / 5); // 20% of Swords skill as extra damage
                        defender.Damage(Utility.RandomMinMax(10, 20) + damageBonus, from); // Base damage + extra damage

                        // Additional effect: Lower Armor Rating using VirtualArmorMod
                        int armorReduction = (int)(defender.ArmorRating * 0.15); // 15% armor reduction
                        defender.VirtualArmorMod -= armorReduction; // Use VirtualArmorMod instead of ArmorRating

                        from.SendMessage("Your strike pierces through the armor!");
                        defender.SendMessage("Your armor has been pierced, reducing its effectiveness!");

                        // Restore the defender's VirtualArmorMod after a duration
                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            if (defender != null && !defender.Deleted) // Ensure the defender still exists and is not deleted
                            {
                                defender.VirtualArmorMod += armorReduction; // Restore original armor effectiveness
                                defender.SendMessage("Your armor's effectiveness has been restored.");
                            }
                        });
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}

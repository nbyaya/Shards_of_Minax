using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class EnchantedSmithing : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enchanted Smithing", "Fabrum Enchanto",
            21018,
            9314,
            false,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle => SpellCircle.First;

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 30;

        public EnchantedSmithing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private EnchantedSmithing m_Owner;

            public InternalTarget(EnchantedSmithing owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseTool)
                {
                    BaseTool tool = (BaseTool)targeted;
                    m_Owner.ApplyEffect(from, tool);
                }
                else
                {
                    from.SendMessage("You can only use this spell on a crafting tool.");
                }
            }
        }

        public void ApplyEffect(Mobile caster, BaseTool tool)
        {
            if (tool == null || tool.Deleted)
                return;

            // Play sound and effects
            caster.PlaySound(0x1FA);
            Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 15, 1153, 0, 5052, 0);

            // Temporary skill boost and item quality enhancement
            double skillBoost = caster.Skills[SkillName.Blacksmith].Value * 0.1; // Boost 10% of current skill
            caster.SendMessage("Your smithing skills are temporarily enhanced!");

            // Apply skill boost
            SkillMod skillMod = new DefaultSkillMod(SkillName.Blacksmith, true, skillBoost);
            caster.AddSkillMod(skillMod);

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                caster.RemoveSkillMod(skillMod);
                caster.SendMessage("Your smithing skill enhancement has worn off.");
            });

            // Optionally, add a random enchantment effect to the next crafted item
            if (caster.Backpack != null)
            {
                foreach (Item item in caster.Backpack.Items)
                {
                    if (item is BaseWeapon || item is BaseArmor)
                    {
                        item.Hue = 1153; // Change color to indicate enchantment
                        caster.SendMessage("Your next crafted item has been enchanted!");
                        break;
                    }
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}

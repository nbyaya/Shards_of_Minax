using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using System.Collections;
using Server.ACC.CSS.Systems.ChivalryMagic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class CrusadersResolve : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Crusader's Resolve", "In Vus Mani",
            21014,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public CrusadersResolve(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile target = Caster;

                // Assuming a valid skill or attribute exists here. Replace with a valid skill or custom implementation.
                int skillValue = (int)(target.Skills[SkillName.Swords].Value); // Replace SkillName.Swords with the correct skill

                int strBoost = 10 + (skillValue / 5);
                int dexBoost = 10 + (skillValue / 5);
                int manaReduction = 5 + (skillValue / 10);

                target.SendMessage("You feel a surge of strength and agility course through your veins!");

                // Apply the stat boosts
                target.AddStatMod(new StatMod(StatType.Str, "CrusadersResolveStr", strBoost, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Dex, "CrusadersResolveDex", dexBoost, TimeSpan.FromSeconds(30)));

                // Instead of modifying spells, use a different method for mana cost reduction
                ApplyManaCostReduction(target, manaReduction, TimeSpan.FromSeconds(30));

                // Visual and sound effects
                target.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);
                target.PlaySound(0x28E);

                // Timer to remove the mana cost reduction after the effect duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (!target.Deleted)
                    {
                        RemoveManaCostReduction(target);
                        target.SendMessage("The effect of Crusader's Resolve has worn off.");
                    }
                });
            }

            FinishSequence();
        }

        private void ApplyManaCostReduction(Mobile target, int reduction, TimeSpan duration)
        {
            // Implement a custom system or effect here to handle mana cost reduction
        }

        private void RemoveManaCostReduction(Mobile target)
        {
            // Implement logic to remove the mana cost reduction effect
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}

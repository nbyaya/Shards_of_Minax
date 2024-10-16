using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DissonantShout : DiscordanceSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Dissonant Shout", "Exuo Vox",
            266,
            9040,
            Reagent.Bloodmoss,
            Reagent.Garlic
        );

        public override SpellCircle Circle => SpellCircle.Second; // Define the appropriate spell circle
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 15;

        public DissonantShout(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!CheckSequence())
                return;

            Caster.RevealingAction();
            Caster.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Visual effect around caster
            Caster.PlaySound(0x300); // Sound effect

            // Get all targets within 8 tiles radius
            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in Caster.GetMobilesInRange(8))
            {
                if (m != Caster && Caster.CanBeHarmful(m, false))
                {
                    targets.Add(m);
                }
            }

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);
                    int damage = Utility.RandomMinMax(5, 10); // Minor damage

                    // Apply a discordance-like debuff for a short duration (optional)
                    target.SendMessage("You are disoriented by the discordant sound!");
                    target.PlaySound(0x5C); // Sound effect for the target
                    target.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist); // Visual effect for the target
                    AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0); // Deal damage

                    // Optional: Apply a temporary debuff similar to Discordance
                    StatMod strMod = new StatMod(StatType.Str, "DissonantShoutStr", -5, TimeSpan.FromSeconds(10));
                    StatMod dexMod = new StatMod(StatType.Dex, "DissonantShoutDex", -5, TimeSpan.FromSeconds(10));
                    target.AddStatMod(strMod);
                    target.AddStatMod(dexMod);

                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        target.RemoveStatMod("DissonantShoutStr");
                        target.RemoveStatMod("DissonantShoutDex");
                    });
                }
            }
            else
            {
                Caster.SendMessage("There are no valid targets in range.");
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}

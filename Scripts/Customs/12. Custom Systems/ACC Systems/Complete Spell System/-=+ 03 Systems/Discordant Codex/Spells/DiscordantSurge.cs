using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.SkillHandlers;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordantSurge : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Discordant Surge", "An Lor Dis",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Third;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;

        public DiscordantSurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FB); // Sound effect for casting
                
                var targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false) && IsHostile(m))
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    Caster.SendMessage("You unleash a discordant surge, causing discordance in all hostile creatures nearby!");

                    foreach (Mobile target in targets)
                    {
                        if (target is BaseCreature bc && !Discordance.UnderEffects(target))
                        {
                            ApplyDiscordanceEffect(Caster, bc);
                        }

                        // Visual effect for each affected target
                        target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head);
                        target.PlaySound(0x1FB); // Sound effect for target hit
                    }
                }
                else
                {
                    Caster.SendMessage("There are no hostile creatures nearby to affect.");
                }
            }

            FinishSequence();
        }

        private static bool IsHostile(Mobile m)
        {
            return m is BaseCreature && ((BaseCreature)m).ControlMaster == null && !m.Alive;
        }

        private static void ApplyDiscordanceEffect(Mobile caster, Mobile target)
        {
            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;
                double discordSkill = caster.Skills[SkillName.Discordance].Value;
                double duration = discordSkill / 50.0; // Duration based on Discordance skill level
                duration = Math.Max(10.0, duration); // Minimum 10 seconds

                int effect = (int)(discordSkill / -4.0); // Effect strength

                List<ResistanceMod> mods = new List<ResistanceMod>
                {
                    new ResistanceMod(ResistanceType.Physical, effect),
                    new ResistanceMod(ResistanceType.Fire, effect),
                    new ResistanceMod(ResistanceType.Cold, effect),
                    new ResistanceMod(ResistanceType.Poison, effect),
                    new ResistanceMod(ResistanceType.Energy, effect)
                };

                // Apply mods to the target
                foreach (var mod in mods)
                {
                    target.AddResistanceMod(mod);
                }

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () => 
                {
                    foreach (var mod in mods)
                    {
                        target.RemoveResistanceMod(mod);
                    }

                    target.SendMessage("The discordance effect has worn off.");
                });

                caster.DoHarmful(target);
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}

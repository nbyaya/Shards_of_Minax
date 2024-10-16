using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class EnigmaticMelody : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enigmatic Melody", 
            "Restore hits, mana, and stamina continuously to all followers in a 3 tile radius for a short time",
            21004, 
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public EnigmaticMelody(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();
                Caster.SendMessage("You play an enigmatic melody, restoring vitality to your followers!");

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m is BaseCreature && ((BaseCreature)m).ControlMaster == Caster)
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x5C2); // Sound of a mystical tune
                    Caster.FixedParticles(0x376A, 1, 32, 9942, EffectLayer.Waist); // Visual effect of swirling light

                    foreach (Mobile follower in targets)
                    {
                        follower.SendMessage("You feel the soothing effects of your master's melody.");
                        follower.FixedParticles(0x376A, 1, 32, 9942, EffectLayer.Waist);

                        Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), () => RestoreFollower(follower, Caster));
                    }
                }
                else
                {
                    Caster.SendMessage("You have no followers within range to affect.");
                }
            }

            FinishSequence();
        }

        private void RestoreFollower(Mobile follower, Mobile caster)
        {
            if (follower.Alive && follower.Map == caster.Map && follower.InRange(caster, 3))
            {
                follower.Hits += Utility.RandomMinMax(5, 10); // Restore a small amount of health
                follower.Mana += Utility.RandomMinMax(5, 10); // Restore a small amount of mana
                follower.Stam += Utility.RandomMinMax(5, 10); // Restore a small amount of stamina

                Effects.PlaySound(follower.Location, follower.Map, 0x1F3); // Sound effect for healing
                follower.FixedParticles(0x376A, 1, 32, 9942, EffectLayer.Head); // Visual effect on the follower

                if (Utility.RandomDouble() < 0.2) // 20% chance to apply a buff
                {
                    follower.SendMessage("The melody grants you a momentary surge of power!");
                    follower.AddStatMod(new StatMod(StatType.Str, "EnigmaticMelodyStr", 10, TimeSpan.FromSeconds(10)));
                    follower.AddStatMod(new StatMod(StatType.Dex, "EnigmaticMelodyDex", 10, TimeSpan.FromSeconds(10)));
                    follower.AddStatMod(new StatMod(StatType.Int, "EnigmaticMelodyInt", 10, TimeSpan.FromSeconds(10)));
                }
            }
        }
    }
}

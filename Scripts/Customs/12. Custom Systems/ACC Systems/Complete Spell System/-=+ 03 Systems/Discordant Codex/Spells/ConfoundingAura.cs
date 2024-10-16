using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class ConfoundingAura : DiscordanceSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Confounding Aura", "Aura Confundi",
            // SpellCircle.Sixth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 25; } }

        public ConfoundingAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Mana -= RequiredMana; // Reduce mana
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Aura visual effect
                Caster.PlaySound(0x1F5); // Sound effect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3)) // Check for enemies within 3 tiles
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);

                    int damage = Utility.RandomMinMax(15, 30); // Random damage between 15 and 30
                    target.Damage(damage, Caster);

                    target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head); // Individual target visual effect
                    target.PlaySound(0x209); // Individual target sound effect
                }

                Caster.SendMessage("You unleash a confounding aura, damaging nearby enemies!"); // Message to caster
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}

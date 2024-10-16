using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class Cleave : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cleave", "Uus Sanct Flam",
            21001,
            9400,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public Cleave(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You unleash a powerful cleave!");

                // Display a flashy visual effect
                Caster.FixedParticles(0x3779, 1, 30, 9945, 1153, 3, EffectLayer.Waist);

                // Play a powerful sound effect
                Caster.PlaySound(0x1FA);

                // Get enemies within a certain range
                List<Mobile> targets = new List<Mobile>();
                Map map = Caster.Map;

                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(3))
                    {
                        if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                        {
                            targets.Add(m);
                        }
                    }
                }

                // Apply damage to all targets in the area
                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    Caster.DoHarmful(m);
                    double damage = Utility.RandomMinMax(20, 40); // Adjust damage range as needed
                    AOS.Damage(m, Caster, (int)damage, 100, 0, 0, 0, 0);

                    // Show visual effects on each target
                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x3B9);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}

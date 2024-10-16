using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class MockingLaugh : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Mocking Laugh", "Ahahahaha!",
                                                        21005, // Icon ID
                                                        9400   // Sound ID
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public MockingLaugh(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Perform the mocking laugh effect
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                Caster.PlaySound(0x212); // Play a laugh sound

                ArrayList targets = new ArrayList();
                Map map = Caster.Map;

                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(5)) // Get enemies within a 5-tile radius
                    {
                        if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                        {
                            if (m is BaseCreature)
                            {
                                BaseCreature bc = (BaseCreature)m;

                                // Removed the Team check
                                if (bc.Controlled || bc.Summoned)
                                {
                                    targets.Add(m);
                                }
                            }
                            else if (m.Player)
                            {
                                targets.Add(m);
                            }
                        }
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);

                    m.SendMessage("You feel demoralized by the mocking laugh!");
                    m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head);
                    m.PlaySound(0x1FE); // Play a taunt sound

                    // Lower attack and defense skills
                    int attackSkillPenalty = 20;
                    int defenseSkillPenalty = 20;

                    // Apply skill penalties for a short duration
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Paralyze, 1075847, 1075848, TimeSpan.FromSeconds(10), m));
                    m.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, -attackSkillPenalty, TimeSpan.FromSeconds(10)));
                    m.AddSkillMod(new TimedSkillMod(SkillName.Wrestling, true, -defenseSkillPenalty, TimeSpan.FromSeconds(10)));
                }
            }

            FinishSequence();
        }
    }
}

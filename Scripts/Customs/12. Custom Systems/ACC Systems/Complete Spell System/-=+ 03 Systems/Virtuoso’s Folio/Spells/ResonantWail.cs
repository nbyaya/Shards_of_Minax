using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class ResonantWail : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Resonant Wail", "Wail!",
                                                        //SpellCircle.Fifth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 25; } }

        public ResonantWail(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.SendMessage("You begin to emit a haunting melody...");
            Caster.PlaySound(0x1FD); // Play a haunting sound effect

            Timer.DelayCall(TimeSpan.FromSeconds(1.5), new TimerStateCallback(ExecuteWail), Caster);
        }

        private void ExecuteWail(object state)
        {
            Mobile caster = (Mobile)state;

            if (CheckSequence())
            {
                Map map = caster.Map;

                if (map == null)
                    return;

                List<Mobile> targets = new List<Mobile>();

                IPooledEnumerable eable = caster.GetMobilesInRange(5); // Range of 5 tiles
                foreach (Mobile m in eable)
                {
                    if (m != caster && SpellHelper.ValidIndirectTarget(caster, m) && caster.CanBeHarmful(m, false) && m.Player)
                    {
                        targets.Add(m);
                    }
                }
                eable.Free();

                if (targets.Count > 0)
                {
                    caster.PlaySound(0x5C7); // Sound effect of fear
                    caster.FixedParticles(0x373A, 10, 15, 5013, EffectLayer.Waist); // Visual effect: haunting green swirls

                    foreach (Mobile m in targets)
                    {
                        caster.DoHarmful(m);
                        m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head); // Visual effect: skull above head
                        m.PlaySound(0x1F8); // Panic sound effect
                        m.SendMessage("You are filled with a sudden fear and begin to flee!");

                        // Apply fear effect: make the target flee for a short duration
                        m.Direction = (Direction)Utility.Random(8);
                        m.Move(m.Direction);
                        m.Frozen = true;

                        Timer.DelayCall(TimeSpan.FromSeconds(3.0), () => m.Frozen = false); // Unfreeze after 3 seconds
                    }
                }
                else
                {
                    caster.SendMessage("There are no enemies in range to frighten.");
                }
            }

            FinishSequence();
        }
    }
}

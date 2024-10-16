using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class RallyingCry : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rallying Cry", "Rally to Me!",
            //SpellCircle.Third,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RallyingCry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Get all followers within 3 tiles
                ArrayList followers = new ArrayList();
                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m is BaseCreature && ((BaseCreature)m).ControlMaster == Caster)
                        followers.Add(m);
                }

                // Apply buffs to followers
                foreach (Mobile follower in followers)
                {
                    if (follower.Alive)
                    {
                        // Increase stats
                        follower.SendMessage("You feel the power of your leader's rallying cry!");
                        follower.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
                        follower.PlaySound(0x1F7); // Sound effect

                        BuffInfo.AddBuff(follower, new BuffInfo(BuffIcon.Strength, 1075845, 1075846, TimeSpan.FromSeconds(20), follower)); // Buff Icon
                        BuffInfo.AddBuff(follower, new BuffInfo(BuffIcon.Strength, 1075845, 1075846, TimeSpan.FromSeconds(20), follower)); // Buff Icon
                        BuffInfo.AddBuff(follower, new BuffInfo(BuffIcon.Strength, 1075845, 1075846, TimeSpan.FromSeconds(20), follower)); // Buff Icon

                        follower.RawStr += 10; // Temporary boost to Strength
                        follower.RawDex += 10; // Temporary boost to Dexterity
                        follower.RawInt += 10; // Temporary boost to Intelligence

                        Timer.DelayCall(TimeSpan.FromSeconds(20), delegate
                        {
                            follower.RawStr -= 10; // Revert Strength boost
                            follower.RawDex -= 10; // Revert Dexterity boost
                            follower.RawInt -= 10; // Revert Intelligence boost
                        });
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}

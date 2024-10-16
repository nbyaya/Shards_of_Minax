using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Linq;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class CompanionsRoar : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Companion’s Roar", "ROAR!",
                                                        21005,
                                                        9400
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 18; } }

        public CompanionsRoar(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                List<BaseCreature> followers = Caster.GetMobilesInRange(5)
                    .OfType<BaseCreature>()
                    .Where(creature => creature.Controlled && creature.ControlMaster == Caster)
                    .ToList();

                if (followers.Count > 0)
                {
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x214); // Roar sound effect
                    Caster.FixedParticles(0x373A, 10, 30, 5012, EffectLayer.Waist); // Visual effect around caster

                    foreach (BaseCreature follower in followers)
                    {
                        if (follower == null || follower.Deleted)
                            continue;

                        // Boost Stats
                        int originalStr = follower.Str;
                        int originalDex = follower.Dex;

                        int boostStr = (int)(follower.Str * 0.5);
                        int boostDex = (int)(follower.Dex * 0.5);

                        follower.Str += boostStr;
                        follower.Dex += boostDex;

                        // Visual Effect on each follower
                        follower.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect on each follower
                        follower.PlaySound(0x659); // Sound effect for boost

                        // Restore original stats after duration
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                        {
                            if (follower != null && !follower.Deleted)
                            {
                                follower.Str = originalStr;
                                follower.Dex = originalDex;

                                follower.FixedParticles(0x373A, 10, 30, 5012, EffectLayer.Waist); // End effect
                                follower.PlaySound(0x1F8); // Sound effect for ending boost
                            }
                        });
                    }
                }
                else
                {
                    Caster.SendMessage("You have no followers within range.");
                }
            }

            FinishSequence();
        }
    }
}

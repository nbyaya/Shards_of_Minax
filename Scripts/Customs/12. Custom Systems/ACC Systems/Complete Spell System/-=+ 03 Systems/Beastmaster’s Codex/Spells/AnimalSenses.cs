using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class AnimalSenses : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Senses", "WILD SOUL",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public AnimalSenses(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual effect at caster location
                Effects.PlaySound(Caster.Location, Caster.Map, 0x3D);
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                
                // Get all mobiles within 5 tiles that are followers of the caster
                List<Mobile> followers = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature creature && creature.ControlMaster == Caster && creature.Alive && !creature.IsDeadBondedPet)
                    {
                        followers.Add(m);
                    }
                }

                // Apply Dexterity increase to all followers
                foreach (Mobile follower in followers)
                {
                    int dexIncrease = (int)(follower.RawDex * 0.2); // 20% increase
                    follower.RawDex += dexIncrease;
                    follower.SendMessage("Your dexterity has been increased by the Animal Senses!");

                    // Flashy visual effect for each follower
                    follower.FixedParticles(0x373A, 1, 15, 9502, 0, 0, EffectLayer.Head);
                    follower.PlaySound(0x213);

                    // Timer to revert the dexterity after a duration
                    Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                    {
                        if (follower != null && !follower.Deleted && follower.Alive)
                        {
                            follower.RawDex -= dexIncrease;
                            follower.SendMessage("The effect of Animal Senses has worn off.");
                            follower.FixedParticles(0x3735, 1, 30, 9503, 0, 0, EffectLayer.Waist);
                        }
                    });
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}

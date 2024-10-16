using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class HeartstopperChili : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Heartstopper Chili", "Chili Incantatio",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle => SpellCircle.First;
		public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public HeartstopperChili(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HeartstopperChili m_Owner;

            public InternalTarget(HeartstopperChili owner) : base(1, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (target == from)
                    {
                        target.AddToBackpack(new HeartstopperChiliItem());
                        from.SendMessage("You summon a Heartstopper Chili in your backpack!");

                        Effects.PlaySound(from.Location, from.Map, 0x208);
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => 
                        {
                            from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                            from.PlaySound(0x44B);
                        });
                    }
                    else
                    {
                        from.SendMessage("You can only summon this chili for yourself.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public class HeartstopperChiliItem : Item
        {
            public HeartstopperChiliItem() : base(0x0C64) // Custom item ID for the chili pepper
            {
                Name = "Heartstopper Chili";
                Hue = 33; // A fiery red color
                Weight = 1.0;
                Movable = true;
            }

            public HeartstopperChiliItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }

            public override void OnDoubleClick(Mobile from)
            {
                if (!Movable)
                    return;

                if (from.InRange(this.GetWorldLocation(), 1))
                {
                    from.SendMessage("You consume the Heartstopper Chili!");
                    from.PlaySound(0x229);
                    from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);

                    int selfDamage = Utility.RandomMinMax(15, 30);
                    from.Damage(selfDamage, from);

                    from.SendMessage("The chili burns your insides as a fiery explosion erupts around you!");

                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                    {
                        Effects.PlaySound(from.Location, from.Map, 0x208);
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044, 0);

                        foreach (Mobile m in from.GetMobilesInRange(2))
                        {
                            if (m != from && m.CanBeHarmful(from))
                            {
                                m.Damage(Utility.RandomMinMax(20, 40), from);
                                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
                            }
                        }
                    });

                    Delete();
                }
                else
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
                }
            }
        }
    }
}

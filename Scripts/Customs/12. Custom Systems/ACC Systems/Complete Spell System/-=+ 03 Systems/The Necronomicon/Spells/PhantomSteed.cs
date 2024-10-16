using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class PhantomSteedSpell : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Phantom Steed", "An Ex Mo Ex",
            21004,
            9300,
            false,
            Reagent.BatWing,
            Reagent.GraveDust
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 20; } }

        public PhantomSteedSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual and sound effect
                Caster.PlaySound(0x64); // Ghostly sound
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist); // Spectral effect

                // Summon the phantom steed
                BaseMount steed = new PhantomSteed();
                SpellHelper.Summon(steed, Caster, 0x217, TimeSpan.FromHours(1), false, false);

                steed.ControlMaster = Caster;
                steed.Controlled = true;
                steed.ControlOrder = OrderType.Follow;

                // Adjust the mount's dexterity using SetDex
                steed.SetDex(200); // High dexterity for increased speed

                Caster.SendMessage("You have summoned a Phantom Steed!");

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        private class PhantomSteed : BaseMount
        {
            public PhantomSteed() : base("a phantom steed", 793, 0x3EBB, AIType.AI_Animal, FightMode.None, 10, 1, 5, 5)
            {
                Hue = 0x4001; // Ghostly hue
                Name = "Phantom Steed";
                Body = 0x316; // Spectral horse body
                BaseSoundID = 0xA8;

                SetStr(100);
                SetDex(200); // High dexterity for increased speed
                SetInt(100);

                SetHits(150);
                SetMana(0);

                SetDamage(5, 10);

                SetDamageType(ResistanceType.Physical, 100);

                SetResistance(ResistanceType.Physical, 30, 40);
                SetResistance(ResistanceType.Fire, 20, 30);
                SetResistance(ResistanceType.Cold, 50, 60);
                SetResistance(ResistanceType.Poison, 20, 30);
                SetResistance(ResistanceType.Energy, 40, 50);

                SetSkill(SkillName.MagicResist, 60.0, 75.0);
                SetSkill(SkillName.Tactics, 65.0, 80.0);
                SetSkill(SkillName.Wrestling, 60.0, 80.0);

                Fame = 0;
                Karma = 0;

                VirtualArmor = 25;

                CanSwim = true; // Allows the steed to pass through water
                CantWalk = true; // Allows passing through terrain obstacles

                Tamable = false; // Not tamable by players
            }

            public PhantomSteed(Serial serial) : base(serial)
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
				if (from == this.ControlMaster && from.Race == null)
				{
					if (this.Controlled)
					{
						if (from.InRange(this, 1))
						{
							if (this.Rider != null)
								from.SendLocalizedMessage(500284); // This mount is already being ridden.
							else
							{
								// Set the Rider property to mount the player
								this.Rider = from; // Correct way to mount the Mobile
							}
						}
						else
							from.SendLocalizedMessage(500206); // That is too far away to ride.
					}
					else
					{
						from.SendLocalizedMessage(1042562); // This is not your mount!
					}
				}
			}

        }
    }
}

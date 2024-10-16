using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;
using Server.Items; // Make sure this is included

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class ScriptOfSummoning : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Script of Summoning", "Vas Kal An Mani",
            21004, // Icon ID
            9300,  // Sound ID
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }

        public ScriptOfSummoning(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        private static Type[] m_Types = new Type[]
        {
            typeof(MagicalConstruct),
            typeof(FireElemental),
            typeof(WaterElemental),
            typeof(EarthElemental),
            typeof(AirElemental)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    Type summonType = m_Types[Utility.Random(m_Types.Length)];
                    BaseCreature summonedCreature = (BaseCreature)Activator.CreateInstance(summonType);

                    SpellHelper.Summon(summonedCreature, Caster, 0x215, TimeSpan.FromSeconds(3.0 * Caster.Skills[CastSkill].Value), false, false);

                    // Visual and Sound Effects
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x212);
                    Caster.SendMessage("You summon a magical construct to aid you!");

                    // Additional summons based on skill level
                    double additionalSummonsChance = Utility.RandomDouble() + (Caster.Skills[CastSkill].Value * 0.01);

                    if (additionalSummonsChance > 0.75)
                    {
                        BaseCreature additionalCreature = (BaseCreature)Activator.CreateInstance(summonType);
                        SpellHelper.Summon(additionalCreature, Caster, 0x215, TimeSpan.FromSeconds(3.0 * Caster.Skills[CastSkill].Value), false, false);
                        Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 9, 32, 5008);
                        Effects.PlaySound(Caster.Location, Caster.Map, 0x1FB);
                        Caster.SendMessage("An additional construct joins the battle!");
                    }
                }
                catch
                {
                    Caster.SendMessage("The summoning spell failed to cast.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

	public class MagicalConstruct : BaseCreature
	{
		public MagicalConstruct() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "Magical Construct";
			Body = 0x190; // Example body ID, replace with appropriate ID
			BaseSoundID = 0x45A; // Example sound ID, replace with appropriate ID

			SetStr(100);
			SetDex(75);
			SetInt(50);

			SetHits(150);
			SetStam(75);
			SetMana(0);

			SetDamage(10, 15);

			SetResistance(ResistanceType.Physical, 20, 30);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			VirtualArmor = 30;
			ControlSlots = 1;
		}

		public MagicalConstruct(Serial serial) : base(serial) { }

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
	}

}

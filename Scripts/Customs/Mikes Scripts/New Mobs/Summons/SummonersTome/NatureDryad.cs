using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a nature dryad's corpse")]
    public class NatureDryad : BaseCreature
    {
        private Mobile _summoner;

        [Constructable]
        public NatureDryad()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Nature Dryad";
            Body = 0x191; // Female elf body
            Hue = 0x47E;  // Natural green tint
            BaseSoundID = 0x4B0;



            // Scale stats based on SpiritSpeak
            double spiritSpeak = 0;

            int bonus = (int)(spiritSpeak * 1.2); // Stat bonus scale

            SetStr(200 + bonus / 2, 250 + bonus / 2);
            SetDex(150 + bonus / 3, 200 + bonus / 3);
            SetInt(250 + bonus, 300 + bonus);

            SetHits(250 + bonus, 300 + bonus);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30 + bonus / 10, 40 + bonus / 10);
            SetResistance(ResistanceType.Fire, 20 + bonus / 12, 30 + bonus / 12);
            SetResistance(ResistanceType.Cold, 40 + bonus / 10, 50 + bonus / 10);
            SetResistance(ResistanceType.Poison, 50 + bonus / 8, 60 + bonus / 8);
            SetResistance(ResistanceType.Energy, 50 + bonus / 10, 60 + bonus / 10);

            SetSkill(SkillName.EvalInt, 80.0 + bonus / 5, 100.0 + bonus / 5);
            SetSkill(SkillName.Magery, 85.0 + bonus / 5, 100.0 + bonus / 5);
            SetSkill(SkillName.MagicResist, 75.0 + bonus / 5, 95.0 + bonus / 5);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);
            SetSkill(SkillName.Healing, 100.0); // Custom behavior

            Fame = 12000;
            Karma = 12000;

            VirtualArmor = 50 + bonus / 4;
        }

        public override bool BardImmune => true;
        public override bool Unprovokable => true;
        public override bool BleedImmune => true;
        public override bool AlwaysMurderer => false;

        public override void OnThink()
        {
            base.OnThink();

            // Every few seconds, check if allies need healing or enemies need entangling
            if (DateTime.UtcNow > m_NextAbilityTime)
            {
                TryHealAlly();
                TryEntangleEnemy();
                m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(6);
            }
        }

        private DateTime m_NextAbilityTime;

        private void TryHealAlly()
        {
            if (_summoner == null || _summoner.Deleted)
                return;

            if (_summoner.Hits < _summoner.HitsMax && this.InRange(_summoner, 8))
            {
                _summoner.FixedParticles(0x376A, 9, 20, 5030, EffectLayer.Waist);
                _summoner.PlaySound(0x1F2);

                int healAmount = Utility.RandomMinMax(20, 40) + (int)(_summoner.Skills[SkillName.SpiritSpeak].Value / 5);
                _summoner.Heal(healAmount);
            }
        }

		private void TryEntangleEnemy()
		{
			if (Combatant is Mobile target && target.Alive && this.InRange(target, 8))
			{
				target.SendMessage("Roots burst from the ground and entangle you!");
				target.Freeze(TimeSpan.FromSeconds(2));
				target.FixedEffect(0x376A, 10, 20);
				target.PlaySound(0x1FB);
			}
		}


        public override void GenerateLoot()
        {
            // No loot - summoned creature
        }

        public override bool DeleteCorpseOnDeath => true;


        public NatureDryad(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

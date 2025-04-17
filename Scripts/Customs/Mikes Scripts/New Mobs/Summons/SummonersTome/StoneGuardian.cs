using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a stone guardian corpse")]
    public class SummonedStoneGuardian : BaseCreature
    {
        private Mobile _summoner;
        private double _spiritSpeakBonus;

        [Constructable]
        public SummonedStoneGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a stone guardian";
            Body = 14;
            BaseSoundID = 268;
            Hue = 2407; // Stone/grayish hue
            Team = 2;


            _spiritSpeakBonus = 0;

            double bonusScalar = _spiritSpeakBonus / 100.0;

            SetStr(150 + (int)(50 * bonusScalar), 180 + (int)(60 * bonusScalar));
            SetDex(60, 80);
            SetInt(30, 50);

            SetHits(250 + (int)(150 * bonusScalar));

            SetDamage(12 + (int)(5 * bonusScalar), 20 + (int)(8 * bonusScalar));
            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 5000;
            Karma = 0;

            VirtualArmor = 60 + (int)(_spiritSpeakBonus * 0.3); // Stone Skin scaling

            ControlSlots = 3;

            // Optional: Give it some kind of visual item or crystal?
            PackItem(new FertileDirt(Utility.RandomMinMax(2, 5)));
        }

        public override bool AutoDispel => false;
        public override double DispelDifficulty => 130.0;
        public override double DispelFocus => 60.0;
        public override bool BleedImmune => true;
        public override bool DeleteCorpseOnDeath => true;

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Staggering Slam: 20% chance to stun for 2 seconds
            if (Utility.RandomDouble() < 0.2)
            {
                defender.Freeze(TimeSpan.FromSeconds(2.0));
                defender.SendMessage("You are staggered by the Stone Guardian's slam!");
                defender.PlaySound(0x213); // Stun sound
                defender.FixedEffect(0x376A, 9, 32); // Visual effect
            }
        }

        public SummonedStoneGuardian(Serial serial) : base(serial) { }

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

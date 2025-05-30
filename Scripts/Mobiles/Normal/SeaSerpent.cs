using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a sea serpents corpse")]
    [TypeAlias("Server.Mobiles.Seaserpant")]
    public class SeaSerpent : BaseCreature
    {
        [Constructable]
        public SeaSerpent()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sea serpent";
            Body = 150;
            BaseSoundID = 447;

            Hue = Utility.Random(0x530, 9);

            SetStr(168, 225);
            SetDex(58, 85);
            SetInt(53, 95);

            SetHits(110, 127);

            SetDamage(7, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 60.1, 75.0);
            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 30;
            CanSwim = true;
            CantWalk = true;

            if (Utility.RandomBool())
                PackItem(new SulfurousAsh(4));
            else
                PackItem(new BlackPearl(4));

            PackItem(new RawFishSteak());

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public SeaSerpent(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel { get { return Utility.RandomList(1, 2); } }
        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override int Scales { get { return 8; } }
        public override ScaleType ScaleType { get { return ScaleType.Blue; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new OceanmastersRobes());
            }				
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}

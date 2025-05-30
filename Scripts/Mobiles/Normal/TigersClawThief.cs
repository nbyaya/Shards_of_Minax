using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black order thief corpse")]
    public class TigersClawThief : BaseCreature
    {
        [Constructable]
        public TigersClawThief()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Black Order Thief";
            Title = "of the Tiger's Claw Sect";
            Female = Utility.RandomBool();
            Race = Race.Human;
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            Race.RandomFacialHair(this);

            AddItem(new ThighBoots(0x51D));
            AddItem(new Wakizashi());
            AddItem(new FancyShirt(0x51D));
            AddItem(new StuddedMempo());
            AddItem(new JinBaori(0x69));

            Item item;

            item = new StuddedGloves();
            item.Hue = 0x69;
            AddItem(item);

            item = new LeatherNinjaPants();
            item.Hue = 0x51D;
            AddItem(item);

            item = new LightPlateJingasa();
            item.Hue = 0x51D;
            AddItem(item);

            // TODO quest items

            SetStr(340, 360);
            SetDex(400, 415);
            SetInt(200, 215);

            SetHits(800, 815);

            SetDamage(13, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 65);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 55, 60);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 115.0, 130.0);
            SetSkill(SkillName.Wrestling, 95.0, 120.0);
            SetSkill(SkillName.Anatomy, 105.0, 120.0);
            SetSkill(SkillName.Fencing, 78.0, 100.0);
            SetSkill(SkillName.Swords, 90.1, 105.0);
            SetSkill(SkillName.Ninjitsu, 90.0, 120.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);

            Fame = 13000;
            Karma = -13000;

            VirtualArmor = 58;
        }

        public TigersClawThief(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 4);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new TigersClawSash());
            }			
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.3)
                c.DropItem(new TigerClawSectBadge());
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
    }
}
